using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.WinServiceLib.Server.WinServices.ParentalControl.Engines.Configuration
{
    internal class ServiceSettingNames
    {

        #region public static readonly

        public static readonly string ModeExecution = "ModeExecution";
        public static readonly string ScheduledTime = "ScheduledTime";
        public static readonly string TimerInterval = "timerinterval";

        public static readonly string ConfigurationDirectoryPath = "ConfigurationDirectoryPath";
        public static readonly string ConfigurationFilePath = "ConfigurationFilePath";

        public static readonly string MailMessageFrom = "Setting.MailMessage.From";
        public static readonly string MailMessageTo = "Setting.MailMessage.To";
        public static readonly string MailMessageSubject = "Setting.MailMessage.Subject";
        public static readonly string MailMessageSubjectError = "Setting.MailMessage.SubjectError";

        #endregion

    }
}
