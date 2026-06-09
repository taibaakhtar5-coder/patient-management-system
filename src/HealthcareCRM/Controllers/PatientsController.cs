using Microsoft.AspNetCore.Mvc;
using System.Linq;
using HealthcareCRM.Models;

namespace HealthcareCRM.Controllers
{
    [ApiController]
    [Route("api/patients")]
    public class PatientsController : ControllerBase
    {
        // 1. GET /api/patients - Get all patients with optional name search bar filtering (Section 3.1)
        [HttpGet]
        public IActionResult GetPatients([FromQuery] string? search)
        {
            var patientsList = InMemoryStorage.Patients.AsEnumerable();

            if (!string.IsNullOrEmpty(search))
            {
                patientsList = patientsList.Where(p => 
                    p.FirstName.Contains(search, System.StringComparison.OrdinalIgnoreCase) || 
                    p.LastName.Contains(search, System.StringComparison.OrdinalIgnoreCase));
            }

            return Ok(new { success = true, data = patientsList.ToList(), message = "Patients retrieved successfully." });
        }

        // 2. POST /api/patients - Add/Create a new patient with validation (Section 3.1 & 4.6)
        [HttpPost]
        public IActionResult CreatePatient([FromBody] Patient newPatient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, data = ModelState, message = "Server-side validation failed." });
            }

            // Auto increment ID simulation
            newPatient.Id = InMemoryStorage.Patients.Count > 0 ? InMemoryStorage.Patients.Max(p => p.Id) + 1 : 1;
            InMemoryStorage.Patients.Add(newPatient);

            return CreatedAtAction(nameof(GetPatients), new { id = newPatient.Id }, new { success = true, data = newPatient, message = "Patient added successfully." });
        }

        // 3. PUT /api/patients/{id} - Update patient records (Section 3.1)
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, [FromBody] Patient updatedPatient)
        {
            var existingPatient = InMemoryStorage.Patients.FirstOrDefault(p => p.Id == id);
            if (existingPatient == null)
            {
                return NotFound(new { success = false, message = "Patient record not found." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, data = ModelState, message = "Validation failed." });
            }

            existingPatient.FirstName = updatedPatient.FirstName;
            existingPatient.LastName = updatedPatient.LastName;
            existingPatient.Age = updatedPatient.Age;
            existingPatient.Gender = updatedPatient.Gender;
            existingPatient.PhoneNumber = updatedPatient.PhoneNumber;
            existingPatient.Email = updatedPatient.Email;
            existingPatient.Address = updatedPatient.Address;

            return Ok(new { success = true, data = existingPatient, message = "Patient records updated successfully." });
        }
    }
}