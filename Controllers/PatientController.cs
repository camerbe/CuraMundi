using CuraMundi.Application.BLL.Dto;
using CuraMundi.Application.BLL.Mappers;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Infrastructure.DAL.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CuraMundi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        //private readonly UnitOfWork _unitOfWork;

        //public PatientController(UserManager<User> userManager, UnitOfWork unitOfWork)
        public PatientController(UserManager<User> userManager)
        {
            _userManager = userManager;
            //_unitOfWork = unitOfWork;
        }

       
        [HttpPost]
        public async Task<ActionResult<PatientDetailDto>> CreatePatient([FromBody] PatientCreateDto patientCreateDto)
        {
            
            Patient patient = patientCreateDto.ToPatient();
            var usr =await _userManager.CreateAsync(patient, patientCreateDto.Password);
            
            await _userManager.AddToRoleAsync(patient, patientCreateDto.Role);
            return Accepted(patient.ToPatientDto());
        }
    }
}
