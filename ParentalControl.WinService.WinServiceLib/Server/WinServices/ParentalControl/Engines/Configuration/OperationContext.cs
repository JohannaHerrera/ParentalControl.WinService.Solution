using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration
{
	internal class OperationContext
	{

		#region public properties
		public DateTime TimeStamp { get; private set; }
		public ServiceSettings ServiceSettings { get; private set; }
		public ServiceConfiguration ServiceConfiguration { get; private set; }

		/// <summary>
		/// Propiedad para ir almacenando el contenido que se quiere enviar en un mail. Este contenido se recoge información de los procesadores
		/// </summary>
		public StringBuilder ContenidoMail { get; set; }

		/// <summary>
		/// Propiedad para indicar si la lógica del servicio está siendo invocada desde la Web
		/// </summary>
		public bool IsExecutionFromWeb { get; set; }

		#endregion

		#region constructors
		public OperationContext()
		{
			// 1. Set the time stamp.
			this.TimeStamp = DateTime.Now;

			// 2. Read the service settings.
			this.ServiceSettings = new ServiceSettings();

			// 3. Read the service configuration.
			this.ServiceConfiguration = new ServiceConfiguration(this.ServiceSettings.ConfigurationDirectoryPath, this.ServiceSettings.ConfigurationFilePath);

			// 4. Inicializo el contenedor de la información a enviar en el mail.
			this.ContenidoMail = new StringBuilder();

			// 5. Por definición indico que no se está corriendo desde la web
			this.IsExecutionFromWeb = false;

		}
		#endregion
	}
}
