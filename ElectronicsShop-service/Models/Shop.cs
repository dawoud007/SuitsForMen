using CommonGenericClasses;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Models
{
    public class Shop : BaseEntity
	{

       
        [Required]
        public  string  UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        public string? Role { get; set; }
        [Required]
        public string WhatToSee { get; set; }
        public DateTime CreationDate { get; set; }

        public List<Worker>? workUsers = new List<Worker>();
	}



    public class Worker
    {
        public int WorkerId { get; set; }
        public string? WorkerName { get; set; }
        public string? WhatToSee { get; set; }
    }


}
