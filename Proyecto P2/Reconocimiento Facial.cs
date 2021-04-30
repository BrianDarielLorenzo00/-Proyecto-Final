using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


#region  Librerias Utilizadas
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Media;
using System.Windows.Forms;
using System.Drawing;
#endregion


namespace Proyecto_P2
{
    public partial class Reconocimiento_Facial : Form
    {
        #region Variables del proyecto
        SoundPlayer media = new SoundPlayer();
        //variables vectores y Harcascades
        int con = 0;
        Image<Bgr, Byte> currentFrame;
        Capture Grabar;
        HaarCascade face;//Rostro
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.4d, 0.4d);
        Image<Gray, byte> result = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NombrePersonas = new List<string>();
        int ContTrain, numLabels, t;
        string Nombre;
        DataGridView d = new DataGridView();
        #endregion

        public Reconocimiento_Facial()
        {
            InitializeComponent();
        }

        public void ConsultarBaseDeDatos()
        {
            reconocer();
            media.SoundLocation = "Sonidos/2.wav";
            media.Play();
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            try
            {
                Base.Consultar(d);

                string[] Labels = Base.Nombre;
                numLabels = Base.TotalRostros;
                ContTrain = numLabels;

                for (int i = 0; i < numLabels; i++)
                {
                    con = i;
                    Bitmap bmp = new Bitmap(Base.ConvertBinaryToImg(con));

                    trainingImages.Add(new Image<Gray, byte>(bmp));
                    labels.Add(Labels[i]);

                }



            }
            catch (Exception)
            {

                MessageBox.Show("sin rostros para cargar");
            }
        }
        private void FrameGrabar(object sender, EventArgs e)
        {
            #region Lectura de camara y reconocimiento facial
            lblCantidad.Text = "0";
            NombrePersonas.Add("");

            try
            {
               
                currentFrame = Grabar.QueryFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);

                gray = currentFrame.Convert<Gray, Byte>();
          
                MCvAvgComp[][] RostrosDetectados = gray.DetectHaarCascade(face, 1.2, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                foreach (MCvAvgComp R in RostrosDetectados[0])
                {
                    #region Reconocimiento Facial
                    t = t + 1;
                    result = currentFrame.Copy(R.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    currentFrame.Draw(R.rect, new Bgr(Color.Green), 1);

                    if (trainingImages.ToArray().Length != 0)
                    {
                     
                        MCvTermCriteria Criterio = new MCvTermCriteria(ContTrain, 0.01);

                        EigenObjectRecognizer recogida = new EigenObjectRecognizer(trainingImages.ToArray(), labels.ToArray(),1500, ref Criterio);
                        var fa = new Image<Gray, byte>[trainingImages.Count];
                    Nombre = recogida.Recognize(result);
                    currentFrame.Draw(Nombre, ref font, new Point(R.rect.X - 2, R.rect.Y - 2), new Bgr(Color.Green));
                    if(Nombre == ""){ currentFrame.Draw("Desconocido", ref font, new Point(R.rect.X - 2, R.rect.Y - 2), new Bgr(Color.Green)); }



                        #endregion

                    }

                    NombrePersonas[t - 1] = Nombre;
                    NombrePersonas.Add("");

                    lblCantidad.Text = RostrosDetectados[0].Length.ToString();
                    lblnadie.Text = Nombre;

                }


                t = 0;
                imageBox1.Image = currentFrame;
                Nombre = "";
                NombrePersonas.Clear();
            }
            catch (Exception Error)
            {
                lblnadie.Text = "";
                imageBox1.Image = null;
                MessageBox.Show(Error.Message);
            }
            #endregion
        }

        private void reconocer()
        {
            try
            {
                Grabar = new Capture();
                Grabar.QueryFrame();

                Application.Idle += new EventHandler(FrameGrabar);
            }
            catch (Exception Error)
            {

                MessageBox.Show(Error.Message);
            }
        }

        private void Reconocimiento_Facial_Load(object sender, EventArgs e)
        {
            
        }


        private void DetenerReconocer()
        {
            try
            {

                Application.Idle -= new EventHandler(FrameGrabar);
                Grabar.Dispose();
                imageBox1.ImageLocation = "img/1.png";
                lblnadie.Text = string.Empty;
                lblCantidad.Text = string.Empty;
                btnDesconectar.Text = "Conectar";
            }
            catch (Exception Error)
            {

                MessageBox.Show(Error.Message);
            }

        }

        private void TimerX_Tick(object sender, EventArgs e)
        {
        }
        #region Botones
        private void btnregistrar_Click(object sender, System.EventArgs e)
        {
            media.SoundLocation = "Sonidos/Open.wav";
            media.Play();
            Form2 f = new Form2();

            DetenerReconocer();
            this.Close();
            f.ShowDialog();
        }
        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            media.SoundLocation = "Sonidos/Open.wav";
            media.Play();
            switch (btnDesconectar.Text)
            {
                case "Conectar":
                    reconocer();
                    btnDesconectar.Text = "Desconectar";
                    btnDesconectar.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
                    break;
                case "Desconectar":
                    DetenerReconocer();
                    btnDesconectar.IconChar = FontAwesome.Sharp.IconChar.Expand;
                    break;
            }

        }
#endregion
    }
}
