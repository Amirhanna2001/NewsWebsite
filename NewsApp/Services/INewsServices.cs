using NewsApp.Models;

namespace NewsApp.Services
{
    public interface INewsServices
    {
        Task<List<News>> GetAll();
        Task<List<News>> GetByAuthorId(int id);

        News GetById(int id);
        Task<News> Create(News entity);
        News Update(News entity);
        News Delete(News entity);
    }
}
