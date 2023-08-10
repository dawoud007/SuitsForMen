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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Net.Security;
using System.Security.Claims;

namespace ElectronicsShop_service.Controllers;
[ApiController]
[Route("[controller]/[action]")]

public class BillController : MyBaseController<Bill, BillDto>
{
	private readonly IBillRepository _billRepository;
	private readonly IClothRepository _clothRepository;
	public BillController(IBillRepository billRepository, IClothRepository clothRepository, IBillUnitOfWork unitOfWork, IMapper mapper, IValidator<Bill> validator) : base(unitOfWork, mapper, validator)
	{
		_billRepository = billRepository;
		_clothRepository = clothRepository;
	}

    [Authorize(AuthenticationSchemes = "Bearer")]
	[HttpGet]
    public async Task<IActionResult> GetAll()
	{
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;

        var results = await _billRepository.GetAllAsync();
        var filteredBillData = results.Where(c => c.SellerName == whatToSeeValue);
        if(!filteredBillData.Any())
        {
            return Ok("no bills for this shop");
        }

        foreach (var bill in filteredBillData)
		{
			var thing = (await _clothRepository.Get(c => c.BillId == bill.Id)).ToList();
			bill.Suits = thing;



		}
		return Ok(filteredBillData);
	}







    public override async Task<ActionResult> Delete(Guid id)
	{
        var FromCloth = (await _clothRepository.Get(c => c.BillId == id)).FirstOrDefault();
        var FromBills = (await _billRepository.Get(c => c.Id == id)).FirstOrDefault();

		_billRepository.Remove(FromBills);
		_clothRepository.Remove(FromCloth);
		await _unitOfWork.SaveAsync();
		return Ok("item deleted");



    }

    [HttpGet]
    public  async Task<ActionResult> GetBillById(Guid id)
    {
    
        var FromBills = (await _billRepository.Get(c => c.Id == id)).FirstOrDefault();

      
        return Ok(FromBills);



    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet()]
	public  async Task<IActionResult> GetBillWithBuyerName([FromQuery]string searchedBuyertName)

	{

        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
        var result= (await _billRepository.Get(b=>b.BuyerName==searchedBuyertName&&
        b.SellerName==whatToSeeValue,null,"")).ToList();
        foreach(var res in result)
        {
            res.Suits = (await _clothRepository.Get(s => s.BillId == res.Id)).ToList();
        }

        if (result == null)
		{
        
            return NotFound("لا توجد فاتورة بهذا الاسم");
		}

        return Ok(result);
	}

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpPost]
	public  async override Task<IActionResult> Post([FromBody] BillDto billDto)
	{

        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
        if (whatToSeeValue != null)
        {
            billDto.SellerName = whatToSeeValue;
            
        }
        else
        {
            return BadRequest("يرجى تسجيل الدخول");
        }
        if (!ModelState.IsValid)
		{
			return BadRequest(ModelState);
		}

		var list=new List<Cloth>();
		int WarningToNumberOfPieces=0;

		
		foreach(var suit in billDto.Suits!)
		{
          
			list.Add(suit);

		}
		var bill = new Bill
		{
			BuyerName = billDto.BuyerName,
			SellerName = billDto.SellerName,
			Description = billDto.Description!,
			DateCreated=DateTime.Now,
			Suits= list
		};

		try
		{
			await _billRepository.AddAsync(bill);

			if (billDto.Suits != null && billDto.Suits.Any())
			{
                foreach (var suitDto in billDto.Suits)
                {

                    suitDto.StoreName = billDto.SellerName;

                    var suits = await FindSuitWithFeatures(suitDto!);
					if (suitDto != null)
					{
						if (suitDto.NumOfPieces <= suits.NumOfPieces)
						{
							suits.NumOfPieces -=suitDto.NumOfPieces;
						/*	if (suits.NumOfPieces <= billDto.limit)
							{
								WarningToNumberOfPieces=suitDto.NumOfPieces;
							}*/
							
						}
						else
						{
							return BadRequest($"Not enough suits available with Size: {suitDto.Size}, Color: {suitDto.Color}, Type: {suitDto.type}");
						}
					}
					else
					{
						return BadRequest($"Suit not found with Size: {suitDto.Size}, Color: {suitDto.Color}, Type: {suitDto.type}");
					}
				}
			}

			await _billRepository.Save();

		/*	if (WarningToNumberOfPieces > 0)
			{
				var addBillWithAmountWarning=bill.Adapt<BillDto>();
				addBillWithAmountWarning.WarningToNumberOfPieces = $"warning only {billDto.limit} pieces";


                return Ok(addBillWithAmountWarning);
			}*/
			return Ok(bill);
		}
		catch (Exception ex)
		{
			return BadRequest(ex.InnerException?.Message ?? ex.Message);
		}
	}
	
	private async Task<Cloth> FindSuitWithFeatures(Cloth suitDto)
	{
	
		var s =(await _clothRepository.Get(c =>
            c.Name == suitDto.Name&&
            c.Size == suitDto.Size &&
			c.Color == suitDto.Color&&
			c.StoreName == suitDto.StoreName &&
			c.type == suitDto.type,null,"")).ToList().FirstOrDefault()!;
		
		return s;
	}

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet]
    public async Task<IActionResult> GetStatics(PeriodType periodType)
    {
        var whatToSeeClaim = User.FindFirst("WhatToSee")?.Value;

        var results = await _billRepository.GetAllAsync();
        var currentDate = DateTime.Now;

        if (periodType == PeriodType.Month)
        {
            var firstDayOfMonth = currentDate.AddDays(1 - currentDate.Day).Date;
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var monthlyStatistics = (await _clothRepository.Get(c =>
                c.DateCreated >= firstDayOfMonth && c.DateCreated <= lastDayOfMonth && c.BillId != null&&c.StoreName==whatToSeeClaim,
                null,
                ""))
                .ToList();

            var groupedMonthlyStatistics = monthlyStatistics
                .GroupBy(cloth => new { cloth.Name, cloth.Color, cloth.Size })
                .Select(groupedCloth => new GroupedStatistics
                {
                    Name = groupedCloth.Key.Name,
                    Color = groupedCloth.Key.Color,
                    Size = groupedCloth.Key.Size,
                    TotalPieces = groupedCloth.Sum(cloth => cloth.NumOfPieces)
                })
                .ToList();

            var mostSoldProduct = groupedMonthlyStatistics
                .OrderByDescending(stat => stat.TotalPieces)
                .FirstOrDefault();

            if (mostSoldProduct != null)
            {
                mostSoldProduct.Label = "Most Sold";
            }

            return Ok(new { Statistics = groupedMonthlyStatistics, MostSoldProduct = mostSoldProduct });
        }
        else if (periodType == PeriodType.Week)
        {
            var lastDayOfWeek = currentDate;
            var firstDayOfWeek = currentDate.AddDays(-6);

            var weeklyStatistics = (await _clothRepository.Get(c =>
                c.DateCreated >= firstDayOfWeek && c.DateCreated <= lastDayOfWeek && c.BillId != null&& c.StoreName == whatToSeeClaim,
                null,
                ""))
                .ToList();

            var groupedWeeklyStatistics = weeklyStatistics
                .GroupBy(cloth => new { cloth.Name, cloth.Color, cloth.Size })
                .Select(groupedCloth => new GroupedStatistics
                {
                    Name = groupedCloth.Key.Name,
                    Color = groupedCloth.Key.Color,
                    Size = groupedCloth.Key.Size,
                    TotalPieces = groupedCloth.Sum(cloth => cloth.NumOfPieces)
                })
                .ToList();

            var mostSoldProduct = groupedWeeklyStatistics
                .OrderByDescending(stat => stat.TotalPieces)
                .FirstOrDefault();

            if (mostSoldProduct != null)
            {
                mostSoldProduct.Label = "Most Sold";
            }

            return Ok(new { Statistics = groupedWeeklyStatistics, MostSoldProduct = mostSoldProduct });
        }

        return BadRequest("Invalid period type.");
    }



    public class GroupedStatistics
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int Size { get; set; }
        public int TotalPieces { get; set; }
        public string Label { get; set; }
    }



    public enum PeriodType
    {
        Month,
        Week
    }


}


