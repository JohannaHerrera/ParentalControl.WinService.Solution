using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.Request
{
    /// <summary>
    /// Modelo de la tabla Request
    /// </summary>
    public class RequestModel
    {
        // Id de la Petición
        public int RequestId { get; set; }
        // Id del tipo de Petición
        public int RequestTypeId { get; set; }
        // Id del Infante
        public int InfantAccountId { get; set; }
        // Objeto de la Petición
        public string RequestObject { get; set; }
        // Tiempo de incremento en el tiempo
        public decimal RequestTime { get; set; }
        // Estado de la Petición
        public bool RequestState { get; set; }
        // Fecha de creación
        public DateTime RequestCreationDate { get; set; }
        // Id del padre
        public int ParentId { get; set; }
    }
}
