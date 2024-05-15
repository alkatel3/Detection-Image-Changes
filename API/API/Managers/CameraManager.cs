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

namespace API.Managers
{
    public class CameraManager : ICameraManager
    {
        private VideoCapture capture;
        private IBackgroundSubtractor backgroundSubtractor;
        private Size frameSize = new Size(640, 480);
        private List<string> Images;
        private Mat smoothFrame;
        private Mat foregroundMask;
        private Mat frame;
        private int minBBoxArea = 6000;

        private static UIState State;
        private static bool streamVideo;


        public CameraManager()
        {
            backgroundSubtractor = new BackgroundSubtractorMOG2();
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
                            Port = i,
                            Src  = ImageToBase64String(frame.ToBitmap()),
                            Name=capture.BackendName
                        });
                        // Отримати індекс камери
                        //int index = (int)capture.GetCaptureProperty(CapProp.PosMsec);
                        //availableCameras.Add(i);
                    }
                }
            }
            return availableCameras;
        }

        public void StartCapture(int port)
        {
            capture = new VideoCapture(port);
            capture.Start();
        }

        public void StopCapture()
        {
            capture.Stop();
            capture.Dispose();
        }

        public string GetProccedImage()
        {
            try
            {
                frame = new Mat();
                capture.Read(frame);

                if (frame.IsEmpty)
                {
                    return "";
                }

                smoothFrame = new Mat();
                CvInvoke.GaussianBlur(frame, smoothFrame, new System.Drawing.Size(5, 5), 10);
                foregroundMask = new Mat();
                backgroundSubtractor.Apply(smoothFrame, foregroundMask);

                if (false) //chkThreshold.Checked
                {
                    Threshold();
                }

                if (true)//chkNoise.Checked
                {
                    FilterNoise();
                }

                if (true)//chkBoxes.Checked
                {
                    BoundingBoxes();
                }

                CvInvoke.Resize(smoothFrame, smoothFrame, frameSize);
                CvInvoke.Resize(foregroundMask, foregroundMask, frameSize);
                CvInvoke.Resize(frame, frame, frameSize);
                var image = DrawMat(2).ToBitmap();
                var stringImage = ImageToBase64String(image);
                return stringImage;
                //Images.Add(stringImage);
                //frame.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ImageToBase64String(System.Drawing.Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] imageBytes = ms.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }

        private void Threshold()
        {
            CvInvoke.Threshold(foregroundMask, foregroundMask, 255, 255, ThresholdType.Binary);
        }

        private void FilterNoise()
        {
            CvInvoke.MorphologyEx(foregroundMask, foregroundMask, MorphOp.Open,
                Mat.Ones(3, 3, DepthType.Cv8U, 1), new Point(-1, -1), 1,
                BorderType.Reflect, new MCvScalar(0));
        }

        private void BoundingBoxes()
        {
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(foregroundMask, contours, null, RetrType.External,
                ChainApproxMethod.ChainApproxSimple);

            for (int i = 0; i < contours.Size; i++)
            {
                var bbox = CvInvoke.BoundingRectangle(contours[i]);
                var area = bbox.Width * bbox.Height;
                var aspectRatio = (double)bbox.Width / bbox.Height;

                if (aspectRatio < 4.0 && area > minBBoxArea /*&& (bbox.Bottom > bboxBottomLimit || (bbox.Right < bboxRightLimit))*/)
                {
                    if (true)//radImage.Checked
                    {
                        CvInvoke.Rectangle(frame, bbox, new MCvScalar(255, 255, 255), 2);
                    }
                    else if (false)//radMask.Checked
                    {
                        CvInvoke.Rectangle(foregroundMask, bbox, new MCvScalar(255, 255, 255), 2);
                    }
                }
            }
        }

        private Mat DrawMat(int boxNum)
        {
            try
            {
                if (boxNum == 2)
                {
                    if (true)//radImage.Checked
                    {
                        //pictureBox1.Image = frame.ToBitmap();
                        return frame;
                    }
                    else if (false)//radMask.Checked
                    {
                        //pictureBox1.Image = foregroundMask.ToBitmap();
                        return foregroundMask;
                    }
                }
                return new Mat();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
