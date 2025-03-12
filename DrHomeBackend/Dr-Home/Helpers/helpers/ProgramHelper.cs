using Dr_Home.Data;
using Dr_Home.DTOs.AuthDTOs;
using Dr_Home.Email_Sender;
using Dr_Home.File_Manager;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.Services.Interfaces;
using Dr_Home.Services.services;
using Dr_Home.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

namespace Dr_Home.Helpers.helpers
{
    public class ProgramHelper
    {
        public IConfiguration _configuration { get; set; }

        public ProgramHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

         //Dependency Injection
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            //Add Services Settings
            //services.AddControllers()
            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
            //    options.JsonSerializerOptions.MaxDepth = 64; // Optional: Increase max depth if necessary
            //});

            services.AddOpenApi();
            services.AddSwaggerGen();

            //Add Cors 
            services.AddCors();

            //DbContext 
            //services.AddDbContext<AppDbContext>(options => options.
            //UseSqlServer(_configuration.GetConnectionString("Default")));

            services.AddDbContext<AppDbContext>(options => options.
           UseSqlServer(_configuration.GetConnectionString("host")),ServiceLifetime.Scoped);

            //FluentValdition 
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            //AuthHelper
            services.AddScoped<IAuthHelper, AuthHelper>();
            //Doctor Helper 
            services.AddScoped <IDoctorHelper, DoctorHelper >();

            //PatientService 
            services.AddScoped<IPatientService,PatientService>();
            //DoctorService
            services.AddScoped<IDoctorService, DoctorService>();
            //unit of work

            services.AddScoped<IUnitOfWork , unitOfWork>();
            
            //Email Sender 
            services.AddScoped<IEmailSender, EmailSender>();
            //User Service 
            services.AddScoped<IUserService, UserService>();

            //Review Service
            services.AddScoped<IReviewService, ReviewService>();

            //Review Helper 
            services.AddScoped<IReviewHelper, ReviewHelper>();

            //Clinic Helper

            services.AddScoped<IClinicHelper, ClinicHelper>();

            //Clinic Service 

            services.AddScoped<IClinicService, ClinicService>();

            //File Manager Service 

            services.AddScoped<IFileManager, FileManager>(); 

            //Jwt Token 
            var JwtOptions = _configuration.GetSection("Jwt").Get<jwtOptions>();
            services.AddSingleton(JwtOptions);

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ValidateIssuer = true,
                        ValidIssuer = JwtOptions.Issuer,
                        // ValidateAudience = true,
                        ValidAudience = JwtOptions.Audience,
                        // ValidateLifetime = true,
                        //  RequireExpirationTime = true,
                        // ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptions.Key)),
                      
                    };
                });

        }

        // Middleware Configuration
        public void Configure(WebApplication app)
        {
           // app.UseHttpsRedirection();

            //Cors 
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

           
            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();

            app.UseStaticFiles();

           // app.MapStaticAssets();
        }
    }
}
