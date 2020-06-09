
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TestApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            weatherComponent.getSearchButton().Click += new EventHandler(search);
            weatherComponent.getClearButton().Click += new EventHandler(clear);
        }

        public async void search(object sender, EventArgs e)
        {
            var searchText = weatherComponent.getSearchField().Text;

            try
            {
                Temperatures weather = await reachForTheWeather(searchText);
                if(weather == null)
                {
                    return;
                }
                string sunrise = obtainDateFromMillis(weather.Sys.Sunrise, "HH:mm:ss");
                string sunset = obtainDateFromMillis(weather.Sys.Sunset, "HH:mm:ss");
                weatherComponent.setWeatherValues("Celsius "+Convert.ToString(weather.Main.Temp-273.15),
                    "Sunrise "+sunrise + " Sunset " + sunset,
                    "City "+weather.Name,
                    "Pressure "+Convert.ToString(weather.Main.Pressure)+"hPa", 
                    "Date "+obtainDateFromMillis(weather.Dt, "dd MM yyyy"),
                    "Lat "+weather.Coord.Lat + " Lon " + weather.Coord.Lon,
                    "Humidity "+Convert.ToString(weather.Main.Humidity)+" %");      
                    
             }
            catch (Exception)
            {
            }

        }

        private string obtainDateFromMillis(double millis,String format)
        {
            return (new DateTime(1970, 1, 1)).AddMilliseconds(millis).ToString(format);
        }

        private async Task<Temperatures> reachForTheWeather(string city)
        {
            var API_URL = "http://api.openweathermap.org/data/2.5/weather?q=" + city + "&appid=";
            var API_KEY = "ca9e8ed54495b520f0eaa6879bc58dfa";
            string response = await new HttpClient().GetStringAsync(API_URL + API_KEY);
            if (response != null)
            {

                return JsonConvert.DeserializeObject<Temperatures>(response);
            }
            return null;
        }

        private void clear(object sender, EventArgs e)
        {
            weatherComponent.clear();
        }
    }
}
