using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sketch_Based
{
    public partial class CriminalControl : UserControl
    {
        public String criminalName="";

        public String path = "";
        public string score="";
        public CriminalControl()
        {
            InitializeComponent();
        }

        private void CriminalControl_Load(object sender, EventArgs e)
        {
            label1.Text = criminalName;
            Bitmap bt = new Bitmap(path);
            label3.Text = score;
            pictureBox1.Image = bt;
        }
    }
}
