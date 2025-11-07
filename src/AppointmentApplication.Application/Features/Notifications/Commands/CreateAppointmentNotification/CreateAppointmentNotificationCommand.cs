using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Shared.Results;

using MediatR;

namespace AppointmentApplication.Application.Features.Notifications.Commands.CreateAppointmentCommand
{
    public class CreateAppointmentNotificationCommand : IRequest<Result<Success>>
    {
        public Guid UserId { get; set; } // Doctor's UserId
        public string Title { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public Guid? RelatedEntityId { get; set; } // AppointmentId
        public string RelatedEntityType { get; set; } // "Appointment"

        public CreateAppointmentNotificationCommand(Guid userId, string title, string message, string type, Guid? relatedEntityId = null)
        {
            UserId = userId;
            Title = title;
            Message = message;
            Type = type;
            RelatedEntityId = relatedEntityId;
            RelatedEntityType = "Appointment";
        }
    }
}