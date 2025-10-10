using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.HealthcareFacilities.Commands.CreateHealthcareFacility;

using AppointmentApplication.Application.Features.HealthcareFacilities.Dtos;
using AppointmentApplication.Application.Features.HealthcareFacilities.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.HealthcareFacilities.Queries.GetHealthCareFacilityByUserId
{
    public sealed class GetHealthCareFacilityByUserIdQueryHandler : IRequestHandler<GetHealthCareFacilityByUserIdQuery, Result<HealthcareFacilityWithUserDto>>
    {
        private readonly IAppDbContext _context;
        private readonly IUserContext _userContext; // للحصول على الـ UserId الحالي

        public GetHealthCareFacilityByUserIdQueryHandler(IAppDbContext context, IUserContext userContext)
        {
            _context = context;
            _userContext = userContext;
        }

        public async Task<Result<HealthcareFacilityWithUserDto>> Handle(GetHealthCareFacilityByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _userContext.UserId;
            var patient = await _context.Patients.ToListAsync(cancellationToken);
            Console.WriteLine("Patinet countes ======++>>" + patient.Count);
            
            var healthCareFacility = await _context.HealthcareFacilities
                .AsNoTracking() // لا حاجة لتتبع التغييرات هنا
                .Where(u => u.UserId == userId)
                .Include(u => u.User)
                .FirstOrDefaultAsync(cancellationToken);
            if (healthCareFacility == null)
            {
                return ApplicationHealthCareFacilityErrors.FacilityNotFound(request.UserId);
            }

            var dto = healthCareFacility.ToDtoWithUser();
            return dto;
        }
    }
}