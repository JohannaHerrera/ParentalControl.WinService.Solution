using Microsoft.Win32;
using ParentalControl.WinService.Business.Enums;
using ParentalControl.WinService.Data;
using ParentalControl.WinService.Models.Device;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.ParentalControl
{
    public class ApplicationBO
    {
        /// <summary>
        /// Método para obtener las aplicaciones bloqueadas
        /// </summary>
        /// <returns>List<ApplicationModel></returns>
        public List<ApplicationModel> GetBlockedApps(int infantId, int deviceId)
        {
            string query = $"SELECT * FROM App WHERE InfantAccountId = {infantId}" +
                           $" AND DevicePCId = {deviceId}" +
                           $" AND (AppAccessPermission <> 0 OR ScheduleId IS NOT NULL)";

            List<ApplicationModel> applicationModelList = this.ObtenerListaSQL<ApplicationModel>(query).ToList();

            return applicationModelList;
        }

        /// <summary>
        /// Método para registrar una aplicación
        /// </summary>
        /// <param name="infantId">Id del Infante</param>
        /// <param name="appName">Nombre de la App</param>
        /// <returns>bool: TRUE(registro exitoso), FALSE(error al registrar)</returns>
        public bool RegisterApps(int infantId, string appName)
        {
            DeviceBO deviceBO = new DeviceBO();
            Constants constants = new Constants();
            var creationDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string deviceCode = deviceBO.GetDeviceIdentifier();
            bool execute = false;

            string query = $"SELECT * FROM DevicePC WHERE DevicePCCode = '{deviceCode}'";
            List<DeviceModel> deviceModelList = this.ObtenerListaSQL<DeviceModel>(query).ToList();

            if (deviceModelList.Count > 0)
            {
                int deviceId = deviceModelList.FirstOrDefault().DevicePCId;
                query = $"INSERT INTO App VALUES (NULL, {infantId}, NULL, {deviceId}, " +
                        $"'{appName}', {constants.Access}, {constants.Access}, '{creationDate}')";

                execute = SQLConexionDataBase.Execute(query);

                if (execute)
                {
                    query = $"INSERT INTO AppDevice VALUES (NULL, {deviceId}, " +
                            $"'{appName}', '{creationDate}')";

                    execute = SQLConexionDataBase.Execute(query);
                }
            }

            return execute;
        }

        /// <summary>
        /// Método para obtener las aplicaciones instaladas en la PC
        /// </summary>
        /// <returns>List<string></returns>
        public List<string> GetNewInstalledApps()
        {
            string displayName;
            string size;
            string displayVersion;

            List<string> gInstalledSoftware = new List<string>();

            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false))
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);

                    displayName = subkey.GetValue("DisplayName") as string;
                    size = subkey.GetValue("EstimatedSize") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (string.IsNullOrEmpty(displayName))
                        continue;
                    if (string.IsNullOrEmpty(displayName))
                        continue;

                    if (displayVersion != null)
                    {
                        displayName = Regex.Replace(displayName, displayVersion, string.Empty);
                    }

                    displayName = this.DeleteSpecialCharacters(displayName);

                    if (!(string.IsNullOrEmpty(displayName) || gInstalledSoftware.Contains(displayName)))
                    {
                        gInstalledSoftware.Add(displayName);
                    }
                }
            }

            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            {
                var key = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    size = subkey.GetValue("EstimatedSize") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (string.IsNullOrEmpty(displayName))
                        continue;
                    if (string.IsNullOrEmpty(displayName))
                        continue;

                    if (displayVersion != null)
                    {
                        displayName = Regex.Replace(displayName, displayVersion, string.Empty);
                    }

                    displayName = this.DeleteSpecialCharacters(displayName);

                    if (!(string.IsNullOrEmpty(displayName) || gInstalledSoftware.Contains(displayName)))
                    {
                        gInstalledSoftware.Add(displayName);
                    }
                }
            }

            using (var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
            {
                var key = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", false);
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    size = subkey.GetValue("EstimatedSize") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (string.IsNullOrEmpty(displayName))
                        continue;
                    if (string.IsNullOrEmpty(displayName))
                        continue;

                    if (displayVersion != null)
                    {
                        displayName = Regex.Replace(displayName, displayVersion, string.Empty);
                    }

                    displayName = this.DeleteSpecialCharacters(displayName);

                    if (!(string.IsNullOrEmpty(displayName) || gInstalledSoftware.Contains(displayName)))
                    {
                        gInstalledSoftware.Add(displayName);
                    }
                }
            }

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall", false))
            {
                foreach (String keyName in key.GetSubKeyNames())
                {
                    RegistryKey subkey = key.OpenSubKey(keyName);
                    displayName = subkey.GetValue("DisplayName") as string;
                    size = subkey.GetValue("EstimatedSize") as string;
                    displayVersion = subkey.GetValue("DisplayVersion") as string;
                    if (string.IsNullOrEmpty(displayName))
                        continue;
                    if (string.IsNullOrEmpty(displayName))
                        continue;

                    if (displayVersion != null)
                    {
                        displayName = Regex.Replace(displayName, displayVersion, string.Empty);
                    }

                    displayName = this.DeleteSpecialCharacters(displayName);

                    if (!(string.IsNullOrEmpty(displayName) || gInstalledSoftware.Contains(displayName)))
                    {
                        gInstalledSoftware.Add(displayName);
                    }
                }
            }

            return gInstalledSoftware;
        }

        /// <summary>
        /// Método para obtener las aplicaciones de una cuenta infantil
        /// </summary>
        /// <param name="infantId">Id del Infante</param>
        /// <param name="deviceId">Id del Dispositivo</param>
        /// <returns>List<ApplicationModel></returns>
        public List<ApplicationModel> GetAppsDevice(int infantId, int deviceId)
        {
            string query = $"SELECT * FROM App WHERE InfantAccountId = {infantId}" +
                           $" AND DevicePCId = {deviceId}";

            List<ApplicationModel> applicationModelList = this.ObtenerListaSQL<ApplicationModel>(query).ToList();

            return applicationModelList;
        }

        /// <summary>
        /// Método para eliminar caracteres especiales
        /// </summary>
        /// <param name="cadena">string</param>
        /// <returns>IList<TModel></returns>
        private string DeleteSpecialCharacters(string cadena)
        {
            cadena = Regex.Replace(cadena, "current user", string.Empty);
            cadena = Regex.Replace(cadena, "user", string.Empty);
            cadena = Regex.Replace(cadena, "-x86", string.Empty);
            cadena = Regex.Replace(cadena, "x86_64", string.Empty);
            cadena = Regex.Replace(cadena, "x86", string.Empty);
            cadena = Regex.Replace(cadena, "x64", string.Empty);
            cadena = Regex.Replace(cadena, "-en-us", string.Empty);
            cadena = Regex.Replace(cadena, "es-ES", string.Empty);
            cadena = Regex.Replace(cadena, "64-bit", string.Empty);
            cadena = Regex.Replace(cadena, "[()]", string.Empty);
            cadena = cadena.TrimEnd(' ');
            if ((cadena.ToLower().Contains("microsoft")
                || cadena.ToLower().Contains("sql")
                || cadena.ToLower().Contains("python")
                || cadena.ToLower().Contains("visual studio")
                || cadena.ToLower().Contains("java")
                || cadena.ToLower().Contains("office")
                || cadena.ToLower().Contains("c++")
                || cadena.ToLower().Contains("php")
                || cadena.ToLower().Contains("windows")
                || cadena.ToLower().Contains("vs")
                || cadena.ToLower().Contains("icecap")
                || cadena.ToLower().Contains(".net")
                || cadena.ToLower().Contains("intellisense")
                || cadena.ToLower().Contains("sdk")
                || cadena.ToLower().Contains("crt")
                || cadena.ToLower().Contains("vcpp")
                || cadena.ToLower().Contains("intelr")
                || (cadena.ToLower().Contains("iis") && cadena.ToLower().Contains("express"))))
            {
                cadena = string.Empty;
            }

            return cadena;
        }

        /// <summary>
        /// Método para eliminar las apps de una cuenta infantil
        /// </summary>
        /// <param name="infantId">Id del Infante</param>
        /// <returns>bool: TRUE(registro exitoso), FALSE(error al registrar)</returns>
        public bool DeleteApps(int infantId)
        {
            DeviceBO deviceBO = new DeviceBO();
            string deviceCode = deviceBO.GetDeviceIdentifier();
            bool execute = false;

            string query = $"SELECT * FROM DevicePC WHERE DevicePCCode = '{deviceCode}'";
            List<DeviceModel> deviceModelList = this.ObtenerListaSQL<DeviceModel>(query).ToList();

            if (deviceModelList.Count > 0)
            {
                int deviceId = deviceModelList.FirstOrDefault().DevicePCId;
                query = $"DELETE FROM App WHERE DevicePCId = {deviceId}" +
                        $" AND InfantAccountId = {infantId}";

                execute = SQLConexionDataBase.Execute(query);
            }

            return execute;
        }

        /// <summary>
        /// Método para convertir una lista DataTable a un TModel (Modelo genérico)
        /// </summary>
        /// <param name="deviceModel">contiene la data del dispositivo PC</param>
        /// <returns>IList<TModel></returns>
        private IList<TModel> ObtenerListaSQL<TModel>(string query)
        {
            try
            {
                DataTable dataTableInformacion = SQLConexionDataBase.Query(query);
                var listaResultante = dataTableInformacion.MapDataTableToList<TModel>();

                return listaResultante;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
