using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        private List<UInt32> _fees = new List<UInt32>(); //Using integer instead of floating point as it's more accurate

        public Member(string cardNumber, string name, string phoneNumber, string email, List<UInt32> fees)
        {
            _cardNumber = cardNumber;
            _name = name;
            _phoneNumber = phoneNumber;
            _email = email;
            _fees = fees;
        }
    }
}
