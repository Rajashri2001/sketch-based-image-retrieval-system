using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Firebase.Database;
using DlibDotNet;
using DlibDotNet.Extensions;
using Dlib = DlibDotNet.Dlib;
using AForge.Math.Geometry;
using AForge.Video;
using System.Drawing.Imaging;
using AForge.Video.DirectShow;
using System.Threading;
using System.Threading.Tasks;
using AForge.Imaging.Filters;
using Firebase.Database.Query;
using FireBaseLib;
using System.Windows.Forms;


namespace Sketch_Based
{
    public partial class FrmFaceRetrive : Form
    {
        string fname = "";
        List<LoadFeature> listFeature = new List<LoadFeature>();
        private VideoCaptureDevice device; //Current chosen device(camera)
        private Dictionary<string, string> cameraDict = new Dictionary<string, string>();
        private const int CameraWidth = 320;  // constant Width
        private const int CameraHeight = 240; // constant Height
        private FilterInfoCollection cameras; //Collection of Cameras that connected to PC                                              
        int st = 0;
        Array2D<RgbPixel> srcImage = new Array2D<RgbPixel>();
        DlibDotNet.Rectangle[] myfaceRect;
        Matrix<RgbPixel> myStoreface;
        Bitmap myface;
        AForge.Video.DirectShow.FileVideoSource f;
        FrontalFaceDetector FaceDetector = Dlib.GetFrontalFaceDetector();
        DlibDotNet.ShapePredictor ShapePredictor = DlibDotNet.ShapePredictor.Deserialize("shape_predictor_5_face_landmarks.dat");
        DlibDotNet.Dnn.LossMetric LossMetric = DlibDotNet.Dnn.LossMetric.Deserialize("dlib_face_recognition_resnet_model_v1.dat");
        List<string> name_list = new List<string>();
        public FrmFaceRetrive()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            pictureBox1.Image = Bitmap.FromFile(ofd.FileName);
            fname = ofd.FileName;
        }
        public async void loadData()
        {
            var firebase = FireBaseDB.init(FBConfig.url);
            try
            {
                var fbdata = await firebase.Child("RegisterInfo").OnceAsync<RegisterPojo>();
                StreamWriter sw = new StreamWriter(Application.StartupPath + "\\train\\feature.csv", true);
                // List<MissingPerson> policePojos = new List<MissingPerson>();
                foreach (var data in fbdata)
                {
                    //Console.WriteLine($"{dino.Key} is {dino.Object.Height}m high.");
                    RegisterPojo mp = new RegisterPojo();
                    mp.Feature = data.Object.Feature;
                    sw.WriteLine(mp.Feature);
                }
                sw.Close();
            }
            catch (Exception ex)
            {

            }         
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            loadData();
            Cursor.Current = Cursors.Default;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\train\\feature.csv";
            string[] lines = File.ReadAllLines(path);
            listFeature.Clear();
            for (int i = 0; i < lines.Length; i++)
            {
                Matrix<float> mat = new Matrix<float>(128, 1);
                LoadFeature lft = new LoadFeature();

                string[] data = lines[i].Split(',');

                string name = data[0];

                for (int j = 1; j < data.Length; j++)
                {
                    mat[j - 1] = float.Parse(data[j]);
                }
                lft.Facediscriptor = mat;
                lft.Name = name;
                listFeature.Add(lft);
            }
        }

        /*
        public void GuiUpdate(String msg)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(new MethodInvoker(delegate { label1.Text = msg + cbCamera.Text; }));
            }
        }
*/
        private Bitmap ConvertTo24(Bitmap inputFileName)
        {
            Bitmap bmpIn = inputFileName;
            Bitmap converted = new Bitmap(bmpIn.Width, bmpIn.Height, PixelFormat.Format24bppRgb);
            using (Graphics g = Graphics.FromImage(converted))
            {
                g.PageUnit = GraphicsUnit.Pixel;
                g.DrawImageUnscaled(bmpIn, 0, 0);
            }
            return converted;
        }

        void detectface(Bitmap btm)
        {
            btm = ConvertTo24(btm);
            var image = BitmapExtensions.ToArray2D<RgbPixel>(btm);
            srcImage = BitmapExtensions.ToArray2D<RgbPixel>(btm);
           
            var faceRects = FaceDetector.Operator(image);
             myfaceRect = faceRects;
            string outputName = "";
            Graphics g = Graphics.FromImage(btm);
            List<SimilarityScore> criminalList = new List<SimilarityScore>();

            foreach (var faceRect in faceRects)
            {
                string output = "unknown";
                // draw a rectangle for each face
                //Dlib.DrawRectangle(image, faceRect, color: new RgbPixel(0, 255, 255), thickness: 4);
                g.DrawRectangle(new Pen(Brushes.Cyan), faceRect.TopLeft.X, faceRect.TopLeft.Y, faceRect.Width, faceRect.Height);
                var shape = ShapePredictor.Detect(srcImage, faceRect);
                var faceChipDetail = Dlib.GetFaceChipDetails(shape, 150, 0.25);
                Array2D<RgbPixel> imageChip = Dlib.ExtractImageChip<RgbPixel>(srcImage, faceChipDetail);
                Matrix<RgbPixel> face = new Matrix<RgbPixel>(imageChip);
                var FaceDescriptors = LossMetric.Operator(face);
                var queryfeature = FaceDescriptors[0];                
                for (int i = 0; i < listFeature.Count; i++)
                {
                    var storefeat = listFeature[i].Facediscriptor;
                    var diff = queryfeature - storefeat;
                    float dist = Dlib.Length(diff);
                    SimilarityScore obj = new SimilarityScore();
                    obj.Name = listFeature[i].Name;
                    obj.Score = dist;
                    criminalList.Add(obj);
                }               
            }
           // float min = 999;
            
            for (int i=0;i<criminalList.Count;i++)
            {
                for (int j= i+1; j < criminalList.Count; j++)
                {
                    SimilarityScore sc1 = criminalList[i];
                    SimilarityScore sc2 = criminalList[j];
                    if(sc1.Score>sc2.Score)
                    {
                        SimilarityScore tmp = sc1;
                        criminalList[i] = sc2;
                        criminalList[j] = tmp;
                    }
                }
            }
            flowLayoutPanel1.Controls.Clear();
            double th = 0.60;
            for (int i = 0; i < criminalList.Count; i++)
            {
                for (int j = i+1; j < criminalList.Count-1; j++)
                {
                   SimilarityScore temp;

                    if(criminalList[i].Score< criminalList[j].Score)
                    {
                        temp = criminalList[i];
                        criminalList[i] = criminalList[j];
                        criminalList[j] = temp;
                    }
                }
            }
                for (int i = 0; i < criminalList.Count; i++)
            {
                if(criminalList[i].Score<th)
                {
                    string name = criminalList[i].Name;
                    try
                    {
                        string path = Application.StartupPath + "\\faces\\" + name + ".jpg";
                        CriminalControl criminalGUI = new CriminalControl();
                        criminalGUI.criminalName = name;
                        criminalGUI.path = path;
                        criminalGUI.score = criminalList[i].Score + "";
                        criminalGUI.Show();
                        flowLayoutPanel1.Controls.Add(criminalGUI);                    
                    }
                    catch { }
                }
            }
        }
        void detectface2(Bitmap btm)
        {
            var image = BitmapExtensions.ToArray2D<RgbPixel>(btm);
            srcImage = BitmapExtensions.ToArray2D<RgbPixel>(btm);
            var faceRects = FaceDetector.Operator(image);
            // myfaceRect = faceRects;
            string outputName = "";
            Graphics g = Graphics.FromImage(btm);
            foreach (var faceRect in faceRects)
            {
                string output = "unknown";
                // draw a rectangle for each face
                //Dlib.DrawRectangle(image, faceRect, color: new RgbPixel(0, 255, 255), thickness: 4);
                g.DrawRectangle(new Pen(Brushes.Cyan), faceRect.TopLeft.X, faceRect.TopLeft.Y, faceRect.Width, faceRect.Height);
                var shape = ShapePredictor.Detect(srcImage, faceRect);
                var faceChipDetail = Dlib.GetFaceChipDetails(shape, 150, 0.25);
                Array2D<RgbPixel> imageChip = Dlib.ExtractImageChip<RgbPixel>(srcImage, faceChipDetail);
                Matrix<RgbPixel> face = new Matrix<RgbPixel>(imageChip);
                var FaceDescriptors = LossMetric.Operator(face);

                var queryfeature = FaceDescriptors[0];
                float min = 999;
                for (int i = 0; i < listFeature.Count; i++)
                {
                    var storefeat = listFeature[i].Facediscriptor;
                    var diff = queryfeature - storefeat;

                    float dist = Dlib.Length(diff);
                    if (dist < min)
                    {
                        min = dist;
                        output = listFeature[i].Name;
                    }
                }
                outputName = outputName + "," + output;
                g.DrawString(output, new Font("arial", 12), Brushes.Red, faceRect.TopLeft.X, faceRect.TopLeft.Y);
            }
            pictureBox1.Image = btm;
            GuiUpdate(outputName + " detected");
        }
        public void GuiUpdate(String msg)
        {
            if (label1.InvokeRequired)
            {
                label1.Invoke(new MethodInvoker(delegate { label1.Text = msg; }));
            }
        }

        private void FrmFaceRetrive_Load(object sender, EventArgs e)
        {
          button5_Click(null, null);

        }

        private void FrmFaceRetrive_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                if (device.IsRunning)
                {
                    device.SignalToStop();
                    device.Stop();
                }
            }
            catch { }
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            Bitmap bt = new Bitmap(pictureBox1.Image);
            detectface(bt);
        }
        void videoNewFrame(object sender, NewFrameEventArgs args)
        {
            Bitmap temp = (Bitmap)args.Frame.Clone();
            Bitmap bt = new Bitmap(temp);
            //  Array2D<RgbPixel> img = BitmapExtensions.ToArray2D<RgbPixel>(bt);

            //Bitmap bt = new Bitmap(temp);
            //fname = Application.StartupPath + "\\img.jpg";
            //  bt.Save(fname);

            pictureBox1.Image = bt;
            detectface2(temp);
            // button3_Click(null, null);

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            if (f != null)
            {
                f.SignalToStop();
                f.WaitForStop();
                f.Stop();
            }
            f = new AForge.Video.DirectShow.FileVideoSource(ofd.FileName);
            f.NewFrame += new AForge.Video.NewFrameEventHandler(videoNewFrame);
            f.Start();
        }
    }
}
