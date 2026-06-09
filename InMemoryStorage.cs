using System.Collections.Generic;
using PatientManagementSystem.Models;

namespace PatientManagementSystem
{
    public static class InMemoryStorage
    {
        public static List<Patient> Patients { get; set; } = new List<Patient>
        {
            new Patient 
            { 
                Id = 1, 
                FirstName = "Zain", 
                LastName = "Abideen", 
                Age = 24, 
                Gender = "Male", 
                PhoneNumber = "03001234567", 
                Email = "mzain@gmail.com", 
                Address = "Rawalpindi" 
            }
        };
    }
}