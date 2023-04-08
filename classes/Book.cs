﻿using System;
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
        public UInt32 _totalStock { get; set; }
        private List<Checkout> _checkoutList = new List<Checkout>();

        private struct Checkout
        {
            public string _cardNumber { get; set; }
            public DateTimeOffset _dueDate { get; set; } //The DateTime struct cannot easily convert between datetime and Unix datetime (which is what gets stored in the XML files).
            //However, the DateTimeOffset struct can do this. It is implemented as one of its methods.
            public bool _renewed { get; set; }
        }

        public Book(string ISBN, string title, string author, string year, string publisher, string category, UInt32 stock, List<string> checkoutCardNumbers = null, List<UInt32> checkoutDueDates = null, List<bool> checkoutRenews = null)
        {
            _ISBN = ISBN;
            _title = title;
            _author = author;
            _year = year;
            _publisher = publisher;
            _category = category;
            _totalStock = stock;
            _checkoutList = new List<Checkout>();
            int i = -1;
            if (checkoutCardNumbers != null)
            {
                foreach (string s in checkoutCardNumbers)
                {
                    i++;
                    _checkoutList.Add(new Checkout
                    {
                        _cardNumber = s,
                        _dueDate = DateTimeOffset.FromUnixTimeSeconds(checkoutDueDates[i]),
                        _renewed = checkoutRenews[i]
                    });
                }
            }
        }

        public int currentStock
        {
            get
            {
                int stock = Convert.ToInt32(_totalStock);
                foreach (Checkout checkout in _checkoutList)
                {
                    if (checkout._dueDate.ToUnixTimeSeconds() > 0)
                    {
                        stock--;
                    }
                }
                return stock;
            }
        }

        public int currentStockAlsoSubtractingQueue
        {
            //Similar to the currentStock integer, but also calculates the total queue. If the end result goes into the negatives, it will return 0.
            get
            {
                int stock = Convert.ToInt32(_totalStock);
                foreach (Checkout checkout in _checkoutList)
                {
                    stock--;
                    if (stock <= 0)
                    {
                        return 0;
                    }
                }
                return stock;
            }
        }

        public int queueForBook
        {
            get
            {
                int queue = 0;
                foreach (Checkout checkout in _checkoutList)
                {
                    if (checkout._dueDate.ToUnixTimeSeconds() == 0)
                    {
                        queue++;
                    }
                }
                return queue;
            }
        }

        public int checkoutListMinusQueueLength
        {
            get
            {
                int list = 0;
                foreach (Checkout checkout in _checkoutList)
                {
                    if (checkout._dueDate.ToUnixTimeSeconds() > 0)
                    {
                        list++;
                    }
                }
                return list;
            }
        }

        public int checkoutListLength
        {
            get { return _checkoutList.Count; }
        }

        public string getCardNumber(int index)
        {
            return _checkoutList[index]._cardNumber;
        }

        public DateTimeOffset getDueDate(string cardNumber)
        {
            foreach (Checkout c in _checkoutList)
            {
                if (c._cardNumber == cardNumber)
                {
                    return c._dueDate;
                }
            }
            return DateTimeOffset.MinValue;
        }

        public bool? getRenewedStatus(string cardNumber)
        {
            foreach (Checkout c in _checkoutList)
            {
                if (c._cardNumber == cardNumber)
                {
                    return c._renewed;
                }
            }
            return null;
        }

        public int checkoutBook(string cardNumber)
        {
            //0 = already checked out
            //1 = already queued
            //2 = out of stock, added to queue
            //3 = successfully checked out book
            foreach (Checkout c in _checkoutList)
            {
                if (c._cardNumber == cardNumber)
                {
                    if (c._dueDate > DateTimeOffset.FromUnixTimeSeconds(0))
                    {
                        return 0;
                    }
                    else
                    {
                        return 1;
                    }
                }
            }
            if (currentStockAlsoSubtractingQueue == 0)
            {
                _checkoutList.Add(new Checkout
                {
                    _cardNumber = cardNumber,
                    _dueDate = DateTimeOffset.FromUnixTimeSeconds(0),
                    _renewed = false
                });
                return 2;
            }
            else
            {
                _checkoutList.Add(new Checkout
                {
                    _cardNumber = cardNumber,
                    _dueDate = DateTimeOffset.Now.AddDays(21), //21 days are added as this is the standard amount of days a book can be borrowed for
                    _renewed = false
                });
                return 3;
            }
        }

        public int returnBook(string cardNumber)
        {
            //0 = book has been returned
            //>0 = book has been returned, days late
            //-1 = book hasn't been returned
            DateTimeOffset currTime = DateTimeOffset.Now;
            foreach (Checkout c in _checkoutList)
            { 
                if (c._cardNumber == cardNumber)
                {
                    _checkoutList.Remove(c);
                    if (c._dueDate > currTime) { return 0; }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(c._dueDate);
                        return Convert.ToInt32(Math.Ceiling((currTime - c._dueDate).TotalDays));
                    }
                }
            }

            return -1;
        }

        public int renewBook(string cardNumber)
        {
            //0 = book has been renewed
            //>0 = book has been renewed, days late
            //-1 = book hasn't been renewed
            DateTimeOffset currTime = DateTimeOffset.Now;
            foreach (Checkout c in _checkoutList)
            {
                if (c._cardNumber == cardNumber)
                {
                    if (c._renewed) { return -1; } //Cannot return books that have already been renewed
                    _checkoutList[_checkoutList.IndexOf(c)] = new Checkout
                    { _cardNumber = c._cardNumber,
                      _dueDate = DateTimeOffset.UtcNow.AddDays(21),
                      _renewed = true };
                    if (c._dueDate > currTime) { return 0; }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine(c._dueDate);
                        return Convert.ToInt32(Math.Ceiling((currTime - c._dueDate).TotalDays));
                    }
                }
            }

            return -1;
        }

        public bool moveQueueToCheckout()
        {
            //Automatically checks the queue and moves the earliest to checkout
            for (int i = 0; i < _checkoutList.Count; i++)
            {
                if (currentStock == 0)
                {
                    return false;
                }
                else if (_checkoutList[i]._dueDate == DateTimeOffset.FromUnixTimeSeconds(0))
                {
                    Checkout c = _checkoutList[i];
                    c._dueDate = DateTimeOffset.Now.AddDays(21);
                    _checkoutList[i] = c;
                }
            }
            return true;
        }

        public bool modifyStock(UInt32 newStock)
        {
            //True = stock can be changed to this, and has been changed
            //False = stock cannot be changed to this
            if (checkoutListMinusQueueLength > newStock)
            {
                return false;
            }
            else
            {
                _totalStock = newStock;
                return true;
            }
        }
    }
}