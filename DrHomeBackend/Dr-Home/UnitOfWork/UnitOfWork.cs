using Dr_Home.Data;
using Dr_Home.Data.Models;
using Dr_Home.Services.Interfaces;

namespace Dr_Home.UnitOfWork
{
    public class unitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
       

        public IPatientService _patientService { get; private set; }

        public IUserService _userService { get; private set; }

        public IDoctorService _doctorService {  get; private set; }

        public IReviewService _reviewService {  get; private set; }

        public IClinicService _clinicalService {  get; private set; }

        public ISpecializationService _specializationService {  get; private set; }

        public IScheduleService _scheduleService {  get; private set; }

        public IAppointmentService _appointmentService { get; private set; }

        public ICityService _cityService {  get; private set; }

        public IRegionService _regionService { get; private set; }

        public unitOfWork(AppDbContext context,
            IPatientService patientService, IUserService userService,
            IDoctorService doctorService, IReviewService reviewService,
            IClinicService clinicService, ISpecializationService specializationService,
            IScheduleService scheduleService, IAppointmentService appointmentService, ICityService cityService, IRegionService regionService)
        {
            _context = context;
            _userService = userService;
            _patientService = patientService;
            _doctorService = doctorService;
            _reviewService = reviewService;
            _clinicalService = clinicService;
            _specializationService = specializationService;
            _scheduleService = scheduleService;
            _appointmentService = appointmentService;
            _cityService = cityService;
            _regionService = regionService;
        }

        public async Task<int> Complete(CancellationToken cancellationToken = default)
        {
            return  await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose(); 
        }
    }
}
