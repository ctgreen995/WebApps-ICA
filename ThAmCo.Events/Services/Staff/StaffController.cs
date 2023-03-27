using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ThAmCo.Events.Events.Data;
using ThAmCo.Events.Models.StaffViewModels;

namespace ThAmCo.Events.Services.Staff
{
    public class StaffController : Controller
    {
        private readonly EventsDbContext _context;

        public StaffController(EventsDbContext context)
        {
            _context = context;
        }

        #region CRUD

        public async Task<IActionResult> ListStaff()
        {
            List<EmployeeVM> employeeVMs = await _context.Staff.Select(s => new EmployeeVM
            {
                Id = s.Id,
                Name = s.Name
            }).ToListAsync();

            return View(employeeVMs);
        }

        public async Task<IActionResult> EmployeeDetails(int id)
        {
            var employee = await _context.Staff
                .Include(es => es.Events)
                .ThenInclude(e => e.Event)
                .Where(i => i.Id == id).FirstOrDefaultAsync();

            var employeeDetailsVM = new EmployeeDetailsVM
            {
                Id = employee.Id,
                Name = employee.Name,
                AssignedEvents = employee.Events.Select(e => new EmployeeEventVM
                {
                    EventId= e.EventId,
                    EventName = e.Event.Title
                }).ToList(),
                IsFirstAider = employee.IsFirstAider,
            };

            return View(employeeDetailsVM);
        }

        public async Task<IActionResult> CreateEmployee()
        {
            var events = await _context.Events
                .Include(s => s.Staff)
                .ThenInclude(e => e.Employee)
                .ToListAsync();

            if (events == null)
            {
                return NotFound("Could not find events.");
            }

            var employeeVM = new CreateEmployeeVM
            {
                AssignedEvents = new List<SelectListItem>(),
                AvailableEvents = events.Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = e.Date + ", " + e.Title
                }).ToList(),
            };

            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeVM employeeVM, string[] selectedEvents)
        {
            if (ModelState.IsValid)
            {
                var newEmployee = new Employee
                {
                    Name = employeeVM.Name,
                    IsFirstAider = employeeVM.IsFirstAider
                };
                _context.Staff.Add(newEmployee);
                await _context.SaveChangesAsync();

                int id = newEmployee.Id;

                List<EventStaff> assignedEvents = selectedEvents.Select(e => new EventStaff
                {
                    EventId = e,
                    EmployeeId = id
                }).ToList();

                _context.EventStaff.AddRange(assignedEvents);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListStaff));
            }
            return View(employeeVM);
        }

        public async Task<IActionResult> EditEmployee(int? id)
        {
            if (id == null || _context.Staff == null)
            {
                return NotFound();
            }

            var employee = await _context.Staff
                .Include(s => s.Events)
                .ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (employee == null)
            {
                return NotFound();
            }

            var events = await _context.Events
                .Include(s => s.Staff)
                .ThenInclude(e => e.Employee)
                .ToListAsync();

            if (events == null)
            {
                return NotFound("Could not find events.");
            }

            var editEmployeeVM = new EditEmployeeVM
            {
                Id = employee.Id,
                Name = employee.Name,
                IsFirstAider = employee.IsFirstAider,
                AssignedEvents = employee.Events.Select(b => new SelectListItem
                {
                    Value = b.EventId,
                    Text = b.Event.Date + ", " + b.Event.Title
                }).ToList(),
                AvailableEvents = events
                .Where(g => !g.Staff
                .Any(g => g.EmployeeId == id))
                .Select(e => new SelectListItem
                {
                    Value = e.Id,
                    Text = e.Date + ", " + e.Title
                }).ToList()
            };

            return View(editEmployeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeVM editEmployeeVM, string[] selectedEvents)
        {
            if (ModelState.IsValid)
            {
                var employee = new Employee
                {
                    Id = editEmployeeVM.Id,
                    Name = editEmployeeVM.Name,
                    IsFirstAider = editEmployeeVM.IsFirstAider
                };

                List<EventStaff> assignedEvents = selectedEvents.Select(e => new EventStaff
                {
                    EventId = e,
                    EmployeeId = employee.Id
                }).ToList();

                try
                {
                    _context.EventStaff.RemoveRange(_context.EventStaff.Where(e => e.EmployeeId == editEmployeeVM.Id));
                    _context.EventStaff.AddRange(assignedEvents);

                    _context.Update(employee);

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException e)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound(e);
                    }
                    else
                    {
                        return BadRequest(e);
                    }
                }
                return RedirectToAction(nameof(ListStaff));
            }
            return View(editEmployeeVM);
        }

        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employeeVM = await _context.Staff
                .Where(e => e.Id == id).Select(em => new EmployeeVM
                {
                    Id = em.Id,
                    Name= em.Name,
                    IsFirstAider=em.IsFirstAider
                }).FirstOrDefaultAsync();

            if (employeeVM == null)
            {
                return NotFound("Failed to locate employee Id in database.");
            }

            return View(employeeVM);
        }

        [HttpPost, ActionName("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployeeConfirmed(int id)
        {
            try
            {
                var employeeToRemove = await _context.Staff.FindAsync(id);
                if (employeeToRemove == null)
                {
                    return NotFound("Employee not found in the database.");
                }

                _context.EventStaff.RemoveRange(_context.EventStaff.Where(e => e.EmployeeId == id));
                _context.Staff.Remove(employeeToRemove);

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(ListStaff));
            }
            catch (Exception e)
            {
                return BadRequest("Failed to delete employee from database." + e.Message);
            }
        }

        #endregion

        private bool EmployeeExists(int id)
        {
            return _context.Staff.Any(e => e.Id == id);
        }

        public async Task<bool> GetIsFirstAider(int employeeId)
        {
            var employee = await _context.Staff.FindAsync(employeeId);
            return employee.IsFirstAider;
        }

        public async Task<List<Employee>> GetAvailableStaff(string id)
        {
            var staff = await _context.Staff.Include(e => e.Events).ToListAsync();

            return staff.Where(s => !s.Events.Any(e => e.EventId == id)).ToList();
        }

        public async Task UpdateEventStaffing(int[] selectedStaff, string eventId)
        {
            List<EventStaff> eventStaff = selectedStaff.Select(s => new EventStaff
            {
                EmployeeId = s,
                EventId = eventId,
            }).ToList();
            _context.EventStaff.RemoveRange(_context.EventStaff.Where(e => e.EventId == eventId));

            await _context.EventStaff.AddRangeAsync(eventStaff);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Employee>> GetAssignedStaff(string id)
        {
            var eventStaff = await _context.EventStaff.Include(s => s.Employee).Where(e => e.EventId == id).ToListAsync();

            return eventStaff.Select(e => e.Employee).ToList();
        }
    }
}
