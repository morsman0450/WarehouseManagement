using Microsoft.EntityFrameworkCore;
using System;
using WarehouseManagement.Data;
using WarehouseManagement.Models;

namespace WarehouseManagement.Services
{
    public class ItemService
    {
        private readonly AppDbContext _context;

        public ItemService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _context.Items
                .Include(i => i.Location)
                .ToListAsync();
        }

        public async Task MoveItemAsync(int itemId, int newLocationId)
        {
            var item = await _context.Items.FindAsync(itemId);
            if (item != null)
            {
                item.LocationId = newLocationId;

                await _context.SaveChangesAsync();

            }
        }

        public async Task<bool> MoveItemToWarehouseAsync(int itemId, int targetWarehouseId, int targetLocationId)
        {
            var item = await _context.Items
                .Include(i => i.Location)
                    .ThenInclude(l => l.Shelf)
                .FirstOrDefaultAsync(i => i.Id == itemId);

            if (item == null)
                return false;

            var targetLocation = await _context.Locations
                .Include(l => l.Shelf)
                .FirstOrDefaultAsync(l => l.Id == targetLocationId);

            if (targetLocation == null)
                return false;
            
            if (targetLocation.Shelf.WarehouseId != targetWarehouseId)
                return false;

            item.LocationId = targetLocationId;

            await _context.SaveChangesAsync();
            return true;
        }


        public async Task AddItemAsync(Item item) 
        {
            item.LocationId = item.SelectedLocationId; 
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateItemAsync(Item item)
        {
            var existing = await _context.Items.FindAsync(item.Id);
            if (existing != null)
            {
                existing.Name = item.Name;
                existing.Description = item.Description;
                existing.Quantity = item.Quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            if (item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Location>> GetLocationsByWarehouseAsync(int warehouseId)
        {
            return await _context.Locations
                .Include(l => l.Shelf)
                .Where(l => l.Shelf.WarehouseId == warehouseId)
                .ToListAsync();
        }

        public async Task<List<Location>> GetAllLocationsAsync()
        {
            return await _context.Locations.ToListAsync();
        }
        public async Task<List<Item>> GetItemsByWarehouseAsync(int warehouseId)
        {
            return await _context.Items
                .Include(i => i.Location)
                .ThenInclude(l => l.Shelf)
                .Where(i => i.Location.Shelf.WarehouseId == warehouseId)
                .ToListAsync();
        }


    }
}
