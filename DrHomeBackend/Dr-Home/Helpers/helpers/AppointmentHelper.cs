﻿using Dr_Home.Data.Models;
using Dr_Home.DTOs.AppointmentDTOs;
using Mapster;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dr_Home.Helpers.helpers
{
    public class AppointmentHelper(AppDbContext db, IUnitOfWork unitOfWork , ILogger<AppointmentHelper> logger) : IAppointmentHelper
    {
        private readonly AppDbContext _db = db;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ILogger<AppointmentHelper> _logger = logger;

        public async Task<Result<AppointmentResponse>> BookAppointmentAsync(Guid ScheduleId, AppointmentRequest request,
            CancellationToken cancellationToken = default)
        {
            var schedule = await _db.Set<Doctor_Schedule>()
                .FindAsync(ScheduleId, cancellationToken);

            if (schedule == null)
                return Result.Failure<AppointmentResponse>(ScheduleErrors.ScheduleNotFound);

            var clinic = await _db.Set<Clinic>().Include(c => c.doctor).FirstOrDefaultAsync(x => x.Id == schedule.ClinicId,cancellationToken);

            if (clinic == null)
                return Result.Failure<AppointmentResponse>(ClinicErrors.ClinicNotFound);

            if (clinic.doctor == null)
                return Result.Failure<AppointmentResponse>(DoctorErrors.DoctorNotFound);

            var TimeIsBooked = await _db.Set<Appointment>().AnyAsync(x => x.ScheduleId == ScheduleId && x.AppointmentTime == request.AppointmentTime && x.IsActive == true);

            if (TimeIsBooked)
                return Result.Failure<AppointmentResponse>(AppointmentErrors.AppointmentConflict);

            var appointment = request.Adapt<Appointment>();

            appointment.DoctorId = schedule.clinic!.DoctorId;
            appointment.ScheduleId = schedule.Id;
            appointment.AppointmentDate = schedule.WorkDay;

            await _db.Set<Appointment>().AddAsync(appointment, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success(appointment.Adapt<AppointmentResponse>());
        }

        public async Task<Result<AppointmentResponse>> GetAppointmentAsync(Guid AppointmentId, CancellationToken cancellationToken = default)
        {
            var appointment = await _db.Set<Appointment>().FirstOrDefaultAsync(x => x.Id == AppointmentId && x.IsActive == true);

            if (appointment == null)
                return Result.Failure<AppointmentResponse>(AppointmentErrors.AppointmentNotFound);

            return Result.Success(appointment.Adapt<AppointmentResponse>());
        }

        public async Task<Result<IEnumerable<GetDoctorAppointments>>> GetDoctorAppointmentsAsync(Guid DoctorId, CancellationToken cancellationToken = default)
        {
            var doctor = await _db.Set<Doctor>().FindAsync(DoctorId , cancellationToken);

            if(doctor is null)
                return Result.Failure<IEnumerable<GetDoctorAppointments>>(DoctorErrors.DoctorNotFound);


            var results = await _db.Set<Appointment>()
                            .Where(a => a.IsActive && a.DoctorId == DoctorId)
                            .Include(a => a._schedule)
                            .ThenInclude(s => s.clinic)
                            .AsNoTracking()
                            .GroupBy(a => new
                            {
                                clinicId = a._schedule!.ClinicId,
                                clinicName = a._schedule.clinic!.ClinicName
                            })
                            .Select(group => new
                            {
                                group.Key.clinicId,
                                group.Key.clinicName,
                                Appointments = group.ToList()
                            })
                            .ToListAsync(cancellationToken);

            var finalResults = results
                .Select(g => new GetDoctorAppointments
                {
                    ClinicId = g.clinicId,
                    ClinicName = g.clinicName,
                    _appointmentsGroupedBYWorkDays = g.Appointments
                        .GroupBy(a => a._schedule!.WorkDay)
                        .Select(gr => new GetAppointmentsGroupedBYWorkDay
                        {
                            WorkDay = gr.Key,
                            Appointments = gr.Adapt<IEnumerable<AppointmentResponse>>()
                                                .OrderBy(a => a.AppointmentTime)
                        })
                        .OrderBy(x => x.WorkDay)
                        .ToList()
                }).ToList();


            return Result.Success<IEnumerable<GetDoctorAppointments>>(finalResults);
        }
        public async Task<Result<IEnumerable<GetPatientAppointmentsResponse>>> GetPatientAppointmentsAsync(Guid PatientId, CancellationToken cancellationToken = default)
        {
            var patient = await _db.Set<Patient>().FindAsync(PatientId , cancellationToken);

            if (patient is null)
                return Result.Failure < IEnumerable < GetPatientAppointmentsResponse >> (PatientErrors.PatientNotFound);

            var appointments = await _db.Set<Appointment>()
                .Where(x => x.PatientId == PatientId && x.IsActive == true)
                .Include(a => a._doctor)
                .Include(a => a._schedule)
                .ThenInclude(s => s.clinic)
                .ToListAsync();

            var result = new List<GetPatientAppointmentsResponse>();

            foreach(var appointment in appointments)
            {
                var item = new GetPatientAppointmentsResponse
                {
                    AppointmentId = appointment.Id,
                    DoctorName = appointment._doctor!.FullName,
                    ClinicName = appointment._schedule!.clinic!.ClinicName,
                    ClinicCity = appointment._schedule!.clinic!.city,
                    ClinicRegion = appointment._schedule!.clinic!.region,
                    ClinicPhoneNumber = appointment._schedule.clinic!.PhoneNumber,
                    AppointmentDate = appointment._schedule.WorkDay, 
                    AppointmentTime = appointment.AppointmentTime,
                    AppointmentFee = appointment._schedule!.clinic!.AppointmentFee
                };
                result.Add(item);
            }

            return Result.Success<IEnumerable<GetPatientAppointmentsResponse>>(
                 result.OrderBy(a => a.AppointmentDate)
                   .ThenBy(a => a.AppointmentTime)
                );
        }

        public async Task<Result> toggleActiveAsync(Guid AppointmentId, CancellationToken cancellationToken = default)
        {
            var appointment = await _db.Set<Appointment>().FindAsync(AppointmentId, cancellationToken);

            if (appointment == null)
                return Result.Failure(AppointmentErrors.AppointmentNotFound);

            appointment.IsActive = false;

            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();

        }

        public async Task<Result> toggleDoneAsync(Guid AppointmentId,AppointmentDoneRequest request, CancellationToken cancellationToken = default)
        {
            var appointment = await _db.Set<Appointment>().FindAsync(AppointmentId, cancellationToken);

            if (appointment == null)
                return Result.Failure(AppointmentErrors.AppointmentNotFound);

            var patient = await _db.Set<Patient>().FindAsync(appointment.PatientId, cancellationToken);

            if (request.IsDone)
            {
                appointment.IsDone = true; 
                appointment.AppointmentDetails = request.AppointmentDetails;
                appointment.IsActive = false;
                patient!.NumberOfWastedAppointments = 0;
            }
            else
            {
                appointment.IsActive = false;
                int numOfDays = patient!.NumberOfWastedAppointments * 10; 
                patient!.BannedTo = DateTime.UtcNow.AddDays(numOfDays);
                patient!.NumberOfWastedAppointments += 1;
            }

            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result> UpdateAppointmentAsync(Guid AppointmentId, AppointmentRequest request, CancellationToken cancellationToken)
        {
            var appointment = await _db.Set<Appointment>()
                .FirstOrDefaultAsync(a => a.Id == AppointmentId && a.IsActive == true);

            if (appointment == null)
                return Result.Failure(AppointmentErrors.AppointmentNotFound);

            var TimeIsBooked = await _db.Set<Appointment>().AnyAsync(a => a.Id != AppointmentId && a.AppointmentTime == request.AppointmentTime
            && a.IsActive == true);

            if (TimeIsBooked)
                return Result.Failure(AppointmentErrors.AppointmentConflict);

            appointment.PatientId = request.PatientId;
            appointment.AppointmentTime = request.AppointmentTime;
            appointment.PatientName = request.PatientName;
            appointment.PatientPhoneNumber = request.PatientPhoneNumber;
            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }

        public async Task<Result<AppointmentIsDoneResponse>> AppointmentIsDoneAsync([FromQuery] Guid PatientId, [FromQuery] Guid DoctorId, CancellationToken cancellationToken = default)
        {
            var IsThereIsCompletedAppointment = await _db.Set<Appointment>().AnyAsync(a => a.IsDone == true && a.DoctorId == DoctorId
            && a.PatientId == PatientId);

            if (IsThereIsCompletedAppointment)
            {
                return Result.Success(new AppointmentIsDoneResponse
                {
                    message = "Yes , This Patient Completed at Least One Appointment With This Doctor",
                    IsDone = true
                });
            }

            return Result.Success(new AppointmentIsDoneResponse
            {
                message = "No , This Patient didn`t Complete at Least One Appointment With This Doctor",
                IsDone = false
            });

            
        }

        public async Task<Result<IEnumerable<DoctorAppointmentsDetailsResponse>>> GetDoctorAppointmentsDetailsAsync(Guid DoctorId)
        {
            var AppointmentsDetails = await _db.Set<Appointment>()
                .Where(a => a.DoctorId == DoctorId && a.IsDone == true)
                .Select(a => new DoctorAppointmentsDetailsResponse(
                    a.Id,
                    a.PatientName,
                    a.AppointmentDate, 
                    a.AppointmentTime,
                    a.AppointmentDetails
                    ))
                .ToListAsync();

            return Result.Success<IEnumerable<DoctorAppointmentsDetailsResponse>>(AppointmentsDetails);

        }

        public async Task<Result<IEnumerable<PatientAppointmentsDetailsResponse>>> GetPatientAppointmentsDetailsAsync(Guid PatientId)
        {
            var AppointmentsDetails = await _db.Set<Appointment>()
                 .Where(a => a.PatientId == PatientId && a.IsDone == true && a.DoctorId != null)
                 .Select(a => new PatientAppointmentsDetailsResponse(
                     a._doctor!.FullName,
                     a.AppointmentDate,
                     a.AppointmentTime,
                     a.AppointmentDetails
                     ))
                 .ToListAsync();

            return Result.Success<IEnumerable<PatientAppointmentsDetailsResponse>>(AppointmentsDetails);
        }

      public async Task<Result> UpdateAppointmentDetails(Guid DoctorId , Guid AppointmentId , UpdateAppointmentDetailsRequest request,
          CancellationToken cancellationToken = default)
        {
            var appointment = await _db.Set<Appointment>()
                .FindAsync(AppointmentId, cancellationToken); 

            if(appointment == null)
                return Result.Failure(AppointmentErrors.AppointmentNotFound);

            if (appointment.DoctorId != DoctorId)
                return Result.Failure(AppointmentErrors.UnauhorizedDetailsUpdate);

            if (!appointment.IsDone)
                return Result.Failure(AppointmentErrors.CanNotUpdateDetails);

            appointment.AppointmentDetails = request.AppointmentDetails;

            await _db.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
