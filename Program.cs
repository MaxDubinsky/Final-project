using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting;
using System.Security.Authentication.ExtendedProtection.Configuration;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.CodeDom;
using System.Diagnostics.Contracts;

namespace ConsoleApp9
{
    class ContactManager
    {
        public XmlDocument numbers { get; set; }

        public ContactManager()
        {
            numbers = new XmlDocument();
        }

        public void LoadDoc(string path)
        {
            if (File.Exists(path))
            {
                numbers.Load(path);
            }
            else
            {
                Console.WriteLine("This database doesn't exist.");
            }
        }

        protected void LoadDataToDB(string name, string surname, string contactNumber)
        {
            XmlElement contact = numbers.CreateElement("Contact");
            XmlElement nameElement = numbers.CreateElement("Name");
            XmlElement surnameElement = numbers.CreateElement("Surname");
            XmlElement contactNumberElement = numbers.CreateElement("Contact Number");

            nameElement.InnerText = name;
            surnameElement.InnerText = surname;
            contactNumberElement.InnerText = contactNumber;

            contact.AppendChild(nameElement);
            contact.AppendChild(surnameElement);
            contact.AppendChild(contactNumberElement);

            numbers.DocumentElement?.AppendChild(contact);
        }

        public void AddContactNumber()
        {
            Console.Write("Enter your name: ");
            string name = Console.ReadLine();

            Console.Write("Enter your surname: ");
            string surname = Console.ReadLine();

            Console.Write("Enter your phone number: ");
            string contactNumber = Console.ReadLine();

            LoadDataToDB(name, surname, contactNumber);
        }

        public void DeleteContactNumber(string name)
        {
            foreach (XmlNode node in numbers.DocumentElement.ChildNodes)
            {
                if (name.Equals(node.SelectSingleNode("Name").InnerText, StringComparison.OrdinalIgnoreCase))
                {
                    numbers.DocumentElement.RemoveChild(node);
                    break; // Assuming there is only one contact with the given name
                }
            }
        }

        public void DisplayNodes()
        {
            foreach (XmlNode node in numbers.DocumentElement.ChildNodes)
            {
                string name = node.SelectSingleNode("Name").InnerText;
                string surname = node.SelectSingleNode("Surname").InnerText;
                string contactNumber = node.SelectSingleNode("Contact Number").InnerText;

                Console.WriteLine($"Full name: {name} {surname}, Contact Number: {contactNumber}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ContactManager manager = new ContactManager();
            manager.LoadDoc("contactNumbers.xml");

            // Example usage:
            manager.AddContactNumber();
            manager.DisplayNodes();
            manager.DeleteContactNumber("John");

            // Save changes back to the XML file
            manager.numbers.Save("contactNumbers.xml");

            Console.ReadKey();
        }
    }
}