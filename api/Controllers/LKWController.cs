using Microsoft.AspNetCore.Mvc;

using api.Models;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LKWController : ControllerBase
    {
        // GET api/LKW
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "LKW";
        }

        // POST api/LKW
        [HttpPost]
        public ActionResult<CreationReward> Post(LKWCreationDto lkw)
        {
            CreationReward reward = new CreationReward(true, 10);

            return Ok(reward);
        }

    }
}