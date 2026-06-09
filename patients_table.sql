-- Database CREATE Script for Patients Module (Section 3.1)
CREATE TABLE Patient (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Age INT NOT NULL,
    Gender VARCHAR(10) NOT NULL,
    PhoneNumber VARCHAR(20) NOT NULL,
    Email VARCHAR(100) NULL,
    Address VARCHAR(255) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);