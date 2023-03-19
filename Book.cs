using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Book
    {
        public string _ISBN { get; set; }
        public string _title { get; set; }
        public string _author { get; set; }
        public string _year { get; set; }
        public string _publisher { get; set; }
        public string _category { get; set; }
        public UInt32 _stock { get; set; }
        private List<Checkout> _checkoutList = new List<Checkout>();

        private struct Checkout
        {
            public string _cardNumber { get; set; }
            public DateTimeOffset _dueDate { get; set; } //The DateTime struct cannot easily convert between datetime and Unix datetime (which is what gets stored in the XML files).
            //However, the DateTimeOffset struct can do this. It is implemented as one of its methods.
        }

        public Book(string ISBN, string title, string author, string year, string publisher, string category, UInt32 stock, List<string> checkoutCardNumbers, List<UInt32> checkoutDueDates)
        {
            _ISBN = ISBN;
            _title = title;
            _author = author;
            _year = year;
            _publisher = publisher;
            _category = category;
            _stock = stock;
            _checkoutList = new List<Checkout>();
            int i = -1;
            foreach (string s in checkoutCardNumbers)
            {
                i++;
                _checkoutList.Add(new Checkout
                {
                    _cardNumber = s,
                    _dueDate = DateTimeOffset.FromUnixTimeSeconds(checkoutDueDates[i])
                });
            }
        }

        public int queueForBook
        {
            get { return _checkoutList.Count; }
        }

        public DateTimeOffset getDueDate(string cardNumber)
        {
            int i = -1;
            foreach (Checkout c in _checkoutList)
            {
                i++;
                if (c._cardNumber == cardNumber)
                {
                    return c._dueDate;
                }
            }
            return DateTimeOffset.MinValue;
        }
    }
}
