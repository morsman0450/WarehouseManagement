using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Data;
using WarehouseManagement.Models;

namespace WarehouseManagement.Services
{
    public class WarehouseService
    {
        private readonly AppDbContext _context;

        public WarehouseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Warehouse>> GetWarehouseAllAsync()
        {
            return await _context.Warehouses
                .Include(w => w.Shelves)
                .ThenInclude(s => s.Locations)
                .ToListAsync();
        }

        public async Task<Warehouse> GetWarehouseByIdAsync(int id)
        {
            if (id != null)
            {
                return await _context.Warehouses
                    .Include(w => w.Shelves)
                    .ThenInclude(s => s.Locations)
                    .FirstOrDefaultAsync(w => w.Id == id);
            }
            else
            {
                return null;
            }
        }
        public async Task AddWarehouseAsync(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateWarehouseAsync(Warehouse warehouse)
        {
            var existing = await _context.Warehouses.FindAsync(warehouse.Id);
            if (existing != null)
            {
                existing.Name = warehouse.Name;
                existing.Description = warehouse.Description;
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteWarehouseAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse != null)
            {
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
            }
        }

    }
}
