using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    internal class Book
    {
        public string _ISBN { get; set; }
        public string _title { get; set; }
        public string _author { get; set; }
        public string _year { get; set; }
        public string _publisher { get; set; }
        public string _category { get; set; }

        public Book(string ISBN, string title, string author, string year, string publisher, string category)
        {
            _ISBN = ISBN;
            _title = title;
            _author = author;
            _year = year;
            _publisher = publisher;
            _category = category;
        }

    }
}
