using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Drawing;
using Microsoft.Web.WebView2.WinForms;
using Microsoft.Web.WebView2.Wpf;

namespace Page1.Intro
{
    public partial class Form6 : Form
    {
        private string subDestination;
        public Form6(string subDestination)
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
                string query = "SELECT sub_destination FROM routes WHERE sub_destination = @subdestination";

                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@subdestination", subDestination);

                conn.Open();
                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    // Update label7 to display only the sub_destination
                    label7.Text = reader["sub_destination"].ToString();

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
            string url = string.Format("https://www.google.com/maps?q={0}", label7.Text);
            webView21.CoreWebView2.Navigate(url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form6_Load(object sender, EventArgs e)
        {

        }
    }
}


