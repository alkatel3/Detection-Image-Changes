namespace API.Model
{
    public class DetectedChange :BaseEntity
    {
        public DateTime Happened { get; set; }
        public string Image { get; set; }
    }
}
