using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration
{
    [Serializable]
    public class GeneralConfiguration
    {

        [XmlArray("Processors")]
        [XmlArrayItem("ProcessorConfiguration")]
        public List<ProcessorConfiguration> ProcessorConfiguration = new List<ProcessorConfiguration>();


        public void AddConfigurarion(ProcessorConfiguration processorConfiguration)
        {
            ProcessorConfiguration.Add(processorConfiguration);
        }

    }
}
