using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Xml;

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

            Title.InnerText = newBook._title;
            Author.InnerText = newBook._author;
            Year.InnerText = newBook._year;
            Publisher.InnerText = newBook._publisher;
            ISBN.InnerText = newBook._publisher;
            Category.InnerText = newBook._category;

            Book.AppendChild(Title);
            Book.AppendChild(Author);
            Book.AppendChild(Year);
            Book.AppendChild(Publisher);
            Book.AppendChild(ISBN);
            Book.AppendChild(Category);
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
                    Member thisMember = new Member(
                        xmlMember.ChildNodes.Item(0).InnerText,
                        xmlMember.ChildNodes.Item(2).InnerText,
                        xmlMember.ChildNodes.Item(3).InnerText,
                        xmlMember.ChildNodes.Item(4).InnerText
                    );
                    return thisMember;
                }
            }

            return null;
        }
    }
}
