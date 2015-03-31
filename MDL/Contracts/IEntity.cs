using System.ComponentModel.DataAnnotations.Schema;

namespace Filewatcher.MDL
{
    public interface IEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        int Id { get; set; }
    }
}
