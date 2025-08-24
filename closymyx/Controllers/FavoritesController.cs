using Microsoft.AspNetCore.Mvc;
using closymyx.DAL.Entities;
using closymyx.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace closymyx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserFavorites(Guid userId)
        {
            var favorites = await _context.Favorites
                .Where(f => f.UserId == userId)
                .Select(f => new
                {
                    f.Product.Id,
                    f.Product.Name,
                    Category = f.Product.Category.Name
                })
                .ToListAsync();

            return Ok(favorites);
        }

        [HttpPost("{userId}/{productId}")]
        public async Task<IActionResult> AddToFavorites(Guid userId, Guid productId)
        {
            var exists = await _context.Favorites
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

            if (exists)
                return BadRequest("Этот товар уже в избранном.");

            var favorite = new Favorite
            {
                UserId = userId,
                ProductId = productId
            };

            _context.Favorites.Add(favorite);
            await _context.SaveChangesAsync();

            return Ok("Товар добавлен в избранное.");
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromFavorites(Guid userId, Guid productId)
        {
            var favorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (favorite == null)
                return NotFound("Товар не найден в избранном.");

            _context.Favorites.Remove(favorite);
            await _context.SaveChangesAsync();

            return Ok("Товар удалён из избранного.");
        }
    }
}
