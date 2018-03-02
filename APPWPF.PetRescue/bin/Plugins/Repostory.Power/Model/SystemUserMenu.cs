namespace Repostory.Power.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemUserMenu")]
    public partial class SystemUserMenu
    {
        [Key]
        public int RId { get; set; }

        [Required]
        [StringLength(50)]
        public string SystemUserId { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuIdentity { get; set; }

        public virtual SystemUserInfo SystemUserInfo { get; set; }
    }
}
