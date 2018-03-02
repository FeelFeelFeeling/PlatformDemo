namespace Repostory.Power.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SystemUserInfo")]
    public partial class SystemUserInfo
    {
        public SystemUserInfo()
        {
            SystemUserMenus = new HashSet<SystemUserMenu>();
        }

        [Key]
        [StringLength(50)]
        public string SystemUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string LoginId { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        public int Dic_AccountState { get; set; }

        [Required]
        [StringLength(20)]
        public string Name { get; set; }

        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string WXCode { get; set; }

        public int Dic_Sex { get; set; }

        [Required]
        [StringLength(50)]
        public string NOCode { get; set; }

        [StringLength(200)]
        public string Remark { get; set; }

        public DateTime CreateTime { get; set; }

        [Required]
        [StringLength(50)]
        public string CreateUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string CreateUserName { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifyUserId { get; set; }

        [Required]
        [StringLength(50)]
        public string ModifyUserName { get; set; }

        public DateTime ModifyTime { get; set; }

        public virtual ICollection<SystemUserMenu> SystemUserMenus { get; set; }
    }
}
