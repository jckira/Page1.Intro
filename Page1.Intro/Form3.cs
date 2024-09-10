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
using System;
using System.Data;
using System.Windows.Forms;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;


namespace Page1.Intro
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        // Method to load locations from the database
        private void LoadLocations()
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
           
            // Clear the ComboBox before loading new data
            comboBox1.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT location FROM routes";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBox1.Items.Add(reader.GetString(0));
                        }
                    }
                }
            }
        }

        // Load destinations based on selected location
        private void LoadDestinations(string location)
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT destination FROM routes WHERE location = @location"; // Query to get destinations based on location

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@location", location); // Pass the selected location as a parameter

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox2.Items.Clear(); // Clear the previous destination items
                        while (reader.Read())
                        {
                            comboBox2.Items.Add(reader.GetString(0)); // Add each destination to comboBox2
                        }
                    }
                }
            }
        }

        // Load sub-destinations based on selected destination
        private void LoadSubDestinations(string destination)
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT sub_destination FROM routes WHERE destination = @destination"; // Query to get sub-destinations based on destination

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@destination", destination); // Pass the selected destination as a parameter

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox3.Items.Clear(); // Clear the previous sub-destination items
                        while (reader.Read())
                        {
                            comboBox3.Items.Add(reader.GetString(0)); // Add each sub-destination to comboBox3
                        }
                    }
                }
            }
        }

        private void LoadDiscount()
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT category_name FROM discount_categories"; // Query to get all categories

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        comboBox4.Items.Clear(); // Clear previous items
                        while (reader.Read())
                        {
                            comboBox4.Items.Add(reader.GetString(0)); // Add each category to comboBox4
                        }
                    }
                }
            }
        }

        // Event handler when the form loads (populate locations)
        private void Form3_Load(object sender, EventArgs e)
        {
            LoadLocations(); // Load locations when the form loads
            LoadDiscount();
        }

        // Event handler for when a location is selected (load destinations)
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedLocation = comboBox1.SelectedItem.ToString();
            LoadDestinations(selectedLocation); // Load destinations based on selected location
        }

        // Event handler for when a destination is selected (load sub-destinations)
        private void comboBoxDestination_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDestination = comboBox2.SelectedItem.ToString();
            LoadSubDestinations(selectedDestination); // Load sub-destinations based on selected destination
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedLocation = comboBox1.SelectedItem.ToString(); // Get the selected location
            LoadDestinations(selectedLocation); // Call the method to load destinations for the selected location
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDestination = comboBox2.SelectedItem.ToString(); // Get the selected destination
            LoadSubDestinations(selectedDestination); // Call the method to load sub-destinations for the selected destination
        }

        private decimal GetFare(string location, string destination, string subDestination)
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
            decimal fare = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT fare FROM routes WHERE location = @location AND destination = @destination AND sub_destination = @subDestination";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@location", location);
                    cmd.Parameters.AddWithValue("@destination", destination);
                    cmd.Parameters.AddWithValue("@subDestination", subDestination);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        fare = Convert.ToDecimal(result);
                    }
                }
            }

            return fare;
        }

        private decimal GetDiscountRate(string category)
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
            decimal discountRate = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT discount_rate FROM discount_categories WHERE category_name = @category";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@category", category);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        discountRate = Convert.ToDecimal(result);
                    }
                }
            }

            return discountRate;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Get selected values
            string location = comboBox1.SelectedItem?.ToString();
            string destination = comboBox2.SelectedItem?.ToString();
            string subDestination = comboBox3.SelectedItem?.ToString();
            string discountCategory = comboBox4.SelectedItem?.ToString();
            string idNumber = textBox1.Text; // Get the entered ID number

            // Check if a discount category is selected and it's not "None"
            if (discountCategory != null && discountCategory != "None")
            {
                // If a discount category is selected, check if ID number is provided
                if (string.IsNullOrWhiteSpace(idNumber))
                {
                    // If ID number is empty or whitespace, show a message and stop the process
                    MessageBox.Show("Please enter your ID number for the selected discount category.", "ID Number Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return; // Exit the method, don't proceed to calculate the fare
                }
            }

            // If "None" is selected, we skip the ID check
            // Proceed with fare calculation as usual

            // Fetch fare and discount
            decimal fare = GetFare(location, destination, subDestination);
            decimal discountRate = GetDiscountRate(discountCategory);

            // Apply discount
            decimal discountedFare = fare * (1 - (discountRate / 100));

            // Display the fare
            textBox2.Text = "₱" + discountedFare.ToString("N2");
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedDiscountCategory = comboBox4.SelectedItem?.ToString();

            // If the user selects "None", clear the textBox1 (ID number)
            if (selectedDiscountCategory == "None")
            {
                textBox1.Clear(); // This will clear the entered ID number
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Get the selected values from the combo boxes
            string selectedLocation = comboBox1.SelectedItem?.ToString();
            string selectedDestination = comboBox2.SelectedItem?.ToString();
            string selectedSubDestination = comboBox3.SelectedItem?.ToString();

            // Check if all selections are made
            if (!string.IsNullOrEmpty(selectedLocation) && !string.IsNullOrEmpty(selectedDestination) && !string.IsNullOrEmpty(selectedSubDestination))
            {
                // Open Form4 and pass the selected values
                Form4 form4 = new Form4(selectedLocation, selectedDestination, selectedSubDestination);
                form4.Show(); // Open Form4 (you can also use ShowDialog() to open it as a modal dialog)
            }
            else
            {
                // Display a message if any selection is missing
                MessageBox.Show("Please select a location, destination, and sub-destination.", "Incomplete Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}

