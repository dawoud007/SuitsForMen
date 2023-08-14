using AutoMapper;
using ElectronicsShop_service.Dtos;
using ElectronicsShop_service.Interfaces;
using ElectronicsShop_service.MapperProfiles;
using ElectronicsShop_service.Models;
using ElectronicsShop_service.Repositories;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Security;
using System.Security.Claims;
using System.Text;
using System.Xml.Linq;

namespace ElectronicsShop_service.Controllers;
[ApiController]
[Route("[controller]/[action]")]

public class ShopController : MyBaseController<Shop, ShopDto>
{
    private readonly IConfiguration _configuration;
    private readonly ApplicationDbContext _dbContext;
    private readonly IShopRepository _shopRepository;
	private readonly IClothRepository _clothRepository;
    public ShopController(IConfiguration configuration,IShopRepository shopRepository, IClothRepository clothRepository,
        ApplicationDbContext dbContext,
        IShopUnitOfWork unitOfWork, IMapper mapper,
        IValidator<Shop> validator) : base(unitOfWork, mapper, validator)
	{
        _configuration = configuration;
        _shopRepository = shopRepository;
		_clothRepository = clothRepository;
  
        _dbContext = dbContext;

    }





    [HttpPost]

    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task<IActionResult> AddShopToRichMan([FromBody] ShopDto model)
    {
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
        var UserClaim = User.FindFirst("Role")?.Value;
        if (UserClaim == null)
        {
            return BadRequest("User role claim not found.");
        }
        if (UserClaim == "Admin")
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("fill all colums");
            }
            var existingUser = (await _shopRepository.Get(s => s.UserName==model.UserName)).FirstOrDefault();


            if (existingUser != null)
            {
                return BadRequest("Username already exists. Please choose a different username.");
            }
            var shop = new Shop
            {   
                UserName = model.UserName!,
                Password= model.Password,
                Role=model.Role,
                WhatToSee=model.WhatToSee!,
                CreationDate=DateTime.Now,
                Id=Guid.NewGuid()
            };
          await _shopRepository.AddAsync(shop);
            await _shopRepository.Save();
            return Ok(shop);
          
        }
        else
        {
            return BadRequest("You are not authorized to create a new user.");
        }
    }



    [HttpPost]

    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {

        Shop user = (await _shopRepository.Get(s => s.UserName == model.Username)).FirstOrDefault()!;

        if (user == null)
            return BadRequest(new { Message = "user does not exist" });
        if (user.Password!=model.Password)
        {
            return BadRequest(new { Message = "password is not correct" });
        }
        var token = GenerateJwtTokehn(user);
        return Ok(new { Token = token });
    }









    [HttpGet]

    public async Task<IActionResult> GetAllUsers()
    {

        /*  string usernameWhoUpdate = User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)!.Value;*/
        var users = await _shopRepository.GetAllAsync();
        var userInfos = users.Select(user => new ShopInfoModel
        {
           
            UserName = user.UserName,
            Role = user.Role,
            WhatToSee = user.WhatToSee
        }).ToList();

        return Ok(userInfos);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string name)
    {

        Shop user = (await _shopRepository.Get(s => s.UserName == name)).FirstOrDefault()!;
        if (user == null)
        {
            return NotFound("User not found.");
        }

     
           _shopRepository.Remove(user);
           return Ok("shop deleted");
        
    }





    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
    public async Task<IActionResult> AddWorkersToShop([FromBody] string workUserName)
    {
        var userNameClaim = User.FindFirst("Name")?.Value;
        var userShopClaim = User.FindFirst("WhatToSee")?.Value;

        Shop user = (await _shopRepository.Get(s => s.UserName == userNameClaim)).FirstOrDefault()!;
        if (user == null)
        {
            return NotFound("User not found.");
        }
        Worker workUser = new Worker
        {
            WorkerName = workUserName,
            WhatToSee = userShopClaim,
        };

        var x = _dbContext.ShopWorkers!.Where(w => w.WhatToSee == userShopClaim).ToList();
        foreach (var u in x)
        {
            if (u.WorkerName == workUser.WorkerName) { return Ok("user alreay exists"); }
            _dbContext.ShopWorkers!.Add(workUser);
        }
        if (x.Count == 0)
        {
            _dbContext.ShopWorkers!.Add(workUser);
        }

        user.workUsers= _dbContext.ShopWorkers!.Where(w => w.WhatToSee == userShopClaim).ToList();
        _dbContext.SaveChanges();
        _shopRepository.Save();

        return Ok("worker is added");

    }

    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetWorkersToShop()
    {
        var userNameClaim = User.FindFirst("Name")?.Value;
        var userShopClaim = User.FindFirst("WhatToSee")?.Value;

        Shop user = (await _shopRepository.Get(s => s.UserName == userNameClaim)).FirstOrDefault()!;
        if (user == null)
        {
            return NotFound("User not found.");
        }
        var workingUsers = _dbContext.ShopWorkers!.Where(w => w.WhatToSee == userShopClaim).ToList();

        return Ok(workingUsers);
    }


    [AllowAnonymous]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete]
    public async Task<IActionResult> DeleteWorkersFromShop(string? workersToDelete)
    {
        var userNameClaim = User.FindFirst("Name")?.Value;
        var userShopClaim = User.FindFirst("WhatToSee")?.Value;

        Shop user = (await _shopRepository.Get(s => s.UserName == userNameClaim)).FirstOrDefault()!;
        if (user == null)
        {
            return NotFound("shop not found.");
        }

        var workersInShop = _dbContext.ShopWorkers!
            .Where(w => w.WhatToSee == userShopClaim)
            .ToList();


        var existingWorker = workersInShop.FirstOrDefault(w => w.WorkerName == workersToDelete);
        if (existingWorker != null)
        {
            _dbContext.ShopWorkers!.Remove(existingWorker);
        }
        else
        {
            return Ok("no worker of this namw exists");
        }

       
     
        _dbContext.SaveChanges();

        return Ok("Workers deleted successfully.");
    }






    private string GenerateJwtTokehn(Shop user)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var Expiry = jwtSection.GetValue<double>("Expiry");
        var Issuer = jwtSection.GetValue<string>("Issuer");
        var Audience = jwtSection.GetValue<string>("Audience");

        var key = jwtSection.GetValue<string>("key");

        if (string.IsNullOrEmpty(key))
        {
            throw new InvalidOperationException("Jwt:Key is not configured or is empty.");
        }

        Claim[] claims = new Claim[]{

        new Claim("WhatToSee", user.WhatToSee!),
        new Claim("Role", user.Role!),
        new Claim("Name", user.UserName!),
        // Add other claims here
        // Example: new Claim("CustomClaimType", "ClaimValue")
    };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key)
            ),
            SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: Issuer,
            audience: Audience,
            claims: claims,
            notBefore: null,
            expires: DateTime.Now.AddHours(Expiry),
            signingCredentials: signingCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        return token;
    }

}


public class ShopInfoModel
{

  
    public string UserName { get; set; }
    public string Role { get; set; }
    public string WhatToSee { get; set; }
}
public class LoginModel
{
    public string? Username { get; set; }
    public string? Password { get; set; }
}