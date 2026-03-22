using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SD_Server.Domain.Base
{
    public class BaseEntity
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
    }
}
