﻿namespace Dr_Home.DTOs.AuthDTOs
{
    public class ResetPasswordDto
    {
        public required string Email { get; set; }

        public required string Code { get; set; }

        public required string NewPassword { get; set; }
    }
}
