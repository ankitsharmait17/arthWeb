namespace BE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Item")]
    public partial class Item
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemID { get; set; }

        [Key]
        [StringLength(20)]
        public string ItemKey { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(500)]
        public string DescriptionLong { get; set; }

        public decimal Price { get; set; }

        public int ItemMappingID { get; set; }

        [StringLength(5)]
        public string Gender { get; set; }

        public virtual ItemMapping ItemMapping { get; set; }
    }
}
