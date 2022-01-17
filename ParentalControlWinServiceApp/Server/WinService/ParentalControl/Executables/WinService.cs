using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace ParentalControlWinServiceApp.Server.WinServices.ParentalControl.Executables
{
    partial class WinService : ServiceBase
    {


        #region private members

        private object _engine;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        #endregion


        #region Constructors

        public WinService()
        {
            InitializeComponent();

            this.ServiceName = ServiceConfiguration.ServiceName;
        }

        #endregion Constructors


        #region public static functions
        /// <summary>
        /// The main entry point for the process
        /// </summary>
        public static void Main()
        {
            try
            {
                // Run WinService
                ServiceBase.Run(new WinService());
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
            }
        }
        #endregion





        #region private functions

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Set private members
            components = new Container();
        }

        #endregion


        #region protected overriden functions
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            // Dispose components
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        protected override void OnStart(string[] args)
        {
            try
            {
                // Create engine
                _engine = ServiceConfiguration.CreateEngine();
                // Start engine
                ServiceConfiguration.StartEngine(_engine);
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
            }
        }

        protected override void OnStop()
        {

            try
            {
                if (_engine != null)
                {
                    // Stop engine
                    ServiceConfiguration.StopEngine(_engine);
                    _engine = null;
                }
            }
            catch (Exception exception)
            {
                throw;
            }
            finally
            {
            }
        }

        #endregion

    }
}
