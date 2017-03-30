﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Journals.Model
{
    public class Journal : BaseEntity
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string FileName { get; set; }

        public string ContentType { get; set; }

        public List<Journal> ToList()
        {
            throw new NotImplementedException();
        }

        public byte[] Content { get; set; }

        public DateTime ModifiedDate { get; set; }

        [ForeignKey("UserId")]
        public UserProfile User { get; set; }

        public int UserId { get; set; }
    }
}