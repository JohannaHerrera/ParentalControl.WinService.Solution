using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ParentalControl.WinService.Business.Mail
{
    public class MailNotification
    {

        #region Private Functions

        /// <summary>
        /// Configura el servidor smtp
        /// </summary>
        /// <returns></returns>
        private SmtpClient PrepareSmptClient()
        {
            // Configura el servidor smtp
            SmtpClient SmtpServer = new SmtpClient();

            SmtpServer.Port = int.Parse(ConfigurationManager.AppSettings["Setting.Smtp.Port"].ToString());
            SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
            SmtpServer.UseDefaultCredentials = false;
            SmtpServer.Host = ConfigurationManager.AppSettings["Setting.Smtp.Host"].ToString();
            SmtpServer.EnableSsl = false;

            return SmtpServer;
        }

        /// <summary>
        /// Prepara un mailMessage
        /// </summary>
        /// <param name="receipmentMail"></param>
        /// <param name="bcc"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private MailMessage PrepareMailMessage(List<string> receipmentMail, List<string> bcc, string body, string subject)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["Setting.MailMessage.From"].ToString());

            mailMessage.Subject = subject;
            mailMessage.Body = body;

            if (receipmentMail != null)
            {
                foreach (var item in receipmentMail)
                {
                    mailMessage.To.Add(item);
                }
            }

            if (bcc != null)
            {
                foreach (var item in bcc)
                {
                    mailMessage.Bcc.Add(item);
                }
            }

            return mailMessage;
        }



        #endregion Private Functions

        #region Public Functions

        /// <summary>
        /// Envia un Correo
        /// </summary>
        /// <param name="receipmentMail"></param>
        /// <param name="bcc"></param>
        /// <param name="body"></param>
        /// <param name="subject"></param>
        /// <returns></returns>
        public bool SendSingleMail(List<string> receipmentMail, List<string> bcc, string body, string subject)
        {
            bool seEnvioMail = false;
            var smtpServer = PrepareSmptClient();
            var mailMessage = PrepareMailMessage(receipmentMail, bcc, body, subject);

            try
            {
                smtpServer.Send(mailMessage);
                seEnvioMail = true;
            }
            catch (Exception ex)
            {
                seEnvioMail = false;
            }
            finally
            {
                if (smtpServer != null)
                {
                    smtpServer.Dispose();
                }
            }

            //if (!seEnvioMail)
            //{
            //    log.InfoFormat("No se pudo enviar el correo de notificación: {0}", body.Truncate(8000));
            //}


            return seEnvioMail;
        }

        public bool SendSingleMail_AvisoIntegracion(string body, string subject)
        {

            List<string> receipmentMail = new List<string>();

            var mailsTo = ConfigurationManager.AppSettings["Setting.MailMessage.To"].ToString();

            foreach (var item in mailsTo.Split(';'))
            {
                if (!string.IsNullOrEmpty(item))
                {
                    receipmentMail.Add(item);
                }
            }



            return this.SendSingleMail(receipmentMail, new List<string>(), body, subject);

        }

        #endregion Public Functions


    }
}
