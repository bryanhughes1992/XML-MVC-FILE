using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using _5204_BRYANHUGHES_LAB05.Models;
using System.Diagnostics;

namespace _5204_BRYANHUGHES_LAB05.Controllers
{
    public class BooksController : Controller
    {
        public IActionResult Index()
        {
            //Create a new list of books named 'BookList'
            IList<Book> BookList = new List<Book>();
            //Create a new XML Document named 'xdoc'
            XmlDocument xdoc = new XmlDocument();
            //Create a string with the value of the path to the XML file
            string path = Request.PathBase + "XML/books.xml";
            //If the path exists, do the following
            if (System.IO.File.Exists(path))
            {
                //Load the XML document
                xdoc.Load(path);
                //Create a new list of XML nodes equal to the XML Element "book"
                XmlNodeList Books = xdoc.GetElementsByTagName("book");
                //For every book in the root element "books" (inside of the XML document)
                foreach (XmlElement BookElement in Books)
                {
                    //Instantiate a new Book object named NewBook
                    Book NewBook = new Book
                    {
                        //An Id member, equal to the first 'id' element in books 
                        Id = Int32.Parse(BookElement.GetElementsByTagName("id")[0].InnerText),
                        //A Title member, equal to the first 'title' in the xml 
                        Title = BookElement.GetElementsByTagName("title")[0].InnerText
                    };
                    //Create a new XML Element equal to the first 'author' element books
                    XmlElement Author = (XmlElement)BookElement.GetElementsByTagName("author")[0];

                    //A member equal to the first 'firstname' element in the 'author' element in books
                    NewBook.FirstName = Author.GetElementsByTagName("firstname")[0].InnerText;
                    //Create a middle name member equal to an empty string
                    NewBook.MiddleName = "";
                    //Check if there is a middle name in the 'author' element
                    if (BookElement.GetElementsByTagName("middlename").Count > 0)
                    {
                        //if there is, set it to the value of the first 'middlename' in the 'author' element in books
                        NewBook.MiddleName = Author.GetElementsByTagName("middlename")[0].InnerText;
                    }//Otherwise, keep the 'MiddleName' value an empty string
                    //A LastName member equal to the value of the first 'lastname' element in the 'author' element in books
                    NewBook.LastName = Author.GetElementsByTagName("lastname")[0].InnerText;
                    //Set the AuthorTitle the value of the 'title' attribute
                    NewBook.AuthorTitle = Author.GetAttribute("title");
                    //Add the NewBook to the new BookList
                    BookList.Add(NewBook);
                }
            }
            //Return the View containing the BookList
            return View(BookList);
        }
        //When a GET request is made, this Create() function is recruited
        [HttpGet]
        public IActionResult Create()
        {
            Book NewBook = new Book();
            return View(NewBook);
        }

        //When the POST request is made with the form submission this Create() method is recruited
        [HttpPost]
        public IActionResult Create(Book NewBook)
        {
            //Create a new XML document named XDoc
            XmlDocument XDoc = new XmlDocument();
            //A string with the value of the path to the .xml file
            string Path = Request.PathBase + "XML/books.xml";
            //If the path exists, then:
            if (System.IO.File.Exists(Path))
            {
                //Load the document (XDoc) via the 'Path'
                XDoc.Load(Path);
                //Create a new XML Element named 'Book'
                XmlElement Book = CreateBookElement(XDoc, NewBook);
                //Append the Book to the XML Document
                XDoc.DocumentElement.AppendChild(Book);
            } else
            {
                //Otherwise, make a new XML Node equal to a new XML Declaration
                XmlNode XmlDeclaration = XDoc.CreateXmlDeclaration("1.0", "utf-8", "");
                //Append the declaration node to the XML document
                XDoc.AppendChild(XmlDeclaration);
                //Create a new XML node named Books
                XmlNode Books = XDoc.CreateElement("books");
                //Append the Books node to the XML document
                XDoc.AppendChild(Books);
                XmlElement book = CreateBookElement(XDoc, NewBook);
                Books.AppendChild(book);
            }
            //Save the document to the value of 'Path'
            XDoc.Save(Path);
            return View();
        }
        public XmlElement CreateBookElement(XmlDocument xDoc, Book Book)
        {
            //Create an XML Element with the value of a 'book' element in the XML document
            XmlElement NewBook = xDoc.CreateElement("book");
            //Capture the last child node in the XML document
            XmlNode LastBook = xDoc.DocumentElement.LastChild;
            //The new 'Id' will be the value of the last child node plus one
            int Id = Int32.Parse(LastBook.ChildNodes[0].InnerText) + 1;
            //Create a new XML element to hold the value of 'Id'
            XmlElement newId = xDoc.CreateElement("id");
            //Coalesce zeros until there's 4 digits to the value of 'Id'. Set 'newId' to this value.
            newId.InnerText = Id.ToString("D4");
            //Append the new id to the new book
            NewBook.AppendChild(newId);

            //Make a new XML element for the book title
            XmlElement NewTitle = xDoc.CreateElement("title");
            //Set the text content to the XML element 'title'
            NewTitle.InnerText = Book.Title;
            //Append the new title to the new book
            NewBook.AppendChild(NewTitle);
            //Create a new XML element for the author
            XmlElement NewAuthor = xDoc.CreateElement("author");
            //Create an attribute for the author named 'title'
            XmlAttribute AuthorTitle = xDoc.CreateAttribute("title");
            //Set the value of the title element to the value of passed in books' author title
            AuthorTitle.Value = Book.AuthorTitle;
            //Create an XML node for the author's first name
            XmlNode AuthorFirst = xDoc.CreateElement("firstname");
            //Set the author's first name to the passed in value
            AuthorFirst.InnerText = Book.FirstName;
            //Create an XML node for the middle name
            XmlNode AuthorMiddle = xDoc.CreateElement("middlename");
            //Set the value to the middle name value passed in
            AuthorMiddle.InnerText = Book.MiddleName;
            //Create an XML node for the author last name
            XmlNode AuthorLast = xDoc.CreateElement("lastname");
            //Set the value of the last name to the value of the last name passed in
            AuthorLast.InnerText = Book.LastName;
            //Append all the attributes and elements to the new author
            NewAuthor.Attributes.Append(AuthorTitle);
            NewAuthor.AppendChild(AuthorFirst);
            NewAuthor.AppendChild(AuthorMiddle);
            NewAuthor.AppendChild(AuthorLast);
            //Append the new author to the new book
            NewBook.AppendChild(NewAuthor);
            //Return the complied new book
            return NewBook;
        }
       
    }
}


