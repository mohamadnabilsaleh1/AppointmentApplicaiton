using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Patients.Allergies.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Patients.Allergies
{
    public sealed class Allergy
    {
        public Guid Id { get; init; }
        public AllergyType Name { get; private set; }

        [JsonIgnore]
        public ICollection<Patient> Patients { get; private set; } = new List<Patient>();

        // ✅ EF Core يحتاج هذا
        private Allergy() { }

        // ✅ Constructor خاص لإنشاء الحساسية مع Id ثابت
        private Allergy(Guid id, AllergyType name)
        {
            Id = id;
            Name = name;
        }
public static Result<Allergy> Create(AllergyType allergyType)
{
    if (!Enum.IsDefined(typeof(AllergyType), allergyType))
        return PatientErrors.InvalidAllergyType;

    var allergy = new Allergy(Guid.NewGuid(), allergyType);
    return allergy; // ✅ يرجع كائن فعلي وليس null
}


        // ✅ Static predefined instances لكل نوع حساسية
        public static readonly Allergy None = new(Guid.Parse("00000000-0000-0000-0000-000000000001"), AllergyType.None);
        public static readonly Allergy Penicillin = new(Guid.Parse("00000000-0000-0000-0000-000000000002"), AllergyType.Penicillin);
        public static readonly Allergy Amoxicillin = new(Guid.Parse("00000000-0000-0000-0000-000000000003"), AllergyType.Amoxicillin);
        public static readonly Allergy SulfaDrugs = new(Guid.Parse("00000000-0000-0000-0000-000000000004"), AllergyType.SulfaDrugs);
        public static readonly Allergy NSAIDs = new(Guid.Parse("00000000-0000-0000-0000-000000000005"), AllergyType.NSAIDs);
        public static readonly Allergy Aspirin = new(Guid.Parse("00000000-0000-0000-0000-000000000006"), AllergyType.Aspirin);
        public static readonly Allergy Codeine = new(Guid.Parse("00000000-0000-0000-0000-000000000007"), AllergyType.Codeine);
        public static readonly Allergy Morphine = new(Guid.Parse("00000000-0000-0000-0000-000000000008"), AllergyType.Morphine);
        public static readonly Allergy Latex = new(Guid.Parse("00000000-0000-0000-0000-000000000009"), AllergyType.Latex);
        public static readonly Allergy Peanuts = new(Guid.Parse("00000000-0000-0000-0000-000000000010"), AllergyType.Peanuts);
        public static readonly Allergy TreeNuts = new(Guid.Parse("00000000-0000-0000-0000-000000000011"), AllergyType.TreeNuts);
        public static readonly Allergy Shellfish = new(Guid.Parse("00000000-0000-0000-0000-000000000012"), AllergyType.Shellfish);
        public static readonly Allergy Fish = new(Guid.Parse("00000000-0000-0000-0000-000000000013"), AllergyType.Fish);
        public static readonly Allergy Eggs = new(Guid.Parse("00000000-0000-0000-0000-000000000014"), AllergyType.Eggs);
        public static readonly Allergy Milk = new(Guid.Parse("00000000-0000-0000-0000-000000000015"), AllergyType.Milk);
        public static readonly Allergy Soy = new(Guid.Parse("00000000-0000-0000-0000-000000000016"), AllergyType.Soy);
        public static readonly Allergy Wheat = new(Guid.Parse("00000000-0000-0000-0000-000000000017"), AllergyType.Wheat);
        public static readonly Allergy Pollen = new(Guid.Parse("00000000-0000-0000-0000-000000000018"), AllergyType.Pollen);
        public static readonly Allergy DustMites = new(Guid.Parse("00000000-0000-0000-0000-000000000019"), AllergyType.DustMites);
        public static readonly Allergy Mold = new(Guid.Parse("00000000-0000-0000-0000-000000000020"), AllergyType.Mold);
        public static readonly Allergy PetDander = new(Guid.Parse("00000000-0000-0000-0000-000000000021"), AllergyType.PetDander);
        public static readonly Allergy BeeStings = new(Guid.Parse("00000000-0000-0000-0000-000000000022"), AllergyType.BeeStings);
        public static readonly Allergy InsectStings = new(Guid.Parse("00000000-0000-0000-0000-000000000023"), AllergyType.InsectStings);
        public static readonly Allergy Other = new(Guid.Parse("00000000-0000-0000-0000-000000000024"), AllergyType.Other);

        // ✅ List of all for easy seeding or looping
        public static IEnumerable<Allergy> GetAll() => new[]
        {
            None,
            Penicillin,
            Amoxicillin,
            SulfaDrugs,
            NSAIDs,
            Aspirin,
            Codeine,
            Morphine,
            Latex,
            Peanuts,
            TreeNuts,
            Shellfish,
            Fish,
            Eggs,
            Milk,
            Soy,
            Wheat,
            Pollen,
            DustMites,
            Mold,
            PetDander,
            BeeStings,
            InsectStings,
            Other
        };
    }
}
