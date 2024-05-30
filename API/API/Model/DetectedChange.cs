namespace API.Model
{
    public class DetectedChange
    {
        public Guid ID { get; set; }
        public DateTime Happened { get; set; }
        public string Image { get; set; }
    }
}
