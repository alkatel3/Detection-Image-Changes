using API.Model;

namespace API.DAL.Repository
{
    public class DetectedChangeRepository : Repository<DetectedChange>
    {
        public DetectedChangeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
