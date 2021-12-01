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
        /*public void setLines(List<string> list)
        {
            string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
            File.AppendAllLines(path, list);
        }*/
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
               /*
               int num = 0;
               if (num == 1)
                {
                    
                    string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                    List<string> list = new List<string>();

                    list.Add("127.0.0.1" + " " + "www.duolingo.com");
                    list.Add("127.0.0.1" + " " + "www.facebook.com");

                    File.AppendAllLines(path, list);
                }
                else
                {
                    string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                    string[] lines = File.ReadAllLines(path);
                    //var varLines = File.ReadLines(@"C:\WINDOWS\system32\drivers\etc\hosts").Count();
                    int counter = 0;
                    foreach (string line in lines)
                    {
                        
                        if (line.ToString() == "127.0.0.1 www.duolingo.com" || line.ToString() == "127.0.0.1 www.facebook.com")
                        {     
                            lines[counter] = null;
                            File.WriteAllLines(path, lines);
                            File.WriteAllLines(path, lines.Take(lines.Length - 1 ).ToArray());                    
                        }
                        counter++;

                    }                        
                }
                */

                /*
                 int num = 1;
                 if (num==1)
                 {
                    using (StreamWriter w = File.AppendText(Path.Combine(Environment.GetFolderPath
                        (Environment.SpecialFolder.System), @"drivers\etc\hosts"))) 
                    {
                        List<string> list = new List<string>();

                        list.Add("127.0.0.1" + " " + "www.duolingo.com");


                        w.WriteLine("127.0.0.1" + " " + "www.duolingo.com");
                    }

                 }

                */


                // Verifico si el usuario Windows Actual tiene vinculada una cuenta infantil

                
                WebConfigurationBO webConfigurationBO = new WebConfigurationBO();
                DeviceBO deviceBO = new DeviceBO();
                WindowsAccountModel windowsAccountModel = deviceBO.GetInfantAccount(Environment.UserName);
                if (windowsAccountModel != null)
                {
                    // Verifico cambios en la base de datos
                    /*
                    string connectionString =
                        @"Data Source = .\Keru; Initial Catalog = ParentalControlDB; Integrated Security = true;";
                    using (TableDependency.SqlClient.SqlTableDependency<Models.Device.WebConfigurationModel> dep =
                        new TableDependency.SqlClient.SqlTableDependency<Models.Device.WebConfigurationModel>(connectionString))
                    {
                        dep.OnChanged += Dep_OnChanged;
                        dep.Start();
                    }*/
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
                                // NUNCA SE VAN A DESBLOQUEAR
                                // Drugs                   
                                if (web.WebConfigurationAccess == true && web.CategoryId == 1)
                                {
                                    try
                                    {
                                        string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                                        List<string> list = new List<string>();
                                        string drugs = @"C:\Users\Keru\Downloads\drugs.txt";
                                        string[] drugsFile = File.ReadAllLines(drugs);
                                        //Valido que la info no este en el hosts, si est[a entonces ya no la añado
                                        bool existencia = GetExistence(drugs);
                                        if (existencia == false)// Si no existe 
                                        {
                                            int contador = 0;
                                            foreach (string lineDrugs in drugsFile)
                                            {
                                                list.Add("127.0.0.1" + " " + lineDrugs.ToString());
                                                contador++;
                                            }
                                            File.AppendAllLines(path, list);
                                            //setLines(list);
                                        }
                                    }catch(Exception ex)
                                    {

                                    }    
                                }
                                //Adult
                                //************* ANADIR VALIDADOR PARA SABER SI ESTE ARCHIVO YA ESTA EN HOST PARA NO VOLVER A ESCRIBIR
                                if (web.WebConfigurationAccess == true && web.CategoryId == 2)
                                {
                                    try
                                    {
                                        string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                                        List<string> list = new List<string>();
                                        string adult = @"C:\Users\Keru\Downloads\adult.txt";
                                        string[] adultFile = File.ReadAllLines(adult);
                                        bool existencia = GetExistence(adult);
                                        if (existencia == false)// Si no existe 
                                        {
                                            foreach (string lineAdult in adultFile)
                                            {
                                                list.Add("127.0.0.1" + " " + lineAdult.ToString());
                                            }
                                            File.AppendAllLines(path, list);
                                            //setLines(list);
                                        }
                                    }
                                    catch (Exception ex)
                                    {

                                    }

                                }
                                
                                //Games
                                if (web.WebConfigurationAccess == true && web.CategoryId == 3)
                                {
                                    try
                                    {
                                        string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                                        List<string> list = new List<string>();
                                        string games = @"C:\Users\Keru\Downloads\games.txt";
                                        string[] gamesFile = File.ReadAllLines(games);
                                        bool existencia = GetExistence(games);
                                        if (existencia == false)// Si no existe 
                                        {
                                            foreach (string gameLine in gamesFile)
                                            {
                                                list.Add("127.0.0.1" + " " + gameLine.ToString());
                                            }
                                            File.AppendAllLines(path, list);
                                            //setLines(list);
                                        }
                                    }catch(Exception ex)
                                    {

                                    }
                                }
                                //Redes
                                if (web.WebConfigurationAccess == true && web.CategoryId == 4)
                                {
                                    try
                                    {


                                        string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                                        List<string> list = new List<string>();
                                        string social = @"C:\Users\Keru\Downloads\socialN.txt";
                                        string[] socialFile = File.ReadAllLines(social);
                                        bool existencia = GetExistence(social);
                                        if (existencia == false)// Si no existe 
                                        {
                                            foreach (string socialLine in socialFile)
                                            {
                                                list.Add("127.0.0.1" + " " + socialLine.ToString());
                                            }
                                            File.AppendAllLines(path, list);
                                            //setLines(list);
                                        }
                                    }catch(Exception ex)
                                    {

                                    }
                                }

                                //******************************* DESBLOQUEO **********************************************
                                // Games
                                
                                if (web.WebConfigurationAccess == false && web.CategoryId == 3)
                                {
                                    try
                                    {
                                        string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                                        string[] hosts = File.ReadAllLines(path);
                                        //var varLines = File.ReadLines(@"C:\WINDOWS\system32\drivers\etc\hosts").Count();
                                        List<string> list = new List<string>();
                                        string games = @"C:\Users\Keru\Downloads\games.txt";
                                        string[] gamesFile = File.ReadAllLines(games);
                                        bool existencia = GetExistence(games);
                                        if (existencia==true)
                                        {
                                            int counter = 0;
                                            foreach (string hostsLine in hosts)
                                            {
                                                int count = 0;
                                                foreach (string gamesLine in gamesFile)
                                                {
                                                    if (hostsLine.ToString() == ("127.0.0.1 " + gamesLine.ToString()))
                                                    {
                                                        hosts[counter] = null;
                                                        File.WriteAllLines(path, hosts);
                                                        //File.WriteAllLines(path, hosts.Take(hosts.Length - 1).ToArray());
                                                    }
                                                    count++;
                                                }
                                                counter++;
                                            }
                                        }
                                    }catch(Exception ex)
                                    {
                                        MessageBox.Show("No se pudo desbloquear la web Redes sociales " + ex.Message);
                                    }    
                                }
                                // SocialNetwork
                                if (web.WebConfigurationAccess == false && web.CategoryId == 4)
                                {
                                    try{
                                        string path = @"C:\WINDOWS\system32\drivers\etc\hosts";
                                        string[] hosts = File.ReadAllLines(path);
                                        //var varLines = File.ReadLines(@"C:\WINDOWS\system32\drivers\etc\hosts").Count();
                                        List<string> list = new List<string>();
                                        string social = @"C:\Users\Keru\Downloads\socialN.txt";
                                        string[] socialFile = File.ReadAllLines(social);
                                        bool existencia = GetExistence(social);
                                        if (existencia==true)
                                        {
                                            int counter = 0;
                                            foreach (string hostsLine in hosts)
                                            {
                                                int count = 0;
                                                foreach (string socialLine in socialFile)
                                                {
                                                    if (hostsLine.ToString() == ("127.0.0.1 " + socialLine.ToString()))
                                                    {
                                                        hosts[counter] = null;
                                                        File.WriteAllLines(path, hosts);
                                                        //File.WriteAllLines(path, hosts.Take(hosts.Length - 1).ToArray());
                                                    }
                                                    count++;
                                                }
                                                counter++;
                                            }
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show("No se pudo desbloquear la web Redes sociales "+ ex.Message);
                                    }                                
                                }                               
                            }
                        }
                        this.webChanges = false;//Variable que local, bandera de los cambios
                    }
                    
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion protected override functions
    }
}
