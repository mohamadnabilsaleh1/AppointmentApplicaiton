// AppointmentApplication.Api/Controllers/PatientMedicalRecordsController.cs
using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API;
using AppointmentApplication.API.Controllers;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.MedicalRecords.Dtos;
using AppointmentApplication.Application.Features.MedicalRecords.Queries.GetAllMedicalRecords;
using AppointmentApplication.Domain.Users;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.Api.Controllers
{
    [Route("api/patients/me/medical-records")]
    public class PatientMedicalRecordsController : ApiController
    {
        private readonly ISender _sender;
        private readonly IUserContext _userContext;

        public PatientMedicalRecordsController(ISender sender, IUserContext userContext)
        {
            _sender = sender;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Patient)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get all patient's medical records")]
        [EndpointDescription("Retrieves all medical records for the currently authenticated patient without pagination.")]
        public async Task<IActionResult> GetAllMyMedicalRecords(CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetMedicalRecordForPaitnetByUserIdQuery(_userContext.UserId),
                cancellationToken);

            return result.Match(
                medicalRecords => Ok(new { data = medicalRecords }),
                Problem);
        }
    }
}