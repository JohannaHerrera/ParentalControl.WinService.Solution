using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ParentalControl.WinService.WinFormDemo.ConfiguracionXML
{
    [Serializable]
    [XmlRoot("GeneralConfiguration")]
    public class ConfigurationDemo
    {
        public ConfigurationDemo()
        { }


        [XmlArray("Processors")]
        [XmlArrayItem("ProcessorConfiguration")]
        public List<ProcessorConfigurationDemo> ProcessorConfigurationDemo = new List<ProcessorConfigurationDemo>();


        public void AddConfigurarion(ProcessorConfigurationDemo processorConfigurationDemo)
        {
            ProcessorConfigurationDemo.Add(processorConfigurationDemo);
        }
    }
}
