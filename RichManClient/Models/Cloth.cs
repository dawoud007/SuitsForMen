﻿using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Models
{
	public class Cloth
	{ 
        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public Cloth() { }
		[Required]
		public string Name { get; set; }

		public string? type { get; set; }
		[Required]
		public int Size { get; set; }

		[Required]
		public string Color { get; set; }

		public int NumOfPieces { get; set; }

		[Required]
		public string StoreName { get; set; }

		public string? Note { get; set; } = "";

		public Guid? BillId { get; set; }





	}
}
