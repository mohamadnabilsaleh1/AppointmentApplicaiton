using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Abstractions;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.HealthcareFacilities;
    public sealed class Address 
    {
        public string Street { get; }
        public string City { get; }
        public string State { get; }
        public string Country { get; }
        public string ZipCode { get; }

        private Address(string street, string city, string state, string country, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipCode;
        }

        public static Result<Address> Create(
            string street, string city, string state, string country, string zipCode)
        {
            if (string.IsNullOrWhiteSpace(street))
        {
            return AddressErrors.StreetRequired;
        }

        if (string.IsNullOrWhiteSpace(city))
        {
            return AddressErrors.CityRequired;
        }

        if (string.IsNullOrWhiteSpace(state))
        {
            return AddressErrors.StateRequired;
        }

        if (string.IsNullOrWhiteSpace(country))
        {
            return AddressErrors.CountryRequired;
        }

        if (string.IsNullOrWhiteSpace(zipCode))
        {
            return AddressErrors.ZipCodeRequired;
        }

        if (street.Length > 200 || city.Length > 100 || state.Length > 100 || country.Length > 100 || zipCode.Length > 20)
        {
            return AddressErrors.InvalidLength;
        }

        return new Address(street.Trim(), city.Trim(), state.Trim(), country.Trim(), zipCode.Trim());
        }

    }

    public static class AddressErrors
    {
        public static readonly Error StreetRequired = Error.Validation("Address.Street.Required", "Street is required.");
        public static readonly Error CityRequired = Error.Validation("Address.City.Required", "City is required.");
        public static readonly Error StateRequired = Error.Validation("Address.State.Required", "State is required.");
        public static readonly Error CountryRequired = Error.Validation("Address.Country.Required", "Country is required.");
        public static readonly Error ZipCodeRequired = Error.Validation("Address.ZipCode.Required", "Zip code is required.");
        public static readonly Error InvalidLength = Error.Validation("Address.Length.Invalid", "One or more address fields exceed the maximum allowed length.");
    }
