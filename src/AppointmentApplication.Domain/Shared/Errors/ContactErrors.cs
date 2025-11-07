using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Shared.Errors
{
    public static class ContactErrors
    {
        // Email Errors
        public static readonly Error EmptyEmailAddress =
            Error.Validation("Contact.EmptyEmailAddress", "Email address cannot be empty.");

        public static readonly Error InvalidEmailFormat =
            Error.Validation("Contact.InvalidEmailFormat", "Email address format is invalid.");

        public static readonly Error EmailAlreadyExists =
            Error.Conflict("Contact.EmailAlreadyExists", "Email address already exists for this owner.");

        public static readonly Error EmailTooLong =
            Error.Validation("Contact.EmailTooLong", "Email address cannot exceed 254 characters.");

        // Phone Errors
        public static readonly Error EmptyPhoneNumber =
            Error.Validation("Contact.EmptyPhoneNumber", "Phone number cannot be empty.");

        public static readonly Error InvalidPhoneFormat =
            Error.Validation("Contact.InvalidPhoneFormat", "Phone number format is invalid.");

        public static readonly Error PhoneAlreadyExists =
            Error.Conflict("Contact.PhoneAlreadyExists", "Phone number already exists for this owner.");

        public static readonly Error PhoneTooLong =
            Error.Validation("Contact.PhoneTooLong", "Phone number cannot exceed 20 characters.");

        // Common Errors
        public static readonly Error EmptyLabel =
            Error.Validation("Contact.EmptyLabel", "Label cannot be empty.");

        public static readonly Error LabelTooLong =
            Error.Validation("Contact.LabelTooLong", "Label cannot exceed 50 characters.");

        public static readonly Error InvalidOwnerType =
            Error.Validation("Contact.InvalidOwnerType", "Owner type is invalid.");

        public static readonly Error OwnerIdEmpty =
            Error.Validation("Contact.OwnerIdEmpty", "Owner ID cannot be empty.");

        // Not Found Errors
        public static Error OwnerNotFound(OwnerType ownerType, Guid ownerId) =>
            Error.NotFound("Contact.OwnerNotFound",
                $"{ownerType} with ID {ownerId} was not found.");

        public static Error EmailNotFound(Guid emailId) =>
            Error.NotFound("Contact.EmailNotFound",
                $"Email with ID {emailId} was not found.");

        public static Error PhoneNotFound(Guid phoneId) =>
            Error.NotFound("Contact.PhoneNotFound",
                $"Phone with ID {phoneId} was not found.");

        public static Error ContactNotFound(Guid contactId) =>
            Error.NotFound("Contact.NotFound",
                $"Contact with ID {contactId} was not found.");

        // Authorization Errors
        public static readonly Error UnauthorizedAccess =
            Error.Unauthorized("Contact.Unauthorized",
                "You are not authorized to access these contacts.");

        // Business Rule Errors
        public static readonly Error CannotDeletePrimaryContact =
            Error.Conflict("Contact.CannotDeletePrimary",
                "Cannot delete primary contact.");

        public static readonly Error DuplicateContact =
            Error.Conflict("Contact.Duplicate",
                "Contact with the same details already exists.");

        // Validation Errors for Specific Owners
        public static Error PatientNotFound(Guid patientId) =>
            Error.NotFound("Contact.PatientNotFound",
                $"Patient with ID {patientId} was not found.");

        public static Error DoctorNotFound(Guid doctorId) =>
            Error.NotFound("Contact.DoctorNotFound",
                $"Doctor with ID {doctorId} was not found.");

        public static Error FacilityNotFound(Guid facilityId) =>
            Error.NotFound("Contact.FacilityNotFound",
                $"Facility with ID {facilityId} was not found.");

        public static Error UserNotFound(Guid userId) =>
            Error.NotFound("Contact.UserNotFound",
                $"User with ID {userId} was not found.");

        // Maximum Limits
        public static readonly Error MaximumEmailsReached =
            Error.Validation("Contact.MaximumEmails",
                "Maximum number of emails reached for this owner.");

        public static readonly Error MaximumPhonesReached =
            Error.Validation("Contact.MaximumPhones",
                "Maximum number of phones reached for this owner.");

        // Update Errors
        public static readonly Error CannotUpdateOtherOwnerContact =
            Error.Unauthorized("Contact.CannotUpdateOtherOwner",
                "Cannot update contact that belongs to another owner.");

        // System Errors
        public static readonly Error ContactCreationFailed =
            Error.Failure("Contact.CreationFailed",
                "Failed to create contact. Please try again.");

        public static readonly Error ContactUpdateFailed =
            Error.Failure("Contact.UpdateFailed",
                "Failed to update contact. Please try again.");

        public static readonly Error ContactDeletionFailed =
            Error.Failure("Contact.DeletionFailed",
                "Failed to delete contact. Please try again.");
    }
}