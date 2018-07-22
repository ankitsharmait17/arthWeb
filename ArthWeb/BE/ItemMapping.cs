namespace BE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ItemMapping")]
    public partial class ItemMapping
    {
        public int ItemMappingID { get; set; }

        public int ItemTypeID { get; set; }

        public int ItemSubTypeID { get; set; }

        [Required]
        [StringLength(1)]
        public string Gender { get; set; }

        public virtual ItemSubType ItemSubType { get; set; }

        public virtual ItemType ItemType { get; set; }
    }
}
