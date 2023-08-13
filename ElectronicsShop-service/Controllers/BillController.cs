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
using System.Collections.Generic;
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
    private readonly ApplicationDbContext _dbContext;
    public BillController(IBillRepository billRepository, IClothRepository clothRepository,
        IBillUnitOfWork unitOfWork, IMapper mapper,
        IValidator<Bill> validator, ApplicationDbContext dbContext) : base(unitOfWork, mapper, validator)
	{
		_billRepository = billRepository;
		_clothRepository = clothRepository;
        _dbContext = dbContext;
    }

    [Authorize(AuthenticationSchemes = "Bearer")]
	[HttpGet]
    public async Task<IActionResult> GetAll()
	{
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;

  

        var ShopBills = (await _billRepository.Get(b => b.SellerName == whatToSeeValue)).ToList();
        if (!ShopBills.Any())
        {
            return Ok("no bills for this shop");
        }
        foreach (var bill in ShopBills)
		{
			var thing = (await _clothRepository.Get(c => c.BillId == bill.Id)).ToList();
			bill.Suits = thing;



		}
		return Ok(ShopBills);
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
    
        int TotalSellingPrice=0;
		var list=new List<Cloth>();

		if(billDto.Suits.Count() ==0)
        {
            return Ok("you must add a least a piece");
        }
		foreach(var suit in billDto.Suits!)
		{
          
			list.Add(suit);

		}
		var bill = new Bill
		{
            WorkerName = billDto.WorkerName,
            BuyerName = billDto.BuyerName!,
            SellerName = billDto.SellerName,
            Description = billDto.Description!,
            SellingPricee = billDto.SellingPricee,
            DateCreated = DateTime.Now,
            Suits = list,

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
                  
                        TotalSellingPrice += suits.Gomla;
                    
					if (suitDto != null)
					{
						if (suitDto.NumOfPieces <= suits.NumOfPieces)
						{
							suits.NumOfPieces -=suitDto.NumOfPieces;
                        
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
            bill.ProfitDifference = (bill.SellingPricee - TotalSellingPrice);
            await _billRepository.Save();
            MoneyUpdateDto AddToSAve = new MoneyUpdateDto
            {
                AccountType = billDto.AccountType,
                Amount = bill.SellingPricee,
                ShopName = whatToSeeValue
            };
            AddMoneyToAccount(AddToSAve);


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

            var ProfitsInMonth = (await _billRepository.Get(c =>
                c.DateCreated >= firstDayOfMonth && c.DateCreated <= lastDayOfMonth && c.SellerName == whatToSeeClaim,
                null,
                "")).ToList().Sum(b=>b.ProfitDifference);
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
                    TotalPieces = groupedCloth.Sum(cloth => cloth.NumOfPieces),
                    TotalProfits= ProfitsInMonth
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


            var ProfitsInWeek = (await _billRepository.Get(c =>
              c.DateCreated >= firstDayOfWeek && c.DateCreated <= lastDayOfWeek && c.SellerName == whatToSeeClaim,
              null,
              "")).ToList().Sum(b => b.ProfitDifference);
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
                    TotalPieces = groupedCloth.Sum(cloth => cloth.NumOfPieces),
                    TotalProfits= ProfitsInWeek
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










    [Authorize(AuthenticationSchemes = "Bearer")]

    [HttpPost("add-money")]
    public async Task<IActionResult> AddMoneyToAccount([FromBody] MoneyUpdateDto moneyUpdateDto)
    {
        if (moneyUpdateDto == null)
        {
            return BadRequest("Invalid data");
        }

        // Use a separate DbContext instance within the method scope
        using (var dbContext = new ApplicationDbContext()) // Replace YourDbContext with your actual DbContext class
        {
            var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
            var money = dbContext.Moneys.FirstOrDefault(m => m.ShopName == whatToSeeValue);
            if (money == null)
            {
                money = new Money(); // Create a new Money instance with default values
                money.ShopName = moneyUpdateDto.ShopName;
                dbContext.Moneys.Add(money); // Add it to the database context
            }

            if (moneyUpdateDto.AccountType == MoneyAccountType.Box)
            {
                money.AddMoneyToBox(moneyUpdateDto.Amount);
            }
            else if (moneyUpdateDto.AccountType == MoneyAccountType.Visa)
            {
                money.AddMoneyToVisa(moneyUpdateDto.Amount);
            }
            else
            {
                return BadRequest("Invalid account type");
            }

            await dbContext.SaveChangesAsync();

            return Ok("Money added successfully");
        }
    }


    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpDelete("delete-money")]
    public async Task<IActionResult> DeleteMoneyFromAccount([FromBody] MoneyUpdateDto moneyUpdateDto)
    {
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
      
        if (moneyUpdateDto == null)
        {
            return BadRequest("Invalid data");
        }

        var money = _dbContext.Moneys!.Where(m => m.ShopName == whatToSeeValue).FirstOrDefault(); // Adjust as needed


        if (money == null)
        {
            return NotFound("Money data not found");
        }

        if (moneyUpdateDto.AccountType == MoneyAccountType.Box)
        {
            money.RemoveMoneyFromBox(moneyUpdateDto.Amount);
        }
        else if (moneyUpdateDto.AccountType == MoneyAccountType.Visa)
        {
            money.RemoveMoneyFromVisa(moneyUpdateDto.Amount);
        }
        else
        {
            return BadRequest("Invalid account type");
        }

        await _dbContext.SaveChangesAsync();

        return Ok("Money deleted successfully");
    }


 

    [Authorize(AuthenticationSchemes = "Bearer")]
    [HttpGet("get-money")]
    public IActionResult GetMoney()
    {
        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
        var money = _dbContext.Moneys!.Where(m => m.ShopName == whatToSeeValue).FirstOrDefault(); // Adjust as needed

        if (money == null)
        {
            return NotFound("Money data not found");
        }

        var moneyData = new
        {
            MoneyInBox = money.MoneyInBox,   // Assuming you have a property MoneyInBox in your Money class
            MoneyInVisa = money.MoneyInVisa  // Assuming you have a property MoneyInVisa in your Money class
        };

        return Ok(moneyData);
    }




    /*
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        public async Task<IActionResult> GetStaticsBasedOnBill(PeriodType periodType)
        {
            List<Cloth> suits = new();
            var whatToSeeClaim = User.FindFirst("WhatToSee")?.Value;

            var currentDate = DateTime.Now;

            if (periodType == PeriodType.Month)
            {
                var firstDayOfMonth = currentDate.AddDays(1 - currentDate.Day).Date;
                var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

                var monthlyStatistics = (await _billRepository.Get(c =>
                    c.DateCreated >= firstDayOfMonth && c.DateCreated <= lastDayOfMonth && c.SellerName == whatToSeeClaim,
                    null,
                    "")).ToList();
              foreach(var bill in monthlyStatistics)
                {

                    bill.Suits.ToList();
                    suits.AddRange(bill.Suits); 
                }

                var groupedMonthlyStatistics = suits
                    .GroupBy(cloth => new { cloth.Name, cloth.Color, cloth.Size })
                    .Select(groupedCloth => new GroupedStatistics
                    {
                        Name = groupedCloth.Key.Name,
                        Color = groupedCloth.Key.Color,
                        Size = groupedCloth.Key.Size,
                        TotalPieces = groupedCloth.Sum(cloth => cloth.NumOfPieces),
                        TotalProfits =groupedCloth.Sum(cloth => cloth.NumOfPieces),
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


                var ProfitsInWeek = (await _billRepository.Get(c =>
                  c.DateCreated >= firstDayOfWeek && c.DateCreated <= lastDayOfWeek && c.SellerName == whatToSeeClaim,
                  null,
                  "")).ToList().Sum(b => b.ProfitDifference);
                var weeklyStatistics = (await _clothRepository.Get(c =>
                    c.DateCreated >= firstDayOfWeek && c.DateCreated <= lastDayOfWeek && c.BillId != null && c.StoreName == whatToSeeClaim,
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
                        TotalPieces = groupedCloth.Sum(cloth => cloth.NumOfPieces),
                        TotalProfits = ProfitsInWeek
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
    */






    [NonAction]
    public async Task<IActionResult> AddMoneyFromBillAdding(MoneyUpdateDto moneyUpdateDto)
    {
        if (moneyUpdateDto == null)
        {
            return BadRequest("Invalid data");
        }

        var whatToSeeValue = User.FindFirst("WhatToSee")?.Value;
        var money = _dbContext.Moneys
            .Where(m => m.ShopName == whatToSeeValue)
            .FirstOrDefault();

        if (money == null)
        {
            money = new Money
            {
                ShopName = whatToSeeValue
            };

            _dbContext.Moneys.Add(money);
        }

        try
        {
            if (moneyUpdateDto.AccountType == MoneyAccountType.Box)
            {
                money.AddMoneyToBox(moneyUpdateDto.Amount);
            }
            else if (moneyUpdateDto.AccountType == MoneyAccountType.Visa)
            {
                money.AddMoneyToVisa(moneyUpdateDto.Amount);
            }
            else
            {
                return BadRequest("Invalid account type");
            }

            await _dbContext.SaveChangesAsync();

            return Ok("Money added successfully");
        }
        catch (Exception ex)
        {
            // Handle exception
            return StatusCode(500, "An error occurred while processing the request.");
        }
    }









}


public class GroupedStatistics
{
    public string Name { get; set; }
    public string Color { get; set; }
    public int Size { get; set; }
    public int TotalPieces { get; set; }
    public int TotalProfits { get; set; }
    public string Label { get; set; }
}



public enum PeriodType
{
    Month,
    Week
}

public class MoneyUpdateDto
{
    public MoneyAccountType AccountType { get; set; }
    public int Amount { get; set; }
    public string? ShopName { get; set; }
}

public enum MoneyAccountType
{
    Box,
    Visa
}