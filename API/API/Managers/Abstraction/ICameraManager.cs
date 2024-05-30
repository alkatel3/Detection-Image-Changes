using API.Model;

namespace API.Managers.Abstraction
{
    public interface ICameraManager
    {
        void StartCapture(int port, HttpResponse response);
        void StopCapture();
        void GetProccedImage();
        List<Camera> GetCameras();
        List<DetectedChange> GetDetectedChangesAsync(int port);
        void ClearImageHistory();
    }
}
