namespace data.raspberry.awards.Repository
{
    using data.raspberry.awards.Context;
    using data.raspberry.awards.Entities;
    using data.raspberry.awards.Infrastructure;

    public interface IMovieRepository : IRepository<Movie> { }
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(RaspberryContext dbContext) : base(dbContext) { }
    }
}
