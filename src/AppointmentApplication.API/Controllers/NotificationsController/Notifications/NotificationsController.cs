using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.OutputCaching;
using AppointmentApplication.Application.Features.Notifications.Commands.DeleteNotification;
using AppointmentApplication.Application.Features.Notifications.Commands.MarkAllNotificationssAsRead;
using AppointmentApplication.Application.Features.Notifications.Commands.MarkNotificationAsRead;
using AppointmentApplication.Application.Features.Notifications.Queries.GetUnreadNotificationsCount;
using AppointmentApplication.Application.Features.Notifications.Queries.GetUserNotifications;
using AppointmentApplication.API.Services;
using AppointmentApplication.Application.Abstractions.Authentication;
using MediatR;

namespace AppointmentApplication.API.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    [Authorize]
    public class NotificationsController : ApiController
    {
        private readonly ISender _sender;
        private readonly LinkService _linkService;
        private readonly IUserContext _userContext;

        public NotificationsController(ISender sender, LinkService linkService, IUserContext userContext)
        {
            _sender = sender;
            _linkService = linkService;
            _userContext = userContext;
        }

        [HttpGet]
        [MapToApiVersion("1.0")]
        [EndpointSummary("Get user notifications")]
        [EndpointDescription("Retrieves notifications for the current user with pagination and filtering")]
        [EndpointName("GetNotifications")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotifications(
            [FromQuery] bool? unreadOnly = false,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var userId = _userContext.UserId;
            var query = new GetUserNotificationsQuery(userId, unreadOnly, page, pageSize);
            var result = await _sender.Send(query, cancellationToken);

            return result.Match<IActionResult>(
                success => Ok(new
                {
                    data = success.Items,
                    pagination = new
                    {
                        success.Page,
                        success.PageSize,
                        success.TotalCount,
                        success.TotalPages,
                        success.HasPreviousPage,
                        success.HasNextPage
                    }
                }),
                Problem // ✅ استخدام Problem بدلاً من BadRequest
            );
        }

        [HttpGet("unread/count")]
        [MapToApiVersion("1.0")]
        [EndpointSummary("Get unread notifications count")]
        [EndpointDescription("Retrieves the count of unread notifications for the current user")]
        [EndpointName("GetUnreadCount")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnreadCount(CancellationToken cancellationToken = default)
        {
            var userId = _userContext.UserId;
            var query = new GetUnreadNotificationsCountQuery(userId);
            var result = await _sender.Send(query, cancellationToken);

            return result.Match<IActionResult>(
                success => Ok(new { data = new { count = success } }),
                Problem // ✅ استخدام Problem بدلاً من BadRequest
            );
        }

        [HttpPut("{id:guid}/read")]
        [MapToApiVersion("1.0")]
        [EndpointSummary("Mark notification as read")]
        [EndpointDescription("Marks a specific notification as read for the current user")]
        [EndpointName("MarkNotificationAsRead")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAsRead(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var userId = _userContext.UserId;
            var command = new MarkNotificationAsReadCommand(id, userId);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                success => NoContent(),
                Problem // ✅ استخدام Problem بدلاً من BadRequest
            );
        }

        [HttpPut("read-all")]
        [MapToApiVersion("1.0")]
        [EndpointSummary("Mark all notifications as read")]
        [EndpointDescription("Marks all notifications as read for the current user")]
        [EndpointName("MarkAllNotificationsAsRead")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAllAsRead(CancellationToken cancellationToken = default)
        {
            var userId = _userContext.UserId;
            var command = new MarkAllNotificationsAsReadCommand(userId);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                success => NoContent(),
                Problem // ✅ استخدام Problem بدلاً من BadRequest
            );
        }

        [HttpDelete("{id:guid}")]
        [MapToApiVersion("1.0")]
        [EndpointSummary("Delete notification")]
        [EndpointDescription("Deletes a specific notification for the current user")]
        [EndpointName("DeleteNotification")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteNotification(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var userId = _userContext.UserId;
            var command = new DeleteNotificationCommand(id, userId);
            var result = await _sender.Send(command, cancellationToken);

            return result.Match<IActionResult>(
                success => NoContent(),
                Problem // ✅ استخدام Problem بدلاً من BadRequest
            );
        }

        [HttpGet("types")]
        [MapToApiVersion("1.0")]
        [EndpointSummary("Get notification types")]
        [EndpointDescription("Retrieves all available notification types")]
        [EndpointName("GetNotificationTypes")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [OutputCache(Duration = 3600)]
        public IActionResult GetNotificationTypes()
        {
            var types = new[]
            {
                new { value = "APPOINTMENT_CREATED", label = "Appointment Created" },
                new { value = "APPOINTMENT_CONFIRMED", label = "Appointment Confirmed" },
                new { value = "APPOINTMENT_CANCELLED", label = "Appointment Cancelled" },
                new { value = "APPOINTMENT_COMPLETED", label = "Appointment Completed" },
                new { value = "BILLING_CREATED", label = "Billing Created" },
                new { value = "BILLING_PAID", label = "Billing Paid" },
                new { value = "REMINDER", label = "Reminder" },
                new { value = "SYSTEM", label = "System Notification" }
            };

            return Ok(new { data = types });
        }

    }
}