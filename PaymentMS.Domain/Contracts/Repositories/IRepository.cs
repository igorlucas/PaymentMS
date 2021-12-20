namespace PaymentMS.Domain.Contracts.Repositories
{
    public interface IRepository<T> : IDisposable
    {   
        IList<T> Get();
        T Get(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}