using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireBaseLib;
using System.IO;


using System.Windows.Forms;
using ControlReverseLib;
namespace Sketch_Based
{
    public partial class FrmMakeSketch : Form
    {
        int image = 0;
        bool isDragged = false;
        Point ptOffset;
        private string controlsInfoStr;
        public FrmMakeSketch()
        {
            InitializeComponent();
            Util.init(FBConfig.url);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            loadShapes("eyes");
        }
        public void loadShapes(String shapeName)
        {
            String path = Application.StartupPath + "\\Face Sketch Elements\\" + shapeName;
            string[] filePaths = Directory.GetFiles(path);
            int x = 10;
            int y = 10;
            panel5.Controls.Clear();

            for (int i = 0; i < filePaths.Length; i++)
            {
                String filName = filePaths[i];
                Bitmap bt = new Bitmap(filName);
                PictureBox pic = new PictureBox();

                pic.Size = new Size(bt.Width, bt.Height);
                pic.Location = new Point(x, y);
                pic.Image = bt;
                pic.Name = shapeName;
              
                // pic.MouseDown += Pic_MouseDown;
                // pic.MouseUp += Pic_MouseUp;
                // pic.MouseMove += Pic_MouseMove;
                pic.Click += Pic_Click;
                y = y + pic.Height + 20;
                panel5.Controls.Add(pic);
            }
        }
        private void Pic_Click(object sender, EventArgs e)
        {
            PictureBox b = sender as PictureBox;
            string name = b.Name;
            PictureBox pic = new PictureBox();
            Bitmap bt = new Bitmap(b.Image);
            pic.Size = new Size(bt.Width, bt.Height);
            pic.Location = new Point(10, 10);
            pic.SizeMode = PictureBoxSizeMode.StretchImage;
          
            pic.Name = name;
            pic.Image = bt;
          
            //pic.ContextMenuStrip = contextMenuStrip1;
            pic.MouseClick += Pic_MouseClick;
            //pic.BackColor = Color.Transparent;
            /*  pic.MouseDown += Pic_MouseDown;
              pic.MouseUp += Pic_MouseUp;
              pic.MouseMove += Pic_MouseMove;*/
            ControlMoverOrResizer.Init(pic);
            panel6.Controls.Add(pic);
            pic.BringToFront();
            panel2.Refresh();
        }

        private void Pic_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                PictureBox pic = sender as PictureBox;
                contextMenuStrip1.Show(pic, pic.PointToClient(Cursor.Position));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            loadShapes("nose");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            loadShapes("hair");
        }

        private void Pic_MouseMove(object sender, MouseEventArgs e)
        {
            PictureBox b = sender as PictureBox;
            if (isDragged)
            {
                Point newPoint = b.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                b.Location = newPoint;
            }
        }

        private void Pic_MouseUp(object sender, MouseEventArgs e)
        {
            isDragged = false;
        }

        private void Pic_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox b = sender as PictureBox;

            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = b.PointToScreen(new Point(e.X, e.Y));

                ptOffset = new Point();
                ptOffset.X = b.Location.X - ptStartPosition.X;
                ptOffset.Y = b.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

            loadShapes("head");
        }

        private void button6_Click(object sender, EventArgs e)
        {

            loadShapes("lips");
        }

        private void button7_Click(object sender, EventArgs e)
        {

            loadShapes("eyebrows");
        }

        private void button8_Click(object sender, EventArgs e)
        {

            loadShapes("mustach");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            FrmDashboard dash = new FrmDashboard();
            dash.Show();
            this.Hide();
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            // panel2_Paint(sender, e);
            panel2.Refresh();

        }

        private void FrmMakeSketch_Load(object sender, EventArgs e)
        {

            
        }

        private void deleteToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem toolstrip = sender as ToolStripItem;
            if (toolstrip != null)
            {
                ContextMenuStrip ctx = toolstrip.Owner as ContextMenuStrip;
                Control control = ctx.SourceControl;
                PictureBox pic = control as PictureBox;

                String name = pic.Name;
                panel6.Controls.Remove(pic);
                //MessageBox.Show(name + " Deleted");
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void bringToFrontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripItem toolstrip = sender as ToolStripItem;
            if (toolstrip != null)
            {
                ContextMenuStrip ctx = toolstrip.Owner as ContextMenuStrip;
                Control control = ctx.SourceControl;
                PictureBox pic = control as PictureBox;
                String name = pic.Name;
                pic.BringToFront();
                panel2.Refresh();
            }
        }

        private void opacityToolStripMenuItem_Click(object sender, EventArgs e)
        {
             ToolStripItem toolstrip = sender as ToolStripItem;
             if (toolstrip != null)
             {
                 ContextMenuStrip ctx = toolstrip.Owner as ContextMenuStrip;
                 Control control = ctx.SourceControl;
                 PictureBox pic = control as PictureBox;
                 System.Drawing.Drawing2D.GraphicsPath obj = new System.Drawing.Drawing2D.GraphicsPath();
                 obj.AddEllipse(0, 0, pic.Width, pic.Height);
                 Region rg = new Region(obj);
                 pic.Region = rg;
                panel2.Refresh();
            }
        }
     
        private void button9_Click(object sender, EventArgs e)
        {
            panel2.Refresh();
        }

        private void button10_Click(object sender, EventArgs e)
        {         
            //new bitmap object to save the image
            Bitmap bitmap = new Bitmap(panel2.Width, panel2.Height);
     
            panel2.DrawToBitmap(bitmap, new Rectangle(0, 0, panel2.Width, panel2.Height));
            // Restore the original z-order.
            String path = "d:\\sketch_" + DateTime.Now.Millisecond + ".jpg";
            bitmap.Save(path);
            MessageBox.Show("Image saved Successfully on "+path); 
         
        }

     
        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            Util.ReversControls(panel6);
            Util.reDraw(panel6, e);
            Util.ReversControls(panel6);
        }

        private void SendToBacktoolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ToolStripItem toolstrip = sender as ToolStripItem;
            if (toolstrip != null)
            {
                ContextMenuStrip ctx = toolstrip.Owner as ContextMenuStrip;
                Control control = ctx.SourceControl;
                PictureBox pic = control as PictureBox;
                String name = pic.Name;
                pic.SendToBack();
                panel2.Refresh();
            }

        }
    }
}

