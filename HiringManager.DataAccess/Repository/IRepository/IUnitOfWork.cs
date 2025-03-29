namespace HiringManager.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        IApplicationRepository Application { get; }
        void Save();
    }
}