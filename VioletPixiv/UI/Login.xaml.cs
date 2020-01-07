using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace VioletPixiv
{
 
    public partial class LoginPage : Page
    {

        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {

            this.IsEnabled = false;
            this.LoginProcess(accountBox.Text, passwordBox.Password);
        
        }

        private async void LoginProcess(string accountText, string passwordText)
        {
            try
            {
                if(accountText == "" | passwordText == "") throw new InvalidOperationException();

                // Mainwindow AuthToken
                MainWindow.PixivWindow.AuthToken = await Pixeez.Auth.AuthorizeAsync(accountText, passwordText);

                // Store account and passwd
                Properties.Settings.Default.account = accountText;
                Properties.Settings.Default.passwd = passwordText;
                Properties.Settings.Default.Save();

                // Mainwindow init
                MainWindow.PixivWindow.Init();

                // Close
                this.IsEnabled = true;
                await UIAccess.FindParentByType<SubPageFrame>(this).FrameOut();


            }
            #pragma warning disable 168
            catch (InvalidOperationException e)
            #pragma warning disable 168
            {
                this.IsEnabled = true;
                errorLabel.Content = "Login Failed!";
            }
            

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.LoginProcess(accountBox.Text, passwordBox.Password);
            }
        }

    }
}
