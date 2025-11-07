using System;

using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Domain.Citizens
{
    public class Citizen
    {
        public Guid Id { get; private set; }  // Must be static for HasData

        public long NationalId { get; private set; }
        public string FirstName { get; private set; }
        public string MiddleName { get; private set; }
        public string LastName { get; private set; }
        public string PhoneNumber { get; private set; }
        public Gender Gender { get; private set; }
        public DateOnly BirthDate { get; set; }

#pragma warning disable CS8618
        private Citizen() { } // For EF Core
#pragma warning restore CS8618

        // Constructor for seeding
        public Citizen(Guid id, long nationalId, string firstName, string middleName, string lastName, string phoneNumber, DateOnly birthDate,Gender gender)
        {
            Id = id;
            NationalId = nationalId;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            BirthDate = birthDate;
            Gender = gender;
        }
    }
}
