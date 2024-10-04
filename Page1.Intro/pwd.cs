using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Page1.Intro
{
    public partial class pwd : Form
    {
        public pwd()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||  // Name
                comboBox1.SelectedIndex == -1 ||              // Municipality (ComboBox)
                comboBox2.SelectedIndex == -1 ||  // Disability
                string.IsNullOrWhiteSpace(textBox4.Text))    // PWD ID
            {
                MessageBox.Show("Please fill in all required fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connect to database and verify PWD information
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM pwd WHERE name = @name AND city = @city AND disability = @disability AND id_num = @idNumber";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@city", comboBox1.SelectedItem.ToString()); // Use selected item from ComboBox
                    cmd.Parameters.AddWithValue("@disability", comboBox2.SelectedItem.ToString()); // Disability
                    cmd.Parameters.AddWithValue("@idNumber", textBox4.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("PWD Verified Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Close the form after successful verification
                        this.DialogResult = DialogResult.OK; // Optional: Return result to parent form
                        this.Close(); // Close the form
                    }
                    else
                    {
                        MessageBox.Show("No matching record found. Please check your information.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}

