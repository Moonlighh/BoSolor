using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace BODEGA_SOLORZANO.Service
{
    public class Recursos
    {
        // Generamos una clave automatica que enviaremos al usuario - no se vuelve a repetir
        public static string GenerarClave()
        {
            string clave = Guid.NewGuid().ToString("N").Substring(0, 6);//Retorna un codigo unico-solo caracteres alfanumericos-longitud de la clave
            return clave;
        }

        // Enviar correo para cualquier metodo
        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool enviado;
            try
            {
                MailMessage mail = new();
                mail.To.Add(correo);//Para quien
                mail.From = new MailAddress("rwdarlin@gmail.com");//De quien
                mail.Subject = asunto;//Asunto
                mail.Body = mensaje;//Mensaje del cuerpo
                mail.IsBodyHtml = true;//Enviamos el correo en formato html

                var smtp = new SmtpClient()//Se encargara de hacer la operacion para enviar el correo
                {
                    Credentials = new NetworkCredential("rwdarlin@gmail.com", "zuhlnwsmrckrzbjz"),
                    Host = "smtp.gmail.com", //Server que usa gmail para enviar los correos
                    Port = 587,//Puerto que usa para enviar los correos
                    EnableSsl = true//Habilitamos el certificado de seguridad
                };

                smtp.Send(mail);
                enviado = true;
            }
            catch
            {
                throw new Exception("El correo no existe o la conexión fue rechazada");
            }

            return enviado;
        }
        public static string GetSHA256(string cadena)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(cadena);
            byte[] hash = SHA256.HashData(bytes);
            StringBuilder builder = new();

            for (int i = 0; i < hash.Length; i++)
            {
                builder.Append(hash[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
