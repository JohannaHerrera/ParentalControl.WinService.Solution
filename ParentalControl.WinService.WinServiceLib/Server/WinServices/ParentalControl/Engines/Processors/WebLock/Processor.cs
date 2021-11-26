using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParentalControl.WinService.Business;
using ParentalControl.WinService.Business.ParentalControl;
using ParentalControl.WinService.Models.Device;
using ParentalControl.WinService.Models.InfantAccount;
using System.Windows.Forms;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Processors.WebLock
{
    internal class Processor : BaseProcessor
    {
        
        #region public override properties

        public override string Name { get { return ServiceConfiguration.WebLock; } }
        
        

        #endregion


        #region protected override functions

        protected override void ExecuteInternal(OperationContext operationContext)
        {
            try
            {
                 int num = 5;
                 if (num==1)
                 {
                    using (StreamWriter w = File.AppendText(Path.Combine(Environment.GetFolderPath
                        (Environment.SpecialFolder.System), @"drivers\etc\hosts"))) 
                    {
                        List<string> list = new List<string>();

                        list.Add("127.0.0.1" + " " + "www.duolingo.com");


                        w.WriteLine("127.0.0.1" + " " + "www.duolingo.com");
                    }
                     //   string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                     //List<string> list = new List<string>();

                     //list.Add("127.0.0.1" + " " + "www.duolingo.com");


                     //File.AppendAllLines(path, list);

                 }
                 else
                 {
                     //string duolingo = "www.duolingo.com";
                      string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                     string[] lines = File.ReadAllLines(path);

                     File.WriteAllLines(path, lines.Take(lines.Length - 1).ToArray());

                 }
                
                // Verifico si el usuario Windows Actual tiene vinculada una cuenta infantil
                /*DeviceBO deviceBO = new DeviceBO();
                WindowsAccountModel windowsAccountModel = deviceBO.GetInfantAccount(Environment.UserName);
                if (windowsAccountModel != null)
                {

                }*/



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion protected override functions
    }
}
