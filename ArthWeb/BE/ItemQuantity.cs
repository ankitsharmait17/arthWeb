using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE
{
    public partial class ItemQuantity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int QtyID { get; set; }

        public int ItemID { get; set; }

        public int ItemSizeID { get; set; }

        public int Quantity { get; set; }
    }
}
