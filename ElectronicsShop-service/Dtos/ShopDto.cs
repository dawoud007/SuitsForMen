using ElectronicsShop_service.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Dtos
{
  
    public class ShopDto:BaseDto
    {

        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }
        public string? WhatToSee { get; set; }
        public DateTime CreationDate { get; set; }= DateTime.Now;

      
    }

  
}
