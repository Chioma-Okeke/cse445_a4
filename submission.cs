using System;
using System.Xml.Schema;
using System.Xml;
using Newtonsoft.Json;
using System.Text;

/**
 * This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
 * Please do not modify or delete any existing class/variable/method names. However, you can add more variables and functions.
 * Uploading this file directly will not pass the autograder's compilation check, resulting in a grade of 0.
 * **/

namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://chioma-okeke.github.io/cse445_a4/NationalParks.xml";
        public static string xmlErrorURL = "https://chioma-okeke.github.io/cse445_a4/NationalParksErrors.xml";
        public static string xsdURL = "https://chioma-okeke.github.io/cse445_a4/NationalParks.xsd";

        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);

            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);

            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }

        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            StringBuilder errorMessages = new StringBuilder();

            try
            {
                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(null, xsdUrl);

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.Schemas = schemaSet;
                settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;

                settings.ValidationEventHandler += delegate (object sender, ValidationEventArgs e)
                {
                    errorMessages.AppendLine(e.Message);
                };

                using (XmlReader reader = XmlReader.Create(xmlUrl, settings))
                {
                    while (reader.Read())
                    {
                        // No manual code is needed here.
                        // The XmlReader validates each XML node automatically as it is read.
                    }
                }

                if (errorMessages.Length == 0)
                {
                    return "No errors are found";
                }

                return errorMessages.ToString();
            }
            catch (Exception ex)
            {
                if (errorMessages.Length > 0)
                {
                    errorMessages.AppendLine(ex.Message);
                    return errorMessages.ToString();
                }

                return ex.Message;
            }
        }

        // Q2.2
        public static string Xml2Json(string xmlUrl)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xmlUrl);

            string jsonText = JsonConvert.SerializeXmlNode(doc);

            // This confirms the returned JSON can be deserialized by Newtonsoft.Json.
            JsonConvert.DeserializeXmlNode(jsonText);

            return jsonText;
        }

        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}
