using CommonGenericClasses;
using ElectronicsShop_service.Interfaces;
using ElectronicsShop_service.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop_service.Repositories;
public class ShopRepository : BaseRepository<Shop>, IShopRepository
{
    private readonly ApplicationDbContext _context;
    public ShopRepository(ApplicationDbContext context) : base(context)
    {

        _context = context;
    }
}