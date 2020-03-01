namespace CarServiceSystemWeb.EntityContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using CustomValidation;

    public partial class User
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public User()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]{1}[a-z]*$", ErrorMessage = "Name can contain only letters and must start with upper case!")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[A-Z]{1}[a-z]*$", ErrorMessage = "SureName can contain only letters and must start with upper case!")]
        public string Surename { get; set; }

        [Required]
        [RegularExpression(@"[1-9]{1}[0-9]{9}$", ErrorMessage = "Id Card Number must be 10 digit number!")]
        [DuplicateValidation("User")]
        public long IdCardNumber { get; set; }

        [Required]
        [RegularExpression(@"[1-9]{1}[0-9]{9}$", ErrorMessage = "EGN must be 10 digit number!")]
        [DuplicateValidation("User")]
        public long EGN { get; set; }

        [Required]
        [StringLength(100)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Choose gender!")]
        [StringLength(100)]
        public string Gender { get; set; }
        //
        [Required]
        [StringLength(100)]
        [RegularExpression( @"^(\d{9})$", ErrorMessage = "Invalid phone number!")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [DuplicateValidation("User")]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Car> Cars { get; set; }
    }
}
