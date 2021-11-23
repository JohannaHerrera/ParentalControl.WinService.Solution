using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration
{
    [Serializable]
    public class ProcessorConfiguration
    {
        /// <summary>
        /// Indica el nombre del Procesador.
        /// </summary>
        public string ProcessorName { get; set; }

        /// <summary>
        /// Indica si el procesador está habilitado
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Contiene el Query para obtener la Info Tributaria
        /// </summary>
        public string InfoTributariaQuery { get; set; }

        /// <summary>
        /// Contiene el Query para obtner los detalles
        /// </summary>
        public string DetalleQuery { get; set; }
    }
}
