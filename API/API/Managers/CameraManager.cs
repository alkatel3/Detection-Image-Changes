using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.IO;
using Emgu.CV;
using System.Text;
using API.Managers.Abstraction;
using Emgu.CV.CvEnum;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using API.Model;
using System.Text.RegularExpressions;
using Azure;
using API.DAL;
using Microsoft.EntityFrameworkCore;
using Emgu.CV.Ocl;
using Emgu.CV.BgSegm;
using Emgu.CV.Cuda;
using Emgu.CV.Dai;
using API.DAL.Abstraction;

namespace API.Managers
{
    public class CameraManager : ICameraManager
    {
        private VideoCapture capture;
        private Size frameSize = new Size(640, 480);
        private Mat smoothFrame;
        private Mat frame;
        private Mat avgFrame;
        private bool streamVideo =false;
        private int minBBoxArea = 2000;
        private HttpResponse _response;
        private int frameCounter = 0;
        private int updatePeriod = 150; // Період перевірки (кадри
        Mat diffFrame = new Mat();
        public IUnitOfWork _unitOfWork { get; set; }

        public bool IsStreaming => streamVideo;

        public CameraManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public List<Camera> GetCameras()
        {
            List<Camera> availableCameras = new List<Camera>();
            for (int i = 0; i < 20; i++) // Перебрати перші 20 можливих камер
            {
                using (VideoCapture capture = new VideoCapture(i))
                {
                    if (capture.IsOpened)
                    {
                        var frame = new Mat();
                        capture.Read(frame);
                        availableCameras.Add(new Camera
                        {
                            ID = Guid.NewGuid(),
                            Port = i,
                            Src  = MatToBase64String(frame),
                            Name=capture.BackendName,
                            Width =frame.Width,
                            Height =frame.Height
                        });
                    }
                }
            }
            return availableCameras;
        }
        public List<DetectedChange> GetDetectedChangesAsync(int port)
        {
           return _unitOfWork.DetectedChanges.GetAll().OrderByDescending(i => i.Happened).ToList();
            //_dbContext.DetectedChanges.OrderByDescending(i =>i.Happened).ToList();
        }

        public void StartCapture(int port, HttpResponse response)
        {
            capture = new VideoCapture(port);
            _response = response;
            //capture = new VideoCapture(@"C:/Users/Dell/Downloads/Explosion_Fighting_Shooting.mp4");
            //capture = new VideoCapture(@"C:/Users/Dell/Downloads/pieces-in-ocean-sea.webm");
            //capture = new VideoCapture(@"C:/Users/Dell/Downloads/Road_Cars_Traffic.mp4");
           //capture = new VideoCapture(@"C:/Users/Dell/Downloads/videoplayback.mp4");
            frameSize = new Size(capture.Width,capture.Height);
            streamVideo = true;
            capture.ImageGrabbed += Capture_ImageGrabbed;
            capture.Start();
            capture.Set(CapProp.Fps, 100);
            avgFrame = new Mat(frameSize, DepthType.Cv8U, 1);
        }
        private void Capture_ImageGrabbed(object? sender, EventArgs e)
        {
            try
            {
                if (streamVideo)
                {
                    GetProccedImage();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void StopCapture()
        {
            streamVideo = false;
            capture.ImageGrabbed -= Capture_ImageGrabbed;
            capture.Stop();
            capture.Dispose();
        }

        public void ClearImageHistory()
        {
            var allEntities = _unitOfWork.DetectedChanges.GetAll();//_dbContext.DetectedChanges.ToList();
            _unitOfWork.DetectedChanges.RemoveRange(allEntities);
            _unitOfWork.Save();
            //_dbContext.DetectedChanges.RemoveRange(allEntities);
            //_dbContext.SaveChanges();
        }

        private async void GetProccedImage()
        {
            try
            {
                frame = capture?.QueryFrame();

                if (frame == null || frame.IsEmpty)
                {
                    return;
                }
                var imageSting = MatToBase64String(frame);
                _response.WriteAsync($"data: {imageSting}\n\n");
                _response.Body.FlushAsync();
                frameCounter++;
                // Нормалізація освітлення та підвищення контрасту
                smoothFrame = new Mat();
                CvInvoke.GaussianBlur(frame, smoothFrame, new System.Drawing.Size(3, 3), 0);
                CvInvoke.CvtColor(smoothFrame, smoothFrame, ColorConversion.Bgr2Gray);
                CvInvoke.EqualizeHist(smoothFrame, smoothFrame);
                if (frameCounter <= updatePeriod)
                {
                    if (avgFrame.NumberOfChannels != 1)
                    {
                        CvInvoke.CvtColor(avgFrame, avgFrame, ColorConversion.Bgr2Gray);
                    }
                    CvInvoke.AddWeighted(smoothFrame, 1.0 / updatePeriod, avgFrame, 1.0 - 1.0 / updatePeriod, 0, avgFrame);
                return;
                }

                CvInvoke.AbsDiff(smoothFrame, avgFrame, diffFrame);
                CvInvoke.Threshold(diffFrame, diffFrame, 100, 255, ThresholdType.Binary);
                //FilterNoise
                CvInvoke.MorphologyEx(diffFrame, diffFrame, MorphOp.Open,
                Mat.Ones(21, 21, DepthType.Cv8U, 1), new Point(-1, -1), 1,
                BorderType.Reflect, new MCvScalar(0));
                Mat kernel = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new System.Drawing.Size(51, 51), new System.Drawing.Point(-1, -1));
                CvInvoke.Dilate(diffFrame, diffFrame, kernel, new System.Drawing.Point(-1, -1), 2, BorderType.Constant, new MCvScalar(0));
                BoundingBoxes();
                avgFrame = smoothFrame.Clone();
                frameCounter = 0;
            }
            catch (Exception ex)
            {
                streamVideo = false;
                //throw ex;
            }
        }

        private string MatToBase64String(Mat frame)
        {
            var image = frame.ToBitmap();
            using (MemoryStream ms = new MemoryStream())
            {
                image?.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms?.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void BoundingBoxes()
        {
            CvInvoke.Resize(frame, frame, frameSize);
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(diffFrame, contours, null, RetrType.External,
                ChainApproxMethod.ChainApproxSimple);
            bool detected = false;
            for (int i = 0; i < contours.Size; i++)
            {
                var bbox = CvInvoke.BoundingRectangle(contours[i]);
                var area = bbox.Width * bbox.Height;
                var aspectRatio = (double)bbox.Width / bbox.Height;

                if (aspectRatio < 4.0 && area > minBBoxArea && area < frame.Width*frame.Height)
                {
                    detected = true;
                    CvInvoke.Rectangle(diffFrame, bbox, new MCvScalar(255, 255, 255), 2);
                    CvInvoke.Rectangle(frame, bbox, new MCvScalar(0, 0, 255), 2);
                }
            }
            if (detected)
            {
                var stringImage = MatToBase64String(frame);
                _unitOfWork.DetectedChanges.Add(new DetectedChange
                {
                    ID = Guid.NewGuid(),
                    Happened = DateTime.Now,
                    Image = stringImage
                });
                //stringImage = MatToBase64String(diffFrame);
                //_unitOfWork.DetectedChanges.Add(new DetectedChange
                //{
                //    ID = Guid.NewGuid(),
                //    Happened = DateTime.Now,
                //    Image = stringImage
                //});
                _unitOfWork.Save();
                //_dbContext.DetectedChanges.Add(new DetectedChange
                //{
                //    ID = Guid.NewGuid(),
                //    Happened = DateTime.Now,
                //    Image = stringImage
                //});
                //_dbContext.SaveChanges();

                //stringImage = MatToBase64String(diffFrame);
                //_dbContext.DetectedChanges.Add(new DetectedChange
                //{
                //    ID = Guid.NewGuid(),
                //    Happened = DateTime.Now,
                //    Image = stringImage
                //});
                //_dbContext.SaveChanges();
            }
        }
    }
}
