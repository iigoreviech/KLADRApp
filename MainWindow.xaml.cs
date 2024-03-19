using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
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
using AdressApp.AppData;

namespace AdressApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.cmbRegion.ItemsSource = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, "[0-9][0-9]00000000000")).ToList();
            this.cmbRaion.ItemsSource = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, "[0-9][0-9][0-9][0-9][0-9]00000000")).ToList();
            this.cmbCity.ItemsSource = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]00000")).ToList();
            this.cmbNP.ItemsSource = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, "[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]00")).ToList();
        }

        private void printAdress()
        {
            txtFull.Text = $"{cmbRegion.Text}, {cmbRaion.Text}, {cmbCity.Text}, {cmbNP.Text}, {cmbStreet.Text}, {cmbHouse.Text}";
        }

        private void setRaions(string region)
        {
            string regionID = region.Substring(0, 2);
            string dota2 = $"{regionID}[0-9][0-9][0-9]00000000";
            List<KLADR> dota = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, dota2)).ToList();
            dota.RemoveAt(0);
            cmbRaion.ItemsSource = dota;
        }

        private void setCities(string pattern)
        {
            string mask = $"{pattern}[0-9][0-9][0-9]00000";
            List<KLADR> dota = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, mask)).ToList();
            if (dota.Count > 0)
            {
                dota.RemoveAt(0);
            }
            cmbCity.ItemsSource = dota;
        }
        private void setStreet(string pattern)
        {
            string mask = $"{pattern}[0-9][0-9][0-9][0-9]00";
            List<STREET> dota = Manager.db.STREETs.Where(p => DbFunctions.Like(p.CODE, mask)).ToList();
            cmbStreet.ItemsSource = dota;
        }

        private void setNP(string pattern)
        {
            string mask = $"{pattern}[0-9][0-9][0-9]00";
            List<KLADR> dota = Manager.db.KLADRs.Where(p => DbFunctions.Like(p.CODE, mask)).ToList();
            if (dota.Count > 0)
            {
                dota.RemoveAt(0);
            }
            cmbNP.ItemsSource = dota;
        }
        
        private void setDom(string pattern)
        {
            string mask = $"{pattern}[0-9][0-9][0-9][0-9]";
            List<DOMA> dota = Manager.db.DOMAs.Where(p => DbFunctions.Like(p.CODE, mask)).ToList();
            List<string> houses = new List<string>();
            foreach (DOMA houseBlock in dota)
            {
                string[] housesInBlock = houseBlock.NAME.Split(',');
                for (int i = 0; i < housesInBlock.Length; i++)
                {
                    housesInBlock[i] = $"{houseBlock.SOCR.ToLower()} {housesInBlock[i]}";
                }
                houses.AddRange(housesInBlock);
            }
            cmbHouse.ItemsSource = houses;
        }

        private void cmbRegion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            KLADR selectedRegion = cmbRegion.SelectedItem as KLADR;
            setRaions(selectedRegion.CODE);
            if (cmbRaion.Items.Count == 0)
                cmbRaion.IsEnabled = false;
            else
                cmbRaion.IsEnabled = true;
            setCities(selectedRegion.CODE.Substring(0, 2) + "000");
            if (cmbCity.Items.Count == 0)
                cmbCity.IsEnabled = false;
            else
                cmbCity.IsEnabled = true;
            setStreet(selectedRegion.CODE.Substring(0, 2) + "000000000");
            if (cmbStreet.Items.Count == 0)
                cmbStreet.IsEnabled = false;
            else
                cmbStreet.IsEnabled = true;
            printAdress();
        }

        private void cmbRaion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                KLADR selectedRaion = cmbRaion.SelectedItem as KLADR;
                setCities(selectedRaion.CODE.Substring(0, 5));
                if (cmbCity.Items.Count == 0)
                    cmbCity.IsEnabled = false;
                else
                    cmbCity.IsEnabled = true;
                setNP(selectedRaion.CODE.Substring(0, 5) + "000");
                if (cmbNP.Items.Count == 0)
                    cmbNP.IsEnabled = false;
                else
                    cmbNP.IsEnabled = true;
                printAdress();
            }
            catch { }
        }

        private void cmbCity_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                KLADR selectedCity = cmbCity.SelectedItem as KLADR;
                setNP(selectedCity.CODE.Substring(0, 8));
                if (cmbNP.Items.Count == 0)
                    cmbNP.IsEnabled = false;
                else
                    cmbNP.IsEnabled = true;
                setStreet(selectedCity.CODE.Substring(0, 8) + "000");
                if (cmbStreet.Items.Count == 0)
                    cmbStreet.IsEnabled = false;
                else
                    cmbStreet.IsEnabled = true;
                printAdress();
            }
            catch
            {
            }

        }

        private void cmbNP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                KLADR selectedNP = cmbNP.SelectedItem as KLADR;
                setStreet(selectedNP.CODE.Substring(0, 11));
                if (cmbStreet.Items.Count == 0)
                    cmbStreet.IsEnabled = false;
                else
                    cmbStreet.IsEnabled = true;
                setDom(selectedNP.CODE.Substring(0, 11) + "0000");
                if (cmbHouse.Items.Count == 0)
                    cmbHouse.IsEnabled = false;
                else
                    cmbHouse.IsEnabled = true;
                printAdress();
            }
            catch 
            {
            }
        }

        private void cmbStreet_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                STREET selectedStreet = cmbStreet.SelectedItem as STREET;
                setDom(selectedStreet.CODE.Substring(0, 15));
                if (cmbHouse.Items.Count == 0)
                    cmbHouse.IsEnabled = false;
                else
                    cmbHouse.IsEnabled = true;
                printAdress();
            }
            catch
            {

            }
        }

        private void cmbHouse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            printAdress();
        }

        private async void btnKoord_Click(object sender, RoutedEventArgs e)
        {
            string apiKey = "00698e88-c3f1-4474-9858-9f387c272925";
            string geo = txtFull.Text;
            string request = $"https://geocode-maps.yandex.ru/1.x?apikey={apiKey}&geocode={geo}&format=json&results=1";

            string[] koordinates = await GetKoord(request);

            txtKoord.Text = $"{koordinates[0]} {koordinates[1]}";
        }

        static async Task<string[]> GetKoord(String requestMessage)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;
            string response = client.DownloadString(requestMessage);
            client.Dispose();

            int index = response.IndexOf("\"Point\":{\"pos\":\"") + 15;
            string coord = response.Substring(index);
            coord = coord.Replace("\"", "");
            coord = coord.Replace("{", "");
            coord = coord.Replace("}", "");
            coord = coord.Replace("[", "");
            coord = coord.Replace("]", "");

            string[] coordinates = coord.Split(',');

            return coordinates;
        }
    }
}