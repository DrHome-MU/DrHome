using Dr_Home.DTOs.AppointmentDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

namespace Dr_Home.Controllers
{
    [Route("api/")]
    [ApiController]
    public class AppointmentsController(IAppointmentHelper appointmentHelepr , ILogger<AppointmentsController>logger) : ControllerBase
    {
        private readonly IAppointmentHelper _appointmentHelepr = appointmentHelepr;
        private readonly ILogger<AppointmentsController> _logger = logger;
        /// <summary>
        /// Get Appointment By Id
        /// </summary>
        /// <param name="AppointmentId"></param>
        /// <returns></returns>
        /// <response code ="200">Appointment Loaded Successfully</response>
        /// <response code ="404">Appointment Doesn`t Exist</response>
        [HttpGet("Schedules/{ScheduleId}/Appointments/{AppointmentId}")]
        public async Task<IActionResult> GetAppointment([FromRoute] Guid AppointmentId)
        {
            var result = await _appointmentHelepr.GetAppointmentAsync(AppointmentId);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Get All The Appointemnt Of Specific Doctor
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200">هيرجعلك داتا ممكن يكون في داتا او داتا فاضية عادي</response>
        /// <response code = "404">there is no doctor with this id</response>

        [HttpGet("Doctors/{DoctorId}/Appointments")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(IEnumerable <GetDoctorAppointments>),StatusCodes.Status200OK)]
        public async Task<IActionResult> GetDoctorAppointments([FromRoute] Guid DoctorId , CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.GetDoctorAppointmentsAsync(DoctorId,cancellationToken);
            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Get All Appointments To Specific Patient
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200">هيرجعلك الداتا عادي لو موجودة ولو مش موجودة هترجعلك فاضية</response>
        /// <response code = "404">Patient Is Not Found </response>
        [HttpGet("Patients/{PatientId}/Appointments")]
        [Authorize(Roles = "Patient")]
        [ProducesResponseType(typeof(IEnumerable<GetPatientAppointmentsResponse>) , StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPatientAppointments([FromRoute] Guid PatientId , CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.GetPatientAppointmentsAsync (PatientId,cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
        }
        /// <summary>
        /// Book Appointment With Doctor By Patient
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <param name="request">
        /// <br/>
        /// -<b>PatientId</b>:The Id Of The Patient Who Want To Book<br/>
        /// <br/>
        /// -<b>PatientName</b>between 2 and 100 characters
        /// <br/>
        /// -<b>PhoneNumber</b>11 Number and starts with 01 and contains only characters from 0 to 9
        /// <br/>
        /// -<b>AppointmentTime</b> between 00:00 and 23:59
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "200">Added Successfully</response>
        /// <response code = "409">This Time Is Already Booked By Someone else</response>
        /// <response code = "404">Clinic Or Schedule Or Doctor Doesn`t Exist</response>

        [HttpPost("Schedules/{ScheduleId}/Appointments")]

        [Authorize(Roles = "Patient")]
        

        public async Task<IActionResult> BookAppointments([FromRoute] Guid ScheduleId , AppointmentRequest request, CancellationToken cancellationToken)
        {
           // _logger.LogInformation("Hello from Booking");
            
            var result = await _appointmentHelepr.BookAppointmentAsync(ScheduleId, request, cancellationToken);

            return result.IsSuccess? Ok(result.Value) : result.ToProblem();

        }
        /// <summary>
        /// Update  Appointment By Patient Or Doctor
        /// </summary>
        /// <param name="ScheduleId"></param>
        /// <param name="AppointmentId"></param>
        /// <param name="request">
        /// <br/>
        /// -<b>The Same Validation as Book Appointment</b>
        /// </param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "204">Updated Successfully</response>
        /// <response code = "409">Conflicts With Existed Appointment</response>
        /// <response code = "404"> Appointment Doesn`t Exist</response>
        [HttpPut("Schedules/{ScheduleId}/Appointments/{AppointmentId}")]
        [Authorize]

        public async Task<IActionResult> UpdateAppointment([FromRoute] Guid ScheduleId , [FromRoute] Guid AppointmentId ,
            AppointmentRequest request , CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.UpdateAppointmentAsync(AppointmentId, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        /// <summary>
        /// Update IsActive(Delete Appointment) By Patient Or Doctor
        /// </summary>
        /// <param name="AppintmentId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "204">Updated Successfully</response>
        /// <response code = "404">Appointment doesn`t exist</response>
        [HttpPut("Appointments/{AppintmentId}/toggleActive")]
        [Authorize(Roles = "Doctor,Patient")]
        public async Task<IActionResult> toggleActive([FromRoute] Guid AppintmentId , CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.toggleActiveAsync(AppintmentId);  

            return result.IsSuccess ? NoContent () : result.ToProblem();
        }
        /// <summary>
        /// Mark Appointment as Done By Doctor
        /// </summary>
        /// <param name="AppintmentId"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="request">
        /// <br/>
        /// IsDone : The Appointment Is Done Or Not (true or false) <br/>
        /// AppointmentDetails:can be empty string not null ,if appointment is not null send it empty, otherwise can be empty or doctor enter value
        /// </param>
        /// <returns></returns>
        /// <response code = "204">Updated Successfully</response>
        /// <response code = "404">Appointment doesn`t exist</response>
        [HttpPut("Appointments/{AppintmentId}/toggleDone")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> toggleDone([FromRoute] Guid AppintmentId, [FromBody] AppointmentDoneRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.toggleDoneAsync(AppintmentId, request, cancellationToken);

            return result.IsSuccess ? NoContent() : result.ToProblem();
        }
        /// <summary>
        /// Update Appointment Details By Doctor Who add it
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="AppointmentId"></param>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code = "404">Appointment Is Not Found</response>
        /// <response code = "401">Unauthorized Update By This Doctor</response>
        /// <response code = "400">cannot update because appointment is not done</response>
        [HttpPut("Doctors/{DoctorId}/AppointmentsDetails/{AppointmentId}")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateAppointmentDetailsByDoctor([FromRoute] Guid DoctorId, [FromRoute] Guid AppointmentId,
            UpdateAppointmentDetailsRequest request, CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.UpdateAppointmentDetails(DoctorId, AppointmentId, request, cancellationToken);
            return result.IsSuccess ? NoContent() : result.ToProblem();
        }

        /// <summary>
        /// Ask if this patient has at least one completed appointment With This Doctor
        /// </summary>
        /// <param name="PatientId"></param>
        /// <param name="DoctorId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <response code="200"> Check IsDone Value</response>
        /// <response code="400">if You missed doctor or patient id</response>
        [HttpGet("Appointments/CompletedAppointment")]
        [Authorize]
        [ProducesResponseType(typeof(AppointmentIsDoneResponse), 200)]
        public async Task<IActionResult>CompletedAppointment([FromQuery] Guid PatientId , [FromQuery] Guid DoctorId , CancellationToken cancellationToken)
        {
            var result = await _appointmentHelepr.AppointmentIsDoneAsync(PatientId , DoctorId , cancellationToken);
            return Ok(result.Value);
        }
        /// <summary>
        /// this will be in doctor profile to show  his appointment details 
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <returns></returns>

        [HttpGet("Doctors/{DoctorId}/AppointmentsDetails")]
        [Authorize(Roles = "Doctor")]
        [ProducesResponseType(typeof(IEnumerable<DoctorAppointmentsDetailsResponse>), 200)]
        public async Task<IActionResult> GetDoctorAppointmentsDetails([FromRoute] Guid DoctorId)
        {
            var result = await _appointmentHelepr.GetDoctorAppointmentsDetailsAsync(DoctorId);  
            return Ok(result.Value);
        }
        /// <summary>
        /// this return the medical history of the patient on the platform
        /// </summary>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        [HttpGet("Patients/{PatientId}/AppointmentsDetails")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<PatientAppointmentsDetailsResponse>) , 200)]
        public async Task<IActionResult> GetPatientAppointmentsDetails([FromRoute] Guid PatientId)
        {
            var result = await _appointmentHelepr.GetPatientAppointmentsDetailsAsync(PatientId);
            return Ok(result.Value);
        }
    }
}
