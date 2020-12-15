using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using TicTacToeGame.Services.Interfaces.Helpers;

namespace TicTacToeGame.Services.Utilities
{
    public class SmtpHelper : ISmtpHelper
    {
        /// <summary>
        /// Gets or sets the SendGrid API Key.
        /// </summary>
        /// <value>
        /// The API Key.
        /// </value>
        public string ApiKey { get; set; }
        
        /// <summary>
        /// Gets or sets the email sender.
        /// </summary>
        /// <value>
        /// The email sender.
        /// </value>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the list of reply-to email.
        /// </summary>
        /// <value>
        /// The list of reply to emails.
        /// </value>
        public List<string> ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the subject of the email.
        /// </summary>
        /// <value>
        /// The subject of the email.
        /// </value>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the list of recipients.
        /// </summary>
        /// <value>
        /// A list of recipients.
        /// </value>
        public List<string> ToRecipients { get; set; }

        /// <summary>
        /// Gets or sets the list of CC recipients.
        /// </summary>
        /// <value>
        /// A list of cc recipients.
        /// </value>
        public List<string> CcRecipients { get; set; }

        /// <summary>
        /// Gets or sets the list of BCC recipients.
        /// </summary>
        /// <value>
        /// A list of BCC recipients.
        /// </value>
        public List<string> BccRecipients { get; set; }

        /// <summary>
        /// Gets or sets the email content.
        /// </summary>
        /// <value>
        /// The email content.
        /// </value>
        public string EmailBody { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets a indicator whether email content is to be rendered in HTML.
        /// </summary>
        /// <value>
        /// <c>true</c> if email content is to be rendered in HTML; otherwise, <c>false</c>.
        /// </value>
        public bool IsHtml { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Gets or sets the SMTP settings to be used for sending the email.
        /// </summary>
        /// <value>
        /// The SMTP settings.
        /// </value>
        public SmtpSettings SmtpSettings { get; set; }

        /// <summary>
        /// Sends the email
        /// </summary>
        /// <returns>
        /// Returns true if email sending is successful; otherwise, returns false
        /// </returns>
        public async Task Send()
        {
            await this.Send(null);
        }

        /// <summary>
        /// Sends email with attachment
        /// </summary>
        /// <param name="attachments">Arraylist of attachment</param>
        /// <returns>Returns true if email sending is successful; otherwise, returns false</returns>
        public async Task Send(ArrayList attachments)
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(this.Sender, "Game Master");

            if (this.ToRecipients != null && this.ToRecipients.Count > 0)
            {
                foreach (string toRecipient in this.ToRecipients)
                {
                    MailAddress toEmail = new MailAddress(toRecipient);
                    mail.To.Add(toEmail);
                }
            }

            if (this.CcRecipients != null && this.CcRecipients.Count > 0)
            {
                foreach (string ccRecipient in this.CcRecipients)
                {
                    MailAddress ccEmail = new MailAddress(ccRecipient);
                    mail.CC.Add(ccEmail);
                }
            }

            if (this.BccRecipients != null && this.BccRecipients.Count > 0)
            {
                foreach (string bccRecipient in this.BccRecipients)
                {
                    MailAddress bccEmail = new MailAddress(bccRecipient);
                    mail.Bcc.Add(bccEmail);
                }
            }

            if (this.ReplyTo != null && this.ReplyTo.Count > 0)
            {
                foreach (string replyTo in this.ReplyTo)
                {
                    MailAddress replyToEmail = new MailAddress(replyTo);
                    mail.ReplyToList.Add(replyToEmail);
                }
            }

            mail.Subject = this.Subject;
            mail.Body = this.EmailBody;
            mail.IsBodyHtml = this.IsHtml;
            if (attachments != null)
            {
                foreach (string att in attachments)
                {
                    mail.Attachments.Add(new Attachment(att));
                }
            }

            var alternameView = AlternateView.CreateAlternateViewFromString(this.EmailBody, new ContentType(MediaTypeNames.Text.Html));

            mail.AlternateViews.Add(alternameView);

            var smtpClient = new SmtpClient(this.SmtpSettings.SmtpHost);

            if (this.SmtpSettings.SmtpPort != null)
            {
                smtpClient.Port = Convert.ToInt32(this.SmtpSettings.SmtpPort);
            }

            var key = string.IsNullOrEmpty(this.SmtpSettings.SmtpPassword)
                ? ApiKey
                : this.SmtpSettings.SmtpPassword;
            smtpClient.Credentials = new NetworkCredential(this.SmtpSettings.SmtpUserName, key);

            try
            {
                // Needed to add try catch here to prevent api from giving error
                // when smtp is not supported.
                // For Azure Deployment

                await Task.Run(() => smtpClient.Send(mail));
            }
            catch(Exception ex)
            {
                Console.WriteLine("Exception message: {0}", ex.Message);
            }

            return;
        }
    }
}
