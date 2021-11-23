using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration
{
    internal class ServiceConfiguration
    {
        #region public static readonly

        /// <summary>
        /// Nombre del Procesador. Es el mismo nombre que tiene en el archivo de XML de configuración
        /// </summary>
        public static readonly string AppLock = "AppLock";
        public static readonly string WebLock = "WebLock";
        public static readonly string DeviceLock = "DeviceLock";

        #endregion

        #region public properties

        public CultureInfo ServiceCultureInfo { get; private set; }
        public string CrmServiceUrl { get; private set; }
        public string CrmServiceUserName { get; private set; }
        public string CrmServicePassword { get; private set; }
        public string CrmServiceDomainName { get; private set; }
        public IDictionary<string, bool> ProcessorIsEnabledByProcessorName { get; private set; }
        #endregion


        #region constructors
        public ServiceConfiguration(
            string configurationDirectoryPath,
            string configurationFilePath)
        {
            try
            {
                // 1.- Obtengo la información de la configuración
                var generalConfiguration = ReadGeneralConfigurations(configurationFilePath);

                if (generalConfiguration != null && generalConfiguration.ProcessorConfiguration.Count > 0)
                {

                    // Obtengo el estado de los procesadores
                    IDictionary<string, bool> processorIsEnabledByProcessorName = new Dictionary<string, bool>();
                    foreach (var item in generalConfiguration.ProcessorConfiguration)
                    {
                        processorIsEnabledByProcessorName.Add(item.ProcessorName, item.IsEnabled);
                    }

                    this.ProcessorIsEnabledByProcessorName = processorIsEnabledByProcessorName;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(string.Format(
                    "Could not parse service configuration file (configurationDirectoryPath={0}, configurationFilePath={1}).",
                    configurationDirectoryPath,
                    configurationFilePath),
                    exception);
            }
        }
        #endregion


        #region private functions

        private GeneralConfiguration ReadGeneralConfigurations(string configurationFilePath)
        {
            try
            {
                var contenidoArchivo = File.ReadAllText(configurationFilePath);

                var generalConfiguration = new GeneralConfiguration();
                using (TextReader reader = new StringReader(contenidoArchivo))
                {
                    var serializer = new XmlSerializer(generalConfiguration.GetType());
                    generalConfiguration = (GeneralConfiguration)serializer.Deserialize(reader);
                }

                return generalConfiguration;
            }
            catch (Exception e)
            {
                throw new Exception("Error al cargar las configuraciones de los Procesadores.: " + e.Message);
            }

        }

        #endregion private functions

    }
}
