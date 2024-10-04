using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;

namespace Page1.Intro
{
    public partial class Form10 : Form
    {
        private string subDestination;
        public Form10(string subDestination)
        {
            InitializeComponent();
            this.subDestination = subDestination;

            // Load the map and labels on form load
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT sub_destination FROM bus_route WHERE sub_destination = @subdestination";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@subdestination", subDestination);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Update label7 to display only the sub_destination
                    label3.Text = reader["sub_destination"].ToString();

                    conn.Close();

                    // Load the map based on sub-destination
                    LoadMap();
                }
            }
        }
        private async void LoadMap()
        {
            // Ensure WebView2 is initialized
            if (webView21.CoreWebView2 == null)
            {
                await webView21.EnsureCoreWebView2Async(null);
            }

            // Construct URL for Google Maps
            string url = string.Format("https://www.google.com/maps?q={0}", label3.Text);
            webView21.CoreWebView2.Navigate(url);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form10_Load(object sender, EventArgs e)
        {

        }
    }
}
