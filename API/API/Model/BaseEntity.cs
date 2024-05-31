using System.ComponentModel.DataAnnotations;

namespace API.Model
{
    public class BaseEntity
    {
        [Key]
        public Guid ID { get; set; }
    }
}
