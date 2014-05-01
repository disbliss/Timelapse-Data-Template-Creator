using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TimelapseTemplateEditor
{
    static class Constants
    {
        
        // About this version (used to construct the About message box)
        public const string ABOUT_VERSION = "Version: 1.0 beta 2";
        public const string ABOUT_AUTHORS = "Authors: Matthew Dunlap and Saul Greenberg";
        public const string ABOUT_DATE = "Date: December 23, 2013";
        public const string ABOUT_CAPTION = "About Timeplapse Template Editor";

        // Code Template file name
        public const string CT_ROOTFILENAME = "CodeTemplate";
        public const string CT_FILENAMEEXTENSION = ".xml";
        
        // Database file name
        public const string DB_ROOTFILENAME = "DataTemplate";
        public const string DB_FILENAMEEXTENSION = ".db";
        
        // Database table name
        public const string TABLENAME = "template_table";                // The name of the primary table in the data template
        public const string TABLECREATIONSTRING = "Id integer primary key autoincrement, SortOrder int, Type text, DefaultValue text, Label text, DataLabel text, Tooltip text, TXTBOXWIDTH text, Copyable text, Visible text, List text";


        // Database fields
        public const string TYPE = "Type";                  // the data type
        public const string ID = "Id";                      // the unique id of the table row
        public const string DEFAULT = "DefaultValue";       // a default value for that code
        public const string LABEL = "Label";                // UI: the label associated with that data
        public const string DATALABEL = "DataLabel";        // if not empty, this label used instead of the label as the header for the column when writing the spreadsheet
        public const string TOOLTIP = "Tooltip";            // UI: tooltip text that describes the code
        public const string TXTBOXWIDTH = "TXTBOXWIDTH";    // UI: the width of the textbox 
        public const string COPYABLE = "Copyable";          // whether the content of this item should be copied from previous values
        public const string VISIBLE = "Visible";            // UI: whether an item should be visible in the user interface
        public const string LIST = "List";                  // indicates a list of items, used in building fixed choices
        public const string SORTORDER = "SortOrder";        // UI: the order position of items to be used in laying out the controls in the interface


        // Defines the allowable types 
        public const string FILE = "File";                  // TYPE: the file name
        public const string FOLDER = "Folder";              // TYPE: the folder path
        public const string DATE = "Date";                  // TYPE: date image was taken
        public const string TIME = "Time";                  // TYPE: time image was taken
        public const string IMAGEQUALITY = "ImageQuality";  // TYPE: a special fixed choice pre-filled with image quality items
        public const string NOTE = "Note";                  // TYPE: A text item
        public const string COUNTER = "Counter";            // TYPE: A counter item
        public const string CHOICE = "FixedChoice";         // TYPE: a fixed choice data type (i.e., a drop-down menu of items)

        // Database default Values for above fields
        public const string LABEL_FILE = "File";                  // Label for: the file name
        public const string LABEL_FOLDER = "Folder";              // Label for: the folder path
        public const string LABEL_DATE = "Date";                  // Label for: date image was taken
        public const string LABEL_TIME = "Time";                  // Label for: time image was taken
        public const string LABEL_IMAGEQUALITY = "Image quality"; // Label for: the Image Quality

        public const string DEFAULT_FILE = "";                  // Default for: the file name
        public const string DEFAULT_FOLDER = "";                // Default for: the folder path
        public const string DEFAULT_DATE = "";                  // Default for: date image was taken
        public const string DEFAULT_TIME = "";                  // Default for: time image was taken
        public const string DEFAULT_IMAGEQUALITY = "";          // Default for: time image was taken
        public const string DEFAULT_COUNTER = "0";              // Default for: counters
        public const string DEFAULT_NOTE = "";                  // Default for: notes
        public const string DEFAULT_CHOICE = "";                // Default for: choices

        public const string TXTBOXWIDTH_FILE = "100";
        public const string TXTBOXWIDTH_FOLDER = "100";
        public const string TXTBOXWIDTH_DATE = "100";
        public const string TXTBOXWIDTH_TIME = "100";
        public const string TXTBOXWIDTH_IMAGEQUALITY = "80";
        public const string TXTBOXWIDTH_COUNTER = "80";
        public const string TXTBOXWIDTH_NOTE = "100";
        public const string TXTBOXWIDTH_CHOICE = "100";

        public const string LIST_IMAGEQUALITY = "Ok| Dark| Corrupted";


        // Tooltip strings
        public const string TOOLTIP_FILE = "The image file name";
        public const string TOOLTIP_FOLDER = "Name of the folder containing the images";
        public const string TOOLTIP_DATE = "Date the image was taken";
        public const string TOOLTIP_TIME = "Time the image was taken";
        public const string TOOLTIP_IMAGEQUALITY = "System-determined image quality: Ok, dark if mostly black, corrupted if it can not be read";

    }
}
