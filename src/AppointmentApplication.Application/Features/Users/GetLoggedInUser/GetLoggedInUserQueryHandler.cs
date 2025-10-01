using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Application.Abstractions.Authentication;

using AppointmentApplication.Application.Features.Users.Dtos;
using AppointmentApplication.Application.Features.Users.Mappers;
using AppointmentApplication.Application.Shared.Interfaces;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

using Microsoft.EntityFrameworkCore;

namespace AppointmentApplication.Application.Features.Users.GetLoggedInUser;

public class GetLoggedInUserQueryHandler : IRequestHandler<GetLoggedInUserQuery, Result<UserDto>>
{
    private readonly IAppDbContext _context;
    private readonly IUserContext _userContext; // للحصول على الـ UserId الحالي

    public GetLoggedInUserQueryHandler(IAppDbContext context, IUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<Result<UserDto>> Handle(GetLoggedInUserQuery request, CancellationToken cancellationToken)
    {
        // الحصول على الـ UserId من السياق
        var userId = _userContext.UserId;
        // جلب بيانات المستخدم من قاعدة البيانات
        var user = await _context.Users
            .AsNoTracking() // لا حاجة لتتبع التغييرات هنا
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);
        var dto = user.ToDto();

        return dto;
    }
}