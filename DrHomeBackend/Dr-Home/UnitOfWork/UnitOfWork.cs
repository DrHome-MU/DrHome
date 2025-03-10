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

        public unitOfWork(AppDbContext context,
            IPatientService patientService , IUserService userService , 
            IDoctorService doctorService , IReviewService reviewService , 
            IClinicService clinicService)
        {
            _context = context;
            _userService = userService;
            _patientService = patientService;
            _doctorService = doctorService;
            _reviewService = reviewService;
            _clinicalService = clinicService;
           

        }

        public async Task<int> Complete()
        {
            return  await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose(); 
        }
    }
}
