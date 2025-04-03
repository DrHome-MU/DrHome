﻿using System.ComponentModel.DataAnnotations;

namespace Dr_Home.DTOs.DoctorDtos
{
    public class AddDoctorDto
    {
       
        public string FullName { get; set; }

       
        public string Gender { get; set; }
       
        public string Email { get; set; }


        public string? PhoneNumber { get; set; } 

        public string city { get; set; }

        public string region { get; set; }


        public string Password { get; set; }

        
        public string ConfirmPassword { get; set; }


        public int SpecializationId { get; set; }

    }
}
