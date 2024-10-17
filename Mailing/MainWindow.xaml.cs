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

public partial class MainWindow : Window
{

    string senderEmail = "mirtalibemirli498@gmail.com";
    string senderPassword = "zbap yyys xtcs thpb";
    string? recipientEmail;
    string smtpServer = "smtp.gmail.com";
    int smptPort = 587;
    public ObservableCollection<EmailDto> Emails { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        Emails = new();
    }

    private async void Starred_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (var imap = new ImapClient())
            {
                await imap.ConnectAsync("imap.gmail.com", 993, true);
                await imap.AuthenticateAsync(senderEmail, senderPassword);
                var starred = await imap.GetFolderAsync("[Gmail]/Starred");
                if (starred != null)
                {
                    await starred.OpenAsync(FolderAccess.ReadWrite);
                    var mails = await starred.SearchAsync(SearchQuery.All);
                    if (Emails == null)
                    {
                        Emails = new ObservableCollection<EmailDto>();
                    }
                    Emails.Clear();
                    if (mails.Count == 0)
                    {
                        MessageBox.Show("No emails found in Starred.");
                        return;
                    }
                    foreach (var id in mails)
                    {

                        var msg = await starred.GetMessageAsync(id);
                        if (msg != null)
                        {
                            var email = new EmailDto
                            {
                                subject = msg.Subject != null ? msg.Subject.ToString() : "No Subject",
                                message = msg.TextBody != null ? msg.TextBody.ToString() : "No Subject",
                            };
                            Emails.Add(email);
                        }
                        else
                        {
                            MessageBox.Show("Message is null");
                        }
                    }


                }
                else
                {
                    MessageBox.Show("The Starred folder could not be found.");
                }
            }

        }
        catch (Exception ex)
        {

            MessageBox.Show(ex.Message);
        }
    }

    private async void Delete_Click(object sender, RoutedEventArgs e)
    {

        try
        {
            using (var imap = new ImapClient())
            {
                await imap.ConnectAsync("imap.gmail.com", 993, true);
                await imap.AuthenticateAsync(senderEmail, senderPassword);
                var deletedFolder = await imap.GetFolderAsync("[Gmail]/Trash");
                if (deletedFolder != null)
                {
                    await deletedFolder.OpenAsync(FolderAccess.ReadWrite);
                    var mails = await deletedFolder.SearchAsync(SearchQuery.All);
                    if (Emails == null)
                    {
                        Emails = new ObservableCollection<EmailDto>();
                    }
                    Emails.Clear();
                    if (mails.Count == 0)
                    {
                        MessageBox.Show("No emails found in Trash.");
                        return;
                    }



                    foreach (var id in mails)
                    {

                        var msg = await deletedFolder.GetMessageAsync(id);
                        if (msg != null)
                        {
                            var email = new EmailDto
                            {
                                subject = msg.Subject != null ? msg.Subject.ToString() : "No Subject",
                                message = msg.TextBody != null ? msg.TextBody.ToString() : "No Subject",
                            };
                            Emails.Add(email);
                        }
                        else
                        {
                            MessageBox.Show("Message is null");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("The Trash folder could not be found.");
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }

    }





    private async void Sent_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (var imap = new ImapClient())
            {
                await imap.ConnectAsync("imap.gmail.com", 993, true);
                await imap.AuthenticateAsync(senderEmail, senderPassword);
                var sentFolder = await imap.GetFolderAsync("[Gmail]/Sent Mail");

                await sentFolder.OpenAsync(FolderAccess.ReadOnly);
                var sentMails = await sentFolder.SearchAsync(SearchQuery.All);
                Emails.Clear();
                if (sentMails.Count == 0)
                {
                    MessageBox.Show("No emails found in Trash.");
                    return;
                }
                foreach (var id in sentMails)
                {
                    var msg = await sentFolder.GetMessageAsync(id);
                    var email = new EmailDto
                    {
                        subject = msg != null ? msg.Subject.ToString() : "No Subject",
                        message = msg != null ? msg.TextBody.ToString() : "No Subject",
                    };
                    Emails.Add(email);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }

    }


    private async void inBox_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            using (var imap = new ImapClient())
            {
                await imap.ConnectAsync("imap.gmail.com", 993, true);
                await imap.AuthenticateAsync(senderEmail, senderPassword);
                var inBox = imap.Inbox;
                if (InBox == null)
                {
                    throw new Exception("Inbox is null");

                }
                await inBox.OpenAsync(FolderAccess.ReadOnly);
                var inMails = await inBox.SearchAsync(SearchQuery.All);
                Emails.Clear();
                if (inMails.Count == 0)
                {
                    MessageBox.Show("No emails found in inbox.");
                    return;
                }
                foreach (var item in inMails)
                {
                    var msg = await inBox.GetMessageAsync(item);

                    if (msg == null)
                    {
                        throw new NullReferenceException("The message object (msg) is null");
                    }
                    var email = new EmailDto
                    {
                        subject = msg != null ? msg.Subject.ToString() : "(No Subject)",
                        message = msg != null && msg.TextBody != null ? msg.TextBody.ToString() : "(No Message)"
                    };


                    Emails.Add(email);
                }

            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error: {ex.Message}");
        }
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
                From = new MailAddress(senderEmail, senderer.Text != null ? senderer.Text : "Nobody"),
                Subject = subject.Text != null ? subject.Text : "Just mail"
               ,
                Body = message.Text
            };
            mailMessage.To.Add(recipientEmail);
            //  mailMessage.CC.Add("miri976y@gmail.com");

            await client.SendMailAsync(mailMessage);

            MessageBox.Show("Sent succes");
            senderer.Text = string.Empty;
            message.Text = string.Empty;
            subject.Text = string.Empty;
            mail.Text = string.Empty;
        }
        catch (Exception ex)
        {

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(ex.Message);
            });
        }


    }



    private async void DeleteAll_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Do you wanna delete all mails?", "Diqqetttt", MessageBoxButton.OKCancel, MessageBoxImage.Warning, MessageBoxResult.Cancel);
        if (result == MessageBoxResult.OK)
        {
            var imap = new ImapClient();
            imap.Connect("imap.gmail.com", 993);
            imap.Authenticate("mirtalibemirli498@gmail.com", "zbap yyys xtcs thpb");
            var inBox = imap.GetFolder("inbox");
            await inBox.OpenAsync(FolderAccess.ReadWrite);
            var inMails = await inBox.SearchAsync(SearchQuery.All);
            await inBox.SetFlagsAsync(inMails, MessageFlags.Deleted, true);
            await inBox.ExpungeAsync();
            Emails.Clear();
        }


    }

    private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {

    }
}