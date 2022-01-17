

using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines;

namespace ParentalControlWinServiceApp.Server.WinServices.ParentalControl.Executables
{
    /// <summary>
    /// This class customizes the operation of the WinService and 
    /// WinServiceInstaller.  If this "winservice framework" needs to be used on 
    /// another project, then this class should be modified.
    /// </summary>
    internal static class ServiceConfiguration
    {

        #region public static readonly

        public static readonly string ServiceDisplayName = "ParentalControl.WinService";
        public static readonly string ServiceName = "ParentalControl.WinService";

        #endregion


        #region public static functions
        public static object CreateEngine()
        {
            return new WinServiceEngine();
        }


        public static void StartEngine(
            object engine)
        {
            //((WinServiceEngine)engine).Start();
            WinServiceEngine x = (WinServiceEngine)engine;
            x.Start();
        }


        public static void StopEngine(
            object engine)
        {
            ((WinServiceEngine)engine).Stop();
        }

        #endregion

    }
}
