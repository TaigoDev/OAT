using System.Net;
using System.Runtime.InteropServices;
using MailKit.Net.Proxy;
using MailKit.Net.Smtp;
using MimeKit;
using NLog;

namespace OMAVIAT.Services;

public class MailService
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task<bool> SendMailAsync(string fullName, string telephone, string email, string school, string level)
	{
		try
		{
			var message = new MimeMessage();
			message.From.Add(new MailboxAddress("Система уведомления ДОД", Configurator.MailConfig.EmailUser));
			message.To.Add(new MailboxAddress("Получатель писем ДОД", Configurator.MailConfig.SendTo));
			message.Subject = "Новое уведомление от www.oat.ru";
			message.Body = new TextPart ("plain") {
				Text = $"""
				        Новая запись на ДОД:
				         ФИО: {fullName}
				         Телефон: {telephone}
				         Почта: {email}
				         ОУ: {school}
				         Класс: {level}
				        """
			};
			using var client = new SmtpClient();
			if (Configurator.MailConfig.EnableProxy)
				client.ProxyClient = new HttpProxyClient ("10.0.55.52", 3128);
			
			await client.ConnectAsync (Configurator.MailConfig.SmtpServer, Configurator.MailConfig.SmtpPort, Configurator.MailConfig.EnableSsl);
			await client.AuthenticateAsync(Configurator.MailConfig.EmailUser, Configurator.MailConfig.EmailPassword);
			await client.SendAsync(message);
			await client.DisconnectAsync(true);
			return true;
		}
		catch (Exception e)
		{
			Logger.Error(e);
			return false;
		}

	}
}