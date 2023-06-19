using Microsoft.EntityFrameworkCore;
using NewsApp.Models;

namespace NewsApp.Services
{
    public class NewsServices : INewsServices
    {
        private readonly ApplicationDbContext _context;

        public NewsServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<News>> GetAll()
            => await _context.News.ToListAsync();

        public async Task<List<News>> GetByAuthorId(int id)
        {
            return await _context.News.Where(n=>n.AuthorId == id).ToListAsync();
        }
        
        public News GetById(int id)
        {
            News news =_context.News.FirstOrDefault(x => x.Id == id);
            return news;
        }
        public async Task<News> Create(News entity)
        {
            await _context.News.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }
        public News Update(News entity)
        {
            _context.News.Update(entity);
            _context.SaveChanges();
            return entity;
        }
        public News Delete(News entity)
        {
            _context.News.Remove(entity);
            File.Delete(entity.ImagePath);
            _context.SaveChanges();
            return entity;
        }

        
    }
}
