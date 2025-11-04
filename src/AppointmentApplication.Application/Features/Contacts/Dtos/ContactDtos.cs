// AppointmentApplication.Application/Features/Contacts/Dtos/ContactDtos.cs
using System;
using System.Collections.Generic;
using AppointmentApplication.Domain.Shared.Enums;

namespace AppointmentApplication.Application.Features.Contacts.Dtos
{
    // Email DTOs
    public record EmailDto(
        Guid Id,
        string EmailAddress,
        string Label,
        OwnerType OwnerType,
        Guid OwnerId,
        DateTime CreatedAtUtc
    );

    public record CreateEmailRequest(
        string EmailAddress,
        string Label,
        OwnerType OwnerType,
        Guid OwnerId
    );

    public record UpdateEmailRequest(
        string EmailAddress,
        string Label
    );

    // Phone DTOs
    public record PhoneDto(
        Guid Id,
        string PhoneNumber,
        string Label,
        OwnerType OwnerType,
        Guid OwnerId,
        DateTime CreatedAtUtc
    );

    public record CreatePhoneRequest(
        string PhoneNumber,
        string Label,
        OwnerType OwnerType,
        Guid OwnerId
    );

    public record UpdatePhoneRequest(
        string PhoneNumber,
        string Label
    );

    // Combined DTOs for owner
    public record OwnerContactsDto(
        Guid OwnerId,
        OwnerType OwnerType,
        List<EmailDto> Emails,
        List<PhoneDto> Phones
    );
}