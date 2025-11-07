using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AppointmentApplication.Domain.Abstractions;

using AppointmentApplication.Domain.Shared.Results;

namespace AppointmentApplication.Domain.Notifications
{
    public class Notification : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string Title { get; private set; }
        public string Message { get; private set; }
        public string Type { get; private set; } // "APPOINTMENT_CREATED", "APPOINTMENT_CANCELLED", etc.
        public bool IsRead { get; private set; }
        public DateTime? ReadAt { get; private set; }
        public Guid? RelatedEntityId { get; private set; }
        public string RelatedEntityType { get; private set; }

        private Notification() { }

        private Notification(Guid id, Guid userId, string title, string message, string type, Guid? relatedEntityId, string relatedEntityType)
            : base(id)
        {
            UserId = userId;
            Title = title;
            Message = message;
            Type = type;
            IsRead = false;
            RelatedEntityId = relatedEntityId;
            RelatedEntityType = relatedEntityType;
        }

        public static Result<Notification> Create(
            Guid userId,
            string title,
            string message,
            string type,
            Guid? relatedEntityId = null,
            string relatedEntityType = null)
        {
            var notification = new Notification(
                Guid.NewGuid(),
                userId,
                title.Trim(),
                message.Trim(),
                type.Trim(),
                relatedEntityId,
                relatedEntityType?.Trim());

            return notification;
        }

        public void MarkAsRead()
        {
            if (!IsRead)
            {
                IsRead = true;
                ReadAt = DateTime.UtcNow;
            }
        }

        public void MarkAsUnread()
        {
            if (IsRead)
            {
                IsRead = false;
                ReadAt = null;
            }
        }
    }
}