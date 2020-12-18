namespace common.raspberry.awards.Contracts.Business
{
    using common.raspberry.awards.Contracts.Base;
    using common.raspberry.awards.Models;

    public interface IMovieBusiness: IGenericReadBusiness<Movie>, IGenericPersistBusiness<Movie> { }
}
