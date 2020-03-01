namespace CarServiceSystemWeb.EntityContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using CustomValidation;

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
        [RegularExpression(@"^[A-Z0-9]{17}$", ErrorMessage = "VIN number must contain 17 symbols!")]
        [DuplicateValidation("Car")]
        public string WINnumber { get; set; }

        [Required]
        [RegularExpression(@"^[ABEKMÍOPCTYX]{2}[0-9]{4}[ABEKMÍOPCTYX]{2}$", ErrorMessage = "Invalid regirstration number!")]
        [DuplicateValidation("Car")]
        public string RegNumber { get; set; }

        [Required]
        public int BrandID { get; set; }

        [Required]
        public int ModelID { get; set; }

        [Required]
        public string Colour { get; set; }

        public int? UserID { get; set; }

        public virtual Brand Brand { get; set; }

        public virtual Model Model { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Repair> Repairs { get; set; }
    }
}
