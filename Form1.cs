using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proiect_Info_Apl_2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // "Exit" Button
        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // "Configurare" Button
        private void button2_Click(object sender, EventArgs e)
        {
            Configurare forma = new Configurare();
            forma.ShowDialog();
            forma.Close();
        }
        
        // "Start" Button
        private void button1_Click(object sender, EventArgs e)
        {
            Start forma = new Start();
            forma.ShowDialog();
            forma.Close();
        }
    }
}
