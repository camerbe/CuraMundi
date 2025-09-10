using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Extensions;
using CuraMundi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuraMundi.Controllers
{
    [Authorize(Roles = "Admin,Secretaire")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class SpecialiteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialiteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        //[Authorize("Admin,Secretaire")]
        [HttpPost]
        public async Task<ActionResult<SpecialiteDto>> CreateSpecialite([FromBody] SpecialiteCreateDto specialiteCreateDto)
        {

            Specialite specialite = specialiteCreateDto.ToSpecialite();
            await _unitOfWork.Specialite.AddAsync(specialite);
            await _unitOfWork.Save();
            return Accepted(specialite.ToSpecialiteDto());
        }
        //[Authorize("Admin,Secretaire")]
        [HttpGet]
        public async Task<ActionResult<SpecialiteDto>> GetAll()
        {
            IEnumerable<Specialite> specialites = await _unitOfWork.Specialite.GetAllAsync(null,"Medecins");
            IEnumerable<SpecialiteDto> specialiteDtos = specialites.Select(s => s.ToSpecialiteDto());
            return Accepted(specialiteDtos.OrderBy(o=>o.Titre));
        }
        //[Authorize("Admin,Secretaire")]
        [HttpGet("{id}")]
        public async Task<ActionResult<SpecialiteDto>> GetSpecialite(Guid id)
        {
            Specialite specialite = await _unitOfWork.Specialite.GetAsync(i => i.Id == id);
            return Accepted(specialite.ToSpecialiteDto());
        }
        //[Authorize("Admin,Secretaire")]
        [HttpPut("{id}")]
        public async Task<ActionResult<SpecialiteDto>> UpdateSpecialite(Guid id, [FromBody] SpecialiteCreateDto specialiteCreateDto)
        {
            if (ModelState.IsValid)
            {
                Specialite specialite = await _unitOfWork.Specialite.GetAsync(i => i.Id == id);
                specialite.Titre = specialiteCreateDto.Titre.Capitalize() ?? specialite.Titre;
                await _unitOfWork.Save();
                return Accepted(specialite.ToSpecialiteDto());
            }
            
            return BadRequest();
        }
        //[Authorize("Admin,Secretaire")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Specialite>> deleteSpecialite(Guid id)
        {
            var specialite = await _unitOfWork.Specialite.GetAsync(i => i.Id == id);
            if(specialite is not null)
            {
                _unitOfWork.Specialite.Delete(specialite);
                _unitOfWork.Save();
                return Accepted();
            }
            return NotFound();
            
        }
    }
}
