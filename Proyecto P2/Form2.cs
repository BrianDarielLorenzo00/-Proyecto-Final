using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FontAwesome.Sharp;
using System.Media;

namespace Proyecto_P2
{
    public partial class Form2 : Form
    {
        #region Variables
        SoundPlayer media = new SoundPlayer();
        //Campos de forma privada
        private IconButton CurrentBtn;
        private Panel leftBorderBtn;
        private Form FormularioP;
        #endregion
        //Constructor
        public Form2()
        {
            InitializeComponent();
            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7, 60);
            panelMenu.Controls.Add(leftBorderBtn);
            //Form
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }
        //Estructura para almacenar colores RGB
        private struct RGBColors
        {
            #region Colores para diseño
            public static Color color1 = Color.FromArgb(30, 185, 41);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(219, 222, 13);
            public static Color color5 = Color.FromArgb(214, 89, 13);
            public static Color color6 = Color.FromArgb(24, 161, 251);
#endregion
        }
        //Metodos
        private void ActivacionBoton(object senderBtn, Color color)
        {


            if (senderBtn != null)
            {
                Botón_de_desactivación();
                //Perzonalizacion
                //Boton
                CurrentBtn = (IconButton)senderBtn;
                CurrentBtn.BackColor = Color.FromArgb(37, 36, 81);
                CurrentBtn.ForeColor = color;
                CurrentBtn.TextAlign = ContentAlignment.MiddleCenter;
                CurrentBtn.IconColor = color;
                CurrentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                CurrentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Botón de borde izquierdo
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, CurrentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Diseno
                iconPictureBoxIconodeFormulario.IconChar = CurrentBtn.IconChar;
                iconPictureBoxIconodeFormulario.IconColor = color;

            }
        }
        //Metodo para el desativado del boton
        private void Botón_de_desactivación()
        {
            if (CurrentBtn != null)
            {
                CurrentBtn.BackColor = Color.FromArgb(31, 30, 68);
                CurrentBtn.ForeColor = Color.Gainsboro;
                CurrentBtn.TextAlign = ContentAlignment.MiddleLeft;
                CurrentBtn.IconColor = Color.Gainsboro;
                CurrentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                CurrentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }

        #region Botones
        private void btnProducto_Click(object sender, EventArgs e)
        {
            sonido();
            ActivacionBoton(sender, RGBColors.color1);
            AbrilForm(new Producto());
        }
        private void btnCategoria_Click(object sender, EventArgs e)
        {
            sonido();
            ActivacionBoton(sender, RGBColors.color2);
            AbrilForm(new Registros());
        }
        private void btnInformacionPer_Click(object sender, EventArgs e)
        {
            sonido();
            ActivacionBoton(sender, RGBColors.color3);
            Reconocimiento_Facial f = new Reconocimiento_Facial();
            AbrilForm(f);
            f.ConsultarBaseDeDatos();
        }
        #endregion
        #region Abrir forms en paneles
        //Metodo Abril formulario Hijos en panel
        private void AbrilForm(Form Abril)
        {
            if (FormularioP != null)
            {
                //Abrir solo formulario
                FormularioP.Close();
            }
            FormularioP = Abril;
            Abril.TopLevel = false;
            Abril.FormBorderStyle = FormBorderStyle.None;
            Abril.Dock = DockStyle.Fill;
            panelDeEscritorio.Controls.Add(Abril);
            panelDeEscritorio.Tag = Abril;
            Abril.BringToFront();
            Abril.Show();
            lblInicio.Text = Abril.Text;
        }
        #endregion





        //Metodo de Reiniar
        private void pictureBtnInicio_Click(object sender, EventArgs e)
        {
            sonido();
            FormularioP.Close();
            Reset();
        }
        private void Reset()
        {
            Botón_de_desactivación();
            leftBorderBtn.Visible = false;
            iconPictureBoxIconodeFormulario.IconChar = IconChar.Home;
            iconPictureBoxIconodeFormulario.IconColor = Color.MediumPurple;
            lblInicio.Text = "Inicio";
        }
        //Forma de arrastre
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();

        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void panelTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void HoraFecha_Tick(object sender, EventArgs e)
        {
            lblHora.Text = DateTime.Now.ToString("hh:mm:ss");
            lblFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void iPictureBoxExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void iPictureBoxMaximizar_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
                WindowState = FormWindowState.Maximized;
            else
                WindowState = FormWindowState.Normal;
        }

        private void iPictureBoxMinimizar_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            media.SoundLocation = "Sonidos/2.wav";
            media.Play();
        }
        public void sonido()
        {
            media.SoundLocation = "Sonidos/Open.wav";
            media.Play();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            sonido();
            ActivacionBoton(sender, RGBColors.color5);
            AbrilForm(new Informacion_Personal());


        }

        private void lblHora_Click(object sender, EventArgs e)
        {

        }

        private void panelDeEscritorio_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
