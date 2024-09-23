using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
    public MainWindow()
    {
        InitializeComponent();

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

    private void Send_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            string? recipientEmail = mail.Text;
            using var client = new SmtpClient(smtpServer, smptPort);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
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