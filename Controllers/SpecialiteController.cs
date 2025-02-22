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
    public class SpecialiteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpecialiteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpPost]
        public async Task<ActionResult<SpecialiteDto>> CreateSpecialite([FromBody] SpecialiteCreateDto specialiteCreateDto)
        {
            
            Specialite specialite = specialiteCreateDto.ToSpecialite();
            await _unitOfWork.Specialite.AddAsync(specialite);
            await _unitOfWork.Save();
            return Accepted(specialite.ToSpecialiteDto());
        }
        [HttpGet]
        public async Task<ActionResult<SpecialiteDto>> GetAll()
        {
            IEnumerable<Specialite> specialites = await _unitOfWork.Specialite.GetAllAsync();
            IEnumerable<SpecialiteDto> specialiteDtos = specialites.Select(s => s.ToSpecialiteDto());
            return Accepted(specialiteDtos);
        }
    }
}
