﻿using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Utility.Helpers;

namespace BusinessObject.Entities
{
    [Index(nameof(Id),IsUnique = true, Name = "Index_Id")]
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            CreatedTime = LastUpdatedTime = CoreHelper.SystemTimeNow;
        }
        
        [Key]
        public string Id { get; set; }
        public string? CreatedBy { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? DeletedBy { get; set; }

        public DateTimeOffset CreatedTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }

        public DateTimeOffset? DeletedTime { get; set; }
    }
}
