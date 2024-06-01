using API.Model;

namespace API.Managers.Abstraction
{
    public interface ICameraManager
    {
        public bool IsStreaming { get; }

        void StartCapture(int port, HttpResponse response);
        void StopCapture();
        List<Camera> GetCameras();
        List<DetectedChange> GetDetectedChangesAsync(int port);
        void ClearImageHistory();
    }
}
