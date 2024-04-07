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
using System.Net.NetworkInformation;

namespace ConsoleApp9
{
    class Number
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string contact_number { get; set; }
        public Number(string name, string surname, string contact_number) { this.name = name; this.surname = surname; this.contact_number = contact_number; }
    }
    class ContactManager
    {
        public XmlDocument numbers { get; set; }
        public XmlElement contacts { get; set; }
        public string path;
        /*
        public ContactManager()
        {
            numbers = new XmlDocument();
        }
        */

        public void LoadDoc(string path)
        {
            if (File.Exists(path))
            {
                numbers.Load(path);
                this.path = path;
            }
            else
            {
                Console.WriteLine("This database doesn't exist.");
            }
        }

        public bool IsFileHasPath()
        {
            if (String.IsNullOrEmpty(this.path))
            {
                return true;
            }
            return false;
        }

        public void OpenFile()
        {
            if (IsFileHasPath())
            {
                File.Open(this.path, FileMode.Open);
            } 
            else
            {
                Console.WriteLine("Wrong path. Try again.");
            }
            
        }

        protected void LoadDataToDB(string name, string surname, string contactNumber)
        {
            XmlElement nameElement = numbers.CreateElement("Name");
            XmlElement surnameElement = numbers.CreateElement("Surname");
            XmlElement contactNumberElement = numbers.CreateElement("Contact Number");

            nameElement.InnerText = name;
            surnameElement.InnerText = surname;
            contactNumberElement.InnerText = contactNumber;

            this.contacts.AppendChild(nameElement);
            this.contacts.AppendChild(surnameElement);
            this.contacts.AppendChild(contactNumberElement);            
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

        public void CloseFile()
        {
            numbers.Save(path); 
            numbers = null; 
            GC.Collect(); 
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
        /*
        public ContactManager(string path) {
            if (File.Exists(path))
            {
                this.path = path;
                LoadDoc(path);
            }
            else
            {
                Console.WriteLine($"This file doesn't exist with path \"{path}\". Retry again.");
            }
        }
        */
        public ContactManager(string path)
        {
            this.numbers = new XmlDocument();

            LoadDoc(path);

            this.contacts = this.numbers.CreateElement("Contacts");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ContactManager manager = new ContactManager("C:\\Users\\Maksim\\source\\repos\\ConsoleApp9\\ConsoleApp9\\contactNumbers.xml");
            XmlElement people = manager.numbers.CreateElement("People");

            if (manager.IsFileHasPath())
            {
                manager.OpenFile();

                while (true)
                {
                    int choice = int.Parse(Console.ReadLine());
                    switch (choice)
                    {
                        case 1:
                            manager.AddContactNumber();
                            continue;
                        case 2:
                            Console.Write("Enter name of contact: ");
                            string name = Console.ReadLine();
                            manager.DeleteContactNumber(name);
                            continue;
                        case 3:
                            manager.DisplayNodes();
                            continue;
                        case 4:
                            Console.Write("Enter your path: ");
                            string path = Console.ReadLine();
                            manager.LoadDoc(path);
                            continue;
                    }
                }
                manager.CloseFile();
            }
            else
            {
                Console.WriteLine("This database isn't exist.");
            }

            manager.CloseFile();
            Console.ReadKey();
        }
    }
}
