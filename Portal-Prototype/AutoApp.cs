using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;


//Add the White reference namespaces
//**White's namespace has changed to 'TestStack.White'!**
using TestStack.White.Factory;
using TestStack.White.UIItems.Finders;
using TestStack.White.InputDevices;

namespace TestAutoApp
{
    class AutoApp
    {
        private string exeSourceFile; //String to hold the path to the program's exe
        private TestStack.White.Application _application;  //TestStack.White application object
        private TestStack.White.UIItems.WindowItems.Window _mainWindow;  //TestStack.White main window object


        //Related Methods
        //Properties
        public string ExeSourceFile   //Property to the source path
        {
            get { return exeSourceFile; }
            set { exeSourceFile = value; }
        }

        public TestStack.White.Application Application
        {
            get { return _application; }
            set { _application = value; }
        }

        public TestStack.White.UIItems.WindowItems.Window MainWindow
        {
            get { return _mainWindow; }
            set { _mainWindow = value; }
        }


        //Default Constructor
        public AutoApp()
        {
            exeSourceFile = "";
            _application = null;   //This is set by the start method
            _mainWindow = null;     //This is set by the start method
        }

        public AutoApp(string iexeSourceFile)
        {
            //check on some data validation later on
            exeSourceFile = iexeSourceFile;
        }

        public void startApp(string appName)
        {
            try
            {
                var psi = new ProcessStartInfo(exeSourceFile);
                // launch the process through white application
                _application = TestStack.White.Application.AttachOrLaunch(psi);

                //Get the window of calculator from white application 
                _mainWindow = _application.GetWindow(SearchCriteria.ByText(appName), InitializeOption.NoCache);

            }
            catch(Exception)
            {
                throw;
            }
        }

        public void closeApp()
        {
            try
            {
                //Dispose the main window
                _mainWindow.Dispose();

                //Dispose the application
                _application.Dispose();
            }
            catch(Exception)
            {
                throw;
            }
        }

    }
}
