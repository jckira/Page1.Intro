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
    public partial class senior : Form
    {
        public senior()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs including issued date
            if (string.IsNullOrWhiteSpace(textBox1.Text) || // Name
                comboBox1.SelectedIndex == -1 ||              // Municipality (ComboBox)
                string.IsNullOrWhiteSpace(textBox4.Text) || // OSCA number
                dateTimePicker1.Value == null)              // Issued Date
            {
                MessageBox.Show("Please fill in all required fields, including issued date.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Match user input with the database (without saving issued date)
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM sc WHERE name = @name AND city = @city AND osc_num = @oscaId";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@city", comboBox1.SelectedItem.ToString()); // Use selected item from ComboBox
                    cmd.Parameters.AddWithValue("@oscaId", textBox4.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show($"Senior Citizen Verified! Issued Date: {dateTimePicker1.Value.ToShortDateString()}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Close the form after successful verification
                        this.DialogResult = DialogResult.OK; // Optional: can return a specific result
                        this.Close(); // Close the form
                    }
                    else
                    {
                        MessageBox.Show("No matching record found. Please check your information.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the key pressed is a digit or a control key like backspace
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Disallow non-numeric characters
            }

            // Allow only 4 characters
            if (textBox4.Text.Length >= 4 && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Disallow further input once length reaches 4
            }
        }
    }
}
