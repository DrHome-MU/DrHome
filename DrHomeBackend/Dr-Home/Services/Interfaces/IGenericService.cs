using System.Linq.Expressions;

namespace Dr_Home.Services.Interfaces
{
    public interface IGenericService<T> where T : class
    {
        Task<T> GetById(int id);

        Task<IEnumerable<T>> GetAll();
       

        Task<T> Add(T entity);
       

        T Update(T entity);
        //  T Add(T entity);
        void Delete(T entity);
    }
}
