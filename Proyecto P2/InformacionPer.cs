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
    public partial class Registros : Form
    {
        SoundPlayer media = new SoundPlayer();
        public Registros()
        {
            InitializeComponent();
        }

        private void InformacionPer_Load(object sender, EventArgs e)
        {
            media.SoundLocation = "Sonidos/2.wav";
            media.Play();
            Base.Consultar(dataGridView1);
        }
    }
}
