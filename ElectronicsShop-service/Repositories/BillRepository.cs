using CommonGenericClasses;
using ElectronicsShop_service.Interfaces;
using ElectronicsShop_service.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop_service.Repositories;
public class BillRepository : BaseRepository<Bill>, IBillRepository
{
    private readonly ApplicationDbContext _context;
    public BillRepository(ApplicationDbContext context) : base(context)
    {

        _context = context;
    }
}