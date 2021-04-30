using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Media;

namespace Proyecto_P2
{
    public partial class Producto : Form
    {
        SoundPlayer media = new SoundPlayer();
        //variables vectores y Harcascades
        int con = 0;
        Image<Bgr, Byte> currentFrame;
        Capture Grabar;
        HaarCascade face;//Rostro
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.4d, 0.4d);
        Image<Gray, byte> result, TraineFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NombrePersonas = new List<string>();
        int ContTrain, numLabels, t;
        string Nombre;
        DataGridView d = new DataGridView();

        private void FrameGrabar(object sender, EventArgs e)
        {
            lblCantidad.Text = "0";
            NombrePersonas.Add("");

            try
            {
                currentFrame = Grabar.QueryFrame().Resize(320, 240, INTER.CV_INTER_CUBIC);

                gray = currentFrame.Convert<Gray, Byte>();

                MCvAvgComp[][] RostrosDetectados = gray.DetectHaarCascade(face, 1.5, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                foreach (MCvAvgComp R in RostrosDetectados[0])
                {
                    t = t + 1;
                    result = currentFrame.Copy(R.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    currentFrame.Draw(R.rect, new Bgr(Color.Green), 1);



                    NombrePersonas[t - 1] = Nombre;
                    NombrePersonas.Add("");

                    lblCantidad.Text = RostrosDetectados[0].Length.ToString();
                }
                t = 0;
                imageBox1.Image = currentFrame;
                Nombre = "";
                NombrePersonas.Clear();
            }
            catch (Exception Error)
            {

                MessageBox.Show(Error.Message);
            }

        }

        private void btnCaturar_Click(object sender, EventArgs e)
        {
            sonido();
            try
            {
                ContTrain += ContTrain;
                gray = currentFrame.Convert<Gray, Byte>();

                MCvAvgComp[][] RostrosDetectados = gray.DetectHaarCascade(face, 1.2, 10, HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));

                foreach (MCvAvgComp R in RostrosDetectados[0])
                {

                    TraineFace = currentFrame.Copy(R.rect).Convert<Gray, byte>().Resize(100, 100, INTER.CV_INTER_CUBIC);
                    break;

                }
                TraineFace = result.Resize(100, 100, INTER.CV_INTER_CUBIC);
                trainingImages.Add(TraineFace);

                imageBox2.Image = TraineFace;
            }
            catch
            {


            }
        }

        private void brnRegistrar_Click(object sender, EventArgs e)
        {
            sonido();
            if (txtNombre.Text != "")
            {
                labels.Add(txtNombre.Text);
                Base.GuardarImagen(txtNombre.Text, Base.ConvertImgToBinary(imageBox2.Image.Bitmap));


            }

            Base.Consultar(dataGridView1);
        }

        private void Producto_FormClosing(object sender, FormClosingEventArgs e)
        {
            DetenerReconocer();
        }
        public void sonido()
        {
            media.SoundLocation = "Sonidos/Open.wav";
            media.Play();
        }

        private void Producto_Load(object sender, EventArgs e)
        {
            reconocer();
            media.SoundLocation = "Sonidos/2.wav";
            media.Play();
            Base.Consultar(dataGridView1);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            sonido();
            Base.eliminar(int.Parse(dataGridView1.CurrentRow.Cells["id"].Value.ToString()));
            Base.listaCaras(dataGridView1);
        }
        void limpiar()
        {
            imageBox2.Image = null;
            pictureBox1.Image = null;
            txtNombre.Clear();
            brnRegistrar.Text = "Registrar";

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            sonido();
            limpiar();
        }

        private void btnDesconectar_Click(object sender, EventArgs e)
        {
            sonido();
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

        private void DetenerReconocer()
        {
            try
            {

                Application.Idle -= new EventHandler(FrameGrabar);
                Grabar.Dispose();
                imageBox1.ImageLocation = "img/1.png";
                lblCantidad.Text = string.Empty;
                btnDesconectar.Text = "Conectar";
            }
            catch (Exception Error)
            {

                MessageBox.Show(Error.Message);
            }

        }
        public Producto()
        {
            InitializeComponent();

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
            catch (Exception e)
            {

                MessageBox.Show("Error" + e);
            }
        }
    }
}
