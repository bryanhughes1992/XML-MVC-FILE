using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/*
 * Book.cs is the model for the class, Book.
 * Book has 
 */
namespace _5204_BRYANHUGHES_LAB05.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string FirstName { get; set; }
        public string  MiddleName { get; set; }
        public string LastName { get; set; }
        public string AuthorTitle { get; set; }
    }
}
