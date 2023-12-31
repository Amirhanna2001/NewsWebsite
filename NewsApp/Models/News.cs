﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace NewsApp.Models
{
    public class News
    {
        public int Id { get; set; }
        public string title { get; set; }
        [DataType(DataType.MultilineText),MaxLength(500)]
        public string TheNews { get; set; }
        public string ImagePath { get; set; }
        public DateTime PublicationDate { get; set; }
        public DateTime CreationDate { get; set; } =DateTime.Now;
        public int AuthorId { get; set; } 
        public Author Author { get; set; }
    }
}
