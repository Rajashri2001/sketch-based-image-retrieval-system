using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireBaseLib;
using System.Windows.Forms;

namespace Sketch_Based
{
    public partial class FrmCriminalHistoryRetrive : Form
    {

        public static FrmCriminalHistoryRetrive frm;
        public static FrmCriminalHistoryRetrive getform
        {
            get
            {
                if (frm == null)
                {
                    frm = new FrmCriminalHistoryRetrive();
                }
                return frm;
            }
        }



        public FrmCriminalHistoryRetrive()
        {
            InitializeComponent();
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }
        

   
        private void FrmCriminalHistoryRetrive_Load(object sender, EventArgs e)
        {
            label16.Text = FrmCriminalHistoryGrid.selectedrow.Cells[0].Value.ToString();
            label20.Text = FrmCriminalHistoryGrid.selectedrow.Cells[1].Value.ToString();
            richTextBox1.Text = FrmCriminalHistoryGrid.selectedrow.Cells[2].Value.ToString();
            label21.Text = FrmCriminalHistoryGrid.selectedrow.Cells[3].Value.ToString();
            label17.Text = FrmCriminalHistoryGrid.selectedrow.Cells[5].Value.ToString();
            label18.Text = FrmCriminalHistoryGrid.selectedrow.Cells[6].Value.ToString();
            label19.Text = FrmCriminalHistoryGrid.selectedrow.Cells[7].Value.ToString();
            label22.Text = FrmCriminalHistoryGrid.selectedrow.Cells[8].Value.ToString();
            richTextBox2.Text = FrmCriminalHistoryGrid.selectedrow.Cells[9].Value.ToString();
            label25.Text = FrmCriminalHistoryGrid.selectedrow.Cells[10].Value.ToString();
            label24.Text = FrmCriminalHistoryGrid.selectedrow.Cells[11].Value.ToString();
            label26.Text = FrmCriminalHistoryGrid.selectedrow.Cells[12].Value.ToString();
            label27.Text = FrmCriminalHistoryGrid.selectedrow.Cells[13].Value.ToString();
            try
            {
                string path = Application.StartupPath + "\\faces\\" + label16.Text + ".jpg";
                pictureBox1.Image = Bitmap.FromFile(path);
            }
            catch(Exception ex)
            {

            }
            if(pictureBox1.Image!=null)
            {
                label23.Text = "Face is Detected";
            }


        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
