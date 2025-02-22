using CuraMundi.Application.BLL.Dto;
using CuraMundi.Application.BLL.Interfaces;
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
        private readonly IUnitOfWork _unitOfWork;

        public PatientController(UserManager<User> userManager, IUnitOfWork unitOfWork)
        //public PatientController(UserManager<User> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

       
        [HttpPost]
        public async Task<ActionResult<PatientDetailDto>> CreatePatient([FromBody] PatientCreateDto patientCreateDto)
        {
            
            Patient patient = patientCreateDto.ToPatient();
            var usr =await _userManager.CreateAsync(patient, patientCreateDto.Password);
            
            await _userManager.AddToRoleAsync(patient, patientCreateDto.Role);
            return Accepted(patient.ToPatientDto());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDetailDto>> GetOne(Guid id)
        {
            
            Patient patient = await _unitOfWork.Patient.GetAsync(i =>i.Id==id);
            return Accepted(patient.ToPatientDto());
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDetailDto>> UpdatePatient(Guid id, [FromForm] PatientUpdateDto patientUpdateDto)
        {
            if (ModelState.IsValid)
            {
                Patient user = await _unitOfWork.Patient.GetAsync(i => i.Id == id);

                user.Adresse = patientUpdateDto.Adresse?? user.Adresse;
                
                user.Nom = patientUpdateDto.Nom ??  user.Nom;
                user.Prenom = patientUpdateDto.Prenom ??  user.Prenom;
                user.PhoneNumber = patientUpdateDto.Telephone ??  user.PhoneNumber;
                user.DateNaiss = patientUpdateDto.DateNaiss ??  user.DateNaiss;
                
                await _unitOfWork.Patient.Update(user);
                await _unitOfWork.Save();
                return Accepted(user.ToPatientDto());
            }
            return BadRequest();
            
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(Guid id)
        {
            _unitOfWork.Patient.RemoveAsync<Patient>(id);
            await _unitOfWork.Save();
            return Accepted();
        }
        [HttpGet]
        public async Task<ActionResult<PatientDetailDto>> GetAll()
        {
            IEnumerable<Patient> patients=await _unitOfWork.Patient.GetAllAsync();
            IEnumerable<PatientDetailDto> patientDetailDto = patients.Select(s => s.ToPatientDto());
            return Accepted(patientDetailDto);
        }

    }
}
