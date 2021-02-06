using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Final_CSV
{

    enum DataTypes
    {
        System_Boolean = 0,
        System_Int32 = 1,
        System_Int64 = 2,
        System_Double = 3,
        System_DateTime = 4,
        System_String = 5
    }

    public partial class Form1 : Form
    {
        string filePath;
        TextFieldParser tfp;
        Type dataPointType;
        string className = "MyDataType";
        Dictionary<String, Type> variables;
        string oldVariableName;
        string selectedFieldSeparator;
        List<object> itemsList;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "CSV File (*.csv)|*.csv";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    filePathTextBox.Text = filePath;

                    //Try reading the csv file
                    try
                    {
                        tfp = new TextFieldParser(filePath);
                    }
                    //If an exception is caught, show an error message
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        string alertMsg;
                        string alertCaption;
                        alertCaption = "Impossible to open file";
                        alertMsg = "It was impossible to open the file \"" + Path.GetFileName(filePath) + "\", please try again.";
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result = MessageBox.Show(alertMsg, alertCaption, buttons);
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            // Closes the parent form.
                            this.Close();
                        }
                    }


                    //once the file is opened, read the first line, so to get the names of the variables
                    tfp.Delimiters = new string[] { selectedFieldSeparator };
                    string[] variableNames = tfp.ReadFields();
                    SuggestVariables(variableNames);
                    CreateType();

                    //import the csv into the list
                    ImportToDataPointList();

                    //fill the table
                    FillTable();
                }
                
            }
        }

        //what to do when clicking a node
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            string name;
            string type;

            //check if i'm either clicking the name or the type
            //this isn't the most elegant way, but it sure works wonders...
            try
            {
                name = e.Node.Text;
                type = e.Node.Nodes[0].Text;
            }
            catch (Exception ex)
            {
                type = e.Node.Text;
                name = e.Node.Parent.Text;
            }

            variableNameTextBox.Text = name;

            //temporary string where to hold the old name of the variable
            oldVariableName = name;

            //Remove "System." from the type name
            variableListBox.SelectedItem = type.Remove(0, 7);

        }
        private void exportCSVButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog fd = new SaveFileDialog();
            fd.Filter = "CSV files (*.csv)|*.csv";
            fd.RestoreDirectory = true;

            if (fd.ShowDialog() == DialogResult.OK)
            {
                var sw = new StreamWriter(fd.FileName);

                //write the names of the variables as the first row
                StringBuilder w = new StringBuilder();
                foreach (String s in variables.Keys)
                {
                    w.Append(s + ",");
                }
                w.Remove(w.Length - 1, 1);
                sw.WriteLine(w.ToString());

                //then write each datapoint
                foreach (object o in itemsList)
                {
                    w = new StringBuilder();
                    foreach (FieldInfo fi in dataPointType.GetFields())
                    {
                        w.Append(fi.GetValue(o).ToString() + ",");
                    }
                    //remove the last field separator at the end of the line
                    w.Remove(w.Length - 1, 1);
                    sw.WriteLine(w.ToString());
                }

                sw.Close();
            }
        }
        private void changeVariableButton_Click(object sender, EventArgs e)
        {
            if (variableListBox.SelectedItem == null || variables == null)
            {
                //MessageBox.Show("Error. Did you import a CSV file?", "Error");
            }
            else
            {

                try
                {
                    //check if it is possible to do the conversion
                    int newVariableType = DataTypesOrder(variableListBox.SelectedItem.ToString());
                    int oldVariableType = DataTypesOrder(variables[oldVariableName].Name);

                    //check if the conversion is allowed (can't convert to a 'higher' type, due to conversion errors)
                    if (newVariableType > oldVariableType && newVariableType != 5 && !(newVariableType == 2 && oldVariableType == 1))
                    {
                        string msg = "Impossible to convert from " + variables[oldVariableName].Name + " to " + variableListBox.SelectedItem + ".";
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {

                        String newName = variableNameTextBox.Text;
                        Type newType = Type.GetType("System." + variableListBox.SelectedItem);

                        Type temp = variables[oldVariableName];
                        variables.Remove(oldVariableName);
                        variables.Add(newName, newType);

                        populateTree();

                        dataGridView1.Columns[oldVariableName].HeaderCell.Value = newName;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        private void exportClassButton_Click(object sender, EventArgs e)
        {
            if (variables != null)
            {
                createClassFile(dataPointType);
            }
            else
            {
                MessageBox.Show("Please import a CSV file.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //create the type from a dictionary
        private void CreateType()
        {

            //className = classNameTextBox.Text.Replace(" ", string.Empty);
            if (string.IsNullOrEmpty(className)) className = "DataType";

            //Create the assembly for the new type
            AssemblyName aName = new AssemblyName("DynamicAssemblyExample");
            AssemblyBuilder ab =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    aName,
                    AssemblyBuilderAccess.RunAndSave);

            // For a single-module assembly, the module name is usually
            // the assembly name plus an extension.
            ModuleBuilder mb = ab.DefineDynamicModule(aName.Name, aName.Name + ".dll");

            //Define a new type, and make all the fields public
            TypeBuilder tb = mb.DefineType(
                className,
                TypeAttributes.Public);

            //add each new field, as detected and saved in the variables map
            foreach (KeyValuePair<String, Type> kvp in variables)
            {
                tb.DefineField(
                    kvp.Key,
                    kvp.Value,
                    FieldAttributes.Public);
            }

            //save the new type in dataPointType
            dataPointType = tb.CreateType();
        }

        //creates a dynamic type based on the variable names defined in the first line of the CSV file
        private void SuggestVariables(string[] variableNames)
        {
            variables = new Dictionary<string, Type>();

            int sampleSize = 20;                                    //size of the sample used to deduct the type
            string[][] sample = new string[sampleSize][];           //matrix of samples

            TextFieldParser parser = new TextFieldParser(filePath);
            parser.Delimiters = new string[] { selectedFieldSeparator };

            //skip first line, we don't want that as it's most probably the name of the fields
            parser.ReadFields();

            for (int i = 0; i < sampleSize || parser.EndOfData; i++)
            {
                sample[i] = parser.ReadFields();
            }

            //I don't need the tfp anymore
            parser.Close();

            //transpose the sample matrix, so we have each entry as a column instead of a row
            string[][] transposedSample = transpose(sample);

            for (int i = 0; i < variableNames.Length; i++)
            {
                variables.Add(variableNames[i], GetSuggestedType(transposedSample[i]));
            }

            //populate the tree view
            populateTree();

        }

        //returns a datatype enum from a string by trying to convert it sequentially
        private DataTypes ParseString(string str)
        {
            bool boolValue;
            Int32 intValue;
            Int64 bigintValue;
            double doubleValue;
            DateTime dateValue;

            // Place checks higher in if-else statement to give higher priority to type.

            if (bool.TryParse(str, out boolValue))
                return DataTypes.System_Boolean;
            else if (Int32.TryParse(str, out intValue))
                return DataTypes.System_Int32;
            else if (Int64.TryParse(str, out bigintValue))
                return DataTypes.System_Int64;
            else if (double.TryParse(str, out doubleValue))
                return DataTypes.System_Double;
            else if (DateTime.TryParse(str, out dateValue))
                return DataTypes.System_DateTime;
            else return DataTypes.System_String;

        }

        private int DataTypesOrder(string str)
        {
            DataTypes test;
            Enum.TryParse("System_" + str, out test);

            if (test.Equals(DataTypes.System_Boolean)) return 0;
            if (test.Equals(DataTypes.System_Int32)) return 1;
            if (test.Equals(DataTypes.System_Int64)) return 2;
            if (test.Equals(DataTypes.System_Double)) return 3;
            if (test.Equals(DataTypes.System_DateTime)) return 4;
            if (test.Equals(DataTypes.System_String)) return 5;
            return -1;
        }

        //deducts the possible datatype from an array of sample strings
        public Type GetSuggestedType(string[] sample)
        {

            Type T;
            int maxLevel = 0;
            for (int i = 0; i < sample.Length; i++)
            {
                if (String.IsNullOrEmpty(sample[i])) continue;
                maxLevel = Math.Max((int)ParseString(sample[i]), maxLevel);
            }

            string enumCheck = ((DataTypes)maxLevel).ToString();
            T = Type.GetType(enumCheck.Replace('_', '.'));

            return T;
        }

        //transpose a string matrix
        public string[][] transpose(string[][] array)
        {

            if (array == null) return null;

            int width = array.Length;
            int height = array[0].Length;

            //initialize the transposed array
            string[][] array_new = new string[height][];
            for (int i = 0; i < height; i++)
            {
                array_new[i] = new string[width];
            }

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    try
                    {
                        array_new[y][x] = array[x][y];
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(array_new.ToString() + "x: " + x + "; y: " + y);
                    }
                }
            }
            return array_new;
        }

        //imports the csv data into a list of objects
        void ImportToDataPointList()
        {
            //initialize the list of datapoints
            itemsList = new List<object>();

            //first import the csv into the list of objects
            tfp = new TextFieldParser(filePath);
            tfp.Delimiters = new string[] { selectedFieldSeparator };

            //skip the first row of names in the csv
            tfp.ReadFields();

            //import the csv into the class list
            string[] fields;
            while ((fields = tfp.ReadFields()) != null)
            {

                //create a new datapoint
                object dataPoint = Activator.CreateInstance(dataPointType);

                int i = 0;
                //for each field in the new type
                foreach (FieldInfo fi in dataPointType.GetFields())
                {
                    try
                    {
                        if (!string.IsNullOrWhiteSpace(fields[i]))
                        {
                            //convert each entry to the correct type. InvariantCulture is needed to accurately convert the decimal separator
                            var value = Convert.ChangeType(fields[i], fi.FieldType, System.Globalization.CultureInfo.InvariantCulture);
                            fi.SetValue(dataPoint, value);
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Failed while trying to convert " + fields[i] + "in the type " + fi.FieldType);
                    }
                    i++;
                }

                itemsList.Add(dataPoint);
            }

            //always close the reader...
            tfp.Close();
        }

        //populate treeView1 with the suggested class fields
        private void populateTree()
        {
            treeView1.BeginUpdate();

            treeView1.Nodes.Clear();

            int i = 0;
            foreach (KeyValuePair<String, Type> kvp in variables)
            {
                treeView1.Nodes.Add(kvp.Key);
                treeView1.Nodes[i].Nodes.Add(kvp.Value.ToString());
                i++;
            }

            treeView1.EndUpdate();
            treeView1.ExpandAll();
        }

        //creates and saves a .cs file that represents the type t
        //as it will be used to represent the dynamic class only, it will only contain fields, no constructors or methods
        private void createClassFile(Type t)
        {
            //className = classNameTextBox.Text;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "C# files (*.cs)|*.cs";
            sfd.RestoreDirectory = true;
            sfd.FileName = t.Name;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var sw = new StreamWriter(sfd.FileName);

                //Class name
                sw.WriteLine("public class " + className + "\n{");

                //Fields
                foreach (FieldInfo fi in t.GetFields())
                {
                    //edit the name so it is a legal variable name
                    string name = fi.Name;
                    name = Char.ToLowerInvariant(name[0]) + name.Substring(1);
                    name = name
                        .Replace('(', '_')
                        .Replace(')', '_')
                        .Replace(' ', '_')
                        .Replace('<', '_')
                        .Replace('>', '_');

                    string type = fi.FieldType.ToString().Remove(0, 7);

                    sw.WriteLine("\tpublic " + type + " " + name + ";");
                }

                //End Class
                sw.WriteLine("}");

                sw.Close();
            }

        }
        void FillTable()
        {
            DataTable table = new DataTable();

            //add all the columns
            FieldInfo[] fields = dataPointType.GetFields();
            string[] varNames = variables.Keys.ToArray();

            int k = 0;
            foreach (FieldInfo fi in fields)
            {
                table.Columns.Add(varNames[k], fi.FieldType);
                k++;
            }

            //add all the rows
            foreach (object o in itemsList)
            {
                DataRow row = table.NewRow();
                int i = 0;
                foreach (FieldInfo fi in fields)
                {
                    row[fi.Name] = fi.GetValue(o);
                    i++;
                }
                table.Rows.Add(row);

            }

            //finally export in the grid view
            dataGridView1.DataSource = table;
        }

        private void newFormButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (itemsList != null && variables.Keys.Count >= 2)
                {
                    Form newForm = new GraphVisualizer(itemsList, variables);
                    newForm.Show();
                }
            } catch (Exception ex)
            {
                Debug.WriteLine(ex.StackTrace);
            }
        }

        private void fieldSeparatorComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (fieldSeparatorComboBox.SelectedIndex)
            {
                case 0:
                    selectedFieldSeparator = ",";
                    break;
                case 1:
                    selectedFieldSeparator = ";";
                    break;
                case 2:
                    selectedFieldSeparator = "-";
                    break;
                case 3:
                    selectedFieldSeparator = " ";
                    break;
                case 4:
                    selectedFieldSeparator = "/";
                    break;
                default:
                    selectedFieldSeparator = ";";
                    break;
            }
        }
    }
}
