using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Models.InfantAccount
{
    /// <summary>
    /// Modelo de la tabla InfantAccount
    /// </summary>
    public class InfantAccountModel
    {
        // Id del Infante
        public int InfantAccountId { get; set; }
        // Nombre del Infante
        public string InfantName { get; set; }
        // Género
        public string InfantGender { get; set; }
        // Fecha de Creación
        public DateTime InfantCreationDate { get; set; }
        // Id del Padre
        public int ParentId { get; set; }
    }
}
