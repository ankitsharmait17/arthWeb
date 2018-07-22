namespace BE
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ItemSubType")]
    public partial class ItemSubType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ItemSubType()
        {
            ItemMappings = new HashSet<ItemMapping>();
        }

        public int ItemSubTypeID { get; set; }

        [Column("ItemSubType")]
        [Required]
        [StringLength(50)]
        public string ItemSubType1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemMapping> ItemMappings { get; set; }
    }
}
