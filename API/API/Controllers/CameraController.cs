using API.Managers.Abstraction;
using API.Model;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/Camera")]
    [ApiController]
    public class CameraController : ControllerBase
    {
        private readonly ICameraManager _manager;

        public CameraController(ICameraManager manager)
        {
            _manager = manager;
        }

        [HttpGet("GetCameras")]
        public IEnumerable<Camera> GetCameras()
        {
            return _manager.GetCameras();
        }

        [HttpGet("StartCameraStream")]
        public async Task StartCameraStream(int port, CancellationToken cancellationToken)
        {
            try
            {
                Response.Headers.Add("Content-Type", "text/event-stream");

                _manager.StartCapture(port, Response);
                while (!cancellationToken.IsCancellationRequested && _manager.IsStreaming)
                {
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally {
                _manager.StopCapture();
            }
            
        }

        [HttpPost]
        public async void StopCameraStream()
        {
            _manager.StopCapture();
        }


        [HttpGet("GetDetectedChanges/{port}")]
        public List<DetectedChange> GetDetectedChanges(int port)
        {
            return _manager.GetDetectedChangesAsync(port);
        }

        [HttpDelete("ClearImageHistory")]
        public void ClearImageHistory()
        {
            _manager.ClearImageHistory();
        }
    }
}
