﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ElectronicsShop_service.Dtos
{
	public class BaseDtos
	{

	}
	public class ClothDto : BaseDto
    {

		public string? Name { get; set; }

		public string? type { get; set; }
		public int Size { get; set; }

		public string? Color { get; set; }
        public Guid? BillId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int NumOfPieces { get; set; }
        public int? limit { get; set; } = 10;
        public int Gomla { get; set; }
        public string? Note { get; set; } = "";
        public string? StoreName { get; set; }
	}

	public class ClothDtos 
	{

		public string? Name { get; set; }

		public string? type { get; set; }
		public int Size { get; set; }

		public string? Color { get; set; }

		public int NumOfPieces { get; set; }

		public string? StoreName { get; set; }
	}
}
