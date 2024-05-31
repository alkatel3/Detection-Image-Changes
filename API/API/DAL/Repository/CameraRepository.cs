using API.Model;

namespace API.DAL.Repository
{
    public class CameraRepository : Repository<Camera>
    {
        public CameraRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
