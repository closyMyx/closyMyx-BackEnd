using closymyx.DAL.Data;
using closymyx.DAL.Entities;
using Microsoft.EntityFrameworkCore;

public class TopService
{
    private readonly AppDbContext _context;

    public TopService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> AddToTop(Guid userId, Guid productId)
    {
        var exists = await _context.Top
            .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

        if (exists)
            return false; 

        _context.Top.Add(new Top
        {
            UserId = userId,
            ProductId = productId
        });

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveFromTop(Guid userId, Guid productId)
    {
        var top = await _context.Top
            .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

        if (top == null)
            return false;

        _context.Top.Remove(top);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<Product>> GetUserTop(Guid userId)
    {
        return await _context.Top
            .Where(f => f.UserId == userId)
            .Select(f => f.Product)
            .ToListAsync();
    }
}
