using API.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<Camera> GetCameras()
        {
            var res = new List<Camera>
            {
                new Camera
                {
                    ID = Guid.NewGuid(),
                    Name ="Camera 1",
                    Src = "../../../../../assets/images/mov_bbb.mp4"
                },
                new Camera
                {
                    ID = Guid.NewGuid(),
                    Name ="Camera 2",
                    Src = "../../../../../assets/images/mov_bbb.mp4"
                },
                new Camera
                {
                    ID = Guid.NewGuid(),
                    Name ="Camera 3",
                    Src = "../../../../../assets/images/mov_bbb.mp4"
                },
            };

            return res;
        }
    }
}
