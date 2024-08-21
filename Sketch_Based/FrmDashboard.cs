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
using System.Windows.Forms;

namespace Sketch_Based
{
    public partial class FrmDashboard : Form
    {
        public FrmDashboard()
        {
            InitializeComponent();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
          //  button5.BackColor = Color.PowderBlue;
            FrmCriminalRegistration pl = new FrmCriminalRegistration();
            pl.FormBorderStyle = FormBorderStyle.None;
            pl.TopMost = false;
            pl.TopLevel = false;
            pl.Dock = DockStyle.Fill;

            pl.AutoScroll = true;
            panel4.Controls.Clear();
            panel4.Controls.Add(pl);
            pl.Show();           
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
          //  button1.BackColor = Color.PowderBlue;
            FrmCriminalHistoryGrid pl = new FrmCriminalHistoryGrid();
            pl.FormBorderStyle = FormBorderStyle.None;
            pl.TopMost = false;
            pl.TopLevel = false;
            pl.Dock = DockStyle.Fill;

            pl.AutoScroll = true;
            panel4.Controls.Clear();
            panel4.Controls.Add(pl);
            pl.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
          //  button2.BackColor = Color.PowderBlue;
            FrmFaceRetrive pl = new FrmFaceRetrive();
            pl.FormBorderStyle = FormBorderStyle.None;
            pl.TopMost = false;
            pl.TopLevel = false;
            pl.Dock = DockStyle.Fill;

            pl.AutoScroll = true;
            panel4.Controls.Clear();
            panel4.Controls.Add(pl);
            pl.Show();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
          //  button4.BackColor = Color.PowderBlue;
            FrmMakeSketch obj = new FrmMakeSketch();
            obj.Show();
            this.Hide();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button5_MouseHover(object sender, EventArgs e)
        {
           // button5.BackColor = Color.PowderBlue;
        }

        private void button5_MouseLeave(object sender, EventArgs e)
        {
           // button5.BackColor = Color.DarkGreen;
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
           // button1.BackColor = Color.PowderBlue;
        }

        private void button2_MouseHover(object sender, EventArgs e)
        {
          //  button2.BackColor = Color.PowderBlue;
        }

        private void button4_MouseHover(object sender, EventArgs e)
        {
           // button4.BackColor = Color.PowderBlue;
        }

        private void button1_MouseLeave(object sender, EventArgs e)
        {
         //   button1.BackColor = Color.DarkGreen;
        }

        private void button2_MouseLeave(object sender, EventArgs e)
        {
          //  button2.BackColor = Color.DarkGreen;
        }

        private void button4_MouseLeave(object sender, EventArgs e)
        {
          //  button4.BackColor = Color.DarkGreen;
        }

        private void FrmDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
