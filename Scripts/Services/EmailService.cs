using System;
using System.Net;
using System.Net.Mail;
using UnityEngine;

public class EmailService
{
    private string fromEmail;
    private string password;

    public EmailService(string fromEmail, string password)
    {
        this.fromEmail = fromEmail;
        this.password = password;
    }

    public EmailResult SendEmail(string toEmail, string subject, string body)
    {
        try
        {
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            smtp.Send(mail);

            return new EmailResult(true, 250, "Correo enviado correctamente");
        }
        catch (SmtpException smtpEx)
        {
            Debug.Log("SMTP ERROR: " + smtpEx.Message);

            int code = (int)smtpEx.StatusCode;

            return new EmailResult(false, code, smtpEx.Message);
        }
        catch (Exception ex)
        {
            Debug.Log("GENERAL ERROR: " + ex.Message);

            return new EmailResult(false, 500, "Error interno: " + ex.Message);
        }
    }
}