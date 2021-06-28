using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace CatCatcher
{
    public static class Emails
    {
        public static string email;
        public static int frequency;
        public static void SendEmail(string email, Bitmap img)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("tanjhosan58@gmail.com");
                message.To.Add(new MailAddress(email));
                message.Subject = "!neko";
                message.IsBodyHtml = false; //to make message body as html  
                message.Body = "Un gatito :3";

                //MemoryStream memStream = new MemoryStream(); //new one

                //img.Save(memStream, ImageFormat.Jpeg);

                Bitmap b = new Bitmap(img);
                ImageConverter ic = new ImageConverter();
                byte[] ba = (byte[])ic.ConvertTo(b, typeof(byte[]));
                MemoryStream logo = new MemoryStream(ba);

                ContentType contentType = new ContentType();
                contentType.MediaType = MediaTypeNames.Image.Jpeg;
                contentType.Name = "gatito.png";
                message.Attachments.Add(new Attachment(logo, contentType));





                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("tanjhosan58@gmail.com", "ueba2013");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }
    }
}
