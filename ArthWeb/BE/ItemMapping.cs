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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemMapping()
        {
            Items = new HashSet<Item>();
        }

        public int ItemMappingID { get; set; }

        public int ItemTypeID { get; set; }

        public int ItemSubTypeID { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }

        public virtual ItemSubType ItemSubType { get; set; }

        public virtual ItemType ItemType { get; set; }
    }
}
