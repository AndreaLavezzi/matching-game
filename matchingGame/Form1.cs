using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace matchingGame
{
    
    public partial class Form1 : Form
    {
        PictureBox[] pictureBoxesList = new PictureBox[18];
        PictureBox card1, card2;
        bool isFirstCard = true;
        
        public Form1()
        {
            InitializeComponent();
        }

        void LoadImages()
        {
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18 };
            for(int i = 0; i < pictureBoxes.Length; i++)
            {
                pictureBoxesList[i] = pictureBoxes[i];
            }
            int[] imagesIndexes = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
            
            Random random = new Random();
            imagesIndexes = imagesIndexes.OrderBy(x => random.Next()).ToArray();

            int j = 1;
            for(int i = 0; i < pictureBoxesList.Length; i++)
            {                
                pictureBoxesList[i].Image = Image.FromFile(@"images/0.jpg");
                pictureBoxesList[i].Name = imagesIndexes[i].ToString() + ".jpg";
                pictureBoxesList[i].Click += new EventHandler(CheckMatch);
            }
        }

        void CheckMatch(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            if(card1 != null || card2 != null)
            {
                if (isFirstCard)
                {
                    card1 = clickedCard;
                    card1.Image = Image.FromFile(@"images/" + card1.Name);
                    isFirstCard = false;
                }
                else
                {
                    card2 = clickedCard;
                    if (card1.Name == card2.Name && card1.Location != card2.Location)
                    {
                        MessageBox.Show("BROOO");
                    }
                    else
                    {
                        card1.Image = Image.FromFile(@"images/0.jpg");
                        card2.Image = Image.FromFile(@"images/0.jpg");
                        card1 = null;
                        card2 = null;
                    }
                    isFirstCard = true;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadImages();
        }

        public void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }
    }
}
