using BookMyCut.Data.Models;

namespace BookMyCut.Data.Repositories
{
    public interface IServiceRepository
    {
        Task<List<Service>> ObtenirTousAsync();
        Task AjouterAsync(Service service);
        Task ModifierAsync(Service service);
        Task SupprimerAsync(int id);
    }
}