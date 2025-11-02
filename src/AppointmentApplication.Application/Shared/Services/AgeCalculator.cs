// AppointmentApplication.Application/Shared/Utilities/AgeCalculator.cs
using System;

namespace AppointmentApplication.Application.Shared.Utilities
{
    public static class AgeCalculator
    {
        public static int CalculateAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            return CalculateAge(dateOfBirth, today);
        }

        public static int CalculateAge(DateOnly dateOfBirth, DateOnly referenceDate)
        {
            var age = referenceDate.Year - dateOfBirth.Year;
            
            // إذا لم يكن قد مر عيد الميلاد بعد هذا العام، نطرح سنة واحدة
            if (referenceDate.Month < dateOfBirth.Month || 
                (referenceDate.Month == dateOfBirth.Month && referenceDate.Day < dateOfBirth.Day))
            {
                age--;
            }
            
            return age;
        }
    }
}