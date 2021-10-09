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
        public Form1()
        {
            InitializeComponent();
        }

        void LoadImages()
        {
            int[] imagesIndexes = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18 };
            Random random = new Random();
            imagesIndexes = imagesIndexes.OrderBy(x => random.Next()).ToArray();
            bool doubleImage = false;
            int j = 1;
            for(int i = 0; i<18; i++)
            {
                
                pictureBoxes[i].Image = Image.FromFile(@"images/" + imagesIndexes[i].ToString() + ".jpg");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadImages();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("hai cliccato su pic1");
        }
    }
}
