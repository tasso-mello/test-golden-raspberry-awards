namespace common.raspberry.awards.Contracts.Base
{
    using System.Threading.Tasks;

    public interface IGenericReadBusiness<TEntity> where TEntity : class
    {
        Task<object> Get();
        Task<object> GetById(long id);
        Task<object> GetByName(string name);
    }
}