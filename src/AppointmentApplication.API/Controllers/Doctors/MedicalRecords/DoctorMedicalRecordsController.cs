// AppointmentApplication.Api/Controllers/DoctorMedicalRecordsController.cs
using System;
using System.Threading;
using System.Threading.Tasks;

using AppointmentApplication.API;
using AppointmentApplication.API.Controllers;
using AppointmentApplication.Application.Abstractions.Authentication;
using AppointmentApplication.Application.Features.MedicalRecords.Dtos;
using AppointmentApplication.Application.Features.MedicalRecords.Queries.GetMedicalRecordsForDoctorByPatientId;
using AppointmentApplication.Domain.Users;
using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentApplication.Api.Controllers
{
    [Route("api/doctors/me/patients/{patientId:guid}/medical-records")]
    public class DoctorMedicalRecordsController : ApiController
    {
        private readonly ISender _sender;
        private readonly IUserContext _userContext;

        public DoctorMedicalRecordsController(ISender sender, IUserContext userContext)
        {
            _sender = sender;
            _userContext = userContext;
        }

        [HttpGet]
        [Authorize(Roles = Roles.Doctor)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [MapToApiVersion("0.1")]
        [EndpointSummary("Get patient's medical records by doctor")]
        [EndpointDescription("Retrieves all medical records for a specific patient that were created by the current doctor.")]
        public async Task<IActionResult> GetMedicalRecordsForPatient(
            Guid patientId,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(
                new GetMedicalRecordsForDoctorByPatientIdQuery(
                    UserId: _userContext.UserId,
                    PatientId: patientId),
                cancellationToken);

            return result.Match(
                medicalRecords => Ok(new { data = medicalRecords }),
                Problem);
        }
    }
}