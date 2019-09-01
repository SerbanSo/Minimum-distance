using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Proiect_Info_Apl_2
{
    public partial class Configurare : Form
    {
        private int[,] matrice;
        int linii = 0, coloane = 0;
        int culoare = 0, editare = 0;

        // Main function
        public Configurare()
        {
            InitializeComponent();
            button3.Enabled = false;

            StreamReader sr = new StreamReader("../../matrice.txt");
            citire_matrice(sr);
            sr.Close();

            initializare_suprafata();
        }

        // Read the matrix from file 
        private void citire_matrice(StreamReader sr)
        {
            string s = sr.ReadLine();
            string[] c = s.Split(' ');
            linii = int.Parse(c[0]);
            coloane = int.Parse(c[1]);
            matrice = new int[linii, coloane];

            for (int i = 0; i < linii; i++)
            {
                s = sr.ReadLine();
                c = s.Split(' ');
                for (int j = 0; j < coloane; j++)
                {
                    matrice[i, j] = int.Parse(c[j]);
                }
            }
        }

        // Initialize the colors for the edit function
        private void initializare_culori()
        {
            Graphics g = pictureBox2.CreateGraphics();

            g.DrawRectangle(new Pen(Color.White), 0, 0, 50, 50);
            g.DrawRectangle(new Pen(Color.White), 0, 50, 50, 50);
            g.DrawRectangle(new Pen(Color.White), 0, 100, 50, 50);
            g.DrawRectangle(new Pen(Color.White), 0, 150, 50, 50);
            g.FillRectangle(new SolidBrush(Color.Black),  1, 1, 49, 49);
            g.FillRectangle(new SolidBrush(Color.Green),  1, 51, 49, 49);
            g.FillRectangle(new SolidBrush(Color.Yellow),  1, 101, 49, 49);
            g.FillRectangle(new SolidBrush(Color.Purple), 1, 151, 49, 49);

        }

        // Initialize the matrix
        private void initializare_suprafata()
        {
            // The dimension of the blocks
            pictureBox1.Width = 50 * coloane;
            pictureBox1.Height = 50 * linii;
            pictureBox1.Refresh();
            this.AutoSize = true;

            Graphics g = pictureBox1.CreateGraphics();

            // Drawing of each block
            for (int i = 0; i < linii; i++)
            {
                for (int j = 0; j < coloane; j++)
                {
                    // White border of the blocks
                    g.DrawRectangle(new Pen(Color.White), j * 50, i * 50, 50, 50);

                    switch (matrice[i, j])
                    {
                        case 0:
                            // Black block
                            g.FillRectangle(new SolidBrush(Color.Black), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                        case 1:
                            // Green block
                            g.FillRectangle(new SolidBrush(Color.Green), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                        case 2:
                            // Yellow block
                            g.FillRectangle(new SolidBrush(Color.Yellow), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                        case 3:
                            // Purple block
                            g.FillRectangle(new SolidBrush(Color.Purple), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                    }
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        // Modify the blocks in the matrix
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if(editare==1)
            {
                MouseEventArgs mouse = (MouseEventArgs)e;
                int x = mouse.X, y = mouse.Y;
                matrice[y / 50, x / 50] = culoare;

                Graphics g = pictureBox1.CreateGraphics();

                switch (culoare)
                {
                    case 0:
                        g.FillRectangle(new SolidBrush(Color.Black), (x / 50) * 50 + 1, (y / 50) * 50 + 1, 49, 49);
                        break;
                    case 1:
                        g.FillRectangle(new SolidBrush(Color.Green), (x / 50) * 50 + 1, (y / 50) * 50 + 1, 49, 49);
                        break;
                    case 2:
                        g.FillRectangle(new SolidBrush(Color.Yellow), (x / 50) * 50 + 1, (y / 50) * 50 + 1, 49, 49);
                        break;
                    case 3:
                        g.FillRectangle(new SolidBrush(Color.Purple), (x / 50) * 50 + 1, (y / 50) * 50 + 1, 49, 49);
                        break;
                }
            }
        }

        // Select the color to modify the blocks
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            MouseEventArgs mouse = (MouseEventArgs)e;
            int y = mouse.Y;

            switch(y/50)
            {
                case 0:
                    // black
                    culoare = 0;
                    break;
                case 1:
                    // green 
                    culoare = 1;
                    break;
                case 2:
                    // yellow
                    culoare = 2;
                    break;
                case 3:
                    // purple
                    culoare = 3;
                    break;
            }
        }

        // "Editare" button
        // The color of the blocks can be changed
        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            button3.Enabled = true;

            editare = 1;
        }

        // "Salvare" button
        // Save the new matrix
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button3.Enabled = false;

            editare = 0;

            // the matrix is save in the "../../matrice.txt" file
            using (StreamWriter sw=new StreamWriter("../../matrice.txt"))
            {
                sw.Write(linii + " ");
                sw.Write(coloane + " ");
                sw.WriteLine();
                
                for(int i=0;i<linii;i++)
                {
                    for(int j=0;j<coloane;j++)
                    {
                        sw.Write(matrice[i, j] + " ");
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }
            
        }

        // "Generare" button
        // Generate the matrix
        private void button1_Click(object sender, EventArgs e)
        {
            initializare_suprafata();
            initializare_culori();
        }
    }
}
