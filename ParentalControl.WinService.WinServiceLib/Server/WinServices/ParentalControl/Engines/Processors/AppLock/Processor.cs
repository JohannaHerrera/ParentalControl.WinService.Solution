using ParentalControl.WinService.Business.Enums;
using ParentalControl.WinService.Business.ParentalControl;
using ParentalControl.WinService.Models.Device;
using ParentalControl.WinService.Models.InfantAccount;
using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Processors.AppLock
{
    internal class Processor : BaseProcessor
    {

        #region public override properties

        public override string Name { get { return ServiceConfiguration.AppLock; } }

        #endregion


        #region protected override functions

        protected override void ExecuteInternal(OperationContext operationContext)
        {
            try
            {
                // Verifico si el usuario Windows Actual tiene vinculada una cuenta infantil
                DeviceBO deviceBO = new DeviceBO();
                WindowsAccountModel windowsAccountModel = deviceBO.GetInfantAccount(Environment.UserName);

                // Si está vinculada a una cuenta infantil aplico las reglas de bloqueo de aplicaciones
                if (windowsAccountModel != null)
                {
                    ApplicationBO applicationBO = new ApplicationBO();
                    EmailBO emailBO = new EmailBO();
                    RequestBO requestBO = new RequestBO();
                    Constants constants = new Constants();
                    ActivityBO activityBO = new ActivityBO();
                    InfantAccountModel infantAccount = deviceBO.GetInfantAccountLinked(windowsAccountModel.InfantAccountId);

                    // Obtengo las aplicaciones bloqueadas
                    List<ApplicationModel> applicationModelList = applicationBO.GetBlockedApps(windowsAccountModel.InfantAccountId, windowsAccountModel.DevicePCId);

                    // Verifico los procesos que estén corriendo y bloqueo las aplicaciones que se deben bloquear
                    if(applicationModelList.Count > 0)
                    {
                        foreach (var app in applicationModelList)
                        {
                            foreach (Process process in Process.GetProcesses())
                            {
                                if (app.AppName.ToUpper().Contains(process.ProcessName.ToUpper()) ||
                                    process.ProcessName.ToUpper().Contains(app.AppName.ToUpper()))
                                {
                                    // Antes de cerrar el proceso verifico si no tiene configurado tiempo de uso
                                    if (app.ScheduleId != null)
                                    {
                                        // Valido si puede usar según la hora actual
                                        if(!(applicationBO.VerifyAppUse(infantAccount.InfantAccountId, app.AppId)))
                                        {
                                            process.Kill();
                                        }
                                    }
                                    else 
                                    {
                                        process.Kill();
                                    }

                                    activityBO.RegisterApps(infantAccount.InfantAccountId, app.AppName);

                                    DialogResult res = MessageBox.Show("Esta aplicación está bloqueada. ¿Deseas solicitar el acceso?", "¡AVISO!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                                    
                                    if (res == DialogResult.Yes)
                                    {
                                        // Valido si no existe una petición de acceso sobre la aplicación
                                        if (!requestBO.VerifyRequest(constants.AppConfiguration, app.AppName))
                                        {
                                            string deviceCode = deviceBO.GetDeviceIdentifier();
                                            string deviceName = deviceBO.GetDeviceName(deviceCode);

                                            string body = $"<p>¡Hola! <br> <br> Queremos informarte que <b>{infantAccount.InfantName}</b> " +
                                               $"está solicitando que le habilites la aplicación <b>{app.AppName}</b> del dispositivo {deviceName}. <br>" +
                                               $"Para aprobar o desaprobar esta petición ingresa a nuestro " +
                                               $"sistema y dirígete a la sección de <b>Notificaciones</b>.<p>";

                                            string parentEmail = deviceBO.GetParentEmail(infantAccount.ParentId);

                                            if (emailBO.SendEmail(parentEmail, body))
                                            {
                                                if (requestBO.RegisterRequestApp(infantAccount.InfantAccountId,
                                                                         infantAccount.ParentId, app.AppName))
                                                {
                                                    MessageBox.Show("Tu petición ha sido enviada correctamente. Te " +
                                                                    "estaremos notificando la respuesta de tus padres.");
                                                }
                                                else
                                                {
                                                    MessageBox.Show("Ocurrión un error al solicitar el acceso. Inténtelo de nuevo.");
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Ocurrión un error al solicitar el acceso. Inténtelo de nuevo.");
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Error, ya enviaste una petición de acceso sobre este recurso.");
                                            return;
                                        }
                                    }                                                                      
                                }
                            }
                        }                        
                    }

                    //// Verifico si se han instalado nuevas aplicaciones
                    //List<string> newInstalledApps = applicationBO.GetNewInstalledApps();
                    //if (newInstalledApps.Count > 0)
                    //{
                    //    List<ApplicationModel> applicationsRegistered = applicationBO.GetAppsDevice(windowsAccountModel.InfantAccountId, windowsAccountModel.DevicePCId);
                        
                    //    foreach (var newApp in newInstalledApps)
                    //    {
                    //        bool containsItem = applicationsRegistered.Any(item => item.DevicePCId == windowsAccountModel.DevicePCId
                    //                                        && item.AppName == newApp && item.InfantAccountId == windowsAccountModel.InfantAccountId);

                    //        if (!containsItem)
                    //        {
                    //            applicationBO.RegisterApps(windowsAccountModel.InfantAccountId, newApp);
                    //        }
                    //    }
                    //}                  
                }

                // Verifico si hay nuevas cuentas Windows por registrar
                List<string> windowsAccounts = deviceBO.GetWindowsAccounts();

                if (windowsAccounts.Count > 0)
                {
                    foreach (var account in windowsAccounts)
                    {
                        List<WindowsAccountModel> verifyAccount = deviceBO.VerifyWindowsAccount(account);

                        if (verifyAccount.Count == 0)
                        {
                            deviceBO.RegisterWindowsAccount(account);
                        }
                    }
                    
                }

                // Si alguna cuenta Windows se eliminó también elimino de la BD
                List<WindowsAccountModel> windowsAccountModelList = deviceBO.GetWindowsAccountsFromDB();
                
                if (windowsAccountModelList.Count > 0)
                {
                    foreach (var account in windowsAccountModelList)
                    {
                        if (!windowsAccounts.Any(x => x.ToUpper().Equals(account.WindowsAccountName.ToUpper())))
                        {
                            deviceBO.DeleteWindowsAccount(account.WindowsAccountName);

                            if (account.InfantAccountId != 0)
                            {
                                // Verifico si existe una cuenta Windows vinculada a la cuenta Infantil
                                List<WindowsAccountModel> windowsAccountsList = deviceBO.VerifyWindowsAccountFromInfants(account.InfantAccountId);

                                if (windowsAccounts.Count == 0)
                                {
                                    // Elimino las aplicaciones de la cuenta infantil
                                    ApplicationBO applicationBO = new ApplicationBO();
                                    applicationBO.DeleteApps(account.InfantAccountId);
                                }
                            }                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        #endregion protected override functions
    }
}
