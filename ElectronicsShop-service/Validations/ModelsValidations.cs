using ElectronicsShop_service.Models;
using FluentValidation;

namespace ElectronicsShop_service.Validations
{
   


	public class ClothValidations : AbstractValidator<Cloth>
	{
		public ClothValidations()
		{

		}
	}
	public class BillValidations : AbstractValidator<Bill>
	{
		public BillValidations()
		{

		}
	}
    public class MoneyValidations : AbstractValidator<Money>
    {
        public MoneyValidations()
        {

        }
    }  public class ShopValidations : AbstractValidator<Shop>
    {
        public ShopValidations()
        {

        }
    }

}