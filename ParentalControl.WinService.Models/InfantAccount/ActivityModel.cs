using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.InfantAccount
{
    /// <summary>
    /// Modelo de la tabla Activity
    /// </summary>
    public class ActivityModel
    {
        // Id de la actividad
        public int ActivityId { get; set; }
        // Id del Infante
        public int InfantAccountId { get; set; }
        // Recurso
        public string ActivityObject { get; set; }
        // Descripción
        public string ActivityDescription { get; set; }
        // Fecha de creación
        public DateTime ActivityCreationDate { get; set; }
        // Número de veces de acceso
        public int ActivityTimesAccess { get; set; }
    }
}
