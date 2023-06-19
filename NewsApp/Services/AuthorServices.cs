using Microsoft.EntityFrameworkCore;
using NewsApp.Models;

namespace NewsApp.Services
{
    public class AuthorServices : IAuthorServices
    {
        private readonly ApplicationDbContext _context;

        public AuthorServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Author>> GetAll()
            => await _context.Authors.ToListAsync();
        public Author GetById(int id)
        { 
            Author author = _context.Authors.FirstOrDefault(auth=>auth.Id == id);

            return author;
        }

        public bool IsExsists(int id)
        {
            Author author = GetById(id);
            return author != null;
        }
        public async Task<Author> Create(Author entity)
        {
            await _context.Authors.AddAsync(entity);
            _context.SaveChanges();
            return entity;
        }

        public Author Update(Author entity)
        {
            _context.Authors.Update(entity);
            _context.SaveChanges();
            return entity;
        }

        public Author Delete(Author entity)
        {
            _context.Authors.Remove(entity);
            _context.SaveChanges();
            return entity;
        }

        
    }
}
