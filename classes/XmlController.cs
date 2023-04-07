using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;
using System.Xml.Serialization;

namespace LibrarySystem
{
    internal class XmlController
    {
        private string bookPath = "Library.xml";
        private string memberPath = "Members.xml";

        public void AddBook(Book newBook)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);

            XmlNode ROOT = xmlDoc.SelectSingleNode("/library");
            XmlNode Book = xmlDoc.CreateElement("book");
            XmlNode Title = xmlDoc.CreateElement("title");
            XmlNode Author = xmlDoc.CreateElement("author");
            XmlNode Year = xmlDoc.CreateElement("year");
            XmlNode Publisher = xmlDoc.CreateElement("publisher");
            XmlNode ISBN = xmlDoc.CreateElement("isbn");
            XmlNode Category = xmlDoc.CreateElement("category");
            XmlNode Stock = xmlDoc.CreateElement("instock");
            XmlNode Checkout = xmlDoc.CreateElement("checkout");

            Title.InnerText = newBook._title;
            Author.InnerText = newBook._author;
            Year.InnerText = newBook._year;
            Publisher.InnerText = newBook._publisher;
            ISBN.InnerText = newBook._ISBN;
            Category.InnerText = newBook._category;
            Stock.InnerText = Convert.ToString(newBook._totalStock);

            Book.AppendChild(Title);
            Book.AppendChild(Author);
            Book.AppendChild(Year);
            Book.AppendChild(Publisher);
            Book.AppendChild(ISBN);
            Book.AppendChild(Category);
            Book.AppendChild(Stock);
            Book.AppendChild(Checkout);
            ROOT.AppendChild(Book);

            xmlDoc.Save(bookPath);
        }

        public void DeleteBook(string ISBN)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);
            XmlNode nodes = xmlDoc.SelectSingleNode(String.Format("//book[isbn='{0}']", ISBN));
            nodes.ParentNode.RemoveChild(nodes);
            xmlDoc.Save(bookPath);
        }

        public void UpdateBook(string ISBN, Book newBook)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);

            XmlNode oldBook = xmlDoc.SelectSingleNode(String.Format("//book[isbn='{0}']", ISBN));
            oldBook.ChildNodes.Item(0).InnerText = newBook._title;
            oldBook.ChildNodes.Item(1).InnerText = newBook._author;
            oldBook.ChildNodes.Item(2).InnerText = newBook._year;
            oldBook.ChildNodes.Item(3).InnerText = newBook._publisher;
            oldBook.ChildNodes.Item(4).InnerText = newBook._ISBN;
            oldBook.ChildNodes.Item(5).InnerText = newBook._category;

            xmlDoc.Save(bookPath);
        }

        public void UpdateBookStock(string ISBN, UInt32 newStock)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);
            XmlNode oldBook = xmlDoc.SelectSingleNode(String.Format("//book[isbn='{0}']", ISBN));
            oldBook.ChildNodes.Item(6).InnerText = Convert.ToString(newStock); //Item 6 is the stock level
            xmlDoc.Save(bookPath);
        }

        public void UpdateBookCheckout(Book thisBook)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);
            XmlNode oldCheckout = xmlDoc.SelectSingleNode(String.Format("//book[isbn='{0}']/checkout", thisBook._ISBN));
            oldCheckout.RemoveAll();

            for (int i = 0; i < thisBook.checkoutListLength; i++)
            {
                XmlNode Member = xmlDoc.CreateElement("member");
                XmlNode CardNumber = xmlDoc.CreateElement("cardnumber");
                XmlNode DueDate = xmlDoc.CreateElement("duedate");
                XmlNode Renewed = xmlDoc.CreateElement("renewed");

                CardNumber.InnerText = thisBook.getCardNumber(i);
                DueDate.InnerText = Convert.ToString(thisBook.getDueDate(thisBook.getCardNumber(i)).ToUnixTimeSeconds());
                if (thisBook.getRenewedStatus(thisBook.getCardNumber(i)) == true) { Renewed.InnerText = "1"; }
                else { Renewed.InnerText = "0"; }

                Member.AppendChild(CardNumber);
                Member.AppendChild(DueDate);
                Member.AppendChild(Renewed);
                oldCheckout.AppendChild(Member);
            }

            xmlDoc.Save(bookPath);
        }

        public List<Book> GetLibrary(string searchQuery = "", string searchBy = "title")
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);

            List<Book> library = new List<Book>();

            foreach (XmlNode node in xmlDoc.SelectSingleNode("//library"))
            {
                //This if statement is for a search query. "ToUpper" makes the Contains method non case sensitive.
                if (node.SelectSingleNode(searchBy).InnerText.ToUpper().Contains(searchQuery.ToUpper()))
                {
                    List<String> checkoutCardNumbers = new List<String>();
                    List<UInt32> checkoutDueDates = new List<UInt32>();
                    List<bool> checkoutRenewals = new List<bool>();
                    foreach (XmlNode cardNumber in node.SelectSingleNode("checkout"))
                    {
                        checkoutCardNumbers.Add(cardNumber.SelectSingleNode("cardnumber").InnerText);
                        checkoutDueDates.Add(Convert.ToUInt32(cardNumber.SelectSingleNode("duedate").InnerText));
                        if (cardNumber.SelectSingleNode("renewed").InnerText == "1") { checkoutRenewals.Add(true); }
                        else { checkoutRenewals.Add(false); }
                    }
                    Book book = new Book(
                        node.SelectSingleNode("isbn").InnerText,
                        node.SelectSingleNode("title").InnerText,
                        node.SelectSingleNode("author").InnerText,
                        node.SelectSingleNode("year").InnerText,
                        node.SelectSingleNode("publisher").InnerText,
                        node.SelectSingleNode("category").InnerText,
                        Convert.ToUInt32(node.SelectSingleNode("instock").InnerText),
                        checkoutCardNumbers,
                        checkoutDueDates,
                        checkoutRenewals
                    );
                    library.Add(book);
                }
            }

            return library;
        }

        public void AddMember(Member newMember)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            XmlNode ROOT = xmlDoc.SelectSingleNode("/members");
            XmlNode Member = xmlDoc.CreateElement("member");
            XmlNode CardNumber = xmlDoc.CreateElement("cardnumber");
            XmlNode Password = xmlDoc.CreateElement("password");
            XmlNode Name = xmlDoc.CreateElement("name");
            XmlNode PhoneNumber = xmlDoc.CreateElement("phonenumber");
            XmlNode Email = xmlDoc.CreateElement("email");
            XmlNode Activated = xmlDoc.CreateElement("activated");
            XmlNode Fees = xmlDoc.CreateElement("fees");
            XmlNode Requests = xmlDoc.CreateElement("requests");

            CardNumber.InnerText = newMember._cardNumber;
            Password.InnerText = newMember.getPassword;
            Name.InnerText = newMember._name;
            PhoneNumber.InnerText = newMember._phoneNumber;
            Email.InnerText = newMember._email;
            Activated.InnerText = "0";
            if (newMember._activated) { Activated.InnerText = "1"; }

            Member.AppendChild(CardNumber);
            Member.AppendChild(Password);
            Member.AppendChild(Name);
            Member.AppendChild(PhoneNumber);
            Member.AppendChild(Email);
            Member.AppendChild(Activated);
            Member.AppendChild(Fees);
            Member.AppendChild(Requests);
            ROOT.AppendChild(Member);

            xmlDoc.Save(memberPath);
        }

        public void DeleteMember(string cardNumber)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);
            XmlNode nodes = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']", cardNumber));
            nodes.ParentNode.RemoveChild(nodes);
            xmlDoc.Save(memberPath);
        }

        public void UpdateMember(string cardNumber, Member newMember)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            XmlNode oldMember = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']", cardNumber));
            oldMember.ChildNodes.Item(0).InnerText = newMember._cardNumber;
            oldMember.ChildNodes.Item(1).InnerText = newMember.getPassword;
            oldMember.ChildNodes.Item(2).InnerText = newMember._name;
            oldMember.ChildNodes.Item(3).InnerText = newMember._phoneNumber;
            oldMember.ChildNodes.Item(4).InnerText = newMember._email;

            xmlDoc.Save(memberPath);
        }

        public void UpdateMemberPassword(string cardNumber, string password)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            XmlNode oldMember = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber]'{0}'", cardNumber));
            oldMember.ChildNodes.Item(1).InnerText = password;

            xmlDoc.Save(memberPath);
        }

        public void AddFee(string cardNumber, string amount, string reason)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            XmlNode Fees = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']/fees", cardNumber));
            XmlNode Fee = xmlDoc.CreateElement("fee");
            XmlNode Amount = xmlDoc.CreateElement("amount");
            XmlNode Reason = xmlDoc.CreateElement("reason");

            Amount.InnerText = amount;
            Reason.InnerText = reason;

            Fee.AppendChild(Amount);
            Fee.AppendChild(Reason);
            Fees.AppendChild(Fee);

            xmlDoc.Save(memberPath);
        }

        public void DeleteFee(string cardNumber, int index)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            XmlNode Fees = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']/fees", cardNumber));
            XmlNode Fee = Fees.ChildNodes.Item(index);
            Fees.RemoveChild(Fee);

            xmlDoc.Save(memberPath);
        }

        public void UpdateFee(string cardNumber, string amount, string reason, int index)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            XmlNode Fees = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']/fees", cardNumber));
            XmlNode Fee = Fees.ChildNodes.Item(index);
            Fee.ChildNodes.Item(0).InnerText = amount;
            Fee.ChildNodes.Item(1).InnerText = reason;

            xmlDoc.Save(memberPath);
        }

        public Member GetMember(string cardNum, string password)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            foreach (XmlNode node in xmlDoc.SelectSingleNode("//members"))
            {
                string xmlCardNum = node.SelectSingleNode("cardnumber").InnerText;
                string xmlPassword = node.SelectSingleNode("password").InnerText;

                if (cardNum == xmlCardNum && password == xmlPassword)
                {
                    XmlNode xmlMember = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']", xmlCardNum));
                    bool activated = false;
                    if (xmlMember.SelectSingleNode("activated").InnerText == "1") { activated = true; }
                    List<string> feeAmounts = new List<string>();
                    List<string> feeReasons = new List<string>();
                    foreach (XmlNode xmlFee in xmlMember.ChildNodes.Item(6))
                    {
                        feeAmounts.Add(xmlFee.SelectSingleNode("amount").InnerText);
                        feeReasons.Add(xmlFee.SelectSingleNode("reason").InnerText);
                    }
                    List<string> requests = new List<string>();
                    foreach (XmlNode xmlRequest in xmlMember.ChildNodes.Item(7))
                    {
                        requests.Add(xmlRequest.InnerText);
                    }
                    Member thisMember = new Member(
                        xmlMember.SelectSingleNode("cardnumber").InnerText, //Card number
                        xmlMember.SelectSingleNode("name").InnerText, //Name
                        xmlMember.SelectSingleNode("phonenumber").InnerText, //Phone number
                        xmlMember.SelectSingleNode("email").InnerText,  //Email
                        activated, //Whether the account is activated or not
                        feeAmounts, //List of fee amounts (in the string format £x.xx)
                        feeReasons, //List of reasons for the fees
                        requests, //List of requests/notifications
                        xmlMember.SelectSingleNode("password").InnerText
                    );
                    return thisMember;
                }
            }
            return null;
        }

        public string GetMemberName(string cardNum)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            foreach (XmlNode node in xmlDoc.SelectSingleNode("//members"))
            {
                string xmlCardNum = node.SelectSingleNode("cardnumber").InnerText;

                if (cardNum == xmlCardNum)
                {
                    XmlNode xmlMember = xmlDoc.SelectSingleNode(String.Format("//member[cardnumber='{0}']", xmlCardNum));
                    return xmlMember.SelectSingleNode("name").InnerText;
                }
            }
            return null;
        }

        public int NumberOfMembersQueuedForBook(string ISBN)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(bookPath);
            int counter = -1;

            XmlNode xmlBook = xmlDoc.SelectSingleNode(String.Format("//book[isbn='{0}']", ISBN));

            foreach (XmlNode member in xmlBook.ChildNodes)
            {
                counter++;
            }

            return counter;
        }

        public bool NotifyMember(string cardNum, string message)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            foreach (XmlNode node in xmlDoc.SelectSingleNode("//members"))
            {
                string xmlCardNum = node.SelectSingleNode("cardnumber").InnerText;

                if (cardNum == xmlCardNum)
                {
                    XmlNode Requests = node.SelectSingleNode("requests");
                    XmlNode Request = xmlDoc.CreateElement("request");
                    Request.InnerText = message;
                    Requests.AppendChild(Request);
                    xmlDoc.Save(memberPath);
                    return true;
                }
            }
            return false;
        }

        public bool ReadNotification(string cardNum, string password, int index) //If index is -1, all notifications are deleted
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(memberPath);

            foreach (XmlNode node in xmlDoc.SelectSingleNode("//members"))
            {
                string xmlCardNum = node.SelectSingleNode("cardnumber").InnerText;
                string xmlPassword = node.SelectSingleNode("password").InnerText;

                if (cardNum == xmlCardNum && password == xmlPassword)
                {
                    XmlNode Requests = node.SelectSingleNode("requests");
                    if (index > -1)
                    {
                        XmlNode Request = Requests.ChildNodes.Item(index);
                        Requests.RemoveChild(Request);
                    }
                    else
                    {
                        Requests.RemoveAll();
                    }
                    xmlDoc.Save(memberPath);
                    return true;
                }
            }
            return false;
        }
    }
}
