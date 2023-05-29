using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;   // I may not need this
using SolidWorks.Interop.swpublished;
using SolidWorks.Interop.sldworks;
using System.ComponentModel;
using System.CodeDom;
using SolidWorks.Interop.swconst;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

// the ProjectProgression addin is for periodically saving images/copies of your part to show its progression during development. 

namespace AnAddin
{
    /// <summary>
    /// The main SolidWorks addin.
    /// </summary>
    [ComVisible(true)]
    [Guid("7B8B3963-51BC-4CBF-A4CD-93DA8853ED9F")]
    [DisplayName("PartProgression")]
    [Description("SOLIDWORKS addin for tracking the development of your part")]
    public class AddinMain : SwAddin
    {
        #region Registration

        private const string ADDIN_KEY_TEMPLATE = @"SOFTWARE\Solidworks\Addins\{{{0}}}";
        private const string ADDIN_STARTUP_KEY_TEMPLATE = @"Software\SolidWorks\AddInsStartup\{{{0}}}";
        private const string ADD_IN_TITLE_REG_KEY_NAME = "Title";
        private const string ADD_IN_DESCRIPTION_REG_KEY_NAME = "Description";

        /// <summary>
        /// The method called during assembly regestration for COM.
        /// </summary>
        /// <param name="t"></param>
        [ComRegisterFunction]
        public static void RegisterFunction(Type t)
        {
            try
            {
                var addInTitle = "";
                var loadAtStartup = true;
                var addInDesc = "";

                // title //

                var dispNameAtt = t.GetCustomAttributes(false).OfType<DisplayNameAttribute>().FirstOrDefault();

                if (dispNameAtt != null)
                {
                    addInTitle = dispNameAtt.DisplayName;
                }
                else
                {
                    addInTitle = t.ToString();
                }

                // description //

                var descAtt = t.GetCustomAttributes(false).OfType<DescriptionAttribute>().FirstOrDefault();

                if (descAtt != null)
                {
                    addInDesc = descAtt.Description;
                }
                else
                {
                    addInDesc = t.ToString();
                }

                // create and set keys //

                var addInkey = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(string.Format(ADDIN_KEY_TEMPLATE, t.GUID));

                // previously i had the bool set to zero, and the addin did load on start up - why did that happen?
                //addInkey.SetValue(null, 0);
                addInkey.SetValue(null, 1);
                addInkey.SetValue(ADD_IN_TITLE_REG_KEY_NAME, addInTitle);
                addInkey.SetValue(ADD_IN_DESCRIPTION_REG_KEY_NAME, addInDesc);

                var addInStartupkey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(string.Format(ADDIN_STARTUP_KEY_TEMPLATE, t.GUID));

                addInStartupkey.SetValue(null, Convert.ToInt32(loadAtStartup), Microsoft.Win32.RegistryValueKind.DWord);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while registering the addin: " + ex.Message);
            }
        }

        /// <summary>
        /// The method called during assembly deregestration for COM
        /// </summary>
        [ComUnregisterFunction]
        public static void UnregisterFunction(Type t)
        {
            try
            {
                // previously this worked, but since the main class links to other classes w/ progids, then i think we need to use the tree method
                //Microsoft.Win32.Registry.LocalMachine.DeleteSubKey(string.Format(ADDIN_KEY_TEMPLATE, t.GUID));
                Microsoft.Win32.Registry.LocalMachine.DeleteSubKeyTree(string.Format(ADDIN_KEY_TEMPLATE, t.GUID));

                // likely need to do something similar for the current user key
                //Microsoft.Win32.Registry.CurrentUser.DeleteSubKey(string.Format(ADDIN_STARTUP_KEY_TEMPLATE, t.GUID));
                Microsoft.Win32.Registry.CurrentUser.DeleteSubKeyTree(string.Format(ADDIN_STARTUP_KEY_TEMPLATE, t.GUID));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error while unregistering the addin: " + ex.Message);
            }
        }

        #endregion


        #region Private Declarations

        /// <summary>
        /// The SolidWorks object that contains all the information from the current SolidWorks instance
        /// </summary>
        // TODO: find out the difference between SldWorks and ISldWorks, then make changes accordingly
        private SldWorks mSwApp;
        //private ISldWorks mSwApp;

        /// <summary>
        /// The integer cookie to the current SolidWorks instance
        /// </summary>
        private int mSwCookie;

        /// <summary>
        /// The taskpane view for displaying and interacting with the addin inside of a SolidWorks session
        /// </summary>
        private TaskpaneView mTaskpaneView;

        /// <summary>
        /// The UI control logic for the taskpane view
        /// </summary>
        private TaskpaneUI mTaskpaneUI;

        /// <summary>
        /// The output of the taskpane AddStandardButton function
        /// </summary>
        private bool buttonResult;

        #endregion


        #region Public Declarations

        /// <summary>
        /// Unique ProgID for classifying the SolidWorks taskpane - will be necessary for COM registration
        /// </summary>
        public const string ADDIN_SWTASKPANE_PROGID = "AnAddin.Taskpane.ProjectProgression";

        /// <summary>
        /// Another unique id for use in the add control function - this is another attempt at getting the ui to show
        /// </summary>
        public const string ctrlName = "My.Updated.Addin.ProjectProgression";

        #endregion

        #region Addin Callbacks

        /// <summary>
        /// The first required method for the ISwAddin interface - called when the addin is loaded.
        /// </summary>
        /// <param name="ThisSw">The passed SolidWorks instance</param>
        /// <param name="Cookie">The passed SolidWorks cookie</param>
        /// <returns>True if the add-in connected successfully, false if not</returns>
        public bool ConnectToSW(object ThisSw, int Cookie)
        {
            // store the passed-in SolidWorks instance inside an easy-to-access reference
            // need an explicit cast from object to ISldWorks ... or is it SldWorks?  ->  i know ISldWorks is the interface, but what else is different?
            //mSwApp = (ISldWorks)ThisSw;
            mSwApp = (SldWorks)ThisSw;

            // store the passed-in cookie ID
            mSwCookie = Cookie;

            // *** Test to see if connection is successful ***
            // mSwApp.SendMsgToUser2("Hello world", (int)swMessageBoxIcon_e.swMbInformation, (int)swMessageBoxBtn_e.swMbOk);

            // setup a callback to the SolidWorks application
            // TODO: come back to this once issues gone and ready to implement further features
            var success = mSwApp.SetAddinCallbackInfo2(0, this, mSwCookie);

            // create the ui for taskpane and other later features
            LoadUI();

            return true;
        }

        /// <summary>
        /// The second required method for the ISwAddin interface - called when SOLIDWORKS is about to be destroyed.
        /// </summary>
        /// <returns>True if the add-in disconnected successfully, false if not.</returns>
        public bool DisconnectFromSW() 
        {
            // ui teardown
            UnloadUI();
            
            return true; 
        }

        #endregion


        #region UI Methods

        /// <summary>
        /// Creates taskpane and loads in UI elements to SolidWorks- currently this is just kept to the taskpane.
        /// </summary>
        private void LoadUI()
        {
            // grab paths and populate the bitmap array for icons
            // TODO: do this in a for loop later
            string icoDirPath = Path.GetDirectoryName(typeof(AddinMain).Assembly.CodeBase).Replace(@"file:\", string.Empty);
            string[] bitmap = new string[6];
            bitmap[0] = Path.Combine(icoDirPath, @"icons\icons8-cat-20.png");
            bitmap[1] = Path.Combine(icoDirPath, @"icons\icons8-cat-32.png");
            bitmap[2] = Path.Combine(icoDirPath, @"icons\icons8-cat-40.png");
            bitmap[3] = Path.Combine(icoDirPath, @"icons\icons8-cat-64.png");
            bitmap[4] = Path.Combine(icoDirPath, @"icons\icons8-cat-96.png");
            bitmap[5] = Path.Combine(icoDirPath, @"icons\icons8-cat-128.png");

            // create the taskpane
            // TODO: note that this is the main functioning code atm -- taskpane loads, not ui buttons and text -- why?
            mTaskpaneView = mSwApp.CreateTaskpaneView3(bitmap, "PartProgression Addin");

            // load UI elements into the taskpane
            // *need to cast
            mTaskpaneUI = (TaskpaneUI)mTaskpaneView.AddControl(ADDIN_SWTASKPANE_PROGID, string.Empty);
        }

        /// <summary>
        /// Tears down UI elements from the SolidWorks instance.
        /// </summary>
        private void UnloadUI()
        {
            mTaskpaneUI = null;

            // the taskpane object comes from the SolidWorks API, and is also a COM object - need to clean up correctly
            mTaskpaneView.DeleteView();
            // clean memory, release the COM ref
            Marshal.ReleaseComObject(mTaskpaneView);
            mTaskpaneView = null;

        }

        #endregion

    }
}
