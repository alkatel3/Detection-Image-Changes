using System.Text.Json;

namespace API.Model
{
    public class Camera
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = null!;
        public string Src { get; set; } = null!;
        public int Port { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }


        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
