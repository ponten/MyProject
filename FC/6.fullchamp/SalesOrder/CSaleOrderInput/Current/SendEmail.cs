using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;  
using System.Net.Mime;
using System.Data;
using System.Web;
using System.Net.Mail;

namespace SajetClass
{
    class SendEmail
    {
        ////发送邮件的类  
        private MailMessage mailMessage;

        private SmtpClient smtpClient;


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

            mailMessage.Subject = Title;
            mailMessage.Body = Body;
            mailMessage.IsBodyHtml = true;
            mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
            mailMessage.Priority = System.Net.Mail.MailPriority.Normal;

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
                throw new Exception("在添加附件有錯誤:" + ex);
            }
        }

        public void Send()
        {
           try
            {
                if (mailMessage != null)
                {

                    //设置发件人身份的票据  
                    //(mailMessage.From.Address, password);

                    string sSQL = "Select r.from_emp_name, r.from_emp_email, r.from_emp_email_pwd, r.server_name, r.server_port from sajet.sys_sendmail_setting r where from_emp_name is not null ";
                    DataSet dsTemp = ClientUtils.ExecuteSQL(sSQL);
                    if (dsTemp.Tables[0].Rows.Count == 0)
                        return;

                    DataRow dr = dsTemp.Tables[0].Rows[0];

                    List<string> lsMsg = new List<string>();
                    foreach (DataColumn c in dr.Table.Columns)
                        if (dr[c.ColumnName] == DBNull.Value)
                            lsMsg.Add(c.ColumnName);


                    string From = dr["from_emp_name"].ToString();
                    string account = dr["from_emp_email"].ToString();
                    string Password = dr["from_emp_email_pwd"].ToString();
                    string host = dr["server_name"].ToString();
                    int post = Convert.ToInt32(dr["server_port"]);

                    smtpClient = new SmtpClient();
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    smtpClient.Credentials = new System.Net.NetworkCredential(account, Password);
                    smtpClient.EnableSsl = true;
                    smtpClient.Host = host;
                    smtpClient.Port = post;

                    mailMessage.From = new System.Net.Mail.MailAddress(From);
                    smtpClient.Send(mailMessage);

                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                throw new Exception("發送mail錯誤:" + ex);
            }

        }
    }
}


