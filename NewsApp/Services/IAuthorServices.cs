using NewsApp.Models;

namespace NewsApp.Services
{
    public interface IAuthorServices
    {
        Task<List<Author>> GetAll();
        Author GetById(int id);
        bool IsExsists(int id);
        Task<Author> Create(Author entity);
        Author Update(Author entity);
        Author Delete(Author entity);
    }
}
