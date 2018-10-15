using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace BE
{
    [Table("ItemSize")]
    public partial class ItemSize
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemSizeID { get; set; }

        public string ItemSizeName { get; set; }
    }
}
