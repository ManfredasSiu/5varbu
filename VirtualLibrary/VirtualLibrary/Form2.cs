using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualLibrary
{
    public partial class Form2 : Form    //Register Langas, padarysiu irgi su face recognition
    {
        MCvFont font = new MCvFont(Emgu.CV.CvEnum.FONT.CV_FONT_HERSHEY_TRIPLEX, 0.6d, 0.6d);
        HaarCascade faceDetect;
        Image<Bgr, Byte> frame;
        Capture cam;
        Image<Gray, byte> result;
        Image<Gray, byte> TrainImg = null;
        Image<Gray, byte> GrayFace = null;
        
        
       // List<string> users = new List<string>();
        int count = 0, numLablels, t;
        string name, names = null;

        private LogicController LogicC;

        public Form2(LogicController LogicC)
        {
            InitializeComponent();
            this.LogicC = LogicC;
            faceDetect = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                string labelsInf = File.ReadAllText(Application.StartupPath + "/faces/faces.txt");
                string[] Labels = labelsInf.Split(',');
                int.TryParse(Labels[0], out numLablels);
                string FacesLoad;
                for (int i = 1; i <= numLablels; i++)
                {
                    FacesLoad = "face" + i + ".bmp";
                    StaticData.training.Add(new Image<Gray, byte>(Application.StartupPath + $"/faces/{FacesLoad}"));
                    StaticData.labels.Add(Labels[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Nothing in the database");
            }
            cam = new Capture();
            cam.QueryFrame();
            Application.Idle += new EventHandler(FrameProcedure);
        }

        private void FrameProcedure(object sender, EventArgs e)
        {
            frame = cam.QueryFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            GrayFace = frame.Convert<Gray, Byte>();
            MCvAvgComp[][] facesDetectedNow = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            foreach (MCvAvgComp f in facesDetectedNow[0])
            {
                result = frame.Copy(f.rect).Convert<Gray, Byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                frame.Draw(f.rect, new Bgr(Color.Green), 3);
                if (StaticData.training.ToArray().Length != 0)
                {
                    MCvTermCriteria termCriteria = new MCvTermCriteria(numLablels, 0.001);
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(StaticData.training.ToArray(), StaticData.labels.ToArray(), 1500, ref termCriteria);
                    name = recognizer.Recognize(result);
                    //frame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.Red));
                }
            }
            imageBox1.Image = frame;
        }

        private void RegisterButton_CLicked(object sender, EventArgs e)
        {
            GrayFace = cam.QueryGrayFrame().Resize(640, 480, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            MCvAvgComp[][] DetectedFaces = GrayFace.DetectHaarCascade(faceDetect, 1.2, 10, Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));
            if (DetectedFaces[0].Length == 0)
                return;
            foreach (MCvAvgComp f in DetectedFaces[0])
            {
                TrainImg = frame.Copy(f.rect).Convert<Gray, byte>();
                break;
            }
            numLablels++;
            TrainImg = TrainImg.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
            StaticData.training.Add(TrainImg);
            StaticData.labels.Add(textBox1.Text);
            //StaticData.passwords.Add(textBox2.Text);
            File.WriteAllText(Application.StartupPath + "/faces/faces.txt", StaticData.training.ToArray().Length + ",");
            for (int i = 1; i <= numLablels; i++)
            {
                StaticData.training.ToArray()[i - 1].Save(Application.StartupPath + "/faces/face" + i + ".bmp");
                File.AppendAllText(Application.StartupPath + "/faces/faces.txt", StaticData.labels.ToArray()[i - 1] + ",");
            }
            MessageBox.Show(textBox1.Text + ", Welcome.");
        }
    }
}
