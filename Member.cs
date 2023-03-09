using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    public class Member
    {
        public string _cardNumber { get; set; }
        private string _password;
        public string _name { get; set; }
        public string _phoneNumber { get; set; }
        public string _email { get; set; }
        private List<DueBook> _booksCheckedOut = new List<DueBook>();
        private struct DueBook
        {
            public string _ISBN { get; set; }
            public DateTimeOffset _dueDate { get; set; } //The DateTime struct cannot easily convert between datetime and Unix datetime (which is what gets stored in the XML files).
            //However, the DateTimeOffset struct can do this. It is implemented as one of its methods.
        }
        public Member(string cardNumber, string name, string phoneNumber, string email)
        {
            _cardNumber = cardNumber;
            _name = name;
            _phoneNumber = phoneNumber;
            _email = email;
        }

        public int numberOfBooksCheckedOut
        {
            get { return _booksCheckedOut.Count(); }
        }
        public string GetDueBookISBN(int index)
        {
            return _booksCheckedOut[index]._ISBN;
        }
        public DateTimeOffset GetDueBookDate(int index)
        {
            return _booksCheckedOut[index]._dueDate;
        }

        public void CheckOutBook(string ISBN, DateTimeOffset dueDate)
        {
            _booksCheckedOut.Add(new DueBook
            {
                _ISBN = ISBN,
                _dueDate = dueDate
            });
        }
    }
}
