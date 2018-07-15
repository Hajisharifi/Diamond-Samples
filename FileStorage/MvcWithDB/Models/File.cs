using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace H2.Mvc_FileStorage.Models
{
    /// <summary>
    /// database model
    /// </summary>
    public class File
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FileId { get; set; }
        public long FileSize { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public DateTime RegisterDate { get; set; }
    }
    //________________________________________________________________________

    /// <summary>
    /// database adapter
    /// </summary>
    public class Context
        : DbContext
    {
        public Context()
            : base("name=DefaultConnection")
        {
            this.Database.CreateIfNotExists();
        }

        public DbSet<File> Files { get; set; }
    }
    //________________________________________________________________________
}