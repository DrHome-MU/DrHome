﻿using Dr_Home.Authentication;
using Dr_Home.BackgroundJobs;
using Dr_Home.Data;
using Dr_Home.Email_Sender;
using Dr_Home.Errors;
using Dr_Home.File_Manager;
using Dr_Home.Helpers.Interfaces;
using Dr_Home.Services.Interfaces;
using Dr_Home.Services.services;
using Dr_Home.UnitOfWork;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
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

            // services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            //Add Cors 
            services.AddCors();

            //Hangfire 
            services.AddHangfire(config => config.
            UseSqlServerStorage(_configuration.GetConnectionString("host")));

            services.AddHangfireServer();

            //Add Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;

            mappingConfig.Scan(Assembly.GetExecutingAssembly());

            services.AddSingleton<IMapper>(new Mapper(mappingConfig));

            //DbContext 
            //services.AddDbContext<AppDbContext>(options => options.
            //UseSqlServer(_configuration.GetConnectionString("Default")));

            services.AddDbContext<AppDbContext>(options => options.
           UseSqlServer(_configuration.GetConnectionString("host")),ServiceLifetime.Scoped);

            //FluentValdition 
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            //Global Exception Handler 
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

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

            //Specialization Service

            services.AddScoped<ISpecializationService, SpecializationService>();

            //Specialization Helper
            services.AddScoped<ISpecializationHelper, SpecializationHelper>();  

            //Schedule Service 

            services.AddScoped<IScheduleService, ScheduleService>();

            //Schedule Helper 

            services.AddScoped<IScheduleHelper , ScheduleHelper>();

            //Appointment Service 

            services.AddScoped<IAppointmentService, AppointmentService>();

            //City Service 
            services.AddScoped<ICityService, CityService>();
            //City Helper 
            services.AddScoped<ICityHelper , CityHelper>();

            //Region Service 
            services.AddScoped<IRegionService, RegionService>();

            //Region Heleper

            services.AddScoped<IRegionHelper, RegionHelepr>();
            //ManageSchedules 
            services.AddScoped<IManageSchedules, ManageSchedules>();

            //Appointment Helper
            services.AddScoped<IAppointmentHelper, AppointmentHelper>();

            //Support Helper

            services.AddScoped<ISupportHelper , SupportHelper>();


            services.AddScoped<IJwtProvider , JwtProvider>();


            //services.Configure<jwtOptions>(_configuration.GetSection(jwtOptions.SectionName));
            services.AddOptions<jwtOptions>()
           .BindConfiguration(jwtOptions.SectionName)
           .ValidateDataAnnotations()
           .ValidateOnStart();

            services.AddSingleton(sp => sp.GetRequiredService<IOptions<jwtOptions>>().Value);
            //get the section jwt and bind it to JwtOptions Class 
            var jwtSettings = _configuration.GetSection(jwtOptions.SectionName).Get<jwtOptions>();

            services.AddAuthentication()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ValidateIssuer = true,
                        ValidIssuer = jwtSettings!.Issuer,
                        // ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        // ValidateLifetime = true,
                        //  RequireExpirationTime = true,
                        // ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                      
                    };
                });


            ///Multiple-Language 
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix);

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                new CultureInfo("en"),
                new CultureInfo("ar")
                };

                
                options.DefaultRequestCulture = new RequestCulture(culture: "ar", uiCulture: "ar");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

        }

        // Middleware Configuration
        public void Configure(WebApplication app)
        {
           // app.UseHttpsRedirection();

            //Cors 
            app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
            });
            //app.UseMiddleware<ExceptionHandlerMiddleware>();
            app.UseExceptionHandler();

            app.UseRouting();

            var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(locOptions.Value);


            app.UseAuthorization();

            app.MapControllers();
           

            app.UseStaticFiles();

            // app.MapStaticAssets();
            // app.UseExceptionHandler();
            //app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
