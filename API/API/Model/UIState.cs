using System.Drawing;

namespace API.Model
{
    public class UIState
    {
        public bool Threshold { get; set; } = true;
        public bool NoiseFilter { get; set; } = true;
        public bool ShowImage { get; set; } = true;

        public bool Boxes { get; set; }
        public bool ShowMask { get; set; }
        public int CameraIDx { get; set; }

        public Size FrameSize { get; set; } = new Size(640, 480);//1920, 1080
        
        public int MinBBoxArea { get; set; } = 6000;
    }
}
