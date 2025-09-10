using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Mappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CuraMundi.Controllers
{
    [Authorize(Roles = "Admin,Medecin,Secretaire")]
    [Route("api/[controller]")]
    [ApiController]
    
    public class CreneauHoraireController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreneauHoraireController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult<CreneauHoraireDetailDto>> CreateCreneauHoraire([FromBody] CreneauHoraireCreateDto creneauHoraireCreateDto)
        {
            if (ModelState.IsValid)
            {
                var creneauHoraire = creneauHoraireCreateDto.ToCreneauHoraire();
                await _unitOfWork.CreneauxHoraire.AddAsync(creneauHoraire);
                await _unitOfWork.Save();
                creneauHoraire.Medecin = await _unitOfWork.Medecin.GetOneMedecin(creneauHoraireCreateDto.MedecinId);

                return Accepted(creneauHoraire.ToCreneauHoraireDto());

            }
            return BadRequest();

        }
        //[Authorize("Admin,Medecin,Secretaire")]
        [HttpGet("{id}")]
        public async Task<ActionResult<CreneauHoraireDetailDto>> GetOneCreneauHoraire(Guid id)
        {
            var creneauHoraire = await _unitOfWork.CreneauxHoraire.GetOneCreneauxHoraire(id);
            return Accepted(creneauHoraire.ToCreneauHoraireDto());
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<CreneauHoraireDetailDto>> UpdateCreneauHoraire(Guid id, [FromBody] CreneauHoraireUpdateDto creneauHoraireUpdateDto)
        {
            if (ModelState.IsValid)
            {
                CrenauxHoraire creneauHoraire = await _unitOfWork.CreneauxHoraire.GetAsync(i => i.Id == id);
                creneauHoraire.Date= creneauHoraireUpdateDto.Date ?? creneauHoraire.Date;
                creneauHoraire.Statut= creneauHoraireUpdateDto.Statut ?? creneauHoraire.Statut;
                creneauHoraire.Debut= creneauHoraireUpdateDto.Debut ?? creneauHoraire.Debut;
                creneauHoraire.Fin = creneauHoraireUpdateDto.Fin ?? creneauHoraire.Fin;
                creneauHoraire.MedecinId = creneauHoraireUpdateDto.MedecinId ?? creneauHoraire.MedecinId;
                await _unitOfWork.CreneauxHoraire.Update(creneauHoraire);
                await _unitOfWork.Save();
                CrenauxHoraire creneauH = await _unitOfWork.CreneauxHoraire.GetOneCreneauxHoraire(id);
                return Accepted(creneauH.ToCreneauHoraireDto());
            }
            return BadRequest();
        }
        //[Authorize("Admin,Medecin,Secretaire")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCreneauHoraire(Guid id)
        {
            CrenauxHoraire creneauHoraire = await _unitOfWork.CreneauxHoraire.GetAsync(i => i.Id == id);
            _unitOfWork.CreneauxHoraire.RemoveAsync<CrenauxHoraire>(id);
            await _unitOfWork.Save();
            return  Accepted();
        }
        //[Authorize("Admin,Medecin,Secretaire")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CreneauHoraireDetailDto>>> GetAllCreneauxHoraires()
        {
            var creneauxHoraires = await _unitOfWork.CreneauxHoraire.GetAllCrenauxHoraire();
            return Accepted(creneauxHoraires.Select(c => c.ToCreneauHoraireDto()));
        }
    }
}
