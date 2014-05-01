using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

/**
 * Modified version of the code here: http://www.dreamincode.net/forums/topic/157830-using-sqlite-with-c%23/
 * Date 16/12/2013
 */

namespace TimelapseTemplateEditor
{
    
    class SQLiteDatabase
    {
        String dbConnection;

        /// <summary>
        ///     Default Constructor for SQLiteDatabase Class.
        ///     Creates the database file by default, will replace it each time...
        /// </summary>
        public SQLiteDatabase()
        {
            dbConnection = "Data Source=TestDatabase.sqlite";
        }

        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        public SQLiteDatabase(String inputFile)
        {
            dbConnection = String.Format("Data Source={0}", inputFile);
        }

        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        public SQLiteDatabase(Dictionary<String, String> connectionOpts)
        {
            String str = "";
            foreach (KeyValuePair<String, String> row in connectionOpts)
            {
                str += String.Format("{0}={1}; ", row.Key, row.Value);
            }
            str = str.Trim().Substring(0, str.Length - 1);
            dbConnection = str;
        }

        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public DataTable GetDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                SQLiteConnection cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn);
                mycommand.CommandText = sql;
                SQLiteDataReader reader = mycommand.ExecuteReader();
                dt.Load(reader);
                reader.Close();
                cnn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return dt;
        }

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int ExecuteNonQuery(string sql)
        {
            SQLiteConnection cnn = new SQLiteConnection(dbConnection);
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            int rowsUpdated = mycommand.ExecuteNonQuery();
            cnn.Close();
            return rowsUpdated;
        }

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public string ExecuteScalar(string sql)
        {
            SQLiteConnection cnn = new SQLiteConnection(dbConnection);
            cnn.Open();
            SQLiteCommand mycommand = new SQLiteCommand(cnn);
            mycommand.CommandText = sql;
            object value = mycommand.ExecuteScalar();
            cnn.Close();
            if (value != null)
            {
                return value.ToString();
            }
            return "";
        }

        /// <summary>
        ///     Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(String tableName, Dictionary<String, String> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }
        
        //expands upon update, allowing to update not just strings
        public bool UpdateVerbose(String tableName, Dictionary<String, Object> data, String where)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, Object> val in data)
                {
                    if (IsNumber(val.Value))
                    {
                        vals += String.Format(" {0} = {1},", val.Key.ToString(), val.Value.ToString());
                    }
                    else if (val.Value == null)
                    {
                        vals += String.Format(" {0} = null,", val.Key.ToString(), val.Value.ToString());
                    }
                    else
                    {
                        vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                    }
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Delete(String tableName, String where)
        {
            Boolean returnCode = true;
            try
            {
                this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
            }
            catch (Exception fail)
            {
                System.Windows.MessageBox.Show(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily insert into the DB
        /// </summary>
        /// <param name="tableName">The table into which we insert the data.</param>
        /// <param name="data">A dictionary containing the column names and data for the insert.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Insert(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception fail)
            {
                System.Windows.MessageBox.Show(fail.Message);
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="table">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearTable(String table)
        {
            try
            {

                this.ExecuteNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }

        //
        ///Methods added by Matthew Dunlap.
        //

        /// <summary>
        /// Created by me, currently pretty crap, just takes in parts of the sql tring
        /// </summary>
        /// <param name="name">The name of the table</param>
        /// <param name="rows">the string for the rows</param>
        /// <returns></returns>
        public bool CreateTable(String name, String rows)
        {
            try
            {
                string sql = "create table " + name + " (" + rows + ")";
                this.ExecuteNonQuery(sql);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Allows the programmer to easily insert multiple rows in the db with one call.
        ///     The normal insert method is extremely slow for many rows.
        ///     The limit on rows per insert is 500.
        ///     Information and code gathered from here: http://stackoverflow.com/questions/1609637/
        ///     Ended up writing the code from scratch, not off already created insert, because I don't write in his style.
        ///     
        ///     This should be tested better for different dataList lengths. 
        ///         May have a problem if the header of the inserted list is all thats sent to the sql.
        ///     May also get a minor speed boost from removing the column names (as discussed in link above).
        /// </summary>
        /// <param name="tableName">Name of the table to insert into</prastam>
        /// <param name="dataArray">Array of dictionaries containing all tha variables to be added.</param>
        /// <returns></returns>
        public bool InsertMultiple(String tableName, List<Dictionary<String, String>> dataList)
        {
            int multiplier = 0;
            int maxRows = 500;

            while (dataList.Count > maxRows * multiplier)
            {
                String insertString = "insert into '" + tableName + "'";
                for (int i = 0; (i < maxRows) && ((i + (multiplier * maxRows)) < dataList.Count); i++) //I need to check the number of remaining records here...
                {
                    insertString += " select '";
                    foreach (KeyValuePair<String, String> val in dataList[i + (multiplier * maxRows)])
                    {
                        insertString += val.Value + "' AS '" + val.Key.ToString() + "', '";
                    }
                    //System.Console.WriteLine("Total: " + (i + (multiplier * maxRows)) + " | i: " + i + " | Mult: " + multiplier);
                    insertString = insertString.Remove(insertString.Length - ", '".Length); //remove last comma
                    insertString += " union";
                }
                multiplier++;
                insertString = insertString.Remove(insertString.Length - " union".Length); //remove last union

                this.ExecuteNonQuery(insertString);
            }
            return true;
        }

        /// <summary>
        /// Rewrote insert multiple to allow dictionarys with values that aren't strings.
        /// Should only take strings and ints
        /// </summary>
        public bool NewInsertMultiple(String tableName, List<Dictionary<String, Object>> dataList)
        {
            int multiplier = 0;
            int maxRows = 500;

            while (dataList.Count > maxRows * multiplier)
            {
                String insertString = "insert into '" + tableName + "'";
                for (int i = 0; (i < maxRows) && ((i + (multiplier * maxRows)) < dataList.Count); i++) //I need to check the number of remaining records here...
                {
                    
                    insertString += " select '"; //this needs to change with the trailing '
                    foreach (KeyValuePair<String, Object> val in dataList[i + (multiplier * maxRows)])
                    {
                        if (IsNumber(val.Value))
                        {
                            insertString = insertString.TrimEnd('\''); // remove single quote from end of string for ints
                            insertString += val.Value + " AS '" + val.Key.ToString() + "', '"; //another quote removed
                        }
                        else if (val.Value == null)
                        {
                            insertString = insertString.TrimEnd('\''); // remove single quote from end of string for ints
                            insertString += "null AS '" + val.Key.ToString() + "', '"; //another quote removed
                        }
                        else
                        {
                            insertString += val.Value + "' AS '" + val.Key.ToString() + "', '";
                        }
                    }
                    //System.Console.WriteLine("Total: " + (i + (multiplier * maxRows)) + " | i: " + i + " | Mult: " + multiplier);
                    insertString = insertString.Remove(insertString.Length - ", '".Length); //remove last comma
                    insertString += " union";
                }
                multiplier++;
                insertString = insertString.Remove(insertString.Length - " union".Length); //remove last union

                this.ExecuteNonQuery(insertString);
            }
            return true;
        }

        //testing if object is number, couldn't find a better way to do it.
        //from http://stackoverflow.com/questions/1130698/
        public bool IsNumber(object value)
        {
            return value is sbyte
                    || value is byte
                    || value is short
                    || value is ushort
                    || value is int
                    || value is uint
                    || value is long
                    || value is ulong
                    || value is float
                    || value is double
                    || value is decimal;
        }
    }



}
