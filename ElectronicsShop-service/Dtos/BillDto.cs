using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using ElectronicsShop_service.Models;

namespace ElectronicsShop_service.Dtos
{
    public class BillDto : BaseDto
    {


        public string BuyerName { get; set; }
        public string SellerName { get; set; }
        public string Description { get; set; }
        public int NumberOfPieces { get; set; }
        public DateTime DateCreated { get; set; }

        // Relationship: Each Bill contains multiple Cloth items
        public string? WarningToNumberOfPieces { get; set; }
        public int? limit { get; set; } = 6;
        public List<Cloth>? Suits { get; set; }
	}
	
}
