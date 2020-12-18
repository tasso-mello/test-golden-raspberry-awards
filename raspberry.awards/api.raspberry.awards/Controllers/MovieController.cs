namespace api.raspberry.awards.Controllers
{
    using common.raspberry.awards.Contracts.Business;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        #region Attributes

        private readonly IMovieBusiness _movieBusiness;

        #endregion Attributes

        #region Constructor

        public MovieController(IMovieBusiness movieBusiness)
        {
            _movieBusiness = movieBusiness;
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Json list</response>
        /// <response code="400">Error to try get by name</response>  
        [HttpGet("All")]
        public async Task<IActionResult> Get()
        {
            var result = await _movieBusiness.Get();

            if (result.ToString().Contains("Error"))
                return BadRequest(result);
            else
                return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Json object</response>
        /// <response code="400">Error to try get by name</response>  
        [HttpGet()]
        public async Task<IActionResult> Get([FromQuery] long id)
        {
            var result = await _movieBusiness.GetById(id);

            if (result.ToString().Contains("Error"))
                return BadRequest(result);
            else
                return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <response code="200">Json object</response>
        /// <response code="400">Error to try get by name</response>  
        [HttpGet("{title}")]
        public async Task<IActionResult> Get(string name)
        {
            var result = await _movieBusiness.GetByName(name);

            if (result.ToString().Contains("Error"))
                return BadRequest(result);
            else
                return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        /// <response code="201">Register is created</response>
        /// <response code="400">Error to try create</response>  
        [HttpPost("{idMovie}")]
        public async Task<IActionResult> Post(common.raspberry.awards.Models.Movie User, long idUser)
        {
            var result = await _movieBusiness.Save(User, idUser);

            if (result.ToString().Contains("Error"))
                return BadRequest(result);
            else
                return Created(string.Empty, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="idUser"></param>
        /// <returns></returns>
        /// <response code="200">Register is edited</response>
        /// <response code="400">Error to try edit</response>
        [HttpPut("{idMovie}")]
        public async Task<IActionResult> Put(common.raspberry.awards.Models.Movie user, long idUser)
        {
            var result = await _movieBusiness.Update(user, idUser);

            if (result.ToString().Contains("Error"))
                return BadRequest(result);
            else
                return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <response code="200">Register is deleted</response>
        /// <response code="400">Error to try delete</response>  
        [HttpDelete]
        public async Task<IActionResult> Delete(common.raspberry.awards.Models.Movie user)
        {
            var result = await _movieBusiness.Delete(user);

            if (result.ToString().Contains("Error"))
                return BadRequest(result);
            else
                return await Delete(user);
        }

        #endregion Public Methods
    }
}
