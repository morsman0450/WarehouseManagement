using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Data;
using WarehouseManagement.Models;

namespace WarehouseManagement.Services
{
    public class LocationService
    {
        private readonly AppDbContext _context;
        public LocationService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Location?> GetLocationByIdAsync(int id)
        {
            return await _context.Locations
                .Include(l => l.Items)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
        public async Task<Location> CreateLocationAsync(int shelfId, string? description = null)
        {
            var shelf = await _context.Shelves.FindAsync(shelfId);
            if (shelf == null) throw new Exception("Regál neexistuje");

            var existingCodes = await _context.Locations
                .Where(l => l.ShelfId == shelfId)
                .Select(l => l.Code)
                .ToListAsync();

            int maxIndex = 0;
            foreach (var code in existingCodes)
            {
                var parts = code.Split('-');
                if (parts.Length == 2 && int.TryParse(parts[1], out int number))
                {
                    if (number > maxIndex)
                        maxIndex = number;
                }
            }

            var location = new Location
            {
                ShelfId = shelfId,
                Code = $"{shelf.Code}-{maxIndex + 1:D2}",
                Description = description
            };

            _context.Locations.Add(location);
            await _context.SaveChangesAsync();

            return location;
        }

        public async Task UpdateLocationAsync(Location location)
        {
            var existingLocation = await _context.Locations.FindAsync(location.Id);
            if (existingLocation == null)
            {
                throw new Exception("Location not found.");
            }

            existingLocation.Description = location.Description;

            _context.Entry(existingLocation).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteLocationAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null)
                throw new Exception("Lokace nebyla nalezena");

            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
        }

    }
}
