using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bioinformatics
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }




        public Random rnd = new Random();
        public List<string> sequence = new List<string>();
        List<int> bucket = new List<int>();
        List<int> bucketIndex = new List<int>();
        List<int> gBest = new List<int>();
        int bestScore = 0;

        private int iteration = 1;
        List<int> seQpBest;

        bool flagCun = true;

        int[,] bloCun = new int[5, 5]{
            {4,1,0,0,-5},
            { 1,5,-2,-1,-5},
            {0,-2,6,-3,-5},
            {0,-1,-3,9,-5},
            { -5, -5, -5, -5, -5 }
        };

        public List<int> seqMainA = new List<int>();
        public List<int> seqMainB = new List<int>();
        public List<int> seqMainC = new List<int>();
        public List<int> seqMainD = new List<int>();
        public List<int> seqMainE = new List<int>();

        public List<int> seqBestA = new List<int>();
        public List<int> seqBestB = new List<int>();
        public List<int> seqBestC = new List<int>();
        public List<int> seqBestD = new List<int>();
        public List<int> seqBestE = new List<int>();


        public List<int>
            seq1 = new List<int>(),
            seq2 = new List<int>(),
            seq3 = new List<int>(),
            seq4 = new List<int>(),
            seq5 = new List<int>();

        public List<int>
            gap1 = new List<int>(),
            gap2 = new List<int>(),
            gap3 = new List<int>(),
            gap4 = new List<int>(),
            gap5 = new List<int>();

        public List<int> seqNew1, seqNew2, seqNew3, seqNew4, seqNew5;

        private void Button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10000; i++)
            {
                button1.PerformClick();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (var item in seqBestA)
            {
                switch (item)
                {
                    case 0:
                        {
                            textBox1.Text += "A";
                            break;
                        }
                    case 1:
                        {
                            textBox1.Text += "T";
                            break;
                        }
                    case 2:
                        {
                            textBox1.Text += "C";
                            break;
                        }
                    case 3:
                        {
                            textBox1.Text += "G";
                            break;
                        }
                    case 4:
                        {
                            textBox1.Text += "_";
                            break;
                        }
                    default:
                        break;
                }

            }
            foreach (var item in seqBestB)
            {
                switch (item)
                {
                    case 0:
                        {
                            textBox2.Text += "A";
                            break;
                        }
                    case 1:
                        {
                            textBox2.Text += "T";
                            break;
                        }
                    case 2:
                        {
                            textBox2.Text += "C";
                            break;
                        }
                    case 3:
                        {
                            textBox2.Text += "G";
                            break;
                        }
                    case 4:
                        {
                            textBox2.Text += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            label3.Text = bestScore.ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = iteration.ToString();
            if (iteration >= 10)
            {
                button2.Visible = true;
                
            }

            seQpBest = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            if(flagCun)
            randomFill();
            else
            {

            }
            randomGap();
            insertGaps();

            score_function(seqNew1, seqNew2, 0);
            score_function(seqNew1, seqNew3, 1);
            score_function(seqNew1, seqNew4, 2);
            score_function(seqNew1, seqNew5, 3);

            score_function(seqNew2, seqNew3, 4);
            score_function(seqNew2, seqNew4, 5);
            score_function(seqNew2, seqNew5, 6);//35

            score_function(seqNew3, seqNew4, 7);
            score_function(seqNew3, seqNew5, 8);//40

            score_function(seqNew4, seqNew5, 9);

            int temp = -999;
            int tempIndex = 0;

            for (int i = 0; i < seQpBest.Count; i++)
            {
                if (temp < seQpBest[i])
                {
                    temp = seQpBest[i];
                    tempIndex = i;
                }
            }
            bucket.Add(temp); // 35 40 2 6 45  6 98 5 99 
            bucketIndex.Add(tempIndex); //6 8 2 3 4 5 
            misGibiAlignment(bucketIndex[bucketIndex.Count - 1]);
            showValue(seqBestA, seqBestB);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            showSeqs();
            iteration++;
        }
        private void randomFill()
        {
            seq1 = new List<int>();
            seq2 = new List<int>();
            seq3 = new List<int>();
            seq4 = new List<int>();
            seq5 = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                seq1.Add(rnd.Next(0, 4));
                seq2.Add(rnd.Next(0, 4));
                seq3.Add(rnd.Next(0, 4));
                seq4.Add(rnd.Next(0, 4));
                seq5.Add(rnd.Next(0, 4));
            }
        }

        private void randomGap()
        {
            gap1 = new List<int>();
            gap2 = new List<int>();
            gap3 = new List<int>();
            gap4 = new List<int>();
            gap5 = new List<int>();

            for (int i = 0; i < 3; i++)
            {
            bas: var bucket = rnd.Next(0, 15);
                if (gap1.Contains(bucket))
                {
                    bucket = rnd.Next(0, 15);
                    goto bas;
                }
                else
                {
                    gap1.Add(bucket);
                }
            }
            gap1.Sort();

            for (int i = 0; i < 3; i++)
            {
            bas: var bucket = rnd.Next(0, 15);
                if (gap2.Contains(bucket))
                {
                    bucket = rnd.Next(0, 15);
                    goto bas;
                }
                else
                {
                    gap2.Add(bucket);
                }
            }
            gap2.Sort();

            for (int i = 0; i < 3; i++)
            {
            bas: var bucket = rnd.Next(0, 15);
                if (gap3.Contains(bucket))
                {
                    bucket = rnd.Next(0, 15);
                    goto bas;
                }
                else
                {
                    gap3.Add(bucket);
                }
            }
            gap3.Sort();

            for (int i = 0; i < 3; i++)
            {
            bas: var bucket = rnd.Next(0, 15);
                if (gap4.Contains(bucket))
                {
                    bucket = rnd.Next(0, 15);
                    goto bas;
                }
                else
                {
                    gap4.Add(bucket);
                }
            }
            gap4.Sort();

            for (int i = 0; i < 3; i++)
            {
            bas: var bucket = rnd.Next(0, 15);
                if (gap5.Contains(bucket))
                {
                    bucket = rnd.Next(0, 15);
                    goto bas;
                }
                else
                {
                    gap5.Add(bucket);
                }
            }
            gap5.Sort();

        }

        private void insertGaps()
        {
            seqNew1 = new List<int>(seq1);
            seqNew2 = new List<int>(seq2);
            seqNew3 = new List<int>(seq3);
            seqNew4 = new List<int>(seq4);
            seqNew5 = new List<int>(seq5);

            foreach (var item in gap1)
            {
                seqNew1.Insert(item, 4);
            }
            foreach (var item in gap2)
            {
                seqNew2.Insert(item, 4);
            }
            foreach (var item in gap3)
            {
                seqNew3.Insert(item, 4);
            }
            foreach (var item in gap4)
            {
                seqNew4.Insert(item, 4);
            }
            foreach (var item in gap5)
            {
                seqNew5.Insert(item, 4);
            }
        }

        private void score_function(List<int> seqA, List<int> seqB, int pBest)
        {
            int fitScr = 0;
            for (int i = 0; i < seqA.Count; i++)
            {
                fitScr = fitScr + bloCun[seqA[i], seqB[i]];
                if (fitScr > seQpBest[pBest])
                {
                    seQpBest[pBest] = fitScr;
                }
            }
        }

        private void misGibiAlignment(int bucketIndex)
        {
            if (flagCun)
            {
                bestScore = bucket[0];
                switch (bucketIndex)
                {
                    case 0:
                        {
                            seqMainA = new List<int>(seqNew1);
                            seqMainB = new List<int>(seqNew2);
                            break;
                        }
                    case 1:
                        {
                            seqMainA = new List<int>(seqNew1);
                            seqMainB = new List<int>(seqNew3);
                            break;
                        }
                    case 2:
                        {
                            seqMainA = new List<int>(seqNew1);
                            seqMainB = new List<int>(seqNew4);
                            break;
                        }
                    case 3:
                        {
                            seqMainA = new List<int>(seqNew1);
                            seqMainB = new List<int>(seqNew5);
                            break;
                        }
                    case 4:
                        {
                            seqMainA = new List<int>(seqNew2);
                            seqMainB = new List<int>(seqNew3);
                            break;
                        }
                    case 5:
                        {
                            seqMainA = new List<int>(seqNew2);
                            seqMainB = new List<int>(seqNew4);
                            break;
                        }
                    case 6:
                        {
                            seqMainA = new List<int>(seqNew2);
                            seqMainB = new List<int>(seqNew5);
                            break;
                        }
                    case 7:
                        {
                            seqMainA = new List<int>(seqNew3);
                            seqMainB = new List<int>(seqNew4);
                            break;
                        }
                    case 8:
                        {
                            seqMainA = new List<int>(seqNew3);
                            seqMainB = new List<int>(seqNew5);
                            break;
                        }
                    case 9:
                        {
                            seqMainA = new List<int>(seqNew4);
                            seqMainB = new List<int>(seqNew5);
                            break;
                        }
                }
                seqBestA = new List<int>(seqMainA);
                seqBestB = new List<int>(seqMainB);
                flagCun = false;
            }
            else // 2. kez basıyorum
            {
                if (bestScore < bucket[bucket.Count - 1])
                {
                    bestScore = bucket[bucket.Count - 1];
                    switch (bucketIndex)
                    {
                        case 0:
                            {
                                seqBestA = new List<int>(seqNew1);
                                seqBestB = new List<int>(seqNew2);
                                break;
                            }
                        case 1:
                            {
                                seqBestA = new List<int>(seqNew1);
                                seqBestB = new List<int>(seqNew3);
                                break;
                            }
                        case 2:
                            {
                                seqBestA = new List<int>(seqNew1);
                                seqBestB = new List<int>(seqNew4);
                                break;
                            }
                        case 3:
                            {
                                seqBestA = new List<int>(seqNew1);
                                seqBestB = new List<int>(seqNew5);
                                break;
                            }
                        case 4:
                            {
                                seqBestA = new List<int>(seqNew2);
                                seqBestB = new List<int>(seqNew3);
                                break;
                            }
                        case 5:
                            {
                                seqBestA = new List<int>(seqNew2);
                                seqBestB = new List<int>(seqNew4);
                                break;
                            }
                        case 6:
                            {
                                seqBestA = new List<int>(seqNew2);
                                seqBestB = new List<int>(seqNew5);
                                break;
                            }
                        case 7:
                            {
                                seqBestA = new List<int>(seqNew3);
                                seqBestB = new List<int>(seqNew4);
                                break;
                            }
                        case 8:
                            {
                                seqBestA = new List<int>(seqNew3);
                                seqBestB = new List<int>(seqNew5);
                                break;
                            }
                        case 9:
                            {
                                seqBestA = new List<int>(seqNew4);
                                seqBestB = new List<int>(seqNew5);
                                break;
                            }
                    }
                }
            }
        }
        private string showValue(List<int> seqA, List<int> seqB)
        {
            string sequence = "";

            foreach (var item in seqA)
            {
                switch (item)
                {
                    case 0:
                        {
                            sequence += "A";
                            break;
                        }
                    case 1:
                        {
                            sequence += "T";
                            break;
                        }
                    case 2:
                        {
                            sequence += "C";
                            break;
                        }
                    case 3:
                        {
                            sequence += "G";
                            break;
                        }
                    case 4:
                        {
                            sequence += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox1.Items.Add(sequence);
            sequence = "";

            foreach (var item in seqB)
            {
                switch (item)
                {
                    case 0:
                        {
                            sequence += "A";
                            break;
                        }
                    case 1:
                        {
                            sequence += "T";
                            break;
                        }
                    case 2:
                        {
                            sequence += "C";
                            break;
                        }
                    case 3:
                        {
                            sequence += "G";
                            break;
                        }
                    case 4:
                        {
                            sequence += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox1.Items.Add(sequence+" "+bestScore);
            listBox1.Items.Add("");
            return sequence;
        }

        private void showSeqs()
        {

            listBox2.Items.Clear();
            string result = "";
            foreach (var item in seq1)
            {
                switch (item)
                {
                    case 0:
                        {
                            result += "A";
                            break;
                        }
                    case 1:
                        {
                            result += "T";
                            break;
                        }
                    case 2:
                        {
                            result += "C";
                            break;
                        }
                    case 3:
                        {
                            result += "G";
                            break;
                        }
                    case 4:
                        {
                            result += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox2.Items.Add(result);
            result = "";
            foreach (var item in seq2)
            {
                switch (item)
                {
                    case 0:
                        {
                            result += "A";
                            break;
                        }
                    case 1:
                        {
                            result += "T";
                            break;
                        }
                    case 2:
                        {
                            result += "C";
                            break;
                        }
                    case 3:
                        {
                            result += "G";
                            break;
                        }
                    case 4:
                        {
                            result += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox2.Items.Add(result);
            result = "";
            foreach (var item in seq3)
            {
                switch (item)
                {
                    case 0:
                        {
                            result += "A";
                            break;
                        }
                    case 1:
                        {
                            result += "T";
                            break;
                        }
                    case 2:
                        {
                            result += "C";
                            break;
                        }
                    case 3:
                        {
                            result += "G";
                            break;
                        }
                    case 4:
                        {
                            result += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox2.Items.Add(result);
            result = "";
            foreach (var item in seq4)
            {
                switch (item)
                {
                    case 0:
                        {
                            result += "A";
                            break;
                        }
                    case 1:
                        {
                            result += "T";
                            break;
                        }
                    case 2:
                        {
                            result += "C";
                            break;
                        }
                    case 3:
                        {
                            result += "G";
                            break;
                        }
                    case 4:
                        {
                            result += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox2.Items.Add(result);
            result = "";
            foreach (var item in seq5)
            {
                switch (item)
                {
                    case 0:
                        {
                            result += "A";
                            break;
                        }
                    case 1:
                        {
                            result += "T";
                            break;
                        }
                    case 2:
                        {
                            result += "C";
                            break;
                        }
                    case 3:
                        {
                            result += "G";
                            break;
                        }
                    case 4:
                        {
                            result += "_";
                            break;
                        }
                    default:
                        break;
                }
            }
            listBox2.Items.Add(result);
            result = "";
        }
    }
}
