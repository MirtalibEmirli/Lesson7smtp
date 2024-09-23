using Mailing.Models;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Windows;
 

namespace Mailing;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{

    string senderEmail = "mirtalibemirli498@gmail.com";
    string senderPassword = "zbap yyys xtcs thpb";
    string? recipientEmail;
    string smtpServer = "smtp.gmail.com";
    int smptPort = 587;
    public ObservableCollection<EmailDto> Emails { get; set; } = new ObservableCollection<EmailDto>();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    private async void Delete_Click(object sender, RoutedEventArgs e)
    {



    }

    private void Starred_Click(object sender, RoutedEventArgs e)
    {

    }

    private void Sent_Click(object sender, RoutedEventArgs e)
    {

    }

    private void InBox_Click(object sender, RoutedEventArgs e)
    {

         

    }

    private async void Send_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string? recipientEmail = mail.Text;
            using var client = new SmtpClient(smtpServer, smptPort);
            client.UseDefaultCredentials = false;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(senderEmail, senderPassword);
            var mailMessage = new MailMessage()
            {
                From = new MailAddress(senderEmail, "A man from anywhere"),
                Subject = "Just late",
                Body = message.Text
            };
            mailMessage.To.Add(recipientEmail);
            //  mailMessage.CC.Add("miri976y@gmail.com");

            await client.SendMailAsync(mailMessage);
            
            MessageBox.Show("Sent succes");
        }
        catch (Exception ex)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(ex.Message);
            });
        }


    }
}