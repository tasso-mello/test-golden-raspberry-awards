namespace common.raspberry.awards.Business
{
    using data.raspberry.awards.Repository;
    using common.raspberry.awards.Contracts.Business;
    using common.raspberry.awards.Models;
    using common.raspberry.awards.Utilities;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class MovieBusiness : IMovieBusiness
    {
        #region Attributes

        private readonly ILogger<Movie> _logger;
        private readonly IMovieRepository _movieRepository;

        #endregion Attributes

        #region Constructor

        public MovieBusiness(ILogger<Movie> logger, IMovieRepository MovieRepository)
        {
            _logger = logger;
            _movieRepository = MovieRepository;
        }

        #endregion

        #region Public Methods

        public async Task<object> Get()
        {
            try
            {
                return Messages.GenerateGenericSuccessObjectMessage("Movie", _movieRepository.GetAll().Select(e => e.ToModelMovie()), 200);
            }
            catch (NullReferenceException e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return Messages.GenerateGenericNullErrorMessage("Movie", "O registro não existe.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return (e.Message != null && e?.InnerException?.Message != null) ? Messages.GenerateGenericErrorMessage(e.Message, e.InnerException.Message) : Messages.GenerateGenericErrorMessage(e.Message ?? e.InnerException.Message);
            }
        }

        public async Task<object> GetById(long id)
        {
            try
            {
                return Messages.GenerateGenericSuccessObjectMessage("Movie", _movieRepository.Get(p => p.Id == id).ToModelMovie(), 200);
            }
            catch (NullReferenceException)
            {
                return Messages.GenerateGenericNullErrorMessage("Movie", "O registro não existe.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return (e.Message != null && e?.InnerException?.Message != null) ? Messages.GenerateGenericErrorMessage(e.Message, e.InnerException.Message) : Messages.GenerateGenericErrorMessage(e.Message ?? e.InnerException.Message);
            }
        }

        public async Task<object> GetByName(string name)
        {
            try
            {
                return Messages.GenerateGenericSuccessObjectMessage("Movie", _movieRepository.GetMany(e => e.Title.Contains(name)).Select(e => e.ToModelMovie()), 200);
            }
            catch (NullReferenceException)
            {
                return Messages.GenerateGenericNullErrorMessage("Movie", "O registro não existe.");
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return (e.Message != null && e?.InnerException?.Message != null) ? Messages.GenerateGenericErrorMessage(e.Message, e.InnerException.Message) : Messages.GenerateGenericErrorMessage(e.Message ?? e.InnerException.Message);
            }
        }

        public async Task<object> Save(Movie obj, long idMovie)
        {
            try
            {
                _movieRepository.Add(obj.ToEntityMovie(), idMovie);
                _movieRepository.SaveChanges();
                return Messages.GenerateGenericSuccessObjectMessage("Movie", "Sucesso", 201);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return (e.Message != null && e?.InnerException?.Message != null) ? Messages.GenerateGenericErrorMessage(e.Message, e.InnerException.Message) : Messages.GenerateGenericErrorMessage(e.Message ?? e.InnerException.Message);
            }
        }

        public async Task<object> Update(Movie obj, long idMovie)
        {
            try
            {
                _movieRepository.Update(obj.ToEntityMovie(), idMovie);
                _movieRepository.SaveChanges();
                return Messages.GenerateGenericSuccessObjectMessage("Movie", "Sucesso", 200);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return (e.Message != null && e?.InnerException?.Message != null) ? Messages.GenerateGenericErrorMessage(e.Message, e.InnerException.Message) : Messages.GenerateGenericErrorMessage(e.Message ?? e.InnerException.Message);
            }
        }

        public async Task<object> Delete(Movie obj)
        {
            try
            {
                _movieRepository.Delete(obj.ToEntityMovie());
                _movieRepository.SaveChanges();
                return Messages.GenerateGenericSuccessObjectMessage("Movie", "Sucesso", 200);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message, null);
                return (e.Message != null && e?.InnerException?.Message != null) ? Messages.GenerateGenericErrorMessage(e.Message, e.InnerException.Message) : Messages.GenerateGenericErrorMessage(e.Message ?? e.InnerException.Message);
            }
        }
        
        #endregion Public Methods
    }
}
