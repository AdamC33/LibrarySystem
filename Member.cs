using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystem
{
    internal class Member
    {
        private string _cardNumber;
        private string _name;
        private string _phoneNumber;
        private string _email;
        private List<DueBook> _booksCheckedOut;

        private struct DueBook
        {
            public string _ISBN;
            public DateTime _dueDate;
        }

        public Member(string cardNumber, string name, string phoneNumber, string email)
        {
            _cardNumber = cardNumber;
            _name = name;
            _phoneNumber = phoneNumber;
            _email = email;
        }

        public void CheckOutBook(string ISBN, DateTime dueDate)
        {
            _booksCheckedOut.Add(new DueBook
            {
                _ISBN = ISBN,
                _dueDate = dueDate
            });
        }
    }
}
