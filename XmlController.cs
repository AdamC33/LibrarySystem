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
            XmlNode oldBook = xmlDoc.SelectSingleNode(String.Format("//book[isbn='{0}']", thisBook._ISBN));
            XmlNode oldCheckout = oldBook.SelectSingleNode("checkout");
            oldBook.RemoveChild(oldCheckout);
            XmlNode newCheckout = xmlDoc.CreateElement("checkout");
            for (int i = 0; i < thisBook.checkoutListLength; i++)
            {
                XmlNode Member = xmlDoc.CreateElement("member");
                XmlNode CardNumber = xmlDoc.CreateElement("cardnumber");
                XmlNode DueDate = xmlDoc.CreateElement("duedate");

                CardNumber.InnerText = thisBook.getCardNumber(i);
                DueDate.InnerText = Convert.ToString(thisBook.getDueDate(thisBook.getCardNumber(i)).ToUnixTimeSeconds());

                Member.AppendChild(CardNumber);
                Member.AppendChild(DueDate);
                newCheckout.AppendChild(Member);
            }
            oldBook.AppendChild(newCheckout);

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
                    foreach (XmlNode cardNumber in node.SelectSingleNode("checkout"))
                    {
                        checkoutCardNumbers.Add(cardNumber.SelectSingleNode("cardnumber").InnerText);
                        checkoutDueDates.Add(Convert.ToUInt32(cardNumber.SelectSingleNode("duedate").InnerText));
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
                        checkoutDueDates
                    );
                    library.Add(book);
                }
            }

            return library;
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
                    List<UInt32> fees = new List<UInt32>();
                    foreach (XmlNode xmlFee in xmlMember.ChildNodes.Item(5))
                    {
                        fees.Add(Convert.ToUInt32(xmlFee.InnerText)); //Fees are in UInt32 to avoid inaccuracy issues with floating point numbers
                    }
                    Member thisMember = new Member(
                        xmlMember.SelectSingleNode("cardnumber").InnerText, //Card number
                        xmlMember.SelectSingleNode("name").InnerText, //Name
                        xmlMember.SelectSingleNode("phonenumber").InnerText, //Phone number
                        xmlMember.SelectSingleNode("email").InnerText,  //Email
                        fees
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
    }
}
