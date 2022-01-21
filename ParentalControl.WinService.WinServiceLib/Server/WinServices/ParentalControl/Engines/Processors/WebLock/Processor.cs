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
using System.Data.SqlClient;
using ParentalControl.WinService.Business.Enums;
using System.Diagnostics;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Processors.WebLock
{
    internal class Processor : BaseProcessor
    {

        #region public override properties
        bool webChanges = false;
        public override string Name { get { return ServiceConfiguration.WebLock; } }

        public void Dep_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<WebConfigurationModel> e)
        {
            //throw new NotImplementedException();
            switch (e.ChangeType)
            {
                case TableDependency.SqlClient.Base.Enums.ChangeType.Update:
                    this.webChanges = true;
                    break;
            }
        }
        public void KillNavigateProcess()
        {
            try
            {
                foreach (Process process in Process.GetProcesses())
                {
                    if (process.ProcessName.ToUpper().Contains("CHROME") ||
                        //process.ProcessName.ToUpper().Contains("EDGE") ||
                        //process.ProcessName.ToUpper().Contains("EXPLORER") ||
                        process.ProcessName.ToUpper().Contains("FIREFOX") ||
                        process.ProcessName.ToUpper().Contains("OPERA") ||
                        process.ProcessName.ToUpper().Contains("VIVALDI") ||
                        process.ProcessName.ToUpper().Contains("BRAVE"))
                    {
                        process.Kill();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error al cerrar el navegador, Reinicielo!" + ex);
            }
        }
        public void DeleteHost(string file)
        {
            try { 
                string[] files = File.ReadAllLines(file);
                string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                string[] hosts = File.ReadAllLines(path);
                List<string> list = new List<string>();
                int counter = 0;
                foreach (string hostsLine in hosts)
                {
                    int count = 0;
                    foreach (string lineFile in files)
                    {
                        if (hostsLine.ToString() == ("127.0.0.1 " + lineFile.ToString()))
                        {
                            hosts[counter] = null;
                            File.WriteAllLines(path, hosts);
                            //File.WriteAllLines(path, hosts.Take(hosts.Length - 1).ToArray());
                        }
                        count++;
                    }
                    counter++;
                }
            }catch(Exception ex)
            {
                //MessageBox.Show("Error al desbloquear webs" + ex);
            }
}
        public void AddHost(string file)
        {
            try
            {
                string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                string[] files = File.ReadAllLines(file);
                List<string> list = new List<string>();
                int contador = 0;
                foreach (string lineFile in files)
                {
                    list.Add("127.0.0.1" + " " + lineFile.ToString());
                    contador++;
                }
                File.AppendAllLines(path, list);
            }catch(Exception ex)
            {
                //MessageBox.Show("Error al bloquear webs" + ex);
            }

        }
        public bool GetExistence(string file)
        {
            string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
            string[] hosts = File.ReadAllLines(path);
            string[] files = File.ReadAllLines(file);
            //Valido que la info no este en el hosts, si est[a entonces ya no la añado
            bool exist = false;
            int count = 0;
            foreach (string line in hosts)
            {
                int counter = 0;
                foreach (string lineDrugs in files)
                {
                    if (("127.0.0.1 "+lineDrugs.ToString()) == line.ToString())
                    {
                        exist = true;
                        break;
                    }
                    counter++;
                }
                if (exist)
                {
                    break;
                }
                count++;
            }
            return exist;
        }

        #endregion


        #region protected override functions

        protected override void ExecuteInternal(OperationContext operationContext)
        {
            try
            {
                // Verifico si el usuario Windows Actual tiene vinculada una cuenta infantil         
                WebConfigurationBO webConfigurationBO = new WebConfigurationBO();
                DeviceBO deviceBO = new DeviceBO();
                WindowsAccountModel windowsAccountModel = deviceBO.GetInfantAccount(Environment.UserName);
                if (windowsAccountModel != null)
                {
                    // Verifico cambios en la base de datos
                    //Confirmo si hay cambios en la base de datos 
                    webChanges = true;
                    if (this.webChanges)
                    {
                        EmailBO emailBO = new EmailBO();
                        RequestBO requestBO = new RequestBO();
                        InfantAccountModel infantAccount = deviceBO.GetInfantAccountLinked(windowsAccountModel.InfantAccountId);
                        // Obtengo las webs        
                        List<WebConfigurationModel> webConfigurationModelList = webConfigurationBO.GetWebConfiguration(Environment.UserName);
                        if (webConfigurationModelList.Count > 0)
                        {
                            foreach (var web in webConfigurationModelList)
                            {
                                //******************** BLOQUEAR ****************************
                                // Drugs                   
                                if (web.WebConfigurationAccess == true && web.CategoryId == 1)
                                {
                                    try
                                    {
                                        string drugs = @"drugs.txt";
                                        //string drugs = @"C:\Users\Keru\Downloads\drugs.txt";
                                        bool existencia = GetExistence(drugs.ToString());
                                        //Valida la existencia de la informacion en el Host
                                        //Si la informacion no existe, enotnces se procese
                                        //A llenarla
                                        if (existencia == false)// Si no existe 
                                        {
                                            AddHost(drugs);
                                            KillNavigateProcess();
                                        }
                                    }catch(Exception ex)
                                    {
                                       // MessageBox.Show("No se pudo bloquear la web Drogas " + ex.Message);
                                    }    
                                }                    
                                //Adult
                                if (web.WebConfigurationAccess == true && web.CategoryId == 2)
                                {
                                    try
                                    {
                                        string adult = @"adult.txt";
                                        //string[] adultFile = File.ReadAllLines(adult);
                                        bool existencia = GetExistence(adult);
                                        if (existencia == false)// Si no existe 
                                        {
                                            AddHost(adult);
                                            KillNavigateProcess();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                       // MessageBox.Show("No se pudo bloquear la web Adultos " + ex.Message);
                                    }
                                }    
                                //Games
                                if (web.WebConfigurationAccess == true && web.CategoryId == 3)
                                {
                                    try
                                    {
                                        string games = @"games.txt";
                                        bool existencia = GetExistence(games);
                                        if (existencia == false)// Si no existe 
                                        {
                                            AddHost(games);
                                            KillNavigateProcess();
                                        }
                                    }catch(Exception ex)
                                    {
                                       // MessageBox.Show("No se pudo bloquear la web Juegos " + ex.Message);
                                    }
                                }
                                //Violence
                                if (web.WebConfigurationAccess == true && web.CategoryId == 4)
                                {
                                    try
                                    {
                                        string violence = @"violence.txt";
                                        bool existencia = GetExistence(violence);
                                        if (existencia == false)// Si no existe 
                                        {
                                            AddHost(violence);
                                            KillNavigateProcess();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                       // MessageBox.Show("No se pudo bloquear la web Violencia " + ex.Message);
                                    }
                                }
                                //Redes
                                if (web.WebConfigurationAccess == true && web.CategoryId == 5)
                                {
                                    try
                                    {
                                        string social = @"socialN.txt";
                                        bool existencia = GetExistence(social);
                                        if (existencia == false)// Si no existe 
                                        {
                                            AddHost(social);
                                            KillNavigateProcess();
                                        }
                                    }catch(Exception ex)
                                    {
                                       // MessageBox.Show("No se pudo bloquear la web Redes Sociales " + ex.Message);
                                    }
                                }
                                
                                //******************************* DESBLOQUEO **********************************************
                                //Drugs
                                if (web.WebConfigurationAccess == false && web.CategoryId == 1)
                                {
                                    try
                                    {
                                        string drugs = @"drugs.txt";
                                        bool existencia = GetExistence(drugs);
                                        if (existencia == true)
                                        {
                                            DeleteHost(drugs);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show("No se pudo desbloquear la web Drogas " + ex.Message);
                                    }
                                }
                                //Adult
                                if (web.WebConfigurationAccess == false && web.CategoryId == 2)
                                {
                                    try
                                    {
                                        string adult = @"adult.txt";
                                        bool existencia = GetExistence(adult);
                                        if (existencia == true)
                                        {
                                            DeleteHost(adult);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show("No se pudo desbloquear la web Contenido Adulto" + ex.Message);
                                    }
                                }
                                // Games
                                if (web.WebConfigurationAccess == false && web.CategoryId == 3)
                                {
                                    try
                                    {
                                        string games = @"games.txt";
                                        bool existencia = GetExistence(games);
                                        if (existencia==true)
                                        {
                                            DeleteHost(games);
                                        }
                                    }catch(Exception ex)
                                    {
                                        //MessageBox.Show("No se pudo desbloquear la web Juegos " + ex.Message);
                                    }    
                                }
                                // Violence
                                if (web.WebConfigurationAccess == false && web.CategoryId == 4)
                                {
                                    try{
                                        string violence = @"violence.txt";
                                        bool existencia = GetExistence(violence);
                                        if (existencia==true)
                                        {
                                            DeleteHost(violence);
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                       // MessageBox.Show("No se pudo desbloquear la web Violence "+ ex.Message);
                                    }                                
                                }
                                // SocialNetwork
                                if (web.WebConfigurationAccess == false && web.CategoryId == 5)
                                {
                                    try
                                    {
                                        string social = @"socialN.txt";
                                        bool existencia = GetExistence(social);
                                        if (existencia == true)
                                        {
                                            DeleteHost(social);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        //MessageBox.Show("No se pudo desbloquear la web Redes sociales " + ex.Message);
                                    }
                                }
                            }  
                        }
                        //this.webChanges = false;//Variable que local, bandera de los cambios
                    }
                }               
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        #endregion protected override functions
    }
}
