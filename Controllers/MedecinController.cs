using CuraMundi.Application.BLL.Dto;
using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Application.BLL.Mappers;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;
using CuraMundi.Infrastructure.DAL.Data.Configs;
using CuraMundi.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CuraMundi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedecinController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly RoleSeeder _roleSeeder;
        public MedecinController(IUnitOfWork unitOfWork, UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
            _roleSeeder = new RoleSeeder(_roleManager, _userManager);
        }
        [HttpPost]
        public async Task<ActionResult<MedecinDetailDto>> CreateMedecin([FromBody] MedecinCreateDto medecinCreateDto)
        {
            Medecin medecin = medecinCreateDto.ToMedecin();
            var toubib = await _userManager.CreateAsync(medecin, medecinCreateDto.Password);
            if (toubib.Succeeded)
            {
                await _roleSeeder.SeedRolesAsync();
                await _userManager.AddToRoleAsync(medecin, medecinCreateDto.Role);
                medecin.Specialite = await _unitOfWork.Specialite.GetAsync(i => i.Id == medecin.SpecialiteId);
                
                return Accepted(medecin.ToMedecinDto());
            }
            return BadRequest();
            
        }
        [HttpGet]
        public async Task<ActionResult<MedecinDetailDto>> GetAll()
        {
            IEnumerable<Medecin> medecins = await _unitOfWork.Medecin.GetAllAsync(null, "Specialite");
            IEnumerable<MedecinDetailDto> medecinDetailDto = medecins.Select(s => s.ToMedecinDto());
            return Accepted(medecinDetailDto);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<MedecinDetailDto>> GetMedecin(Guid id)
        {
            Medecin medecin =  await _unitOfWork.Medecin.GetOneMedecin(id);
            return Accepted(medecin.ToMedecinDto());
        }
        [HttpDelete]
        public async Task<ActionResult<Medecin>> DeleteMedecin(Guid id)
        {
            User medecin = await _userManager.FindByIdAsync(id.ToString());
            var result = await _userManager.DeleteAsync(medecin);
            return result.Succeeded ? Accepted() : BadRequest();
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<MedecinDetailDto>> UpdateMedecin(Guid id, [FromForm] MedecinUpdateDto medecinUpdateDto)
        {
            
            if (ModelState.IsValid)
            {
                Medecin medecin = await _unitOfWork.Medecin.GetAsync(i => i.Id == id);
                medecin.Inami = medecinUpdateDto.Inami ?? medecin.Inami;
                medecin.Nom = medecinUpdateDto.Nom.ToUpperCase() ?? medecin.Nom;
                medecin.Prenom = medecinUpdateDto.Prenom.Capitalize() ?? medecin.Prenom;
                medecin.PhoneNumber = medecinUpdateDto.Telephone ?? medecin.PhoneNumber;
                medecin.SpecialiteId = medecinUpdateDto.SpecialiteId ?? medecin.SpecialiteId;
                await _unitOfWork.Medecin.Update(medecin);
                await _unitOfWork.Save();
                Medecin toubib =await _unitOfWork.Medecin.GetOneMedecin(id);
                return Accepted(toubib.ToMedecinDto());
            }
            
            return BadRequest();
        }
        
    }
}
