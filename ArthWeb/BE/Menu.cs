using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    [Table("Menu")]
    public class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuID { get; set; }

        [StringLength(50)]
        public string ItemType { get; set; }

        [StringLength(50)]
        public string ItemSubType { get; set; }

        [StringLength(5)]
        public string Gender { get; set; }
    }
}
