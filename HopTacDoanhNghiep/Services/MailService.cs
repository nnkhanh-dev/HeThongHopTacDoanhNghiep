using HopTacDoanhNghiep.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HopTacDoanhNghiep.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        //injection MailSetting vào lớp này để dùng
        public MailService(IOptions<MailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }
        // xử lý gửi mail
        public bool SendMail(MailData mailData)
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                var emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                var emailTo = new MailboxAddress(mailData.ReceiverName, mailData.ReceiverEmail);

                emailMessage.From.Add(emailFrom);
                emailMessage.To.Add(emailTo);
                emailMessage.Subject = mailData.Title;

                var emailBodyBuilder = new BodyBuilder();

                // Gán nội dung HTML vào HtmlBody
                emailBodyBuilder.HtmlBody = @"
                    <!DOCTYPE html>
                    <html lang='en'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>THÔNG BÁO ĐĂNG KÝ DOANH NGHIỆP</title>
                        <style>
                            .container
                            {
                                        width: 100%;
                                        padding: 64px 0;
                                        background-color: #e1eef7;
                                        display: flex;
                                        justify-content: center;
                                        align-items: center;
                             }
                            .card {
                                border: 1px solid #e6e3e3;
                                width: 500px;
                                margin: auto;
                                font-family: Arial, sans-serif;
                                border-radius: 6px;
                                overflow: hidden;
                                box-shadow: 0 4px 12px 0 rgba(0, 0, 0, 0.1);
                            }
                            .card-header {
                                padding: 16px 32px;
                                text-align: center;
                                font-size: 20px;
                                font-weight: bold;
                                border-bottom: 1px solid #e6e3e3;
                                background-color: #fcfbfb;
                            }
                            .card-body {
                                padding: 32px 32px;
                                font-size: 18px;
                                background-color: white;
                                line-height: 1.5;
                            }
                            .logo {
                                max-width: 100px;
                                width: 100px;
                                aspect-ratio: 2/1;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='card'>
                                <div class='card-header'>
                                   <img src=""https://htdn.ute.id.vn/image/layout/logo_htdn_basic.png"" class=""logo""/>
                                </div>
                                <div class='card-body'>"
                                    + mailData.Body +
                                @"</div>
                            </div>
                        </div>
                    </body>
                    </html>";

                // Không dùng TextBody nếu đã dùng HtmlBody, tránh email xuống dòng không đẹp
                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                using (var mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

            return true;
        }

    }
}
