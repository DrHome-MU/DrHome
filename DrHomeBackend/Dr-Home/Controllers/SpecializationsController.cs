﻿using Dr_Home.Data.Models;
using Dr_Home.UnitOfWork;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dr_Home.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationsController(ISpecializationHelper _specializationHelper) : ControllerBase
    {

        [HttpPost]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Add([FromQuery] string SpecializationName , CancellationToken cancellationToken)
        {
           var response = await _specializationHelper.AddAsync(SpecializationName,cancellationToken);


            return (response.Success) ? Ok(response) : BadRequest(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var response = await _specializationHelper.GetAllAsync(cancellationToken);

            return (response.Success) ? Ok(response) : NotFound(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
        {
            var response = await _specializationHelper.DeleteAsync(id, cancellationToken);

            return (response.Success) ? Ok(response) : NotFound(response);
        }

    }
}
