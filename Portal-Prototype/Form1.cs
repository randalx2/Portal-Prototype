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
//**White's namespace has changed to 'TestStack.White'!**
using TestStack.White.Factory;
using TestStack.White.UIItems.Finders;
using TestStack.White.InputDevices;

//Namespaces for getting the executable paths of the running processes
using System.IO;

//Namespace for the automation class I developed
using TestAutoApp;

//NB BUILD THIS APPLICATION FOR 64 BIT PLATFORMS TO AVOID CONFLICTS

namespace Portal_Prototype
{
    public partial class Form1 : Form
    {
        //Globals
        //string path = "";
        List<string> exePaths = new List<string>();  // List vector to store exe path
        List<AutoApp> myApps = new List<AutoApp>();  //List of automation app objects
        
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

        private void Form1_Load(object sender, EventArgs e)
        {
            string line = "";
            int counter = 0;
            //Upon Form load show previously loaded applications
            lblListOfApps.Text = "Previously Loaded Apps";

            //Read the text file one line at a time
            // Read the file and display it line by line.  
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Praneel\Documents\Visual Studio 2015\Projects\Portal-Prototype\Portal-Prototype\exePaths.txt");

            while ((line = file.ReadLine()) != null)
            {
                //System.Console.WriteLine(line);

                listBox1.Items.Add(line);
                counter++;
            }

            file.Close(); //close the file
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string line = "";
            int counter = 0;

            //Save the exe paths to a text file
            System.IO.File.WriteAllLines(@"C:\Users\Praneel\Documents\Visual Studio 2015\Projects\Portal-Prototype\Portal-Prototype\exePaths.txt", exePaths);
            MessageBox.Show("Apps Saved!!");

            //Since the text file gets rid of duplicates use it to create our AutoApp objects
            System.IO.StreamReader file = new System.IO.StreamReader(@"C:\Users\Praneel\Documents\Visual Studio 2015\Projects\Portal-Prototype\Portal-Prototype\exePaths.txt");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            lblListOfApps.Text = "Apps Currently Opened";
            listBox1.Items.Clear();  //Clear the previous items

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
                    catch (Exception)
                    {
                        //Console.WriteLine("n/a");
                        listBox1.Items.Add("N/A");
                    }

                }
            }
        }
    }
}
