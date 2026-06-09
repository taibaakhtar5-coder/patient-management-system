using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using HealthcareCRM.Models;
// Note: Agar un ka Data folder alag hai to un ke namespace ke mutabiq context automatic resolve ho jayega

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // DB Context Inject kar rahe hain repository pattern/EF Core ke mutabiq
        public PatientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET /api/patients - Get all patients with filter search
        [HttpGet]
        public async Task<IActionResult> GetPatients([FromQuery] string? search)
        {
            var query = _context.Patients.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.FullName.Contains(search));
            }

            var patientsList = await query.ToListAsync();
            return Ok(new { success = true, data = patientsList, message = "Patients retrieved successfully." });
        }

        // 2. POST /api/patients - Add a new patient
        [HttpPost]
        public async Task<IActionResult> CreatePatient([FromBody] Patient newPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, data = ModelState, message = "Validation failed." });
            }

            newPatient.Id = Guid.NewGuid();
            newPatient.CreatedAt = DateTime.UtcNow;

            _context.Patients.Add(newPatient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPatients), new { id = newPatient.Id }, new { success = true, data = newPatient, message = "Patient added successfully." });
        }

        // 3. PUT /api/patients/{id} - Update patient record
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(Guid id, [FromBody] Patient updatedPatient)
        {
            var existingPatient = await _context.Patients.FirstOrDefaultAsync(p => p.Id == id);
            if (existingPatient == null)
            {
                return NotFound(new { success = false, message = "Patient not found." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, data = ModelState, message = "Validation failed." });
            }

            existingPatient.FullName = updatedPatient.FullName;
            existingPatient.Email = updatedPatient.Email;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.DateOfBirth = updatedPatient.DateOfBirth;
            existingPatient.Gender = updatedPatient.Gender;
            existingPatient.Address = updatedPatient.Address;

            await _context.SaveChangesAsync();
            return Ok(new { success = true, data = existingPatient, message = "Patient records updated successfully." });
        }
    }
}