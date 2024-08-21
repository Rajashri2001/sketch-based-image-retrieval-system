using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

using DlibDotNet;
using DlibDotNet.Extensions;
using Dlib = DlibDotNet.Dlib;
using System.Runtime.InteropServices;

using AForge;

using AForge.Math.Geometry;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Threading;
using AForge.Imaging.Filters;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
//using System.Web.Script.Serialization;
using Firebase.Database;
using FireBaseLib;
using Firebase.Database.Query;

namespace Sketch_Based
{
    public partial class FrmRegistration : Form
    {
        string fname = "";
       // Image OrigionalImage = null;
        //Image tempimg = null;
        //int circlecnt = 0;
        int cnt = 0;


        private VideoCaptureDevice device; //Current chosen device(camera) 
        private Dictionary<string, string> cameraDict = new Dictionary<string, string>();
        private const int CameraWidth = 320;  // constant Width
        private const int CameraHeight = 240; // constant Height
        private FilterInfoCollection cameras; //Collection of Cameras that connected to PC
        //private int frameCounter = 0;
        //int camWidth = 320;
        //int camHeight = 240;
        //int fcnt = 0;
       // AForge.Video.DirectShow.FileVideoSource f;
        Bitmap srcimg = new Bitmap(256, 256);

        Array2D<RgbPixel> srcImage = new Array2D<RgbPixel>();
        DlibDotNet.Rectangle[] myfaceRect;
        Matrix<RgbPixel> myStoreface;
        Bitmap myface;


        FrontalFaceDetector FaceDetector = Dlib.GetFrontalFaceDetector();
        DlibDotNet.ShapePredictor ShapePredictor = DlibDotNet.ShapePredictor.Deserialize("shape_predictor_5_face_landmarks.dat");
        DlibDotNet.Dnn.LossMetric LossMetric = DlibDotNet.Dnn.LossMetric.Deserialize("dlib_face_recognition_resnet_model_v1.dat");
        public FrmRegistration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();

            pictureBox1.Image = Bitmap.FromFile(ofd.FileName);
            fname = ofd.FileName;
        }
        void detectface(Bitmap btm)
        {
            var FaceDetector = Dlib.GetFrontalFaceDetector();

            var image = BitmapExtensions.ToArray2D<RgbPixel>(btm);
            srcImage = BitmapExtensions.ToArray2D<RgbPixel>(btm);

            var faceRects = FaceDetector.Operator(image);
            myfaceRect = faceRects;
            foreach (var faceRect in faceRects)
            {
                // draw a rectangle for each face
                Dlib.DrawRectangle(image, faceRect, color: new RgbPixel(0, 255, 255), thickness: 4);
            }
            pictureBox9.Image = image.ToBitmap();
        }
        private void StartCapture()
        {
            try
            {
                // _capture = new Capture();
                this.device = new VideoCaptureDevice(this.cameraDict[cbCamera.SelectedItem.ToString()]);
                this.device.NewFrame += new NewFrameEventHandler(videoNewFrame);
                this.device.DesiredFrameSize = new Size(CameraWidth, CameraHeight);

                device.Start();
                //    ApplyCamSettings();
            }
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void videoNewFrame(object sender, NewFrameEventArgs args)
        {
            Bitmap temp = (Bitmap)args.Frame.Clone();
            Array2D<RgbPixel> img = BitmapExtensions.ToArray2D<RgbPixel>(temp);
            pictureBox1.Image = temp;
            detectface(temp);       
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var FaceDetector = Dlib.GetFrontalFaceDetector();

            var image = Dlib.LoadImage<RgbPixel>(fname);
            srcImage = Dlib.LoadImage<RgbPixel>(fname);


            var faceRects = FaceDetector.Operator(image);
            myfaceRect = faceRects;
            foreach (var faceRect in faceRects)
            {
                // draw a rectangle for each face
                Dlib.DrawRectangle(image, faceRect, color: new RgbPixel(0, 255, 255), thickness: 4);
            }
            label8.Text = "detected faces :" + myfaceRect.Length;
            pictureBox9.Image = image.ToBitmap();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (myfaceRect.Length > 0)
            {
                var shape = ShapePredictor.Detect(srcImage, myfaceRect[cnt]);
                var faceChipDetail = Dlib.GetFaceChipDetails(shape, 150, 0.25);
                Array2D<RgbPixel> imageChip = Dlib.ExtractImageChip<RgbPixel>(srcImage, faceChipDetail);
                Matrix<RgbPixel> face = new Matrix<RgbPixel>(imageChip);
                myStoreface = face;
                myface = face.ToBitmap();
                pictureBox10.Image = myface;

                cnt++;
                if (cnt >= myfaceRect.Length)
                {
                    cnt = 0;
                }
            }
        }


        public async void Upload(string feature)
        {

            FirebaseClient fb = FireBaseDB.init(FBConfig.url);
             SharePojo.criminal.Feature = feature;           
            try
            {
                await fb.Child("RegisterInfo").PostAsync(SharePojo.criminal);
              //  MessageBox.Show("Registration Done Successfully");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex);
            }
        }


        private void button6_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + "\\faces\\" + SharePojo.criminal.Name + ".jpg";
            myface.Save(path);
            var FaceDescriptors = LossMetric.Operator(myStoreface);

            var feature = FaceDescriptors[0];
            string str = SharePojo.criminal.Name+ "";
            for (int i = 0; i < feature.Size; i++)
            {
                str = str + "," + feature[i];
            }
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\train\\feature.csv", true);
            sw.WriteLine(str);
            sw.Close();
            Upload(str);
            MessageBox.Show("Face stored & Registration Done Successfully");
        }

        private void Criminal_reg_Load(object sender, EventArgs e)
        {
            this.cameras = new FilterInfoCollection(AForge.Video.DirectShow.FilterCategory.VideoInputDevice);
            int i = 1;

            foreach (AForge.Video.DirectShow.FilterInfo camera in this.cameras)
            {
                if (!this.cameraDict.ContainsKey(camera.Name))
                    this.cameraDict.Add(camera.Name, camera.MonikerString);
                else
                {
                    this.cameraDict.Add(camera.Name + "-" + i.ToString(), camera.MonikerString);
                    i++;
                }
            }
            this.cbCamera.DataSource = new List<string>(cameraDict.Keys); //Bind camera names to combobox
        }

        private void cbCamera_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                StartCapture();
            }
            else
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
        }

        
    }
}
