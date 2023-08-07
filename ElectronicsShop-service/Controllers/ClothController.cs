using AutoMapper;
using ElectronicsShop_service.Dtos;
using ElectronicsShop_service.Interfaces;
using ElectronicsShop_service.Models;
using ElectronicsShop_service.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Controllers;
[ApiController]
[Route("[controller]/[action]")]
/*[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin")]*/
public class ClothController : MyBaseController<Cloth, ClothDto>
{
	private readonly IClothRepository _clothRepository;
    private readonly ApplicationDbContext _context;
	public ClothController(IClothUnitOfWork unitOfWork, IMapper mapper, IClothRepository clothRepository, IValidator<Cloth> validator) : base(unitOfWork, mapper, validator)
    {
		_clothRepository = clothRepository;
	}

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;

     /*   var clothData=await _clothRepository.GetAllAsync();*/
        var clothData = (await _clothRepository.Get(c => c.StoreName == whatToSeeValue)).ToList();
   /*     var filteredClothData = clothData.Where(c => c.StoreName == whatToSeeValue);*/

        return Ok(clothData);
    }

    /*   [HttpGet]
       public virtual async Task<IActionResult> Get([FromQuery] string clothName)
       {
           return Ok((await _unitOfWork.ReadAsync(b => b.Name == clothName, null, "")).FirstOrDefault()!);
       }*/


    [HttpGet("{id}")]
    public async Task<ActionResult> Get(Guid id)
    {
        var FromCloth = (await _clothRepository.Get(c => c.Id == id)).FirstOrDefault();



        return Ok(FromCloth);



    }

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> Search([FromBody] ClothSearchDto searchDto)
    {
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
        searchDto.WhatToSee = whatToSeeValue;
        var result = await _clothRepository.GetFiltered(searchDto);
        if(result.Count==0)
        {
            return NotFound("there is not matching products");
        }
        return Ok(result);
    }
  

    public class ClothSearchDto
    {
        public string? Name { get; set; }
        public int? Size { get; set; }
        public string? Color { get; set; }
        public string? Make { get; set; }
        public string? WhatToSee { get; set; }
    }

}


