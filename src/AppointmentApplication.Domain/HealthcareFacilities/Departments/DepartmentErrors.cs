using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities.Departments;

    public static class DepartmentErrors
    {
        public static readonly Error FacilityIdRequired = 
            Error.Validation("Department.FacilityId.Required", "Facility ID is required.");

        public static readonly Error NameRequired = 
            Error.Validation("Department.Name.Required", "Name is required.");

        public static readonly Error NameTooLong = 
            Error.Validation("Department.Name.TooLong", "Name cannot exceed 200 characters.");

        public static readonly Error DescriptionTooLong = 
            Error.Validation("Department.Description.TooLong", "Description cannot exceed 1000 characters.");

        public static readonly Error DoctorDepartmentRequired = 
            Error.Validation("Department.DoctorDepartment.Required", "Doctor department is required.");

        public static readonly Error AlreadyInactive = 
            Error.Validation("Department.AlreadyInactive", "Department is already inactive.");

        public static readonly Error AlreadyActive = 
            Error.Validation("Department.AlreadyActive", "Department is already active.");
    }