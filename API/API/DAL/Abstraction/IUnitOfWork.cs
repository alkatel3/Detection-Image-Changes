using API.Model;

namespace API.DAL.Abstraction
{
        public interface IUnitOfWork
        {
            IRepository<Camera> Cameras { get; }
            IRepository<DetectedChange> DetectedChanges { get; }

            void Save();
        }
}
