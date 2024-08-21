using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sketch_Based
{
    public partial class Splash : Form
    {
        int ticks;

        public Splash()
        {
            InitializeComponent();
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ticks++;
            if(ticks==3)
            {
                FrmAdminLogin ob = new FrmAdminLogin();
                ob.Show();
                this.Hide();
                timer1.Stop();
            }
        }
    }
}
