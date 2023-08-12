using CommonGenericClasses;
using ElectronicsShop_service.Controllers;
using ElectronicsShop_service.Dtos;
using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Models
{
	public class Bill:BaseEntity

	{
		public Bill() {

			Suits = new List<Cloth>();
		}

	
		public string BuyerName { get; set; }
		public string SellerName { get; set; }
		public string Description { get; set; }
		public int NumberOfPieces { get; set; }
        public DateTime DateCreated { get; set; }
        public MoneyAccountType AccountType { get; set; }

        public string WorkerName { get; set; }
		public int SellingPricee { get; set; }
		public int ProfitDifference { get; set; }
        public ICollection<Cloth> Suits { get; set; }




	}
}
