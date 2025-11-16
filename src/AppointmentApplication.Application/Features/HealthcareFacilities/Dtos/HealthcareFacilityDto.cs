using System;
using System.Collections.Generic;

using AppointmentApplication.Application.Features.HealthcareFacilities.Departments.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.ScheduleExceptions.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.Users.Dtos;
using AppointmentApplication.Domain.HealthcareFacilities.Enums;
using AppointmentApplication.Domain.Users;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;

// Record الرئيسي
public record HealthcareFacilityDto(
    Guid Id,
    string Name,
    HealthCareType Type,
    AddressDto Address,
    double GPSLatitude,
    double GPSLongitude,
    string Description,
    IReadOnlyCollection<DepartmentDto> Departments,
    IReadOnlyCollection<ScheduleDto> Schedules,
    IReadOnlyCollection<ScheduleExceptionDto> ScheduleExceptions
)
{
    // حقول خاصة للتخزين الداخلي
    private AddressDto _addressDto;
    private List<DepartmentDto> _departmentDtos;
    private List<ScheduleDto> _scheduleDtos;
    private List<ScheduleExceptionDto> _scheduleExceptionDtos;

    // منشئ ثانوي يستقبل List بدل IReadOnlyCollection
    public HealthcareFacilityDto(
        Guid id,
        string name,
        HealthCareType type,
        AddressDto addressDto,
        double gpsLatitude,
        double gpsLongitude,
        string description,
        List<DepartmentDto> departmentDtos,
        List<ScheduleDto> scheduleDtos,
        List<ScheduleExceptionDto> scheduleExceptionDtos)
        : this(
            id,
            name,
            type,
            addressDto,
            gpsLatitude,
            gpsLongitude,
            description,
            departmentDtos.AsReadOnly(),
            scheduleDtos.AsReadOnly(),
            scheduleExceptionDtos.AsReadOnly()
        )
    {
        // حفظ القوائم الداخلية الخاصة
        _addressDto = addressDto;
        _departmentDtos = departmentDtos;
        _scheduleDtos = scheduleDtos;
        _scheduleExceptionDtos = scheduleExceptionDtos;
    }
}

// Record يحتوي على مستخدم
public record HealthcareFacilityWithUserDto(
    Guid Id,
    string Name,
    HealthCareType Type,
    AddressDto Address,
    double GPSLatitude,
    double GPSLongitude,
    string Email,
    string description,
    IReadOnlyCollection<DepartmentDto> Departments,
    IReadOnlyCollection<ScheduleDto> Schedules,
    IReadOnlyCollection<ScheduleExceptionDto> ScheduleExceptions
);

// Record للعنوان
public record AddressDto(
    string Street,
    string City,
    string Country,
    string ZipCode
)
{
    public string FullAddress => $"{Street}, {City}, {Country} {ZipCode}";
}
