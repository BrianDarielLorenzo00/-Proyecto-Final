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
    public partial class Informacion_Personal : Form
    {
        SoundPlayer media = new SoundPlayer();
        public Informacion_Personal()
        {
            InitializeComponent();
        }

        private void Informacion_Personal_Load(object sender, EventArgs e)
        {
            media.SoundLocation = "Sonidos/2.wav";
            media.Play();
        }
    }
}
