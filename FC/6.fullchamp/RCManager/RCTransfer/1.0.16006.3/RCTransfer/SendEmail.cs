using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;  
using System.Net.Mail;  
using System.Net.Mime;
using System.Data;


namespace SajetClass
{
    class SendEmail
    {
        //发送邮件的类  
        private MailMessage mailMessage;

        private SmtpClient smtpClient;

        private string Password;
        //private string password;//发件人密码  

        /**/
        /// <summary>  

        /// 处审核后类的实例  

        /// </summary>  

        /// <param name="To">收件人地址</param>  

        /// <param name="From">发件人地址</param>  

        /// <param name="Body">邮件正文</param>  

        /// <param name="Title">邮件的主题</param>  

        /// <param name="Password">发件人密码</param>  

        public SendEmail(string To, string Body, string Title)
        {
            string[] MailList = To.Split(';');

            mailMessage = new MailMessage();

            foreach (var item in MailList)
            {
                if (item != string.Empty)
                {
                    mailMessage.To.Add(item);
                }
            }

            //mailMessage.From = new System.Net.Mail.MailAddress(From);

            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;
            mailMessage.IsBodyHtml = true;
            mailMessage.Subject = Title;
            mailMessage.Body = Body;

            //this.password = Password;
        }

        public void Attachments(string Path)
        {
            try
            {
                string[] path = Path.Split(',');

                Attachment data;

                ContentDisposition disposition;

                for (int i = 0; i < path.Length; i++)
                {
                    data = new Attachment(path[i], MediaTypeNames.Application.Octet);

                    //实例化附件  
                    disposition = data.ContentDisposition;

                    //添加到附件中
                    mailMessage.Attachments.Add(data);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("附加檔案失敗:" + ex);
            }
        }

        /**/
        /// <summary>  

        /// 发送邮件  

        /// </summary>  

        public void Send()
        {
            try
            {
                if (mailMessage != null)
                {
                    smtpClient = new SmtpClient();

                    //设置发件人身份的票据  
                    //(mailMessage.From.Address, password);

                    string sSQL = "Select * from sajet.sys_sendmail_setting";
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);

                    if (dsTemp.Tables[0].Rows.Count == 0)
                        return;

                    string From = dsTemp.Tables[0].Rows[0]["from_emp_email"].ToString();
                    mailMessage.From = new System.Net.Mail.MailAddress(From);

                    smtpClient.Host = dsTemp.Tables[0].Rows[0]["server_name"].ToString();
                    Password = dsTemp.Tables[0].Rows[0]["from_emp_email_pwd"].ToString();

                    //smtpClient.Host = "smtp." + mailMessage.From.Host;

                    // 2016.5.3 By Jason
                    smtpClient.Port = Convert.ToInt32(dsTemp.Tables[0].Rows[0]["server_port"].ToString());
                    smtpClient.EnableSsl = true;
                    smtpClient.UseDefaultCredentials = false;
                    // 2016.5.3 End

                    smtpClient.Credentials = new System.Net.NetworkCredential(mailMessage.From.Address, Password);
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;

                    smtpClient.Send(mailMessage);
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw new Exception("郵件發送失敗:" + ex);
            }
        }
    }
}


