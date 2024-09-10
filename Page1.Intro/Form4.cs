using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Page1.Intro
{
    public partial class Form4 : Form
    {
        public Form4(string location, string destination, string subDestination)
        {
            InitializeComponent();
            LoadRouteInfo(location, destination, subDestination); // Call method to load the route info
        }

        private void LoadRouteInfo(string location, string destination, string subDestination)
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
            string routeInfo = "";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT route_info FROM routes WHERE location = @location AND destination = @destination AND sub_destination = @subDestination";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@destination", destination);
                    cmd.Parameters.AddWithValue("@subDestination", subDestination);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        routeInfo = result.ToString();
                    }
                }
            }

            // Display the route_info in a label
            label2.Text = routeInfo;  // Assuming you have a label named labelRouteInfo in Form4
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Close the entire application
            Application.Exit();
        }
    }
}