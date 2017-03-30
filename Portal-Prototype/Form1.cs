using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

//Add the White reference namespaces
//================================================ White's namespace has changed to 'TestStack.White ===========================================================//
using TestStack.White.Factory;
using TestStack.White.UIItems.Finders;
using TestStack.White.InputDevices;

//NB ADD OTHER USED AUTOMATION TOOLS HERE

//Namespaces for getting the executable paths of the running processes
using System.IO;

//Namespace for the automation class I developed
using TestAutoApp;

//NB BUILD THIS APPLICATION FOR 64 BIT PLATFORMS TO AVOID CONFLICTS

//NB FOR EDITING PLEASE CHANGE THE SAVE PATH FOR THE TEXT FILE ACCORDINGLY
//USE SAVE FILE DIALOGUE BOX IN FUTURE TO LET THE USER CHOOSE THE PATH

namespace Portal_Prototype
{
    public partial class Form1 : Form
    {
        //Globals
        //string path = "";
        List<string> exePaths = new List<string>();  // List vector to store exe paths
        List<string> appNames = new List<string>();

        //TestStack.White Objects
        TestStack.White.Application _application;
        TestStack.White.UIItems.WindowItems.Window _mainWindow;

        string sSelectedFolder;
   
        public string GetProcessPath(string name)
        {
            Process[] processes = Process.GetProcessesByName(name);

            if (processes.Length > 0)
            {
                return processes[0].MainModule.FileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void RemoveDuplicates(ListBox lb)
        {
            //method to remove duplicates from a list

            for (int Row = 0; Row <= lb.Items.Count - 2; Row++)
            {
                for (int RowAgain = lb.Items.Count - 1; RowAgain >= Row + 1; RowAgain += -1)
                {
                    if (lb.Items[Row].ToString() == lb.Items[RowAgain].ToString())
                    {
                        lb.Items.RemoveAt(RowAgain);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string line = "";
            int counter = 0;

            //Upon Form load show previously loaded applications
            lblListOfApps.Text = "Previously Loaded Apps";

            //Read the text file one line at a time
            // Read the file and display it line by line.  

            try
            {
                //System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Name\Documents\Visual Studio 2017\Projects\Portal-Prototype\Portal-Prototype\exePaths.txt");
                //NB THIS IS ONLY FOR TESTING. IN FUTURE LET THE USER CHOOSE THE SAVE FILE PATH

                //System.IO.StreamReader file = new System.IO.StreamReader(@sSelectedFolder + @"\exePaths.txt");

                //NB THE Text box's property is bound to the app settings and saved on the closed event
                //Therefore we can use its property to reload the previous path

                System.IO.StreamReader file = new System.IO.StreamReader(@txtFilePath.Text + @"\exePaths.txt");

                while ((line = file.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);

                    listBox1.Items.Add(line);
                    counter++;
                }

                file.Close(); //close the file
            }
            catch(Exception)
            {
                lblListOfApps.Text = "No Previously Loaded Apps";
            }
             
        }





        private void button1_Click(object sender, EventArgs e)
        {
         
            try
            {
                if(txtFilePath.Text != "")
                {
                    //Save the exe paths to a text file
                    //System.IO.File.WriteAllLines(@"C:\Users\Name\Documents\Visual Studio 2017\Projects\Portal-Prototype\Portal-Prototype\exePaths.txt", exePaths);

                    System.IO.File.WriteAllLines(@txtFilePath.Text + @"\exePaths.txt", exePaths);
                    MessageBox.Show("Apps Saved!!");

                    //Since the text file gets rid of duplicates use it to create our AutoApp objects
                    //System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Name\Documents\Visual Studio 2017\Projects\Portal-Prototype\Portal-Prototype\exePaths.txt");

                    System.IO.StreamReader file = new System.IO.StreamReader(@txtFilePath.Text + @"\exePaths.txt");

                    //Close the Stream Reader asset
                    file.Close();

                    //***********************************************************************************************************//
                    //********************Try to elegantly close the apps here!!************************************************//
                    /************************************************************************************************************/

                }
                else
                {
                    MessageBox.Show("The file path does not exist or the file may have been moved to another location");
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + "Please choose a save path first! You may not save twice in one session!");
            }



        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();  //Clear the previous items on the form list
            exePaths.Clear();        //Clear the previous items on the paths list object

            lblListOfApps.Text = "Apps Currently Opened";
            

            Process[] processlist = Process.GetProcesses(); //Array to hold the Process ID'

            //Check out which windows are currently open

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle)) //only check for the ones with open windows
                {
                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);

                    try
                    {
                        //Console.WriteLine("Process Path: " + process.MainModule.FileName);

                        //Store the executable paths in a list

                        exePaths.Add(@process.MainModule.FileName);

                        //Clear the list of duplicates ====figure out how to eliminate duplicates
                        //var paths = exePaths.Distinct();

                       
                   
                        //Display the list of open apps in the list box
                        foreach (string path in exePaths)
                        {
                            listBox1.Items.Add(path);
                            //listBox1.Items.Add(process.MainWindowTitle + "   Path: " + path);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("n/a");
                        listBox1.Items.Add("N/A");
                    }

                }
            }

            RemoveDuplicates(listBox1);
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            //fbd.Description = "Custom Description"; //not mandatory

            if (fbd.ShowDialog() == DialogResult.OK)
                sSelectedFolder = fbd.SelectedPath;
            else
                sSelectedFolder = string.Empty;

            txtFilePath.Text = sSelectedFolder;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            //Save Form Settings
            Properties.Settings.Default.Save();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //******************RESTORE APPS HERE*************************//
            //====First attempt to open each app, then the associated file / browser page that was opened within that app==//
            //=============================================================================================================//

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //******************CLOSE ALL APPS HERE*************************//
            //===Use the AutoApp Object to close all open apps================//
            //=============================================================================================================//
            //NB CLOSE ALL APPS BESIDES Portal-Protype.exe itself

            //Create AutoApp Object
           // AutoApp myAutoApp = new AutoApp();

           /****Try not to use the AutoApp class just yet. Use TestStack Manually as AutoApp is still under development***********/

            listBox2.Items.Clear();  //Clear the previous items on the form list
            appNames.Clear();        //Clear the previous items on the names list object

            lblAppsClose.Text = "Closing these Applications";

            Process[] processlist = Process.GetProcesses(); //Array to hold the Process ID'

            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle)) //only check for the ones with open windows
                {
                    //Console.WriteLine("Process: {0} ID: {1} Window title: {2}", process.ProcessName, process.Id, process.MainWindowTitle);

                    try
                    {
                        //Console.WriteLine("Process Path: " + process.MainModule.FileName);

                        //Store the executable paths in a list

                        appNames.Add(@process.MainWindowTitle);

                        //Display the list of open apps in the list box
                        foreach (string name in appNames)
                        {
                            listBox2.Items.Add(name);
                            //listBox1.Items.Add(process.MainWindowTitle + "   Path: " + path);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("n/a");
                        listBox2.Items.Add("N/A");
                    }

                  

                }
            }

            RemoveDuplicates(listBox2);

            //Iterate through listbox2 and close the apps using TestStack

            //Assuming the paths in listbox1 refer to the names in listbox2

            foreach (string path in listBox1.Items)
            {
                foreach(string name in listBox2.Items)
                {
                    try
                    {
                        //Attach the current apps / windows to TestStack.White
                        //var psi = new ProcessStartInfo(path);
                        
                        //try just using Attach instead
                        _application = TestStack.White.Application.AttachOrLaunch(psi);
                        _mainWindow = _application.GetWindow(SearchCriteria.ByText(name), InitializeOption.NoCache);

                        if(_mainWindow.Title != "Form1" || _mainWindow.Title != "Portal-Prototype (Running) - Microsoft Visual Studio (Administrator)")
                        {
                            //Dispose the main window
                            _mainWindow.Dispose();

                            //Dispose the application
                            _application.Dispose();
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message + " \n Unable to close app: " + name);
                    }                 
                }
            } 
        }
    }
}
