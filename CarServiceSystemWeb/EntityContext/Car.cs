namespace CarServiceSystemWeb.EntityContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Car")]
    public partial class Car
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Car()
        {
            Repairs = new HashSet<Repair>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string WINnumber { get; set; }

        [Required]
        [StringLength(100)]
        public string RegNumber { get; set; }

        public int BrandID { get; set; }

        public int ModelID { get; set; }

        [Required]
        [StringLength(100)]
        public string Colour { get; set; }

        public int? UserID { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Model Model { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
