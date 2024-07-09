using System.ComponentModel.DataAnnotations.Schema;

namespace Ikagai.HelperModels
{
    public class Base
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        public string Name { get; set; }

    }
}
