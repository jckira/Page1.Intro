using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;


namespace Page1.Intro
{
    public partial class Form8 : Form
    {
        private bool isVerified = false; // Flag to check if discount is verified
        public Form8()
        {
            InitializeComponent();
        }

        private void LoadLocations()
        {
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";

            // Clear the ComboBox before loading new data
            comboBox1.Items.Clear();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT location FROM bus_route";
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
                string query = "SELECT DISTINCT destination FROM bus_route WHERE location = @location"; // Query to get destinations based on location

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
                string query = "SELECT DISTINCT sub_destination FROM bus_route WHERE destination = @destination"; // Query to get sub-destinations based on destination

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
        private void Form8_Load(object sender, EventArgs e)
        {
            LoadLocations(); // Load locations when the form loads
            LoadDiscount();
        }

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
                string query = "SELECT fare FROM bus_route WHERE location = @location AND destination = @destination AND sub_destination = @subDestination";

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
        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Reset the verification flag
            isVerified = false;

            // Store the selected category for checking after verification
            string selectedCategory = comboBox4.SelectedItem?.ToString();

            if (selectedCategory == "Student")
            {
                student studentForm = new student();
                if (studentForm.ShowDialog() == DialogResult.OK)
                {
                    isVerified = true;
                }
            }
            else if (selectedCategory == "Senior Citizen")
            {
                senior seniorForm = new senior();
                if (seniorForm.ShowDialog() == DialogResult.OK)
                {
                    isVerified = true;
                }
            }
            else if (selectedCategory == "PWD")
            {
                pwd pwdForm = new pwd();
                if (pwdForm.ShowDialog() == DialogResult.OK)
                {
                    isVerified = true;
                }
            }
            // No need for verification when "None" is selected
            else if (selectedCategory == "None")
            {
                isVerified = true; // Set to true to bypass the verification
            }

            // Notify user if verification failed, but only if the category is not "None"
            if (!isVerified && selectedCategory != "None")
            {
                MessageBox.Show("Please complete the discount verification to apply the discount.", "Verification Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox4.SelectedIndex = -1; // Reset the discount category selection
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!isVerified)
            {
                MessageBox.Show("Please verify your discount information first.", "Verification Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string location = comboBox1.SelectedItem?.ToString();
            string destination = comboBox2.SelectedItem?.ToString();
            string subDestination = comboBox3.SelectedItem?.ToString();
            string discountCategory = comboBox4.SelectedItem?.ToString();

            decimal fare = GetFare(location, destination, subDestination);
            decimal discountRate = GetDiscountRate(discountCategory);
            decimal discountedFare = fare * (1 - (discountRate / 100));
            textBox2.Text = "₱" + discountedFare.ToString("N2");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedLocation = comboBox1.SelectedItem?.ToString();
            string selectedDestination = comboBox2.SelectedItem?.ToString();
            string selectedSubDestination = comboBox3.SelectedItem?.ToString();

            // Check if all selections are made
            if (!string.IsNullOrEmpty(selectedLocation) && !string.IsNullOrEmpty(selectedDestination) && !string.IsNullOrEmpty(selectedSubDestination))
            {
                // Open Form4 and pass the selected values
                Form9 form9 = new Form9(selectedLocation, selectedDestination, selectedSubDestination);
                form9.Show(); // Open Form4 (you can also use ShowDialog() to open it as a modal dialog)
            }
            else
            {
                // Display a message if any selection is missing
                MessageBox.Show("Please select a location, destination, and sub-destination.", "Incomplete Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 nextForm = new Form2();
            nextForm.Show();
            this.Hide();
        }
    }
}
