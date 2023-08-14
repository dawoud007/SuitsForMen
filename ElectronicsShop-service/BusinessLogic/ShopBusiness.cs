using CommonGenericClasses;
using ElectronicsShop_service.Interfaces;
using ElectronicsShop_service.Models;
using ElectronicsShop_service.Repositories;

namespace ElectronicsShop_service.BusinessLogic
{
    public class ShopBusiness : BaseUnitOfWork<Shop>, IShopUnitOfWork
    {
        public ShopBusiness(IShopRepository shopRepository) : base(shopRepository)
        {
        }
    }
}
