using CuraMundi.Application.BLL.Dto;
using CuraMundi.Application.BLL.Interfaces;
using CuraMundi.Domain.Entities;
using CuraMundi.Dto;
using CuraMundi.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CuraMundi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedecinController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public MedecinController(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<ActionResult<MedecinDetailDto>> CreateMedecin([FromBody] MedecinCreateDto medecinCreateDto)
        {
            Medecin medecin = medecinCreateDto.ToMedecin();
            var toubib = await _userManager.CreateAsync(medecin, medecinCreateDto.Password);
            //await _unitOfWork.Medecin.AddAsync(medecin);
            //await _unitOfWork.Save();
            return Accepted(medecin.ToMedecinDto());
        }
        [HttpGet]
        public async Task<ActionResult<MedecinDetailDto>> GetAll()
        {
            IEnumerable<Medecin> medecins = await _unitOfWork.Medecin.GetAllAsync(null, "Specialite");
            IEnumerable<MedecinDetailDto> medecinDetailDto = medecins.Select(s => s.ToMedecinDto());
            return Accepted(medecinDetailDto);

        }
    }
}
