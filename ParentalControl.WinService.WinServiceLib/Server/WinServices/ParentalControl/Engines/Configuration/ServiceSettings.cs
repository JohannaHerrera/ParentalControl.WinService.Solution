using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration
{
	internal class ServiceSettings
	{

		#region public properties

		public string ModeExecution { get; private set; }               // eg. Intervalo, aplica TimerInterval,  Dialy, aplica ScheduledTime

		public double TimerInterval { get; private set; }               // eg. 5000 (specified in milliseconds).

		public string ScheduledTime { get; set; }

		public string ConfigurationDirectoryPath { get; private set; }  // eg. C:\Program Files\ParentalControl\ParentalControlWinService
		public string ConfigurationFilePath { get; private set; }       // eg. C:\Program Files\ParentalControl\ParentalControlWinService\configuration\ProcessorConfigurationFile.xml


		#region Mail

		/// <summary>
		/// Se obtiene desde el archivo de configuración el mail desde donde se va a enviar el Mail
		/// </summary>
		public string MailMessageFrom { get; set; }

		/// <summary>
		/// Se obtiene desde el archivo de configuración la lista de direcciones a donde se debe enviar los correos
		/// </summary>
		public string MailMessageTo { get; set; }

		/// <summary>
		/// Se obtiene desde el archivo de configuración el motivo del mail
		/// </summary>
		public string MailMessageSubject { get; set; }

		/// <summary>
		/// Se obtiene desde el archivo de configuración el motivo de Error del mail
		/// </summary>
		public string MailMessageSubjectError { get; set; }

		#endregion Mail


		#endregion


		#region constructors
		public ServiceSettings()
		{
			ReadServiceSettings();
		}
		#endregion


		#region private functions
		private void ReadServiceSettings()
		{
			// Set private members
			this.ModeExecution = GetConfigurationSetting(ServiceSettingNames.ModeExecution).ToString();
			this.ScheduledTime = GetConfigurationSetting(ServiceSettingNames.ScheduledTime).ToString();

			this.MailMessageFrom = GetConfigurationSetting(ServiceSettingNames.MailMessageFrom).ToString();
			this.MailMessageTo = GetConfigurationSetting(ServiceSettingNames.MailMessageTo).ToString();
			this.MailMessageSubject = GetConfigurationSetting(ServiceSettingNames.MailMessageSubject).ToString();
			this.MailMessageSubjectError = GetConfigurationSetting(ServiceSettingNames.MailMessageSubjectError).ToString();

			//  Indico si es la ejecución a una determinada hora o cada cierto intervalo de tiempo.
			if (ModeExecution.ToLower() == "dialy")
			{
				DateTime scheduleTime = DateTime.Parse(this.ScheduledTime);

				if (scheduleTime > DateTime.Now)
				{
					// Si aun no se pasa la hora de calendarización, calendarizo para que se ejecute hoy.
					this.TimerInterval = (scheduleTime - DateTime.Now).TotalMilliseconds;
				}
				else
				{
					// Si la hora de ejecución es mayor a la hora actual, seteo la calendarización para el siguient día.
					scheduleTime = scheduleTime.AddDays(1);
					this.TimerInterval = (scheduleTime - DateTime.Now).TotalMilliseconds;
				}
			}
			else
			{
				this.TimerInterval = double.Parse(GetConfigurationSetting(ServiceSettingNames.TimerInterval));
			}


			this.ConfigurationDirectoryPath = GetConfigurationSetting(ServiceSettingNames.ConfigurationDirectoryPath);
			this.ConfigurationFilePath = GetConfigurationSetting(ServiceSettingNames.ConfigurationFilePath);
		}


		private string GetConfigurationSetting(string settingName)
		{
			try
			{
				// Read configuration setting
				return ConfigurationManager.AppSettings[settingName].ToString();
			}
			catch (Exception exception)
			{
				//throw new UdlaException(string.Format("Could not read configuration setting ({0}).", settingName), exception);
				throw new Exception(string.Format("Could not read configuration setting ({0}).", settingName), exception);
			}
		}
		#endregion

		#region Public Functions

		/// <summary>
		/// Imprime la proxima hora de ejecución del servicio
		/// </summary>
		public void ImprimirProximaHoraEjecucion()
		{
			TimeSpan t = TimeSpan.FromMilliseconds(this.TimerInterval);
			DateTime scheduleTime = DateTime.Now;
			if (ModeExecution.ToLower() == "dialy")
			{
				scheduleTime = DateTime.Parse(this.ScheduledTime);

				if (DateTime.Now > scheduleTime)
				{
					scheduleTime = scheduleTime.AddDays(1);
				}
			}
			else
			{
				scheduleTime = scheduleTime.Add(t);
			}

			string answer = string.Format("{0:D2}h:{1:D2}m:{2:D2}s:{3:D3}ms",
									t.Hours,
									t.Minutes,
									t.Seconds,
									t.Milliseconds);
		}

		#endregion Public Functions

	}
}
