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
    public partial class Start : Form
    {
        int linii = 0, coloane = 0, numar_grafuri=0;
        int[,] matrice, matrice_grafuri;
        const int inf = 99999;

        List<int> pozGrafX = new List<int>();
        List<int> pozGrafY = new List<int>();
        List<int> ordine_noduri = new List<int>();
        List<int> drumfinalX = new List<int>();
        List<int> drumfinalY = new List<int>();

        // Main function
        public Start()
        {
            InitializeComponent();

            StreamReader sr = new StreamReader("../../matrice.txt");
            citire_matrice(sr);
            sr.Close();

            initializare_suprafata();
        }

        // Read the matrix from the file
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

        // Initialize the matrix
        private void initializare_suprafata()
        {
            pictureBox1.Width = 50 * coloane;
            pictureBox1.Height = 50 * linii;
            pictureBox1.Refresh();
            this.AutoSize = true;

            Graphics g = pictureBox1.CreateGraphics();

            for (int i = 0; i < linii; i++)
            {
                for (int j = 0; j < coloane; j++)
                {
                    g.DrawRectangle(new Pen(Color.White), j * 50, i * 50, 50, 50);
                    switch (matrice[i, j])
                    {
                        case 0:
                            g.FillRectangle(new SolidBrush(Color.Black), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                        case 1:
                            g.FillRectangle(new SolidBrush(Color.Green), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                        case 2:
                            g.FillRectangle(new SolidBrush(Color.Yellow), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                        case 3:
                            g.FillRectangle(new SolidBrush(Color.Purple), j * 50 + 1, i * 50 + 1, 49, 49);
                            break;
                    }
                }
            }
        }

        // Graph search algorithm
        private void parcurgere(int pozX, int pozY,int distanta,int min)
        {
            int[] vi = { -1, 0, 1, 0 };
            int[] vj = { 0, 1, 0, -1 };
            matrice[pozX, pozY] = 3;
            for (int i = 0; i < 4; i++) 
            {
                if ((pozX + vi[i] > -1 && pozX + vi[i] < linii) && (pozY + vj[i] > -1 && pozY + vj[i] < coloane))
                {
                    if(matrice[pozX + vi[i],pozY + vj[i]]==2)
                    {
                        min = distanta;
                        MessageBox.Show(min.ToString());
                    }
                    else if(matrice[pozX + vi[i],pozY + vj[i]]==1)
                    {
                        distanta++;
                        parcurgere(pozX + vi[i], pozY + vj[i], distanta, min);
                        distanta--;
                    }
                }
            }

        }
        
        // TO DO: comments
        private void determinare_numar_grafuri()
        {
            int s = 0; 
            for (int i = 0; i < linii; i++)
            {
                for (int j = 0; j < coloane; j++)
                {
                    if(matrice[i,j]==2 || matrice[i,j]==3)
                    {
                        pozGrafX.Add(i);
                        pozGrafY.Add(j);
                        //MessageBox.Show(pozGrafX[s].ToString() + "  " + pozGrafY[s].ToString());
                        s++;
                    }
                }
            }
            numar_grafuri = s;
            matrice_grafuri = new int[s, s];
        }

        // TO DO: comments
        private void determinare_distanta_grafuri(int pozX,int pozY,int s, int distanta, int[,] matrice_d)
        {
            int[] vi = { -1, 0, 1, 0 };
            int[] vj = { 0, 1, 0, -1 };
            for (int i = 0; i < 4; i++)
            {
                if ((pozX + vi[i] > -1 && pozX + vi[i] < linii) && (pozY + vj[i] > -1 && pozY + vj[i] < coloane))   //verificare dc urmatoarea pozitie este in afara indexului
                {
                    if (matrice_d[pozX + vi[i], pozY + vj[i]] == 3)     //dc elem matricei este nod
                    {
                        //MessageBox.Show((pozX + vi[i]).ToString() + " " + (pozY + vj[i]).ToString());
                        for(int j=0;j<numar_grafuri;j++)
                        {
                            
                            if(pozGrafX[j]==pozX + vi[i] && pozGrafY[j]==pozY + vj[i] )     //al catelea nod este
                            {
                                //MessageBox.Show(distanta.ToString());
                                matrice_grafuri[s, j] = distanta+1;
                                matrice_grafuri[j, s] = distanta + 1;
                                //MessageBox.Show(pozGrafX[j].ToString() + " " + pozGrafY[j].ToString() + " " + (distanta+1).ToString());
                            }
                        }
                    }
                    else if (matrice_d[pozX + vi[i], pozY + vj[i]] == 1)    // 
                    {
                        distanta++;
                        int a = matrice_d[pozX, pozY];
                        matrice_d[pozX, pozY] = 0;
                        determinare_distanta_grafuri(pozX + vi[i], pozY + vj[i], s,distanta,matrice_d);
                        matrice_d[pozX , pozY ] = a;
                        distanta--;
                    }
                }
            }

        }

        // Make a copy of the matrix
        private void dublare_matrice(int[,] matrice_d)
        {
            for (int i = 0; i < linii; i++)
            {
                for (int j = 0; j < coloane; j++)
                {
                    matrice_d[i, j] = matrice[i, j];
                }
            }
        }

        // Initialize the matrix for Dijkstra
        private void aranjare_matrice_grafuri()
        {

            for (int i = 0; i < numar_grafuri; i++)
            {
                for (int j = 0; j < numar_grafuri; j++)
                {
                    if (i != j && matrice_grafuri[i, j] == 0)
                    {
                        matrice_grafuri[i, j] = inf;
                    }
                }
            }                

        }

        // Dijkstra algorithm implementation
        private void Dijkstra()
        {
            int[] D = new int[numar_grafuri];
            int[] S = new int[numar_grafuri];
            int[] T = new int[numar_grafuri];

            S[0] = 1;
            for(int i=0;i<numar_grafuri;i++)
            {
                D[i] = matrice_grafuri[0, i];
                if(i!=0)
                {
                    if(D[i]!=inf)
                    {
                        T[i] = 0;
                    }
                }
            }

            int pozitie = 0,min;
            for (int i = 0; i < numar_grafuri - 1; i++)
            {
                min = inf;
                for (int j = 0; j < numar_grafuri; j++)
                {
                    if (S[j] == 0)
                    {
                        if(D[j]<min)
                        {
                            min = D[j];
                            pozitie = j;
                        }
                    }
                }
                S[pozitie] = 1;

                for(int j=0;j<numar_grafuri;j++)
                {
                    if(S[j]==0)
                    {
                        if(D[j]>D[pozitie] + matrice_grafuri[pozitie,j])
                        {
                            D[j] = D[pozitie] + matrice_grafuri[pozitie, j];
                            T[j] = pozitie;
                        }
                    }
                }
            }
            
            drum(numar_grafuri - 1,T);

        }

        // Determine the road to be traveled
        private void drum(int nod, int[] T)
        {
            if (T[nod] != 0)
                drum(T[nod], T);
            ordine_noduri.Add(nod);
            //MessageBox.Show(nod.ToString());
        }

        // Draws the path; the red blocks
        private void desenare_traseu()
        {
            int pozX = pozGrafX[0], pozY = pozGrafY[0];
            foreach (int pozitie in ordine_noduri)
            {
                drumfinalX.Add(pozX);
                drumfinalY.Add(pozY);
                parcurgere_traseu(pozX, pozY, pozGrafX[pozitie], pozGrafY[pozitie], new List<int>(), new List<int>());
                pozX = pozGrafX[pozitie];
                pozY = pozGrafY[pozitie];
            }
            drumfinalX.Add(pozX);
            drumfinalY.Add(pozY);
            
            Graphics g = pictureBox1.CreateGraphics();
            for (int i=0;i<drumfinalY.Count;i++)
            {
                //MessageBox.Show(drumfinalX[i].ToString() + " " + drumfinalY[i].ToString());
                g.FillRectangle(new SolidBrush(Color.Red), drumfinalY[i] * 50 + 1, drumfinalX[i] * 50 + 1, 49, 49);
                System.Threading.Thread.Sleep(500);
            }
        }

        private void parcurgere_traseu(int poziX, int poziY, int pozfX, int pozfY, List<int> drumX, List<int> drumY)
        {
            int[] vi = { -1, 0, 1, 0 };
            int[] vj = { 0, -1, 0, 1 };
            for (int i = 0; i < 4; i++)
            {
                if ((poziX + vi[i] > -1 && poziX + vi[i] < linii) && (poziY + vj[i] > -1 && poziY + vj[i] < coloane))
                {
                    if (matrice[poziX + vi[i], poziY + vj[i]] == 3 || matrice[poziX + vi[i], poziY + vj[i]] == 2)
                    {
                        if ((poziX + vi[i] == pozfX) && (poziY + vj[i] == pozfY))
                        {
                            if (drumX.Count == 0)
                            {
                                drumX.Add(poziX);
                                drumY.Add(poziY);
                            }
                            drumfinalX.AddRange(drumX);
                            drumfinalY.AddRange(drumY);
                        }
                        drumX.Clear();
                        drumY.Clear();
                    }

                    if (matrice[poziX + vi[i], poziY + vj[i]] == 1)
                    {
                        if (drumX.Count == 0)
                        {
                            drumX.Add(poziX);
                            drumY.Add(poziY);
                        }
                        drumX.Add(poziX + vi[i]);
                        drumY.Add(poziY + vj[i]);
                        matrice[poziX + vi[i], poziY + vj[i]] = 4;
                        parcurgere_traseu(poziX + vi[i], poziY + vj[i], pozfX, pozfY,drumX,drumY);
                        matrice[poziX + vi[i], poziY + vj[i]] = 1;
                    }
                }
            }
        }

        // "Start" button
        // Starts the crossing of the graph
        private void button2_Click(object sender, EventArgs e)
        {
            int[,] matrice_d = new int[linii, coloane];
            determinare_numar_grafuri();

            //determinarea distantei dintre noduri
            for (int i = 0; i < numar_grafuri; i++)
            {
                //resetarea matricei
                // Reset the matrix
                dublare_matrice(matrice_d);

                determinare_distanta_grafuri(pozGrafX[i], pozGrafY[i], i, 0,matrice_d);
            }

            aranjare_matrice_grafuri();

            Dijkstra();

            desenare_traseu();

            //scriere in fisier
            // Writes in the file
            using (StreamWriter sw = new StreamWriter("../../matrice_grafuri.txt"))
            {
                for(int i=0;i<numar_grafuri;i++)
                {
                    for(int j=0;j<numar_grafuri;j++)
                    {
                        sw.Write(matrice_grafuri[i, j] + " ");
                    }
                    sw.WriteLine();
                }
            }
        }

        // "Generare" button
        // Generate the matrix
        private void button1_Click(object sender, EventArgs e)
        {
            initializare_suprafata();
        }
    }
}
