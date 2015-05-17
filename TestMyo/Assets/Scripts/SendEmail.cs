using UnityEngine;
using System.Collections;
using System.Net.Mail;

public class SendEmail : MonoBehaviour {

	public bool go = false;

	// Use this for initialization
	void Start () {
	
	}
	
//	void DoTheThing() {
//		SmtpMail oMail = new SmtpMail("TryIt");
//		SmtpClient oSmtp = new SmtpClient();
//		
//		// Your gmail email address
//		oMail.From = "gmailid@gmail.com";
//		
//		// Set recipient email address
//		oMail.To = "support@emailarchitect.net";
//		
//		// Set email subject
//		oMail.Subject = "test email from gmail account";
//		
//		// Set email body
//		oMail.TextBody = "this is a test email sent from c# project with gmail.";
//		
//		// Gmail SMTP server address
//		SmtpServer oServer = new SmtpServer("smtp.gmail.com");
//		
//		// Set 465 port
//		oServer.Port = 465;
//		
//		// detect SSL/TLS automatically
//		oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;
//		
//		// Gmail user authentication
//		// For example: your email is "gmailid@gmail.com", then the user should be the same
//		oServer.User = "gmailid@gmail.com";
//		oServer.Password = "yourpassword";
//		
//		try
//		{
//			Console.WriteLine("start to send email over SSL ...");
//			oSmtp.SendMail(oServer, oMail);
//			Console.WriteLine("email was sent successfully!");
//		}
//		catch (Exception ep)
//		{
//			Console.WriteLine("failed to send email with the following error:");
//			Console.WriteLine(ep.Message);
//		}
//	}
	
//	public int SendTheEmail(string to, string subject, string body)
//	{
//		//http://www.c-sharpcorner.com/Blogs/14416/send-mail-using-gmail-smtp-in-C-Sharp.aspx
//		string sToEmail;
//		bool fSSL = true;
////		try
////		{
//			//Creating Message object
//			string from = "drunkpaintinggame@gmail.com";
//			
//			var message = new MailMessage();
//			message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserver", "smtp.gmail.com");
//			message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusing", "2");
//			message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", "465");
//			if (fSSL)
//				message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");
//			message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
//		message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", from);
//			message.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "c2%Y5HRm6O4e");
//			
//			//Preparing the message object....
//			
//		message.From = from;
//			message.To = to;
//			message.Subject = subject;
//		message.BodyFormat = System.Net.Mail.MailFormat.Html;
//			string html = @"<html><head><link href='CSS/WebCss/WebCss.css' rel='stylesheet' type='text/css' />
//         </head><body >";
//			html += "<h1>Welcome to Avinash Aher World</h1><br>"+body;
//			html += "</body></html>";
//			message.Body = html;
//		System.Net.Mail.SmtpMail.SmtpServer = "smtp.gmail.com";
//		System.Net.Mail.SmtpMail.Send(message);
//			return 1;
////		}
////		catch (Exception)
////		{
////			return 0;
////		}
//	} 

	void SendEmail3() {
		Debug.Log ("Building email");
		// http://csharp.net-informations.com/communications/csharp-smtp-mail.htm
		MailMessage mail = new MailMessage();
		SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
		
		mail.From = new MailAddress("drunkpaintinggame@gmail.com");
		mail.To.Add("greg@ncroses.com");
		mail.Subject = "Test Mail";
		mail.Body = "This is for testing SMTP mail from GMAIL";
		
		SmtpServer.Port = 587;
		var credentials = new System.Net.NetworkCredential("drunkpaintinggame@gmail.com", "c2%Y5HRm6O4e");
		SmtpServer.Credentials = (System.Net.ICredentialsByHost)credentials;
		SmtpServer.EnableSsl = true;
		
		Debug.Log ("Now sending...");
		SmtpServer.Send(mail);
		Debug.Log("Sent!");
		
		// TODO: catch exception
	}
	
	// Update is called once per frame
	void Update () {
		if (go) {
//			DoTheThing();
//			SendTheEmail("greg@ncroses.com", "Hey buddy", "Looking good");
			SendEmail3();
			go = false;
		}
	}
}
