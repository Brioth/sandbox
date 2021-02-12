using SSMLVerifier;
using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;

namespace Sandbox
{
    class Program
    {
        static void Main(string[] args)
        {

            // system.xml
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();

                // without explicitly adding 1988/namespace I get a missing attribute, but it should be loaded when importing
                // at this point it runs but doesn't throw an error when an invalid element has been added
                settings.Schemas.Add("http://www.w3.org/XML/1998/namespace", "http://www.w3.org/2001/xml.xsd");
                settings.Schemas.Add("", "synthesis-nonamespace.xsd");

                // This is the schema that I should use, it references the nonamespace, which references the 1988/namespace
                // When using this, it doesn't want to resolve synthesis-nonamespace, so I'm trying to get that one to work first
                //settings.Schemas.Add("http://www.w3.org/2001/10/synthesis", "synthesis.xsd"); 

                // Microsoft example to verify it works without references
                settings.Schemas.Add("http://www.contoso.com/books", "contosoBooks.xsd");
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags |= XmlSchemaValidationFlags.AllowXmlAttributes;

                XmlReader rd = XmlReader.Create("input.xml", settings);

                //Microsoft example
                //XmlReader rd = XmlReader.Create("contosoBooks.xml", settings);
                XmlDocument doc = new XmlDocument();
                doc.Load(rd);

                ValidationEventHandler eventHandler = new ValidationEventHandler(ValidationEventHandler);

                doc.Validate(eventHandler);


                // add a node so that the document is no longer valid
                //XPathNavigator navigator = doc.CreateNavigator();
                //navigator.MoveToFollowing("price", "http://www.contoso.com/books");
                //XmlWriter writer = navigator.InsertAfter();
                //writer.WriteStartElement("anotherNode", "http://www.contoso.com/books");
                //writer.WriteEndElement();
                //writer.Close();

                //doc.Validate(eventHandler);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            // nuget package SSML verifier
            //var verifier = new Verifier();
            //var errors = verifier.Verify(testSsml);
            //if (errors.Count() == 0)
            //{
            //    Console.WriteLine("SSML is valid!");
            //}
            //else
            //{
            //    foreach (var error in errors)
            //    {
            //        Console.WriteLine(error.Error);
            //    }
            //}

        }

        static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    Console.WriteLine("Error: {0}", e.Message);
                    break;
                case XmlSeverityType.Warning:
                    Console.WriteLine("Warning {0}", e.Message);
                    break;
            }
        }
    }
}
