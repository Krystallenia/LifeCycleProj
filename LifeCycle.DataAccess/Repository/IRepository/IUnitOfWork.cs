namespace LifeCycle.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        ApplicationDbContext Context { get; }

        Task<int> SaveAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollBackTransactionAsync();

    }
}
