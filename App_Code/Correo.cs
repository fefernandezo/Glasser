using System.Net;
using System.Net.Mail;
using System.Text;

namespace ProRepo
{
    public class Correo
    {
        public void Enviar(string body, string Asunto, string[] Para, string NameFrom, string prioridad)
        {
            MailAddress from = new MailAddress("ecommerce@phglass.cl", NameFrom);
            MailMessage msj = new MailMessage();
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;


            for (int i = 0; i < Para.Length; i++)
            {
                string valor = Para[i];
                msj.To.Add(new MailAddress(valor));
            }

            AlternateView htmlview = AlternateView.CreateAlternateViewFromString(body, Encoding.UTF8, "text/html");

            msj.Subject = Asunto;
            msj.IsBodyHtml = true;
            msj.Body = body;
            msj.From = from;
            msj.AlternateViews.Add(htmlview);
            if (prioridad == "Alta")
            {
                msj.Priority = MailPriority.High;
            }
            else
            {
                msj.Priority = MailPriority.Normal;
            }



            client.Credentials = new NetworkCredential()
            {
                UserName = "ecommerce@phglass.cl",
                Password = "TwH5oU431Aaj"
            };





            client.Send(msj);

        }


    }
}