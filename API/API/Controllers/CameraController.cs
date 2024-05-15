﻿using API.Managers.Abstraction;
using API.Model;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;

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

        [HttpGet("{port}")]
        public async Task StartCameraStream(int port, CancellationToken cancellationToken)
        {
            Response.Headers.Add("Content-Type", "text/event-stream");

            _manager.StartCapture(port);
            while (!cancellationToken.IsCancellationRequested)
            {
                await Task.Delay(100);
                var processedImage = _manager.GetProccedImage();
                await Response.WriteAsync($"data: {processedImage}\n\n");
                await Response.Body.FlushAsync();
            }
            _manager.StopCapture();
        }

        [HttpPost]
        public async void StopCameraStream()
        {
            _manager.StopCapture();
        }
    }
}
