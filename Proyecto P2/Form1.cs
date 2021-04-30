using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;

namespace Proyecto_P2
{
    public partial class Form1 : Form
    {
        SoundPlayer media = new SoundPlayer();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            media.SoundLocation = "Sonidos/Bienvenida.wav";
            media.Play();

        }
        int cont = 0;

        private void timer1_Tick(object sender, EventArgs e)
        {
            cont += 1;
            label1.Text = label1.Text + ".";
            label2.Text = label2.Text + "/////////////////////////";
            if (cont == 5)
            {

                timer1.Stop();
                if (Base.PP() == "Conexion exitosa")
                {
                    Form2 f = new Form2();

                    f.Show();
                    this.Hide();
                }


            }
        }
    }
}
