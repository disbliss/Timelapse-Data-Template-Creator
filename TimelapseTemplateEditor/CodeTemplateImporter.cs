using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;

using System.Data;
using System.Data.SQLite;
using System.IO;
using Microsoft.Windows.Controls;

namespace TimelapseTemplateEditor
{
    // This clase reads in the code_template.xml file (the old way that we used to specify the template) 
    // and converts it into a data template database.
    public class CodeTemplateImporter
    {
        #region constants
        // STRING PORTIONS to find the XML tags in the XML Code Template File.
        const string SLASH = "/";
        const string CODES = "Codes";

        // Paths to standard elements, always included but not always made visible
        const string _FILE = "_File";
        const string FILEPATH = CODES + SLASH + _FILE;

        const string _FOLDER = "_Folder";
        const string FOLDERPATH = CODES + SLASH + _FOLDER;

        const string _DATE = "_Date";
        const string DATEPATH = CODES + SLASH + _DATE;

        const string _TIME = "_Time";
        const string TIMEPATH = CODES + SLASH + _TIME;

        const string _IMAGEQUALITY = "_ImageQuality";
        const string IMAGEQUALITYPATH = CODES + SLASH + _IMAGEQUALITY;

        const string DATA = "Data";             // the data describing the attributes of that code
        const string ITEM = "Item";             // and item in a list

        // Paths to Notes, counters, and fixed choices
        const string NOTEPATH = CODES + SLASH + Constants.NOTE;
        const string COUNTERPATH = CODES + SLASH + Constants.COUNTER;
        const string FIXEDCHOICES = "FixedChoices";
 
        const string FIXEDCHOICEPATH = CODES + SLASH + Constants.CHOICE;

        #endregion

        #region Read the Codes
        static public void Convert(string filePath, DataTable templateTable)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNodeList nodelist;
            XmlNodeList nodeData;
            int index = -1;
            xmlDoc.Load(filePath);  // Load the XML document (the code template file)

            nodelist = xmlDoc.SelectNodes(FILEPATH); // Convert the File type 
            nodeData = nodelist[0].SelectNodes(DATA);
            index = FindRow (nodelist, templateTable, Constants.FILE);
            UpdateRow (nodeData, templateTable, Constants.FILE, index);

            nodelist = xmlDoc.SelectNodes(FOLDERPATH); // Convert the Folder type
            nodeData = nodelist[0].SelectNodes(DATA);
            index = FindRow(nodelist, templateTable, Constants.FOLDER); 
            UpdateRow(nodeData, templateTable, Constants.FOLDER, index);

            nodelist = xmlDoc.SelectNodes(DATEPATH); // Convert the Date type
            nodeData = nodelist[0].SelectNodes(DATA);
            index = FindRow(nodelist, templateTable, Constants.DATE); 
            UpdateRow(nodeData, templateTable, Constants.DATE, index);

            nodelist = xmlDoc.SelectNodes(TIMEPATH); // Convert the Time type
            nodeData = nodelist[0].SelectNodes(DATA);
            index = FindRow(nodelist, templateTable, Constants.TIME); 
            UpdateRow(nodeData, templateTable, Constants.TIME, index);

            nodelist = xmlDoc.SelectNodes(IMAGEQUALITYPATH); // Convert the Image Quality type
            nodeData = nodelist[0].SelectNodes(DATA);
            index = FindRow(nodelist, templateTable, Constants.IMAGEQUALITY); 
            UpdateRow(nodeData, templateTable, Constants.IMAGEQUALITY, index);

            // Convert the Notes types, if any
            nodelist = xmlDoc.SelectNodes(NOTEPATH);
            for (int i = 0; i < nodelist.Count; i++)
            {
                // Get the XML section containing values for each note
                nodeData = nodelist[i].SelectNodes(DATA);
                AddRow(nodeData, templateTable, Constants.NOTE);
            }

            // Convert the Choices types, if any
            nodelist = xmlDoc.SelectNodes(FIXEDCHOICEPATH);
            for (int i = 0; i < nodelist.Count; i++)
            {
                // Get the XML section containing values for each choice
                nodeData = nodelist[i].SelectNodes(DATA);
                AddRow(nodeData, templateTable, Constants.CHOICE);
            }
            
            // Convert the Counts types, if any
            nodelist = xmlDoc.SelectNodes(COUNTERPATH);
            for (int i = 0; i < nodelist.Count; i++)
            {
                // Get the XML section containing values for each note
                nodeData = nodelist[i].SelectNodes(DATA);
                AddRow(nodeData, templateTable, Constants.COUNTER);
            }
        }
        #endregion

        #region Find, update and add rows 
        // Given a typeWanted (i.e., which should be one of the default types as only one of them exists), find its first occurance. 
        // If and only if its found, update the row with the XML information.
        private static int FindRow(XmlNodeList nodelist, DataTable templateTable, string typeWanted)
        {
            int index = -1;
            DataRow row = null;

            // Update the File type 
            if (nodelist.Count == 1) // There should be only one
            {
                // Find the row of a given type
                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    row = templateTable.Rows[i];
                    string type = row[Constants.TYPE].ToString();
                    if (type == typeWanted)
                    {
                        index = i;
                        break;
                    }
                }
                if (index >= 0) UpdateRow(nodelist, templateTable, typeWanted, index);
            }
            return index;
        }

        //currently used to update the default table with new values
        private static void UpdateRow(XmlNodeList nodeData, DataTable templateTable, string typeWanted, int index)
        {
            templateTable.Rows[index][Constants.DEFAULT] = TextFromNode(nodeData, 0, Constants.DEFAULT);
            templateTable.Rows[index][Constants.LABEL] = TextFromNode(nodeData, 0, Constants.LABEL);
            templateTable.Rows[index][Constants.DATALABEL] = TextFromNode(nodeData, 0, Constants.DATALABEL);
            templateTable.Rows[index][Constants.TOOLTIP] = TextFromNode(nodeData, 0, Constants.TOOLTIP);
            templateTable.Rows[index][Constants.TXTBOXWIDTH] = TextFromNode(nodeData, 0, Constants.TXTBOXWIDTH);
            templateTable.Rows[index][Constants.COPYABLE] = TextFromNode(nodeData, 0, Constants.COPYABLE);
            templateTable.Rows[index][Constants.VISIBLE] = TextFromNode(nodeData, 0, Constants.VISIBLE);

            //if the type has a list, we have to do more work.
            if (typeWanted == Constants.IMAGEQUALITY || typeWanted == Constants.CHOICE)  // Load up the menu items
            {
                templateTable.Rows[index][Constants.LIST] = "";
                XmlNodeList nItems = nodeData[0].SelectNodes(Constants.LIST + SLASH + ITEM);  
                bool firsttime = true;
                foreach (XmlNode nodeItem in nItems)
                {
                    if (firsttime)
                    {
                        templateTable.Rows[index][Constants.LIST] = nodeItem.InnerText; //also clears the list's default values
                    }
                    else
                    {
                        templateTable.Rows[index][Constants.LIST] += "|" + nodeItem.InnerText;
                    }
                    firsttime = false;
                }
            }
        }

        // Add a new row onto the table
        private static void AddRow(XmlNodeList nodelist, DataTable templateTable, string typeWanted)
        {
            // First, populate the row with default values
            templateTable.Rows.Add();

            int index = templateTable.Rows.Count - 1;
            templateTable.Rows[index][Constants.SORTORDER] = templateTable.Rows.Count;
            if (typeWanted.Equals(Constants.COUNTER))
            {
                templateTable.Rows[index][Constants.DEFAULT] = "0";
                templateTable.Rows[index][Constants.TYPE] = Constants.COUNTER;
                templateTable.Rows[index][Constants.TXTBOXWIDTH] = Constants.TXTBOXWIDTH_COUNTER;
                templateTable.Rows[index][Constants.COPYABLE] = false;
                templateTable.Rows[index][Constants.VISIBLE] = true;
            }
            else if (typeWanted.Equals(Constants.NOTE))
            {
                templateTable.Rows[index][Constants.TYPE] = Constants.NOTE;
                templateTable.Rows[index][Constants.TXTBOXWIDTH] = Constants.TXTBOXWIDTH_NOTE;
                templateTable.Rows[index][Constants.VISIBLE] = true;
            }
            else if (typeWanted.Equals(Constants.CHOICE))
            {
                templateTable.Rows[index][Constants.TYPE] = Constants.CHOICE;
                templateTable.Rows[index][Constants.TXTBOXWIDTH] = Constants.TXTBOXWIDTH_CHOICE;
                templateTable.Rows[index][Constants.COPYABLE] = true;
                templateTable.Rows[index][Constants.VISIBLE] = true;
            }

            // Now update the templatetable with the new values
            UpdateRow(nodelist, templateTable, typeWanted, index);
        }
        #endregion Find, update and add rows

        #region Utilities
        // Given a nodelist, get the text associated with it 
        private static string TextFromNode(XmlNodeList node, int nodeIndex, string nodeToFind)
        {
            XmlNodeList n = node[nodeIndex].SelectNodes(nodeToFind);
            if (n.Count == 0) return ""; //The node doesn't exist
            return n[0].InnerText;
        }
        #endregion
    }
}
