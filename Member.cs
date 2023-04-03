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
        private List<Fee> _fees = new List<Fee>();
        private List<string> _requests = new List<string>();

        private struct Fee
        {
            public string _amount { get; set; }
            public string _reason { get; set; }
        }

        public Member(string cardNumber, string name, string phoneNumber, string email, bool activated = false, List<string> feeAmounts = null, List<string> feeReasons = null, List<string> requests = null, string password = null)
        {
            _cardNumber = cardNumber;
            _password = password;
            _name = name;
            _phoneNumber = phoneNumber;
            _email = email;
            _activated = activated;
            _fees = new List<Fee>();
            int i = -1;
            if (feeAmounts != null)
            {
                foreach (string s in feeAmounts)
                {
                    i++;
                    _fees.Add(new Fee
                    {
                        _amount = s,
                        _reason = feeReasons[i]
                    });
                }
            }
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

        public int feeListCount
        {
            get { return _fees.Count; }
        }

        public string getFeeAmount(int index)
        {
            return _fees[index]._amount;
        }

        public string getFeeReason(int index)
        {
            return _fees[index]._reason;
        }

        public void addFee(string amount, string reason)
        {
            _fees.Add(new Fee
            {
                _amount = amount,
                _reason = reason
            });
        }

        public void updateFee(int index, string amount, string reason)
        {
            Fee thisFee = _fees[index];
            thisFee._amount = amount;
            thisFee._reason = reason;
            _fees[index] = thisFee;
        }

        public List<string> getRequests
        {
            get { return _requests; }
        }
    }
}
