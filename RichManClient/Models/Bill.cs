using System.ComponentModel.DataAnnotations;

namespace ElectronicsShop_service.Models
{
	public class Bill

	{
		public Bill() {

			Suits = new List<Cloth>();
		}

        public Guid Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;
        public string BuyerName { get; set; }
		public string SellerName { get; set; }
		public string Description { get; set; }
		public int NumberOfPieces { get; set; }

		// Relationship: Each Bill contains multiple Cloth items
		public ICollection<Cloth> Suits { get; set; }




	}
}
