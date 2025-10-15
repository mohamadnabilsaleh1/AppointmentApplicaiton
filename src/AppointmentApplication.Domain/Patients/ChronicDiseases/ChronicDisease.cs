using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients.ChronicDiseases.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Patients.ChronicDiseases
{
    public sealed class ChronicDisease
    {
        public Guid Id { get; init; }

        public ChronicDiseaseType Name { get; private set; }

        [JsonIgnore]
        public ICollection<Patient> Patients { get; private set; } = new List<Patient>();

        // ✅ Required by EF Core
        private ChronicDisease() { }

        // ✅ Main constructor
        private ChronicDisease(Guid id, ChronicDiseaseType name)
        {
            Id = id;
            Name = name;
        }
        public static Result<ChronicDisease> Create(ChronicDiseaseType chronicDiseaseType)
        {
            return new ChronicDisease(Guid.NewGuid(), chronicDiseaseType);
        }

        // ✅ Static predefined instances (with fixed Guids for seeding)
        public static readonly ChronicDisease None = new(Guid.Parse("00000000-0000-0000-0000-000000000001"), ChronicDiseaseType.None);
        public static readonly ChronicDisease Diabetes = new(Guid.Parse("00000000-0000-0000-0000-000000000002"), ChronicDiseaseType.Diabetes);
        public static readonly ChronicDisease Hypertension = new(Guid.Parse("00000000-0000-0000-0000-000000000003"), ChronicDiseaseType.Hypertension);
        public static readonly ChronicDisease Asthma = new(Guid.Parse("00000000-0000-0000-0000-000000000004"), ChronicDiseaseType.Asthma);
        public static readonly ChronicDisease HeartDisease = new(Guid.Parse("00000000-0000-0000-0000-000000000005"), ChronicDiseaseType.HeartDisease);
        public static readonly ChronicDisease ChronicKidneyDisease = new(Guid.Parse("00000000-0000-0000-0000-000000000006"), ChronicDiseaseType.ChronicKidneyDisease);
        public static readonly ChronicDisease ChronicLiverDisease = new(Guid.Parse("00000000-0000-0000-0000-000000000007"), ChronicDiseaseType.ChronicLiverDisease);
        public static readonly ChronicDisease Epilepsy = new(Guid.Parse("00000000-0000-0000-0000-000000000008"), ChronicDiseaseType.Epilepsy);
        public static readonly ChronicDisease COPD = new(Guid.Parse("00000000-0000-0000-0000-000000000009"), ChronicDiseaseType.COPD);
        public static readonly ChronicDisease Arthritis = new(Guid.Parse("00000000-0000-0000-0000-000000000010"), ChronicDiseaseType.Arthritis);
        public static readonly ChronicDisease Cancer = new(Guid.Parse("00000000-0000-0000-0000-000000000011"), ChronicDiseaseType.Cancer);
        public static readonly ChronicDisease Depression = new(Guid.Parse("00000000-0000-0000-0000-000000000012"), ChronicDiseaseType.Depression);
        public static readonly ChronicDisease Anxiety = new(Guid.Parse("00000000-0000-0000-0000-000000000013"), ChronicDiseaseType.Anxiety);
        public static readonly ChronicDisease ThyroidDisorder = new(Guid.Parse("00000000-0000-0000-0000-000000000014"), ChronicDiseaseType.ThyroidDisorder);
        public static readonly ChronicDisease Osteoporosis = new(Guid.Parse("00000000-0000-0000-0000-000000000015"), ChronicDiseaseType.Osteoporosis);
        public static readonly ChronicDisease Alzheimer = new(Guid.Parse("00000000-0000-0000-0000-000000000016"), ChronicDiseaseType.Alzheimer);
        public static readonly ChronicDisease Parkinson = new(Guid.Parse("00000000-0000-0000-0000-000000000017"), ChronicDiseaseType.Parkinson);
        public static readonly ChronicDisease HIV = new(Guid.Parse("00000000-0000-0000-0000-000000000018"), ChronicDiseaseType.HIV);
        public static readonly ChronicDisease Hepatitis = new(Guid.Parse("00000000-0000-0000-0000-000000000019"), ChronicDiseaseType.Hepatitis);
        public static readonly ChronicDisease Stroke = new(Guid.Parse("00000000-0000-0000-0000-000000000020"), ChronicDiseaseType.Stroke);
        public static readonly ChronicDisease Tuberculosis = new(Guid.Parse("00000000-0000-0000-0000-000000000021"), ChronicDiseaseType.Tuberculosis);
        public static readonly ChronicDisease Obesity = new(Guid.Parse("00000000-0000-0000-0000-000000000022"), ChronicDiseaseType.Obesity);
        public static readonly ChronicDisease Other = new(Guid.Parse("00000000-0000-0000-0000-000000000023"), ChronicDiseaseType.Other);

        // ✅ List of all for easy seeding or looping
        public static IEnumerable<ChronicDisease> GetAll() => new[]
        {
            None,
            Diabetes,
            Hypertension,
            Asthma,
            HeartDisease,
            ChronicKidneyDisease,
            ChronicLiverDisease,
            Epilepsy,
            COPD,
            Arthritis,
            Cancer,
            Depression,
            Anxiety,
            ThyroidDisorder,
            Osteoporosis,
            Alzheimer,
            Parkinson,
            HIV,
            Hepatitis,
            Stroke,
            Tuberculosis,
            Obesity,
            Other
        };
        public static ChronicDisease? GetChronicDiseaseByType(ChronicDiseaseType chronicDiseaseType)
        {
            return GetAll().FirstOrDefault(cd => cd.Name == chronicDiseaseType);
        }
    }
}
