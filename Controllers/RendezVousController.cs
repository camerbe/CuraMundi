using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuraMundi.Controllers
{
    [Authorize(Roles = "Admin,Medecin,Secretaire,Patient")]

    [Route("api/[controller]")]
    [ApiController]
    public class RendezVousController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public RendezVousController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpPost]
        public async Task<ActionResult<RendezVousDetailDto>> CreateRendezVous([FromBody] RendezVousCreateDto rendezVousCreateDto)
        {
            RendezVous rendezVous = rendezVousCreateDto.ToRendezVous();
            await _unitOfWork.RendezVous.AddAsync(rendezVous);
            await _unitOfWork.Save();
            try
            {
                RendezVous rdv = await _unitOfWork.RendezVous.GetOneRendezVous(rendezVous.Id);
                return Accepted(rdv.ToRendezVousDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        //[Authorize("Admin,Secretaire,Patient,Medecin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<RendezVousDetailDto>> GetRendezVous(Guid id)
        {
            RendezVous rendezVous = await _unitOfWork.RendezVous.GetOneRendezVous(id);
            return Accepted(rendezVous.ToRendezVousDto());
        }
        //[Authorize("Admin,Secretaire,Medecin")]
        [HttpPut("{id}")]
        public async Task<ActionResult<RendezVousDetailDto>> UpdateRendezVous(Guid id, [FromBody] RendezVousUpdateDto rendezVousUpdateDto)
        {
            if (ModelState.IsValid)
            {
                RendezVous rendezVous = await _unitOfWork.RendezVous.GetAsync(i => i.Id == id);
                rendezVous.DateRdv = rendezVousUpdateDto.DateRdv ?? rendezVous.DateRdv;
                rendezVous.Debut = rendezVousUpdateDto.Debut ?? rendezVous.Debut;
                rendezVous.MedecinId = rendezVousUpdateDto.MedecinId ?? rendezVous.MedecinId;
                rendezVous.PatientId = rendezVousUpdateDto.PatientId ?? rendezVous.PatientId;
                _unitOfWork.RendezVous.Update(rendezVous);
                _unitOfWork.Save();
                return Accepted(rendezVous.ToRendezVousDto());
            }
            return BadRequest();
        }
        //[Authorize("Admin,Secretaire,Patient,Medecin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<RendezVous>> DeleteRendezVous(Guid id)
        {
            _unitOfWork.RendezVous.RemoveAsync<RendezVous>(id);
            _unitOfWork.Save();
            return Accepted();
        }
        //[Authorize("Admin,Secretaire,Patient,Medecin")]
        [HttpGet]
        public async Task<ActionResult<RendezVousDetailDto>> GetAll()
        {
            IEnumerable<RendezVous> rendezVous = await _unitOfWork.RendezVous.GetAllRendezVous();
            IEnumerable<RendezVousDetailDto> rendezVousDetailDto = rendezVous.Select(s => s.ToRendezVousDto());
            return Accepted(rendezVousDetailDto);
        }
        //[Authorize("Admin,Secretaire,Medecin")]
        [HttpGet("medecin/{id}")]
        public async Task<ActionResult<RendezVousDetailDto>> GetRendezVousByMedecin(Guid id)
        {
            IEnumerable<RendezVous> rendezVous = await _unitOfWork.RendezVous.GetRendezVousByMedecin(id);
            IEnumerable<RendezVousDetailDto> rendezVousDetailDto = rendezVous.Select(s => s.ToRendezVousDto());
            return Accepted(rendezVousDetailDto);
        }
    }
}
