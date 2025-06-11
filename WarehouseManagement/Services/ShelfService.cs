using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Data;
using WarehouseManagement.Models;

namespace WarehouseManagement.Services
{
    public class ShelfService
    {
        private readonly AppDbContext _context;
        public ShelfService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Shelf>> GetAllAsync()
        {
            return await _context.Shelves
                .Include(s => s.Locations)
                .ToListAsync();
        }
        public async Task<Shelf> GetByIdAsync(int id)
        {
            return await _context.Shelves
                .Include(s => s.Locations)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Shelf>> GetShelvesByWarehouseIdAsync(int warehouseId)
        {
            return await _context.Shelves
                .Where(s => s.WarehouseId == warehouseId)
                .Include(s => s.Locations) 
                .ToListAsync();
        }

        public async Task AddShelfAsync(Shelf shelf)
        {
            _context.Shelves.Add(shelf);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateShelfAsync(Shelf shelf)
        {
            var existing = await _context.Shelves.FindAsync(shelf.Id);
            if (existing != null)
            {
                existing.Code = shelf.Code;
                existing.Description = shelf.Description;
                await _context.SaveChangesAsync();
            }
        }
        public async Task DeleteShelfAsync(int id)
        {
            var shelf = await _context.Shelves.FindAsync(id);
            if (shelf != null)
            {
                _context.Shelves.Remove(shelf);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AddLocationAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Location>> GetLocationsByShelfIdAsync(int shelfId)
        {
            return await _context.Locations
                .Where(l => l.ShelfId == shelfId)
                .ToListAsync();
        }

    }
}