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
        private string _password { get; set; }
        public string _name { get; set; }
        public string _phoneNumber { get; set; }
        public string _email { get; set; }
        public bool _activated { get; set; }
        private List<UInt32> _fees = new List<UInt32>(); //Using integer instead of floating point as it's more accurate
        private List<string> _requests = new List<string>();

        public Member(string cardNumber, string name, string phoneNumber, string email, bool activated = false, List<UInt32> fees = null, List<string> requests = null)
        {
            _cardNumber = cardNumber;
            _name = name;
            _phoneNumber = phoneNumber;
            _email = email;
            _activated = activated;
            _fees = fees;
            _requests = requests;
        }

        public string getPassword
        {
            get { return _password; }
        }

        public bool ActivateAccount(string newPassword)
        {
            if (!_activated)
            {
                _activated = true;
                _password = newPassword;
                return true;
            }
            else { return false; }
        }
    }
}
