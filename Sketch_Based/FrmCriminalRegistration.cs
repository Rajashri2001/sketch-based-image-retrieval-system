using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using FireBaseLib;
using System.Windows.Forms;

namespace Sketch_Based
{
    public partial class FrmCriminalRegistration : Form
    {
        public FrmCriminalRegistration()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }
        public void Upload()
        {
            String Type = "Male";
            if (radioButton2.Checked)
            {
                Type = "Female";
            }
            FirebaseClient fb = FireBaseDB.init(FBConfig.url);
            RegisterPojo dc = new RegisterPojo();
            dc.Crime = textBox2.Text;
            dc.Date = dateTimePicker1.Text;
            dc.Description = richTextBox1.Text;
            dc.Name = textBox1.Text;
            dc.Gender = Type;
            // dc.Feature = feature;
            dc.Middlename = textBox3.Text;
            dc.Surname = textBox4.Text;
            dc.Registrationdate = dateTimePicker2.Text;
            dc.Age = textBox5.Text;
            dc.Address = richTextBox2.Text;
            dc.Mobno = textBox6.Text;
            dc.Dob = textBox7.Text;
            dc.Aadharno = textBox8.Text;
            SharePojo.criminal = dc;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";

            FrmRegistration pl = new FrmRegistration();
            pl.FormBorderStyle = FormBorderStyle.None;
            pl.TopMost = false;
            pl.TopLevel = false;
            pl.Dock = DockStyle.Fill;
            pl.AutoScroll = true;
            panel1.Controls.Clear();
            panel1.Controls.Add(pl);
            pl.Show();
        }


        private void button6_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "")
            {
                MessageBox.Show("Field Cannot be Empty");
                return;
            }
            if (textBox2.Text == "" || textBox5.Text == "" || textBox7.Text == "" || textBox8.Text == "")
            {
                MessageBox.Show("Field Cannot be Empty");
                return;
            }
            if (textBox6.TextLength != 10)
            {
                MessageBox.Show("Contact Must be 10 Numbers");
                return;
            }
            if (richTextBox1.Text == "" || richTextBox2.Text == "")
            {
                MessageBox.Show("Field Cannot be Empty");
                return;
            }
            if (textBox8.TextLength != 12)
            {
                MessageBox.Show("Aadhar must be Number");
                return;
            }
            Upload();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            textBox7.Text = "";
            textBox8.Text = "";
            richTextBox1.Text = "";
            richTextBox2.Text = "";
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsControl(e.KeyChar) || char.IsLetter(e.KeyChar))
            {
                return;
            }
            e.Handled = true;
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

       
        }
    }

