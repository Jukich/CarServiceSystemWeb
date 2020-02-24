namespace CarServiceSystemWeb.EntityContext
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Repair")]
    public partial class Repair
    {
        public int Id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? DayOfRepair { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public double? PriceOfRepair { get; set; }

        public int CarID { get; set; }

        public virtual Car Car { get; set; }
    }
}
