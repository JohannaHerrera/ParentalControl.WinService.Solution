using ParentalControl.WinService.Business.ParentalControl;
using ParentalControl.WinService.Models.Device;
using ParentalControl.WinService.Models.InfantAccount;
using ParentalControl.WinService.Models.ScheduleAccount;
using ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Processors.DeviceLock
{
    internal class Processor : BaseProcessor
    {

        #region public override properties

        public override string Name { get { return ServiceConfiguration.DeviceLock; } }

        #endregion


        #region protected override functions
        /// <summary>
        /// Funcionalidad del procesador
        /// </summary>
        /// <param name="operationContext"></param>
        protected override void ExecuteInternal(OperationContext operationContext)
        {
            try
            {
                // Se verifica que la cuenta de Windows actual esté vinculada a un infante.
                DeviceBO deviceBO = new DeviceBO();
                WindowsAccountModel windowsAccountModel = deviceBO.GetInfantAccount(Environment.UserName);

                // En el caso de estar vinculada a un infante se verifican las horas para proceder al bloqueo
                // del dispositivo.
                if (windowsAccountModel != null)
                {
                    ScheduleBO scheduleBO = new ScheduleBO();
                    DeviceUseBO deviceUseBO = new DeviceUseBO();
                    EmailBO emailBO = new EmailBO();
                    RequestBO requestBO = new RequestBO();
                    InfantAccountModel infantAccount = deviceBO.GetInfantAccountLinked(windowsAccountModel.InfantAccountId);
                    DateTime dateValue = DateTime.Now;
                    string dia = dateValue.ToString("dddd", new CultureInfo("es-ES"));
                    List<DeviceUseModel> deviceUseModelList = new List<DeviceUseModel>();

                    // Se obtiene el uso del dispositivo para la cuenta de infante
                    deviceUseModelList = deviceUseBO.GetDeviceUse(windowsAccountModel.InfantAccountId, dia);

                    // Una vez obtenido el uso del dispositivo se procede a bloquear el dispositivo en
                    // el horario y día establecido.
                    if (deviceUseModelList.Count > 0)
                    {
                        foreach (var deviceUse in deviceUseModelList)
                        {
                            ScheduleModel scheduleModel = scheduleBO.GetSchedule(deviceUse.ScheduleId);

                            //Se verifica si en el día actual el infante realizó una petición de aumento de tiempo
                            //de uso del dispositvo, en caso de que esté aprobada, se tomará en cuenta el incremento.
                            if (scheduleBO.DeviceUseRequestApproved(windowsAccountModel.InfantAccountId))
                            {
                                scheduleModel = scheduleBO.GetNewScheduleIfDeviceUseRequestIsApproved(scheduleModel, windowsAccountModel.InfantAccountId);
                            }
                            //Cuando queden aproximadamente 10 minutos para el fin del uso del dispositivo, se enviará un mensaje.
                            if (scheduleBO.ShowMessageScheduleWithSystemTime(scheduleModel,10))
                            {
                                DialogResult res = MessageBox.Show("Por Favor Guarde su trabajo, el dispositivo se bloqueará en aproximadamente 10 minutos", "¡AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            //Cuando queden aproximadamente 6 minutos para el fin del uso del dispositivo, se enviará un mensaje.
                            else if (scheduleBO.ShowMessageScheduleWithSystemTime(scheduleModel, 6))
                            {
                                DialogResult res = MessageBox.Show("Por Favor Guarde su trabajo, el dispositivo se bloqueará en aproximadamente 6 minutos", "¡AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            //Cuando queden aproximadamente 3 minutos para el fin del uso del dispositivo, se enviará un mensaje.
                            else if (scheduleBO.ShowMessageScheduleWithSystemTime(scheduleModel, 3))
                            {
                                DialogResult res = MessageBox.Show("Por Favor Guarde su trabajo, el dispositivo se bloqueará en aproximadamente 3 minutos", "¡AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            //Cuando queden aproximadamente 1 minutos para el fin del uso del dispositivo, se enviará un mensaje.
                            else if (scheduleBO.ShowMessageScheduleWithSystemTime(scheduleModel, 1))
                            {
                                DialogResult res = MessageBox.Show("Por Favor Guarde su trabajo, el dispositivo se bloqueará en aproximadamente 1 minutos", "¡AVISO!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            //El dispositivo se bloqueará en caso de que esté fuera del tiempo de uso.
                            else if (!scheduleBO.CompareScheduleWithSystemTime(scheduleModel))
                            {
                                Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
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
