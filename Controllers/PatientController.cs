using CuraMundi.Application.BLL.Dto;
using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Application.BLL.Mappers;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;
using CuraMundi.Infrastructure.DAL.Data.Configs;
using CuraMundi.Infrastructure.DAL.Repositories;
using CuraMundi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace CuraMundi.Controllers
{
    [Authorize(Roles = "Admin,Patient,Secretaire")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly RoleSeeder _roleSeeder;
        private readonly IEmailConfirmationService _emailService;
        private readonly IConfiguration _config;

        public PatientController(
            UserManager<User> userManager, 
            IUnitOfWork unitOfWork, 
            RoleManager<IdentityRole<Guid>> roleManager,
            IEmailConfirmationService emailService,
            IConfiguration config
            )
        //public PatientController(UserManager<User> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
            _roleSeeder = new RoleSeeder(_roleManager, _userManager);
            _emailService = emailService;
            _config = config;
        }

        //[Authorize("Admin,Patient,Secretaire")]
        [HttpPost]
        public async Task<ActionResult<PatientDetailDto>> CreatePatient([FromBody] PatientCreateDto patientCreateDto)
        {
            bool emailIsNull = (patientCreateDto.Email is null)? true:false;
            if (emailIsNull)
            {
                patientCreateDto.Email= patientCreateDto.Nom + "." + patientCreateDto.Prenom + "@gmail.com";
                patientCreateDto.Password = patientCreateDto.Nom + "." + patientCreateDto.Prenom;
            }
            Patient patient = patientCreateDto.ToPatient();
            var usr = await _userManager.CreateAsync(patient, patientCreateDto.Password);
            if (usr.Succeeded)
            {
                await _roleSeeder.SeedRolesAsync();
                await _userManager.AddToRoleAsync(patient, patientCreateDto.Role);
                await _emailService.SendConfirmedEmailAsync(patient, null);
                return Accepted(patient.ToPatientDto());
            }
            return BadRequest();
        }
        //[Authorize("Admin,Patient,Secretaire,Medecin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<PatientDetailDto>> GetOne(Guid id)
        {
            
            Patient patient = await _unitOfWork.Patient.GetAsync(i =>i.Id==id);
            return Accepted(patient.ToPatientDto());
        }
        //[Authorize("Admin,Patient,Secretaire")]
        [HttpPut("{id}")]
        public async Task<ActionResult<PatientDetailDto>> UpdatePatient(Guid id, [FromForm] PatientUpdateDto patientUpdateDto)
        {
            if (ModelState.IsValid)
            {
                Patient user = await _unitOfWork.Patient.GetAsync(i => i.Id == id);

                user.Adresse = patientUpdateDto.Adresse?? user.Adresse;

                user.Nom = patientUpdateDto.Nom.ToUpperCase() ?? user.Nom;
                user.Prenom = patientUpdateDto.Prenom.Capitalize() ??  user.Prenom;
                user.PhoneNumber = patientUpdateDto.Telephone ??  user.PhoneNumber;
                user.DateNaiss = patientUpdateDto.DateNaiss ??  user.DateNaiss;
                
                await _unitOfWork.Patient.Update(user);
                await _unitOfWork.Save();
                return Accepted(user.ToPatientDto());
            }
            return BadRequest();
            
        }
        //[Authorize("Admin,Patient,Secretaire")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePatient(Guid id)
        {
            User patient = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.DeleteAsync(patient);
            return result.Succeeded ? Accepted() : BadRequest();
        }
        //[Authorize("Admin,Secretaire")]
        [HttpGet]
        public async Task<ActionResult<PatientDetailDto>> GetAll()
        {
            IEnumerable<Patient> patients=await _unitOfWork.Patient.GetAllAsync();
            IEnumerable<PatientDetailDto> patientDetailDto = patients.Select(s => s.ToPatientDto());
            return Accepted(patientDetailDto);
        }
        #region Helper
        //private async Task<bool> SendConfirmedEmailAsync(User user)
        //{
        //    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        //    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
        //    var url = $"{_config["JWT:Issuer"]}/api/Auth/email-confirm/?email={user.Email}&token={token}";
        //    var body = $"<p>Bonjour {user.Prenom} {user.Nom}</p>" +
        //                "<<p>Afin de pouvoir activer votre compte, nous devons valider votre adresse email. Cliquez simplement sur le lien :</p>" +
        //                $"<p><a style='padding:3px;background:blue;color:white' href='{url}'>Activer mon compte</a></p>" +
        //                "<p>Bienvenue à bord !</p>";
        //    var emailSend = new EmailSendDTO(user.Email, "Confirmation", body);
        //    return await _emailService.SendEmailAsync(emailSend);
        //}
        #endregion
    }
}
