﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Journals.Model
{
    [Table("UserProfile")]
    public class UserProfile : BaseEntity
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }

        public string UserName { get; set; }

        [EmailAddress]
        public string EmailAddress { get; set; }

    }
}