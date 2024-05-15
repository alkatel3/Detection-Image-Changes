using API.Model;

namespace API.Managers.Abstraction
{
    public interface ICameraManager
    {
        void StartCapture(int port);
        void StopCapture();
        string GetProccedImage();
        List<Camera> GetCameras();
    }
}
