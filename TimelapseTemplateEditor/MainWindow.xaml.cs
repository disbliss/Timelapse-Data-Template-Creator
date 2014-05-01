// <copyright company="University of Calgary">
// Open source, please ask at this point, license undecided
// </copyright>
// <author>Matthew Alan Dunlap and Saul Greenberg</author>
// <version>1.0.beta.1</version>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using Microsoft.Windows.Controls;
using System.Collections.ObjectModel;
using System.Globalization;
using Microsoft.Windows.Controls.Primitives;

namespace TimelapseTemplateEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        // Interface values
        System.Windows.Media.SolidColorBrush notEditableCellColor = System.Windows.Media.Brushes.LightGray; // Color of non-editable data grid items 

        // Database-related values
        SQLiteDatabase db;                                  // The Database where the template is stored
       
        String dbFileName;                                  //  Filename of the database; by convention it is DataTemplate.db
        public DataTable templateTable = new DataTable();   // The table holding the primary data template

        // Combobx manipulations and values
        CsvHelperMethods h = new CsvHelperMethods();        // A class for converting data into comma-separated values to deal with our lists in our choice items
        String currentComboItem; //The last comboBoxItem (in string form) selected. Used in extending the combobox.
        int minimumComboBoxItems = 1; //There will always be an edit box in the combobox
 
        // Booleans for tracking state
        bool rowsActionsOn = false;
        bool tabWasPressed = false; //to make tab trigger row update.
        #endregion Variables

        #region Constants
        const string WINDOWBASETITLE = "Timelapse Template Editor";  // The initial title shown in the window title bar
        const string REMOVEBUTTON_REMOVEITEM = "Remove Item";
        const string REMOVEBUTTON_REMOVEITEMNUMBER = REMOVEBUTTON_REMOVEITEM + " #";
        const string REMOVEBUTTON_CANTDELETE = "Item Cannot Be Deleted";
        const string TAG_UP = "Up";
        const string TAG_DOWN = "Down";

        // Database query phrases
        const string SELECTSTAR = "SELECT * FROM ";
        const string BYSORTORDER = " ORDER BY " + Constants.SORTORDER;
        #endregion

        #region Constructors/Destructors
        /// <summary>
        /// Starts the UI.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += MainWindow_Closing;
        }
         
        // When the main window closes, apply any pending edits.
        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TemplateDataGrid.CommitEdit(); 
        }

         #endregion Constructors/Destructors

        #region DataGrid and New Database Initialization
        /// <summary>
        /// Given a database file path,create a new DB file if one does not exist, or load a DB file if there is one.
        /// After a DB file is loaded, the table is extracted and loaded a DataTable for binding to the DataGrid.
        /// Some listeners are added to the DataTable, and the DataTable is bound. The add row buttons are enabled.
        /// </summary>
        /// <param name="databasePath">The path of the DB file created or loaded</param>
        /// <param name="ourTableName">The name of the table loaded in the DB file. Always the same in the current implementation</param>
        public void InitializeDataGrid(String databasePath, String ourTableName)
        {
            // Create a new DB file if one does not exist, or load a DB file if there is one.
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
                db = new SQLiteDatabase(databasePath);
                PopulateNewDB();
            }
            else
            {
                db = new SQLiteDatabase(databasePath);
            }
            // Have the window title include the database file name
            OnlyWindow.Title = WINDOWBASETITLE + " | File: " + System.IO.Path.GetFileName(dbFileName);

            // Load the  template table from the database into the data table, map to the data grid, and create a callback execured whenever the datatable row changes
            templateTable = db.GetDataTable(SELECTSTAR + ourTableName + BYSORTORDER);
            TemplateDataGrid.DataContext = templateTable;
            templateTable.RowChanged += templateTable_RowChanged;

            //Now that there is a data table, enable the buttons that allows rows to be added.
            this.AddCountRowButton.IsEnabled = true;
            this.AddChoiceRowButton.IsEnabled = true;
            this.AddNoteRowButton.IsEnabled = true;

            // Generate the example user interface specified by the contents of the table
            ControlGeneration.GenerateControls(this, this.wp, this.templateTable);
        }
   

        /// <summary>
        /// Called when a new DB is created. A new table is created and populated with required fields set to their default values.
        /// </summary>
        public void PopulateNewDB()
        {
            int order = 1; // The order, incremented by 1 for every new entry

            // Create a table. 
            this.db.CreateTable(Constants.TABLENAME, Constants.TABLECREATIONSTRING);

            // To fill the table, we will  create a line with default values for each row, then add that line to the dataList,
            // then add the dataList to the table
            List<Dictionary<String, Object>> dataList = new List<Dictionary<String, Object>>();

            //File
            Dictionary<String, Object> dataLine = new Dictionary<String, Object>();
            dataLine.Add(Constants.ID, null);
            dataLine.Add(Constants.SORTORDER, (order++).ToString () );
            dataLine.Add(Constants.TYPE, Constants.FILE);
            dataLine.Add(Constants.DEFAULT, Constants.DEFAULT_FILE);
            dataLine.Add(Constants.LABEL, Constants.LABEL_FILE);
            dataLine.Add(Constants.DATALABEL, Constants.LABEL);
            dataLine.Add(Constants.TOOLTIP, Constants.TOOLTIP_FILE);
            dataLine.Add(Constants.TXTBOXWIDTH, Constants.TXTBOXWIDTH_FILE);
            dataLine.Add(Constants.COPYABLE, "false");
            dataLine.Add(Constants.VISIBLE, "true");
            dataLine.Add(Constants.LIST, "");
            dataList.Add(dataLine);

            //Folder
            dataLine = new Dictionary<String, Object>();
            dataLine.Add(Constants.ID, null);
            dataLine.Add(Constants.SORTORDER, (order++).ToString ()  );
            dataLine.Add(Constants.TYPE, Constants.FOLDER);
            dataLine.Add(Constants.DEFAULT, Constants.DEFAULT_FOLDER);
            dataLine.Add(Constants.LABEL, Constants.LABEL_FOLDER);
            dataLine.Add(Constants.DATALABEL, Constants.LABEL);
            dataLine.Add(Constants.TOOLTIP, Constants.TOOLTIP_FOLDER);
            dataLine.Add(Constants.TXTBOXWIDTH, Constants.TXTBOXWIDTH_FOLDER);
            dataLine.Add(Constants.COPYABLE, "false");
            dataLine.Add(Constants.VISIBLE, "true");
            dataLine.Add(Constants.LIST, "");
            dataList.Add(dataLine);

            //Date
            dataLine = new Dictionary<String, Object>();
            dataLine.Add(Constants.ID, null);
            dataLine.Add(Constants.SORTORDER, (order++).ToString());
            dataLine.Add(Constants.TYPE, Constants.DATE);
            dataLine.Add(Constants.DEFAULT, Constants.DEFAULT_DATE);
            dataLine.Add(Constants.LABEL, Constants.LABEL_DATE);
            dataLine.Add(Constants.DATALABEL, Constants.LABEL);
            dataLine.Add(Constants.TOOLTIP, Constants.TOOLTIP_DATE);
            dataLine.Add(Constants.TXTBOXWIDTH, Constants.TXTBOXWIDTH_DATE);
            dataLine.Add(Constants.COPYABLE, "false");
            dataLine.Add(Constants.VISIBLE, "true");
            dataLine.Add(Constants.LIST, "");
            dataList.Add(dataLine);

            //Time
            dataLine = new Dictionary<String, Object>();
            dataLine.Add(Constants.ID, null);
            dataLine.Add(Constants.SORTORDER, (order++).ToString());
            dataLine.Add(Constants.TYPE, Constants.TIME);
            dataLine.Add(Constants.DEFAULT, Constants.DEFAULT_TIME);
            dataLine.Add(Constants.LABEL, Constants.LABEL_TIME);
            dataLine.Add(Constants.DATALABEL, Constants.LABEL);
            dataLine.Add(Constants.TOOLTIP, Constants.TOOLTIP_TIME);
            dataLine.Add(Constants.TXTBOXWIDTH, Constants.TXTBOXWIDTH_TIME);
            dataLine.Add(Constants.COPYABLE, "false");
            dataLine.Add(Constants.VISIBLE, "true");
            dataLine.Add(Constants.LIST, "");
            dataList.Add(dataLine);

            //Image Quality
            dataLine = new Dictionary<String, Object>();
            dataLine.Add(Constants.ID, null);
            dataLine.Add(Constants.SORTORDER, (order++).ToString());
            dataLine.Add(Constants.TYPE, Constants.IMAGEQUALITY);
            dataLine.Add(Constants.DEFAULT, Constants.DEFAULT_IMAGEQUALITY);
            dataLine.Add(Constants.LABEL, Constants.LABEL_IMAGEQUALITY);
            dataLine.Add(Constants.DATALABEL, Constants.LABEL);
            dataLine.Add(Constants.TOOLTIP, Constants.TOOLTIP_IMAGEQUALITY);
            dataLine.Add(Constants.TXTBOXWIDTH, Constants.TXTBOXWIDTH_IMAGEQUALITY);
            dataLine.Add(Constants.COPYABLE, "false");
            dataLine.Add(Constants.VISIBLE, "true");
            dataLine.Add(Constants.LIST, Constants.LIST_IMAGEQUALITY);
            dataList.Add(dataLine);

            // Insert the datalist into the table
            db.NewInsertMultiple(Constants.TABLENAME, dataList);
        }
        #endregion DataGrid and New Database Initialization

        #region Data Changed Listeners and Methods
        ///////////////////////////////////////////////////////////////////////////////////////////
        //// DATA TABLE CHANGED LISTENERS & METHODS.
        ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Updates the db with the current state of the DataGrid. 
        /// Essentially clears and rebuilds the database table, so is very inefficient unless one really wants to do this
        /// </summary>
        private void UpdateDBFull()
        {
            //Build a Dictionary with all the rows/columns corresponding to the grid. 
            List<Dictionary<String, String>> dictionaryList = new List<Dictionary<string, string>>();
            DataRowCollection rowCol = templateTable.Rows;
            foreach (DataRow row in templateTable.Rows)
            {
                Dictionary<String, String> dataLine = new Dictionary<String, String>();
                for (int i = 0; i < row.ItemArray.Length; i++)
                {
                    //sanitize quotes and add to Dictionary.
                    String value = row[i].ToString();
                    value = value.Replace("'", "''");
                    dataLine.Add(templateTable.Columns[i].ToString(), value);
                }
                dictionaryList.Add(dataLine);
            }
            
            // Clear the existing table and add the new values
            db.ClearTable(Constants.TABLENAME);
            db.InsertMultiple(Constants.TABLENAME, dictionaryList);

            // Update the simulated UI controls so that it reflects the current values in the database
            ControlGeneration.GenerateControls(this, this.wp, this.templateTable);
        }

        /// <summary>
        /// Updates a given row in the db with the current state of the DataGrid. 
        /// </summary>
        private void UpdateDBRow(DataRow row)
        {
            Dictionary<String, Object> rowDict = new Dictionary<String, Object>();
            for (int i = 0; i < row.ItemArray.Length; i++)
            {
                //sanitize quotes and add to Dictionary.
                String value = row[i].ToString();
                value = value.Replace("'", "''");
                rowDict.Add(templateTable.Columns[i].ToString(), value);
            }
            String where = Constants.ID + " = " + row[Constants.ID];
            db.UpdateVerbose(Constants.TABLENAME, rowDict, where);

            // Update the simulatedcontrols so that it reflects the current values in the database
            ControlGeneration.GenerateControls(this, this.wp, this.templateTable);
        }

        ///// <summary>
        ///// Whenever a row changes , save the db, which also updates the grid colors.
        ///// </summary>
        void templateTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            if (!rowsActionsOn)
            {
                UpdateDBRow(e.Row);
            }
        }
        #endregion Data Changed Listeners and Methods=

        #region Datagrid Row Modifyiers listeners and methods
        ///////////////////////////////////////////////////////////////////////////////////////////
        //// DATAGRID ROW MODIFYING LISTENERS & METHODS.
        ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Logic to enable/disable editing buttons depending on there being a row selection
        /// Also sets the text for the remove row button.
        /// </summary>
        private void TemplateDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView selectedRowView = TemplateDataGrid.SelectedItem as DataRowView;
            if (selectedRowView == null)
            {
                RemoveRowButton.IsEnabled = false;
                MoveRowUpButton.IsEnabled = false;
                MoveRowDownButton.IsEnabled = false;
                RemoveRowButton.Content = REMOVEBUTTON_REMOVEITEM;
            }
            else if (( selectedRowView.Row[Constants.TYPE].Equals(Constants.FILE)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.FOLDER)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.DATE)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.TIME)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.IMAGEQUALITY)))
            {
                RemoveRowButton.IsEnabled = false;
                MoveRowUpButton.IsEnabled = true;
                MoveRowDownButton.IsEnabled = true;
                RemoveRowButton.Content = REMOVEBUTTON_CANTDELETE;
            }
            else if (selectedRowView != null)
            {
                RemoveRowButton.IsEnabled = true;
                MoveRowUpButton.IsEnabled = true;
                MoveRowDownButton.IsEnabled = true;
                RemoveRowButton.Content = REMOVEBUTTON_REMOVEITEMNUMBER + selectedRowView.Row[Constants.SORTORDER];
            }
        }

        /// <summary>
        /// Adds a row to the table. The row type is decided by the button tags.
        /// Default values are set for the added row, differing depending on type.
        /// </summary>
        private void AddRowButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            templateTable.Rows.Add();
            //System.Console.WriteLine(button.Tag.ToString());
            templateTable.Rows[templateTable.Rows.Count - 1][Constants.SORTORDER] = templateTable.Rows.Count;
            if (button.Tag.ToString().Equals("Count"))
            {
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.DEFAULT] = Constants.DEFAULT_COUNTER;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.TYPE] = Constants.COUNTER;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.TXTBOXWIDTH] = Constants.TXTBOXWIDTH_COUNTER;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.COPYABLE] = false;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.VISIBLE] = true;
            }
            else if (button.Tag.ToString().Equals(Constants.NOTE))
            {
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.DEFAULT] = Constants.DEFAULT_NOTE;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.TYPE] = Constants.NOTE;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.TXTBOXWIDTH] = Constants.TXTBOXWIDTH_NOTE;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.COPYABLE] = true;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.VISIBLE] = true;
            }
            else if (button.Tag.ToString().Equals("Choice"))
            {
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.DEFAULT] = Constants.DEFAULT_CHOICE;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.TYPE] = Constants.CHOICE;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.TXTBOXWIDTH] = Constants.TXTBOXWIDTH_CHOICE;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.COPYABLE] = true;
                templateTable.Rows[templateTable.Rows.Count - 1][Constants.VISIBLE] = true;
            }

            List<Dictionary<String, Object>> list = new List<Dictionary<String, Object>>();
            Dictionary<String, Object> rowDict = new Dictionary<String, Object>();
            for (int i = 0; i < templateTable.Columns.Count; i++)
            {
                rowDict.Add(templateTable.Columns[i].ColumnName, templateTable.Rows[templateTable.Rows.Count - 1][i]);
            }
            list.Add(rowDict); //the insertmult only takes lists
            db.NewInsertMultiple(Constants.TABLENAME, list);
            TemplateDataGrid.ScrollIntoView(TemplateDataGrid.Items[TemplateDataGrid.Items.Count-1]);
         }

        /// <summary>
        /// Removes a row from the table and shifts up the ids on the remaining rows.
        /// The required rows are unable to be deleted.
        /// </summary>
        private void RemoveRowButton_Click(object sender, RoutedEventArgs e)
        {
            rowsActionsOn = true;
            DataRowView selectedRowView = TemplateDataGrid.SelectedItem as DataRowView;
            if (!(selectedRowView == null
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.FILE)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.FOLDER)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.DATE)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.TIME)
                || selectedRowView.Row[Constants.TYPE].Equals(Constants.IMAGEQUALITY)))
            {
                db.Delete(Constants.TABLENAME, Constants.ID + " = " + selectedRowView.Row[Constants.ID]);
                templateTable.Rows.Remove(selectedRowView.Row);
                for (int i = 0; i < templateTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(templateTable.Rows[i][Constants.SORTORDER]) != i + 1)
                    {
                        Dictionary<String, Object> rowDict = new Dictionary<String, Object>();
                        rowDict.Add(Constants.SORTORDER, i+1);
                        templateTable.Rows[i][Constants.SORTORDER] = i + 1;
                        db.UpdateVerbose(Constants.TABLENAME, rowDict, Constants.ID + " = " + templateTable.Rows[i][Constants.ID]);
                    }
                }

                //update the controls so that it reflects the current values in the database
                ControlGeneration.GenerateControls(this, this.wp, this.templateTable);
            }
            rowsActionsOn = false;
        }

        private void MoveRowButton_Click_Efficient(object sender, RoutedEventArgs e)
        {
            rowsActionsOn = true;
            Button button = sender as Button;
            String direction = button.Tag.ToString();
            DataRowView selectedRowView = TemplateDataGrid.SelectedItem as DataRowView;
            int selectedRowSortOrder = Convert.ToInt32(selectedRowView.Row[Constants.SORTORDER]);
            int setSelectedIndex = 0;

            if (!((selectedRowSortOrder == 1 && direction.Equals(TAG_UP))
                || (selectedRowSortOrder == templateTable.Rows.Count && direction.Equals(TAG_DOWN)))) // boundary conditions
            {   
                if (direction.Equals(TAG_UP))
                {
                    setSelectedIndex = selectedRowSortOrder - 2;
                    //For updating the table
                    //create new rows for the table, that aren't actually in the table
                    DataRow selRow = templateTable.NewRow(); //its assigning Ids... dunno why
                    DataRow rowAbove = templateTable.NewRow();
                    //set the values in those rows to be the values of the rows we are replacing
                    selRow.ItemArray = templateTable.Rows[selectedRowSortOrder - 1].ItemArray;
                    rowAbove.ItemArray = templateTable.Rows[selectedRowSortOrder - 2].ItemArray;
                    //switch SortOrder indexies
                    selRow[Constants.SORTORDER] = selectedRowSortOrder - 1;
                    rowAbove[Constants.SORTORDER] = selectedRowSortOrder;
                    //remove the two old rows and insert the new ones.
                    templateTable.Rows.RemoveAt(selectedRowSortOrder - 1);
                    templateTable.Rows.RemoveAt(selectedRowSortOrder - 2);
                    templateTable.Rows.InsertAt(selRow, selectedRowSortOrder - 2);
                    templateTable.Rows.InsertAt(rowAbove, selectedRowSortOrder - 1);
                    
                    //For updating the DB
                    Dictionary<String,Object> selRowDict = new Dictionary<String, Object>();
                    Dictionary<String,Object> rowAboveDict = new Dictionary<String, Object>();
                    for(int i = 1; i < templateTable.Columns.Count; i++)
                    {
                        selRowDict.Add(templateTable.Columns[i].ColumnName, selRow[i]);
                        rowAboveDict.Add(templateTable.Columns[i].ColumnName, rowAbove[i]);
                    }
                    //this would probably be more efficient with a multi-line update statement
                    db.UpdateVerbose(Constants.TABLENAME, selRowDict, Constants.ID + " = " + selRow[Constants.ID]);
                    db.UpdateVerbose(Constants.TABLENAME, rowAboveDict, Constants.ID + " = " + rowAbove[Constants.ID]);
                }
                else if (direction.Equals(TAG_DOWN))
                {
                    setSelectedIndex = selectedRowSortOrder;
                    //For updating the table
                    //create new rows for the table, that aren't actually in the table

                    DataRow selRow = templateTable.NewRow(); //its assigning Ids... dunno why
                    DataRow rowBelow = templateTable.NewRow();
                    //set the values in those rows to be the values of the rows we are replacing
                    selRow.ItemArray = templateTable.Rows[selectedRowSortOrder - 1].ItemArray;
                    rowBelow.ItemArray = templateTable.Rows[selectedRowSortOrder].ItemArray;
                    //switch SortOrder indexies
                    selRow[Constants.SORTORDER] = selectedRowSortOrder + 1;
                    rowBelow[Constants.SORTORDER] = selectedRowSortOrder;
                    //remove the two old rows and insert the new ones.
                    templateTable.Rows.RemoveAt(selectedRowSortOrder);
                    templateTable.Rows.RemoveAt(selectedRowSortOrder - 1);
                    templateTable.Rows.InsertAt(rowBelow, selectedRowSortOrder - 1);
                    templateTable.Rows.InsertAt(selRow, selectedRowSortOrder);

                    //For updating the DB
                    Dictionary<String, Object> selRowDict = new Dictionary<String, Object>();
                    Dictionary<String, Object> rowBelowDict = new Dictionary<String, Object>();
                    for (int i = 1; i < templateTable.Columns.Count; i++)
                    {
                        selRowDict.Add(templateTable.Columns[i].ColumnName, selRow[i]);
                        rowBelowDict.Add(templateTable.Columns[i].ColumnName, rowBelow[i]);
                    }
                    //this would probably be more efficient with a multi-line update statement
                    db.UpdateVerbose(Constants.TABLENAME, selRowDict, Constants.ID + " = " + selRow[Constants.ID]);
                    db.UpdateVerbose(Constants.TABLENAME, rowBelowDict, Constants.ID + " = " + rowBelow[Constants.ID]);
                }

                TemplateDataGrid.SelectedIndex = setSelectedIndex; //set the selected index to the item moved
            }
            rowsActionsOn = false;
            //update the controls so that it reflects the current values in the database
            ControlGeneration.GenerateControls(this, this.wp, this.templateTable);
        }

        #endregion Datagrid Row Modifyiers listeners and methods

        #region Combobox Listeners and Methods
        ///////////////////////////////////////////////////////////////////////////////////////////
        //// COMBOBOX  LISTENERS AND METHODS. COMBOBOXES HAVE BEEN ALTERED FOR LIST EDITING
        ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Performs different combobox actions when enter keys is pressed.
        /// The action performed depends on user input.
        /// Delete: If a user selects an item and deletes all its characters in the textinput, it is deleted.
        /// Save: If a user enters characters into the blank textbox, a new item is added.
        /// Edit: If a user selects an item and changes it in the textinput, it is changed
        /// </summary>
        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (e.Key == Key.Enter)
            {
                //Delete item, calling helper method
                if (comboBox.Text.Trim().Equals(""))
                {
                    if (!String.IsNullOrEmpty(currentComboItem))
                    {
                        String comboBoxString = templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST].ToString();
                        String newComboString = h.deleteItemFromCSV(comboBoxString, currentComboItem);
                        templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST] = newComboString;
                    }
                }

                else
                {
                    //Saves an item
                    if (String.IsNullOrEmpty(currentComboItem))
                    {
                        comboBox.Text = comboBox.Text.Replace("|", " "); //remove csv delimiter
                        if (comboBox.Items.Count == minimumComboBoxItems)
                        { // if only item, it is the whole list
                            templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST] = comboBox.Text;
                        }
                        else
                        { // if there are items, add saved item to the end
                            templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST] += "| " + comboBox.Text;
                        }
                        comboBox.Text = "";
                    }

                    //Edits item, calling helper method
                    else
                    {
                        String comboBoxString = templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST].ToString();
                        String newComboString = h.editItemInCSV(comboBoxString, currentComboItem, comboBox.Text.Trim());
                        templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST] = newComboString;
                    }
                }
            }
        }

        /// <summary>
        /// Combo Box Context Menu Edit listener. Does the same thing as clicking an item in the combobox.
        /// In Context Menu for functionality completeness.
        /// </summary>
        private void EditMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Getting the ComboBox & ComboBoxItem from sender
            var menuItem = (sender as MenuItem);
            var parentMenu = menuItem.Parent as ContextMenu;
            var cBItem = parentMenu.PlacementTarget as ComboBoxItem;
            var comboBox = ItemsControl.ItemsControlFromItemContainer(cBItem) as ComboBox;

            //Pretty much emulates the item being clicked
            //Selects the item and disables the dropdown
            cBItem.IsSelected = true;
            comboBox.IsDropDownOpen = false;
        }

        /// <summary>
        /// Combo Box Context Menu Delete listener. Does the same thing as clicking an item.
        /// Calls listener to remove item from comboBoxString.
        /// </summary>
        private void DeleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Getting the ComboBox & ComboBoxItem from sender
            var menuItem = (sender as MenuItem);
            var parentMenu = menuItem.Parent as ContextMenu;
            var cBItem = parentMenu.PlacementTarget as ComboBoxItem;
            var comboBox = ItemsControl.ItemsControlFromItemContainer(cBItem) as ComboBox;

            //deletes the item by modifying the string, calling a helper method.
            String comboBoxString = templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST].ToString();
            String selectedItemString = cBItem.Content.ToString();
            String newComboString = h.deleteItemFromCSV(comboBoxString, selectedItemString);
            templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST] = newComboString;
        }

        /// <summary>
        /// Combo Box Context Menu Move Up/Move Down listener.
        /// Creates new array with items in the moved location, turns this into the CSV string for the comboBox
        /// </summary>
        private void MoveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //Getting the ComboBox & ComboBoxItem from sender
            var menuItem = (sender as MenuItem);
            var parentMenu = menuItem.Parent as ContextMenu;
            var cBItem = parentMenu.PlacementTarget as ComboBoxItem;
            var comboBox = ItemsControl.ItemsControlFromItemContainer(cBItem) as ComboBox;

            //Moving Combobox items by shifting array elements
            String comboBoxString = templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST].ToString();
            String[] valArray = h.csvToArray(comboBoxString);
            String selectedItemString = cBItem.Content.ToString();
            int selectedItemIndex = -1;
            String[] movedValArray = new string[valArray.Length];

            // finds the index of the item being moved.
            for (int i = 0; i < valArray.Length; i++)
            {
                if (valArray[i].Equals(selectedItemString))
                {
                    selectedItemIndex = i;
                    break;
                }
            }

            //does the actual move up/down.
            if (!((selectedItemIndex == 0 && menuItem.Tag.ToString().Equals(TAG_UP))
                || (selectedItemIndex == valArray.Length - minimumComboBoxItems && menuItem.Tag.ToString().Equals(TAG_DOWN))
                || selectedItemIndex == -1)) //end if moves can't happen
            {
                if (menuItem.Tag.ToString().Equals(TAG_UP))
                {
                    for (int i = 0; i < valArray.Length; i++)
                    {
                        if (i + 1 == selectedItemIndex)
                        { //switch
                            movedValArray[i + 1] = valArray[i];
                            movedValArray[i] = valArray[i + 1];
                            i++; //increment because we already did the next one
                        }
                        else //just transfer old to new
                        {
                            movedValArray[i] = valArray[i];
                        }
                    }
                }
                else if (menuItem.Tag.ToString().Equals(TAG_DOWN))
                {
                    for (int i = 0; i < valArray.Length; i++)
                    {
                        if (i == selectedItemIndex)
                        { //switch
                            movedValArray[i] = valArray[i + 1];
                            movedValArray[i + 1] = valArray[i];
                            i++; //increment because we already did the next one
                        }
                        else //just transfer old to new
                        {
                            movedValArray[i] = valArray[i];
                        }
                    }
                }
                else
                {
                    throw new System.Exception("The xaml for item moving has been broken");
                }
                String newComboString = h.arrayToCSV(movedValArray);
                templateTable.Rows[Convert.ToInt32(comboBox.Tag) - 1][Constants.LIST] = newComboString;
            }
        }

        /// <summary>
        /// When comboBox selection changes, checks if selected item is a normal element or the special "edit item" element.
        /// Sets globals depending on the selection, which informs the keydown event on what to do.
        /// </summary>
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            //check that selected item isn't the null item 
            if (Convert.ToInt32(comboBox.SelectedIndex) != -1 && Convert.ToInt32(comboBox.SelectedIndex) != comboBox.Items.Count - 1) // this works well
            {
                currentComboItem = comboBox.SelectedItem.ToString();
            }
            else
            {
                currentComboItem = "";
                comboBox.SelectedValue = "";
                comboBox.SelectedItem = "";
            }
        }

        /// <summary>
        /// When a combobox is no longer being edited, the text field is blank and the tracker of the last combo item is reset.
        /// </summary>
        private void comboBox_IsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.Text = "";
            currentComboItem = "";
        }
        #endregion Combobox Listeners and Methods

        #region Cell Editing / Coloring Listeners and Methods
        ///////////////////////////////////////////////////////////////////////////////////////////
        /// CELL EDITING/COLORING LISTENERS AND METHODS
        ///////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Informs application tab was used, which allows it to more quickly visualize the grid values in the preview.
        /// Tab does not normal raise the rowedited listener, which we are using to do the update.
        /// </summary>
        private void TemplateDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Tab)
            {
                tabWasPressed = true;
            }
        }

        /// <summary>
        /// Before cell editing begins on a cell click, the cell is disabled if it is grey (meaning cannot be edited).
        /// Another method re-enables the cell immediately afterwards.
        /// The reason for this implementation is because disabled cells cannot be single clicked, which is needed for row actions.
        /// </summary>
        private void TemplateDataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (TemplateDataGrid.SelectedIndex != -1)
            {
                //this is how you can get an actual cell.
                DataGridRow aRow = (DataGridRow)TemplateDataGrid.ItemContainerGenerator.ContainerFromIndex(TemplateDataGrid.SelectedIndex);
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(aRow);
                if (TemplateDataGrid.CurrentColumn != null)
                {
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(TemplateDataGrid.CurrentColumn.DisplayIndex);

                    if (cell.Background.Equals(notEditableCellColor))
                    {
                        cell.IsEnabled = false;
                        TemplateDataGrid.CancelEdit();
                    }
                }
            }

        }

        /// <summary>
        /// After cell editing ends (prematurely or no), the cell is enabled.
        /// See TemplateDataGrid_BeginningEdit for full explaination.
        /// </summary>
        private void TemplateDataGrid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (TemplateDataGrid.SelectedIndex != -1)
            {
                //this is how you can get an actual cell.
                DataGridRow aRow = (DataGridRow)TemplateDataGrid.ItemContainerGenerator.ContainerFromIndex(TemplateDataGrid.SelectedIndex);
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(aRow);
                if (TemplateDataGrid.CurrentColumn != null)
                {
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(TemplateDataGrid.CurrentColumn.DisplayIndex);
                    cell.IsEnabled = true;
                }
            }
            if (tabWasPressed)
            {
                tabWasPressed = false;
                DataRowView selectedRowView = TemplateDataGrid.SelectedItem as DataRowView; //current cell
                UpdateDBRow(selectedRowView.Row);
            }
        }

        //constants for below method. Could not find another way to reference the DataGrid items by name
        //If the DataGrid columns change, this needs to be adjusted to index correctly.
        int DG_TYPE         = 2;

        //more constants to access checkbox columns and combobox columns.
        //the sortmemberpath is include, not the sort name, so we are accessing by head, which may change.
        string DG_HEAD_COPYABLE = Constants.COPYABLE;
        string DG_HEAD_LIST     = Constants.LIST;

        /// <summary>
        /// Greys out cells as defined by logic. 
        /// This is to visually show the user uneditable cells, and informs events about whether a cell can be edited.
        /// This is called after row are added/moved/deleted to update the colors. 
        /// This also disables checkboxes that cannot be edited. Disabling checkboxes does not effect row interactions.
        /// </summary>
        public void UpdateCellColors()
        {
            for (int i = 0; i < TemplateDataGrid.Items.Count; i++)
            {
                DataGridRow aRow = (DataGridRow)TemplateDataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                if (aRow != null)
                {
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(aRow);
                    for (int j = 0; j < TemplateDataGrid.Columns.Count; j++)
                    {
                        DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(j);
                        DataRowView thisRow = (DataRowView)TemplateDataGrid.Items[i];
                         
                        if(TemplateDataGrid.Columns[j].SortMemberPath.Equals(Constants.ID)
                            || TemplateDataGrid.Columns[j].SortMemberPath.Equals(Constants.SORTORDER)
                            || TemplateDataGrid.Columns[j].SortMemberPath.Equals(Constants.TYPE)
                            || thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.FILE)
                            || thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.FOLDER)
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.DATE) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_LIST))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.TIME) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_LIST))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.COUNTER) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_LIST))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.NOTE) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_LIST))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.FILE) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_COPYABLE))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.FOLDER) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_COPYABLE))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.DATE) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_COPYABLE))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.TIME) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_COPYABLE))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.IMAGEQUALITY) && TemplateDataGrid.Columns[j].Header.Equals(DG_HEAD_COPYABLE))
                            || (thisRow.Row.ItemArray[DG_TYPE].Equals(Constants.IMAGEQUALITY) && TemplateDataGrid.Columns[j].SortMemberPath.Equals(Constants.DEFAULT)))
                        {
                            cell.Background = notEditableCellColor;

                            //if cell has a checkbox, also disable it.
                            var cp = cell.Content as ContentPresenter;
                            if (cp != null)
                            {
                                var checkbox = cp.ContentTemplate.FindName("CheckBox", cp) as CheckBox;
                                if (checkbox != null)
                                {
                                    checkbox.IsEnabled = false;
                                }
                            }
                        }
                        else
                        {
                            cell.ClearValue(DataGridCell.BackgroundProperty); //otherwise when scrolling cells offscreen get colored randomly
                            var cp = cell.Content as ContentPresenter;

                            //if cell has a checkbox, enable it.
                            if (cp != null)
                            {
                                var checkbox = cp.ContentTemplate.FindName("CheckBox", cp) as CheckBox;
                                if (checkbox != null)
                                {
                                    checkbox.IsEnabled = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Updates colors when the Layout changes.
        /// </summary>
        private void TemplateDataGrid_LayoutUpdated(object sender, EventArgs e)
        {
            UpdateCellColors();
        }

        /// <summary>
        /// Used in this code to get the child of a DataGridRows, DataGridCellsPresenter. This can be used to get the DataGridCell.
        /// WPF does not make it easy to get to the actual cells.
        /// Code from: http://techiethings.blogspot.com/2010/05/get-wpf-datagrid-row-and-cell.html
        /// </summary>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
        #endregion Cell Editing / Coloring Listeners and Methods

        #region Menu listeners
        ///////////////////////////////////////////////////////////////////////////////////////////
        //// MENU BAR METHODS & LISTENERS
        ///////////////////////////////////////////////////////////////////////////////////////////		
        
        /// <summary>
        /// Creates a new database file of a user chosen name in a user chosen location.
        /// </summary>
        private void NewFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TemplateDataGrid.CommitEdit(); //to apply edits that the enter key was not pressed
            
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = Constants.DB_ROOTFILENAME; // Default file name without the extension
            dlg.DefaultExt = Constants.DB_FILENAMEEXTENSION; // Default file extension
            dlg.Filter = "Database Files (" + Constants.DB_FILENAMEEXTENSION + ")|*" + Constants.DB_FILENAMEEXTENSION; // Filter files by extension 
            dlg.Title = "Select Location to Save New Template File";
            
            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results 
            if (result == true)
            {
                SaveDbBackup();
                if (File.Exists(dlg.FileName))     // Overwrite the file if it exists
                {
                    File.Delete(dlg.FileName);
                }
                
                // Open document 
                dbFileName = dlg.FileName;
                InitializeDataGrid(dbFileName, Constants.TABLENAME);
            }

        }

        /// <summary>
        /// Opens a database file.
        /// </summary>
        private void OpenFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TemplateDataGrid.CommitEdit(); //to save any edits that the enter key was not pressed
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = Constants.DB_ROOTFILENAME; // Default file name without the extension
            dlg.DefaultExt = Constants.DB_FILENAMEEXTENSION; // Default file extension
            dlg.Filter = "Database Files (" + Constants.DB_FILENAMEEXTENSION + ")|*" + Constants.DB_FILENAMEEXTENSION; // Filter files by extension 
            dlg.Title = "Select an Existing Template File to Open";

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                SaveDbBackup();

                // Open document 
                dbFileName = dlg.FileName;
                InitializeDataGrid(dbFileName, Constants.TABLENAME);
            }
        }

        //Commented until I know how save should work.
        private void SaveAsFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            
            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            //dlg.FileName = dbName + "(Copy)"; // Default file name
            //dlg.DefaultExt = ".db"; // Default file extension
            //dlg.Filter = "Database Files (.db)|*.db"; // Filter files by extension 

            //// Show save file dialog box
            //Nullable<bool> result = dlg.ShowDialog();

            //// Process save file dialog box results 
            //if (result == true)
            //{
            //    // Save document 
            //    string filename = dlg.FileName;
            //    LoadOrCreateNewDBFile(filename, Constants.TABLENAME);
            //}
        }

        private void ConvertCodeTemplateFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string codeTemplateFileName = "";  // The code template file name

            TemplateDataGrid.CommitEdit(); //to save any edits that the enter key was not pressed

            // Get the name of the Code Template file to open
            Microsoft.Win32.OpenFileDialog dlg1 = new Microsoft.Win32.OpenFileDialog();
            dlg1.FileName = Constants.CT_ROOTFILENAME; // Default file name
            dlg1.DefaultExt = Constants.CT_FILENAMEEXTENSION; // Default file extension
            dlg1.Filter = "Code Template Files (" + Constants.CT_FILENAMEEXTENSION + ")|*" + Constants.CT_FILENAMEEXTENSION; // Filter files by extension 
            dlg1.Title = "Select Code Template File to convert...";

            Nullable<bool> result = dlg1.ShowDialog(); // Show the open file dialog box
            if (result == true) codeTemplateFileName = dlg1.FileName;  // Process open file dialog box results 
            else return;

            // Get the name of the new database file to create (over-writes it if it exists)
            Microsoft.Win32.SaveFileDialog dlg2 = new Microsoft.Win32.SaveFileDialog();
            dlg2.Title = "Select Location to Save the Converted Template File";
            dlg2.FileName = Constants.DB_ROOTFILENAME; // Default file name
            dlg2.DefaultExt = Constants.DB_FILENAMEEXTENSION; // Default file extension
            dlg2.Filter = "Database Files (" + Constants.DB_FILENAMEEXTENSION + ")|*" + Constants.DB_FILENAMEEXTENSION; // Filter files by extension 


            result = dlg2.ShowDialog(); // Show open file dialog box
            if (result == true)         // Process open file dialog box results 
            {
                SaveDbBackup();
                if (File.Exists (dlg2.FileName))     // Overwrite the file if it exists
                {
                    File.Delete (dlg2.FileName);
                }
                // Open document 
                dbFileName = dlg2.FileName;
            }
            else return;

            // Start with the default layout of the data template
            InitializeDataGrid(dbFileName, Constants.TABLENAME);

            // Now convert the code template file into a Data Template, overwriting values and adding rows as required
            CodeTemplateImporter.Convert(codeTemplateFileName, this.templateTable);
            UpdateDBFull();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        private void ExitFileMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TemplateDataGrid.CommitEdit(); //to save any edits that the enter key was not pressed
            Application.Current.Shutdown();
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            string message = Constants.ABOUT_VERSION + Environment.NewLine;
            message += Constants.ABOUT_AUTHORS + Environment.NewLine;
            message += Constants.ABOUT_DATE ;
            System.Windows.MessageBox.Show(message, Constants.ABOUT_CAPTION);
        }

        #endregion Menu Listeners

        #region Helper Methods
        /// <summary>
        /// Helper method that creates a database backup. Used when performing file menu options.
        /// </summary>
        public void SaveDbBackup()
        {
            if (!String.IsNullOrEmpty(dbFileName))
            {
                String backupPath = System.IO.Path.GetDirectoryName(dbFileName) + "\\"
                    + "(backup)" 
                    + System.IO.Path.GetFileNameWithoutExtension(dbFileName) 
                    + System.IO.Path.GetExtension(dbFileName);
                File.Copy(dbFileName, backupPath, true);
            }
        }
        #endregion Helper Methods

    }

    #region Converter Classes
    /// <summary>
    /// Converter for ComboBox. Turns the CSV string into an array
    /// Also adds the edit item element. 
    /// Done here, we don't have to ever check for it when performing delete/edit/add (surprises/scares me a bit)
    /// </summary>
    public class ListBoxDBOutputConverter : IValueConverter
    {
        String newItemString = "[NEW ITEM]";
        CsvHelperMethods h = new CsvHelperMethods();
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            string valString = value as string;
            if (!string.IsNullOrEmpty(valString))
            {
                valString += "| " + newItemString;
                return h.csvToArray(valString);
            }
            return new String[1] { newItemString }; //if string is empty, return just the newItemString
        }

        //This does nothing, but it has to be here.
        //Kinda surprised I don't need it for the binding... but I don't.
        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    /// <summary>
    /// Converter for CellTextBlock. Removes spaces from beginning and end of string.
    /// </summary>
    public class CellTextBlockConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            string valString = value as string;
            if (!string.IsNullOrEmpty(valString))
            {
                return valString.Trim();
            }
            return "";
        }
    }
    #endregion Converters

    // This static class generates controls in the provided wrap panel based upon the information in the data grid templateTable
    // It is meant to roughly approximate what the controls will look like in the user interface
    public class ControlGeneration
    {
        #region ControlGeneration
        
        static public void GenerateControls(Window win, WrapPanel wp, DataTable templateTable)
        {
            const string EXAMPLE_DATE = "28-Dec-2014";
            const string EXAMPLE_TIME = "04:00 PM";

            wp.Children.Clear();
            for (int i = 0; i < templateTable.Rows.Count; i++)
            {
                DataRow row = templateTable.Rows[i];
                string type = row[Constants.TYPE].ToString();
                string defaultValue = row[Constants.DEFAULT].ToString();
                string label = row[Constants.LABEL].ToString();
                string tooltip = row[Constants.TOOLTIP].ToString();
                string width = row[Constants.TXTBOXWIDTH].ToString();
                string visiblity = row[Constants.VISIBLE].ToString();
                string list = row[Constants.LIST].ToString();

                int iwidth = (width == "") ? 0 : Convert.ToInt32(width);

                bool bvisiblity = (visiblity == "true" || visiblity == "True") ? true : false;
                if (true == bvisiblity)
                {

                    StackPanel sp = null;

                    if (type == Constants.DATE && defaultValue == "")
                    {
                        defaultValue = EXAMPLE_DATE;
                    }
                    else if (type == Constants.TIME && defaultValue == "")
                    {
                        defaultValue = EXAMPLE_TIME;
                    }

                    if (type == Constants.FILE || type == Constants.FOLDER || type == Constants.DATE || type == Constants.TIME || type == Constants.NOTE)
                    {
                        Label labelctl = CreateLabel(win, label, tooltip);
                        TextBox txtbox = CreateTextBox(win, defaultValue, tooltip, iwidth);
                        sp = CreateStackPanel(win, labelctl, txtbox);
                    }
                    else if (type == Constants.COUNTER)
                    {
                        RadioButton rb = CreateRadioButton(win, label, tooltip);
                        TextBox txtbox = CreateTextBox(win, defaultValue, tooltip, iwidth);
                        sp = CreateStackPanel(win, rb, txtbox);
                    }
                    else if (type == Constants.CHOICE || type == Constants.IMAGEQUALITY)
                    {
                        Label labelctl = CreateLabel(win, label, tooltip);
                        ComboBox combobox = CreateComboBox(win, list, tooltip, iwidth);
                        sp = CreateStackPanel(win, labelctl, combobox);
                    }

                    if (sp != null)
                        wp.Children.Add(sp);
                }
            }
        }

        // Returns a stack panel containing two controls
        // The stack panel ensures that controls are layed out as a single unit with certain spatial characteristcs 
        // i.e.,  a given height, right margin, where contents will not be broken durring (say) panel wrapping
        static public StackPanel CreateStackPanel(Window win, Control control1, Control control2)
        {
            StackPanel sp = new StackPanel();

            // Delete this or comment it out
            //if (this.useColor) sp.Background = Brushes.Red;   // Color red to check spacings

            // Add controls to the stack panel
            sp.Children.Add(control1);
            sp.Children.Add(control2);

            // Its look is dictated by this style
            Style style = win.FindResource("StackPanelCodeBar") as Style;
            sp.Style = style;
            return sp;
        }

        static public Label CreateLabel(Window win, string labeltxt, string tooltiptxt)
        {
            Label label = new Label();

            // Delete this or comment it out
            //if (this.useColor) label.Background = Brushes.Orange;  // Color  to check spacings

            // Configure it
            label.Content = labeltxt;
            label.ToolTip = tooltiptxt;

            // Its look is dictated by this style
            Style style = win.FindResource("LabelCodeBar") as Style;
            label.Style = style;

            return label;
        }

        static public TextBox CreateTextBox(Window win, string textboxtxt, string tooltiptxt, int width)
        {
            TextBox txtbox = new TextBox();

            // Delete this or comment it out
            //if (this.useColor) txtbox.Background = Brushes.Gold;  // Color  to check spacings

            // Configure the Textbox
            txtbox.Width = width;
            txtbox.Text = textboxtxt;
            txtbox.ToolTip = tooltiptxt;

            // Its look is dictated by this style
            Style style = win.FindResource("TextBoxCodeBar") as Style;
            txtbox.Style = style;

            return txtbox;
        }

        static public RadioButton CreateRadioButton(Window win, string labeltxt, string tooltiptxt)
        {
            RadioButton radiobtn = new RadioButton();

            // Delete this or comment it out
            //if (this.useColor) radiobtn.Background = Brushes.White;  // Color  to check spacings

            // Configure the Radio Button
            radiobtn.GroupName = "A";
            radiobtn.Content = labeltxt;
            radiobtn.ToolTip = tooltiptxt;

            // Its look is dictated by this style
            Style style = win.FindResource("RadioButtonCodeBar") as Style;
            radiobtn.Style = style;

            return radiobtn;
        }

        static public ComboBox CreateComboBox(Window win, string list, string tooltiptxt, int width)
        {
            ComboBox combobox = new ComboBox();

            // Configure the ComboBox
            combobox.ToolTip = tooltiptxt;
            combobox.Width = width;
            List<string> result = list.Split(new char[] { '|' }).ToList();
            foreach (string str in result)
            {
                combobox.Items.Add(str.Trim());
            }
            combobox.SelectedIndex = 0;

            // Its look is dictated by this style
            Style style = win.FindResource("ComboBoxCodeBar") as Style;
            combobox.Style = style;
            return combobox;
        }
        #endregion ControlGeneration
    }
}