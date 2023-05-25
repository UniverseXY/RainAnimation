using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RainAnimation
{
    public partial class Form1 : Form
    {
        private Animator a;
        public Form1()
        {
            /*  SetStyle(ControlStyles.AllPaintingInWmPaint, true);
              SetStyle(ControlStyles.OptimizedDoubleBuffer, true);*/
            InitializeComponent();
            a = new Animator(panel1.CreateGraphics());
            a.IsStartPressed = false;


        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (!a.IsStartPressed)
            {
                a.IsStartPressed = true;
                a.Start();
            }
            else {
                a.Stop();
                a.Start(true);
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            a.OffsetX = trackBar2.Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
            a.Stop();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            a.Stop();
        }

        private void panel1_Resize(object sender, EventArgs e)
        {
            if (Form1.ActiveForm != null)
                a.MainGraphics = panel1.CreateGraphics();
        }
    }
}
