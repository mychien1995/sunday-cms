﻿using System;

namespace Sunday.Core.Domain
{
    public interface IChangeTrackable
    {
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
}
