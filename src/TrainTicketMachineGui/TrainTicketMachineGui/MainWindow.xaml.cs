using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TrainTicketMachineGui
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient httpClient;
        private string prefix = "";

        public MainWindow()
        {
            InitializeComponent();
            httpClient = new HttpClient();
            LetterButton_Click(null, null);
        }



        private async void LetterButton_Click(object? sender, RoutedEventArgs? e)
        {
            Button button = (Button)sender;
            string letter = button?.Content.ToString();

            // Aktualizacja prefixu
            prefix += letter;
            if (prefix == string.Empty)
            {
                listBoxStations.ItemsSource = null;
                DisableUnnecessaryButtons(null);
                return;
            }

            // Wysłanie zapytania do REST API
            string apiUrl = $"http://localhost:80/Stations?prefix={prefix}";
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();

            // Odczytanie odpowiedzi jako JSON
            var responseData = await response.Content.ReadAsStringAsync();
            var stationsResponse = JsonSerializer.Deserialize<StationsResponse>(responseData);

            // Wyświetlenie listy stacji
            listBoxStations.ItemsSource = stationsResponse.stationsNames;

            // Wyłączenie niepotrzebnych przycisków
            DisableUnnecessaryButtons(stationsResponse.nextLetters);

            textBoxStationName.Text = prefix;
        }

        private void DisableUnnecessaryButtons(List<string>? nextLetters = null)
        {
            if (nextLetters == null)
            {
                foreach (var button in GetAllButtons())
                {
                    button.IsEnabled = true;
                }

                return;
            }
            // Wyłączenie wszystkich przycisków
            foreach (var button in GetAllButtons())
            {
                button.IsEnabled = false;
            }

            // Włączenie tylko tych przycisków, które reprezentują możliwe kolejne litery
            foreach (var letter in nextLetters)
            {
                Button? button = GetButtonByContent(letter.ToUpper());
                if (button != null)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private IEnumerable<Button> GetAllButtons()
        {
            var allButtons = new List<Button>();
            FindChildButtons(KeyboardGrid, allButtons);
            return allButtons;
        }

        private void FindChildButtons(DependencyObject parent, List<Button> buttons)
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is Button button)
                {
                    buttons.Add(button);
                }
                else
                {
                    FindChildButtons(child, buttons);
                }
            }
        }

        private Button? GetButtonByContent(string content)
        {
            foreach (var button in GetAllButtons())
            {
                if (button.Content.ToString() == content)
                {
                    return button;
                }
            }

            return null;
        }
        // Obsługa przycisku kasowania ostatniego znaku
        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                // Usunięcie ostatniego znaku z prefixu
                prefix = prefix.Substring(0, prefix.Length - 1);

                // Wysłanie zapytania do REST API z nowym prefixem
                LetterButton_Click(null, null);

                textBoxStationName.Text = prefix;
            }
        }
        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(prefix))
            {
                // Usunięcie ostatniego znaku z prefixu
                prefix = string.Empty;

                // Wysłanie zapytania do REST API z nowym prefixem
                LetterButton_Click(null, null);

                textBoxStationName.Text = prefix;
            }
        }

        private void StationSelected(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxStations.SelectedItem != null)
            {
                string selectedStation = listBoxStations.SelectedItem.ToString();
                prefix = selectedStation.ToUpper();
                textBoxStationName.Text = prefix;
                LetterButton_Click(null, null);
            }
        }
    }

    public class StationsResponse
    {
        public required List<string> stationsNames { get; set; }
        public required List<string> nextLetters { get; set; }
    }
}
