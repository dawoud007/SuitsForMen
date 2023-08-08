using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ElectronicsShop_service.Dtos
{
	public class BaseDtos
	{

	}
	public class ClothDto 
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

		public string? type { get; set; }
		public int Size { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string? Color { get; set; }
        public Guid? BillId { get; set; }
        
        public int NumOfPieces { get; set; }
        public string? Note { get; set; } = "";
        public string? StoreName { get; set; }
	}

	
}
