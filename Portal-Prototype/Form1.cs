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

//Add in the Selenium namespaces after installing from Nuget
//Selenium Namespaces
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

//Special Support Classes for Selenium from Selenium.support package
using OpenQA.Selenium.Support.UI;

namespace Portal_Prototype
{
    public partial class Form1 : Form
    {
        //Globals
        //string path = "";
        List<string> exePaths = new List<string>();  // List vector to store exe paths
        List<string> appNames = new List<string>();
        List<string> windowNames = new List<string>();
        List<int> appIDs = new List<int>();  //Hold the app IDs

        //TestStack.White Objects
        TestStack.White.Application _application = null;
        TestStack.White.UIItems.WindowItems.Window _mainWindow = null;

        //String to hold selected save folder path
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

        //My Custom Functions for use with Selenium WebDriver
        //Method used to check if element exists
        private bool isElementPresent(By by, IWebDriver driver)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException e)
            {
                //MessageBox.Show(e.Message + "\n Could not find required element");
                return false;
            }
        }


        private bool IsAlertShown(IWebDriver driver)
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException e)
            {
                //MessageBox.Show(e.Message);
                return false;
            }
        }



        private void WaitForPageLoad(IWebDriver driver)
        {
            IWebElement page = null;
            if (page != null)
            {
                var waitForCurrentPageToStale = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
                waitForCurrentPageToStale.Until(ExpectedConditions.StalenessOf(page));
            }

            var waitForDocumentReady = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            waitForDocumentReady.Until((wdriver) => (driver as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"));

            page = driver.FindElement(By.TagName("html"));

        }


        //Method to remove duplicates from a listbox
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

        //Method called upon form loading
        private void Form1_Load(object sender, EventArgs e)
        {
            //=========================FORM LOAD Functions here============================================//

            string line = "";
            int counter = 0;

            //Upon Form load show previously loaded applications
            lblListOfApps.Text = "Previously Loaded Apps";
            lblAppsClose.Text = "Current Application Window Names";

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
            //==============SHOW APPS CURRENTLY OPENED============================================//
            //**************************************************************************************//

            listBox1.Items.Clear();  //Clear the previous items on the form list
            listBox2.Items.Clear();
            listBox3.Items.Clear();

            exePaths.Clear();        //Clear the previous items on the paths list object
            appNames.Clear();
            appIDs.Clear();

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
                        appNames.Add(process.ProcessName);
                        windowNames.Add(process.MainWindowTitle);
                        appIDs.Add(process.Id);

                        //Clear the list of duplicates ====figure out how to eliminate duplicates
                        //var paths = exePaths.Distinct();



                        //Display the list of open apps in the list box
                        foreach (string path in exePaths)
                        {
                            listBox1.Items.Add(path);
                            //listBox1.Items.Add(process.MainWindowTitle + "   Path: " + path);
                        }

                        foreach (int id in appIDs)
                        {
                            listBox3.Items.Add(id);
                        }

                        foreach (string name in appNames)
                        {
                            listBox2.Items.Add(name);
                        }

                        foreach(string window in windowNames)
                        {
                            listBox4.Items.Add(window);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.WriteLine("n/a");
                        listBox1.Items.Add("N/A");
                        listBox3.Items.Add("N/A");
                        listBox2.Items.Add("N/A");
                        listBox4.Items.Add("N/A");
                        MessageBox.Show(ex.Message + "\n No Such App or process ID opened");
                    }

                }
            }

            RemoveDuplicates(listBox1);
            RemoveDuplicates(listBox2);
            RemoveDuplicates(listBox3);
            RemoveDuplicates(listBox4);
        }

        private void btnFilePath_Click(object sender, EventArgs e)
        {
            //=================USE FOLDER DIALOGUES TO SET THE SAVE PATH FOR THE TEXT FILES =====================================//

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
            //******************RESTORE APPS HERE***************************************************************************//
            //====First attempt to open each app, then the associated file / browser page that was opened within that app==//
            //=============================================================================================================//

            //Read the text file line by line to get previous stored paths
            int counter = 0;
            string line = "";

            // Read the file and display it line by line.
            //Thereafter launch each of the previously used apps through TestStack.White
            //NB Alter this later to let selenium open up the browsers
            //NB Don't open up another instance of Portal itself as the user is expected to launch Portal Himself
            //NB Another instance of Visual Studio will be opened up as Portal is run from its environment during this phase
            //==> Opening up another instance of visual studio may not be suitable during the debugging phase
            //Also the launch directory of Portal will change once it is installed on PC

            System.IO.StreamReader file = new System.IO.StreamReader(@txtFilePath.Text + @"\exePaths.txt");
            while ((line = file.ReadLine()) != null)
            {
                if (line == @"C:\Users\Name\Documents\Visual Studio 2017\Projects\Portal-Prototype\Portal-Prototype\bin\Debug\Portal-Prototype.exe"
                    || line == @"C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\devenv.exe"
                    || line == @"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe")
                {
                    //NB REMOVE EXCLUSION OF VISUAL STUDIO BEFORE THE FINAL BUILD!!
                    //Just increment the loop counter here as we don't want to open up another instance of Portal or VS
                    counter++;
                }
                else if(line == @"C:\Program Files (x86)\Mozilla Firefox\firefox.exe")
                {
                    //What if Firefox was opened ==> Use Selenium to launch it
                    //Also hide the command prompt windows from the drivers

                    var driverService = FirefoxDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    IWebDriver driver = new FirefoxDriver(driverService);

                    //=========CODE FOR FIREFOX MANIPULATION UPON STARTUP HERE ========================== //

                    counter++;
                }
                else if(line == @"C:\Program Files (x86)\Google\Chrome\Application\chrome.exe")
                {
                    //What if Chrome was opened ==> Use Selenium to launch it

                    var driverService = ChromeDriverService.CreateDefaultService();
                    driverService.HideCommandPromptWindow = true;
                    IWebDriver driver = new ChromeDriver(driverService);

                    //========= CODE FOR CHROME MANIPULATION UPON STARTUP HERE ===========================//

                    counter++;
                }
                else if(line == @"C:\Program Files\Internet Explorer\iexplore.exe" || line == @"C:\Program Files (x86)\Internet Explorer\iexplore.exe")
                {
                    //What if IE was opened ==> Use Selenium to launch it

                    //For IE We need to check if Protected Mode was enables first
                    try
                    {
                        var driverService = InternetExplorerDriverService.CreateDefaultService();
                        driverService.HideCommandPromptWindow = true;

                        IWebDriver driver = new InternetExplorerDriver(driverService);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message + "\n" + "\n Protected Mode is Enabled in all zones for Internet Explorer! To enable automation please disable protected mode in IE"
                            + "\nIn IE, from the Tools menu (or the gear icon in the toolbar in later versions), select \"Internet options\" Go to the Security tab. At the bottom of the dialog for each zone, you should see a check box labeled \"Enable Protected Mode.\" Set the value of the check box to the same value, either checked or unchecked, for each zone.");
                    }

                    
                    

                    //========= CODE FOR IE MANIPULATION UPON START UP HERE ==============================//

                    counter++;
                }
                else
                {
                    //Console.WriteLine(line);
                    _application = TestStack.White.Application.Launch(line);
                    counter++;
                }
                
            }

            //Use the Show Open Apps Button to show all the apps currently opened BY Portal
            button3.PerformClick();

            //Release the File Resource Asset
            file.Close();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            //******************CLOSE ALL APPS HERE*************************//
            //===Use the AutoApp Object to close all open apps================//
            //===== NB Using TestStack to shut down browsers causes them to give a crash error message upon starting again ie they weren't closed properly======//
            //===== Try to resolve this we cannot use Selenium to shut them down as this requires the creation of new selenium browser objects =================//
            //==================================================================================================================================================//

            //=============ALSO MAKE ALLOWANCE IF THE USER HAD ALREADY MANUALLY CLOSED SOME APPS =======================================================//

            int counter = 0;
            string name = "";
            string windowTitle = "";
            try
            {
                foreach (int id in listBox3.Items)
                {
                    name = listBox2.Items[counter].ToString();
                    windowTitle = listBox4.Items[counter].ToString();
                    //MessageBox.Show("ID: " + id + "\n Window Name: " + name);

                    _application = TestStack.White.Application.Attach(id);
                    _mainWindow = _application.GetWindow(SearchCriteria.ByText(windowTitle), InitializeOption.NoCache);
                    
                    //NB FOR BEST RESULTS RUN WITHOUT DEBUGGING
                    //NB If Running as admin please change "Portal-Prototype - Microsoft Visual Studio" to "Portal-Prototype - Microsoft Visual Studio (Administrator)"

                    /*if (name == "Form1" || name == "Portal-Prototype - Microsoft Visual Studio" || name == "Portal-Prototype - Microsoft Visual Studio (Administrator)"
                        || name == "Portal-Prototype (Running) - Microsoft Visual Studio" || name == "Portal-Prototype (Running) - Microsoft Visual Studio (Administrator)")*/
                    if(name == "Portal-Prototype" || name == "devenv" || name == "Portal-Prototype.vshost")
                    {
                        counter++;
                        //continue;
                    }
                    else
                    {
                        //Enter saving application specific info here before closing instances of the app
                        //Also check if any browsers where open and try using Selenium instead to close them.

                        //May be better to use the close() for the browsers

                        //To safely close an app try closing the window first

                        _mainWindow.Dispose();
                        _application.Dispose();
                        counter++;
                    }

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\n Application may have been closed manually by user or window not visible");
            }

            //Clear the listboxes
            //NB Abstraction ==>  This is irrelevant to the user but clear the list once this button is pressed
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox3.Items.Clear();
            listBox4.Items.Clear();     
        }
    }
}
