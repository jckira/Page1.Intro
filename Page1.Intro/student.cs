using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
namespace Page1.Intro
{
    public partial class student : Form
    {

        public student()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(textBox1.Text) || // Name
                string.IsNullOrWhiteSpace(textBox2.Text) || // School Name
                string.IsNullOrWhiteSpace(textBox4.Text))   // School ID
            {
                MessageBox.Show("Please fill in all fields.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Connect to database and verify information
            string connectionString = "Server=localhost;Database=public_transport_kiosk;Uid=root;Pwd=Kahitano_910;";
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT COUNT(*) FROM students WHERE name = @name AND school = @schoolName AND id_num = @idNumber";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@name", textBox1.Text);
                    cmd.Parameters.AddWithValue("@schoolName", textBox2.Text);
                    cmd.Parameters.AddWithValue("@idNumber", textBox4.Text);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());

                    if (count > 0)
                    {
                        MessageBox.Show("Student Verified Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
