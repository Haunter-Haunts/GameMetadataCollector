using GameMetadataCollector.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace GameMetadataCollector
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RestClient _client;

        public MainWindow()
        {
            InitializeComponent();
            _client = new RestClient();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Make sure the user inputs good data
            if (string.IsNullOrEmpty(AppIdTextBox.Text) || !long.TryParse(AppIdTextBox.Text, out long _))
            {
                MessageBox.Show("Please enter an integer app ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Load the Steam game data
            SteamGame game = GetSteamGame(AppIdTextBox.Text);

            // Make sure a game is found
            if (game == null)
            {
                MessageBox.Show($"No game found for app ID {AppIdTextBox.Text}.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Dump the data to a text file
            string fileName = $"{Path.GetTempPath()}{Guid.NewGuid()}.txt";
            string fileContents = FormatForForumPost(game);

            // Cleaer input
            AppIdTextBox.Text = UrlTextBox.Text = string.Empty;

            // Try to open temp file
            try
            {
                File.WriteAllText(fileName, fileContents);
                Process.Start("notepad.exe", fileName);
            }
            catch (Exception)
            {
                Clipboard.SetText(fileContents);
                MessageBox.Show("Failed to create/open temp file. Results are copied to clipboard instead.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string FormatForForumPost(SteamGame game)
        {
            StringBuilder b = new StringBuilder();
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;

            // Create the header of the post
            b.AppendLine("[center]");
            b.AppendLine("[img]");
            b.AppendLine(game.HeaderImage);
            b.AppendLine("[/img]");
            b.AppendLine("[size=150]");
            b.AppendLine(game.Name);
            b.AppendLine("[/size]");
            b.AppendLine("[/center]");
            b.AppendLine(string.Empty);

            // Create game description
            if (!string.IsNullOrEmpty(game.DetailedDescription))
            {
                b.AppendLine("[quote]");
                b.AppendLine(StripHtml(game.DetailedDescription));
                b.AppendLine("[/quote]");
                b.AppendLine(string.Empty);
            }

            // Controller support
            if (!string.IsNullOrEmpty(game.ControllerSupport))
            {
                b.Append("[i]Controller Support: ");
                b.Append(textInfo.ToTitleCase(game.ControllerSupport));
                b.AppendLine("[/i]");
            }

            // Metacritic
            if (game.Metacritic != null)
            {
                b.Append("[i]Metacritic Score: ");
                b.Append("[url=");
                b.Append(game.Metacritic.Url);
                b.Append("]");
                b.Append(game.Metacritic.Score);
                b.Append("[/url]");
                b.AppendLine("[/i]");
            }

            // Developers
            if (game.Developers != null && game.Developers.Length > 0)
            {
                b.Append("[i]Developers: ");
                b.Append(string.Join(", ", game.Developers));
                b.AppendLine("[/i]");
            }

            // Publishers
            if (game.Publishers != null && game.Publishers.Length > 0)
            {
                b.Append("[i]Publishers: ");
                b.Append(string.Join(", ", game.Publishers));
                b.AppendLine("[/i]");
            }

            // Genres
            if (game.Genres != null && game.Genres.Length > 0)
            {
                b.Append("[i]Genres: ");
                b.Append(string.Join(", ", game.Genres.Select(x => x.Description).ToArray()));
                b.AppendLine("[/i]");
            }

            // Store page
            b.Append("[i]Store Page: ");
            b.Append("[url=");
            b.Append("https://store.steampowered.com/app/");
            b.Append(AppIdTextBox.Text);
            b.Append("/");
            b.Append("]Steam[/url]");
            b.AppendLine("[/i]");
            b.AppendLine(string.Empty);

            // Create share tag
            if (!string.IsNullOrEmpty(UrlTextBox.Text) && UrlTextBox.Text.Length > 0)
            {
                b.AppendLine("[hide]");
                b.AppendLine("[code]");
                b.AppendLine(UrlTextBox.Text);
                b.AppendLine("[/code]");
                b.AppendLine("[/hide]");
                b.AppendLine(string.Empty);
            }

            // Create screenshots
            b.AppendLine("[center]");
            foreach (SteamScreenshot screenshot in game.Screenshots)
            {
                b.Append("[img]");
                b.Append(screenshot.PathThumbnail); // Thumbnail to save bandwith
                b.Append("[/img]");
            }
            b.AppendLine("[/center]");

            return b.ToString();
        }

        private SteamGame GetSteamGame(string appId)
        {
            RestRequest request = new RestRequest($"https://store.steampowered.com/api/appdetails?appids={appId}", Method.GET);
            request.AddHeader("Accept", "*/*");

            IRestResponse result = _client.Execute(request);
            if (result.StatusCode == HttpStatusCode.OK && result.Content.Length > 0)
            {
                // The JSON comes back with some unnecessary fluff so this trims it to the part we need
                JObject obj = JObject.Parse(result.Content);
                JToken token = obj.SelectToken(appId);
                string json = JsonConvert.SerializeObject(token.SelectToken("data"));
                return JsonConvert.DeserializeObject<SteamGame>(json);
            }

            return null;
        }

        private string StripHtml(string input)
        {
            string noTags = Regex.Replace(input, "<.*?>", string.Empty);
            return Regex.Replace(noTags, "&.*?;", string.Empty);
        }
    }
}