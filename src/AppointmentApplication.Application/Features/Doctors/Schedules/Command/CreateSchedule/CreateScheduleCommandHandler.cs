using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.Application.Features.Doctors.Schedules.Mapper;

using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;

using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Schedules.Mappers;

using AppointmentApplication.Application.HealthcareFacilities.Schedules.Commands;
using AppointmentApplication.Application.Shared.Interfaces;
using AppointmentApplication.Domain.HealthcareFacilities;
using AppointmentApplication.Domain.Shared.Enums;
using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Doctors.Schedules.Commands
{
    public class CreateScheduleCommandHandler : IRequestHandler<CreateScheduleCommand, Result<ScheduleDto>>
    {
        private readonly IAppDbContext _context;

        public CreateScheduleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<Result<ScheduleDto>> Handle(CreateScheduleCommand request, CancellationToken cancellationToken)
        {
            // الحصول على المنشأة مع جداولها
            var doctor = await _context.Doctors
                .Include(f => f.Schedules)
                .FirstOrDefaultAsync(f => f.UserId == request.UserId, cancellationToken);

            if (doctor is null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            // إنشاء الجدول

            var scheduleResult = doctor.AddSchedule(
                doctor.Id,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime,
                request.Status,
                request.Note);

            if (scheduleResult.IsError)
            {
                return scheduleResult.Errors;
            }

            // إضافة وحفظ

            await _context.SaveChangesAsync(cancellationToken);

            return scheduleResult.Value.ToDto();
        }
    }
}