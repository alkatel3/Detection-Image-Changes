using API.DAL.Abstraction;
using API.Model;

namespace API.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IRepository<Camera> _cameras = null!;
        private IRepository<DetectedChange> _detectedChange = null!;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Camera> Cameras
        {
            get
            {
                if (_cameras == null)
                    _cameras = new CameraRepository(_context);

                return _cameras;
            }
        }

        public IRepository<DetectedChange> DetectedChanges
        {
            get
            {
                if (_detectedChange == null)
                    _detectedChange = new DetectedChangeRepository(_context);

                return _detectedChange;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
