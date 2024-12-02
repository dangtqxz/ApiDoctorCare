using System.Net.Mail;
using System.Net;
using System.Reflection;
using System.Text;

namespace ApiDoctorCare.Services
{
    public class EmailServices
    {
        public class EmailConfigs
        {
            public string Email { get; set; } = @"bookingcare2003@gmail.com";
            public string Password { get; set; } = @"mvac tdfi onml rjoc"; // Send Mail
        }

        public static void Send(string ngayHen, string email, string gioHen, string tenBacSi)
        {
            try
            {
                var ngayHD = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");

                EmailConfigs _email = new EmailConfigs();
                var StringBody = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"EmailTemplate\Goc.html");
                var bodyEmail = File.ReadAllText(StringBody);
                var _passMail = _email.Password;
                var _port = 587;
                var _smtpAddress = "smtp.gmail.com";
                var _enableSSL = true;
                var from = _email.Email;

                var body = bodyEmail
                .Replace("@ngayHen", ngayHen)
                .Replace("@ngayHD", ngayHD)
                .Replace("@gioHen", gioHen)
                .Replace("@tenBacSi", tenBacSi)
                ;

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(from);
                mail.To.Add(email);
                mail.Subject = $"Thông tin đặt lịch";
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;


                SmtpClient smtp = new SmtpClient(_smtpAddress, _port);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = _enableSSL;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, _passMail);

                smtp.Timeout = 60000;
                smtp.Send(mail);
            }
            catch (Exception ex) { }

        }

        public static void SendAccout(string tenDangNhap, string matKhau, string email)
        {
            try
            {

                EmailConfigs _email = new EmailConfigs();
                var StringBody = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), @"EmailTemplate\QuenMK.html");
                var bodyEmail = File.ReadAllText(StringBody);
                var _passMail = _email.Password;
                var _port = 587;
                var _smtpAddress = "smtp.gmail.com";
                var _enableSSL = true;
                var from = _email.Email;

                var body = bodyEmail
                .Replace("@tenDangNhap", tenDangNhap)
                .Replace("@matKhau", matKhau)
                ;

                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(from);
                mail.To.Add(email);
                mail.Subject = $"Thông tin tài khoản";
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;


                SmtpClient smtp = new SmtpClient(_smtpAddress, _port);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.EnableSsl = _enableSSL;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(from, _passMail);

                smtp.Timeout = 60000;
                smtp.Send(mail);
            }
            catch (Exception ex) { }

        }
    }
}
