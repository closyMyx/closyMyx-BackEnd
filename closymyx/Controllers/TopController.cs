using Microsoft.AspNetCore.Mvc;
using closymyx.DAL.Entities;
using closymyx.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace closymyx.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TopController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserTop(Guid userId)
        {
            var top = await _context.Top
                .Where(f => f.UserId == userId)
                .Select(f => new
                {
                    f.Product.Id,
                    f.Product.Name,
                    Category = f.Product.Category.Name
                })
                .ToListAsync();

            return Ok(top);
        }

        [HttpPost("{userId}/{productId}")]
        public async Task<IActionResult> AddToTop(Guid userId, Guid productId)
        {
            var exists = await _context.Top
                .AnyAsync(f => f.UserId == userId && f.ProductId == productId);

            if (exists)
                return BadRequest("Этот товар уже в списке Top.");

            var top = new Top
            {
                UserId = userId,
                ProductId = productId
            };

            _context.Top.Add(top);
            await _context.SaveChangesAsync();

            return Ok("Товар добавлен в список Top.");
        }

        [HttpDelete("{userId}/{productId}")]
        public async Task<IActionResult> RemoveFromTop(Guid userId, Guid productId)
        {
            var top = await _context.Top
                .FirstOrDefaultAsync(f => f.UserId == userId && f.ProductId == productId);

            if (top == null)
                return NotFound("Товар не найден в списке Top.");

            _context.Top.Remove(top);
            await _context.SaveChangesAsync();

            return Ok("Товар удалён из списка Top.");
        }
    }
}
