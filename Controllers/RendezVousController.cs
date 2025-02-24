using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CuraMundi.Controllers
{
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
            try {
                RendezVous rdv = (RendezVous)_unitOfWork.RendezVous.GetOneRendezVous(rendezVous.Id);
                return Accepted(rdv.ToRendezVousDto());
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
