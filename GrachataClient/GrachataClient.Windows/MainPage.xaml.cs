using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization.Json;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GrachataClient.Helpers;
using GrachataClient.Models;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace GrachataClient
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void loginButton_Click(object sender, RoutedEventArgs e)
        {
            var client = new WebApiClient();
            try
            {
                var res = await client.LoginAsync(userNameTextBox.Text, passwordTextBox.Password);
                if (res.UserName == "")
                    return;
                
            }
            catch (Exception ex)
            {

            }
            //using (var httpClient = new HttpClient())
            //{
            //    httpClient.DefaultRequestHeaders.Add("ContentType", "application/x-www-form-urlencoded");


            //    var loginModel = new LoginModel()
            //    {
            //        UserName = userNameTextBox.Text,
            //        Password = passwordTextBox.Password
            //    };


            //    var content = new StringContent(loginModel.ToString());
            //    var response = await httpClient.PostAsync(App.ApiToken, content);

            //    MessageDialog dialog;
            //    IUICommand buttonClicked;
            //    string error = "";

            //    if (response.IsSuccessStatusCode)
            //    {


            //        var jsonD = new DataContractJsonSerializer(typeof(TokenModel));

            //        var resultContent = await response.Content.ReadAsByteArrayAsync();
            //        using (var innerStream = new MemoryStream(resultContent))
            //        {
            //            var token = jsonD.ReadObject(innerStream) as TokenModel;
            //            App.Token = token;
            //            return;
            //        }

            //    }
            //    error = await response.Content.ReadAsStringAsync();


            //    dialog = new MessageDialog("Error:\r\n" + error);
            //    dialog.Commands.Add(new UICommand("Ok"));
            //    buttonClicked = await dialog.ShowAsync();

            //}
        }
    }
}
