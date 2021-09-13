using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weather
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            var lon = "44.538125";
            var lat = "48.741972";
            var part = "current, minutely, hourly, alerts";
            var api = "60d574a8f2d130060fbdf757c8471c28";
            var dt = DateTime.Today.AddDays(-3).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            var metric = "metric";
            var url = String.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&exclude={2}&units={3}&appid={4}",
                lat, lon, part, metric, api);
            var cli = new HttpClient();
            var response = cli.GetStringAsync(url).Result;
          //  label1.Text = response;
            var s = JsonSerializer.Deserialize<Root>(response);
            var min=150.000;
            var min_dt = 0;
            var max = 0.00;
            var max_dt = 0;
            var fiveday = DateTime.Today.AddDays(5).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            foreach (var w in s.daily) 
            {
                
                if (min > Math.Abs(w.feels_like.night - w.temp.night)) {
                    min_dt = w.dt;
                    min = Math.Abs(w.feels_like.night - w.temp.night);
                }

                if (w.dt <= fiveday && max < (w.sunset - w.sunrise)) {
                    max_dt = w.dt;
                    max = w.sunset - w.sunrise;
                }
             }
            var data = new DateTime(1970, 1, 1);
          label1.Text = "Минимальная разница температур "+min.ToString("0.###")+"°C "+data.AddSeconds(min_dt).ToString("dd.MM.yyyy");
          label2.Text = "Максимальная длительность светового дня " + data.AddSeconds(max_dt).ToString("dd.MM.yyyy");

        }
    }
}
