using closymyx.DAL.Data;
using closymyx.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class FavoriteService
{
    private readonly AppDbContext _context;

    public FavoriteService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddToFavorites(Guid userId, Guid productId)
    {
        var exists = await _context.Favorites
            .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

        if (exists)
            return false; 

        _context.Favorites.Add(new Favorite
        {
            UserId = userId,
            ProductId = productId
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromFavorites(Guid userId, Guid productId)
    {
        var favorite = await _context.Favorites
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (favorite == null)
            return false;

        _context.Favorites.Remove(favorite);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetUserFavorites(Guid userId)
    {
        return await _context.Favorites
            .Where(f => f.UserId == userId)
            .Select(f => f.Product)
            .ToListAsync();
    }
}
