﻿using System;
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
        Player p1 = new Player("player1");
        Player p2 = new Player("player2");
        int turn = 0;
        int card1, card2;
        int matchedCards = 0;
        bool isFirstCard = true;
        bool canClick = true;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadImages();
            ChangeTurn();
        }

        void LoadImages()
        {
            int[] imagesIndexes = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18 };
            
            for(int i = 0; i < pictureBoxes.Length; i++)
            {
                pictureBoxesList[i] = pictureBoxes[i];
            }        
            
            Random random = new Random();
            imagesIndexes = imagesIndexes.OrderBy(x => random.Next()).ToArray();

            for(int i = 0; i < pictureBoxesList.Length; i++)
            {                
                pictureBoxesList[i].Image = Image.FromFile(@"images/0.jpg");
                pictureBoxesList[i].Name = imagesIndexes[i].ToString() + ".jpg";
                pictureBoxesList[i].Click += new EventHandler(CheckMatch);
            }
            p1Score.Text = p1.ToString();
            p2Score.Text = p2.ToString();
        }

        async void CheckMatch(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;
            int index = Array.IndexOf(pictureBoxesList, clickedCard);
            if(canClick == true)
            {
                if (isFirstCard)
                {
                    card1 = index;
                    pictureBoxesList[card1].Image = Image.FromFile(@"images/" + pictureBoxesList[card1].Name);
                    isFirstCard = false;
                }
                else if(pictureBoxesList[card1].Location != pictureBoxesList[index].Location)
                {
                    card2 = index;
                    pictureBoxesList[card2].Image = Image.FromFile(@"images/" + pictureBoxesList[card2].Name);
                    canClick = false;
                    if (pictureBoxesList[card1].Name == pictureBoxesList[card2].Name)
                    {
                        pictureBoxesList[card1].Click -= CheckMatch;
                        pictureBoxesList[card2].Click -= CheckMatch;
                        await Task.Delay(1000);
                        pictureBoxesList[card1].Image = null;
                        pictureBoxesList[card2].Image = null;
                        UpdateScore();
                        
                    }
                    else
                    {
                        await Task.Delay(500);
                        pictureBoxesList[card1].Image = Image.FromFile(@"images/0.jpg");
                        pictureBoxesList[card2].Image = Image.FromFile(@"images/0.jpg");
                        ChangeTurn();
                    }
                    isFirstCard = true;
                    canClick = true;
                }
            }
            if(matchedCards == 9)
            {
                matchedCards = 0;
                EndGame(0);
            }
        }

        //codice 0 = partita svolta normalmente, codice 1 = partita terminata tramite resa
        void EndGame(int code)
        {
            if(code == 0)
            { 
                switch (p1.CompareTo(p2))
                {
                    case 1:
                        whoseTurn.Text = "Ha vinto " + p1.playerName;
                        break;
                    case -1:
                        whoseTurn.Text = "Ha vinto " + p2.playerName;
                        break;
                    case 0:
                        whoseTurn.Text = "PAREGGIO!";
                        break;
                }
            } else if(code == 1)
            {
                if(turn == 1)
                {
                    whoseTurn.Text = "Ha vinto " + p2.playerName;
                } else
                {
                    whoseTurn.Text = "Ha vinto " + p1.playerName;
                }
            }
            surrender.Visible = false;
            skipTurn.Visible = false;
            playAgain.Visible = true;

        }

        private void surrender_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Attenzione! Se ti arrendi, sarai il giocatore perdente anche se sei in vantaggio di punteggio. Vuoi arrenderti lo stesso?", "Vuoi veramente arrenderti?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                EndGame(1);
            }
        }

        private void skipTurn_Click(object sender, EventArgs e)
        {
            ChangeTurn();
        }

        void ChangeTurn()
        {
            while (turn == 0)
            {
                Random random = new Random();
                turn = random.Next(-1, 2);
            }
            turn = turn * -1;
            switch (turn)
            {
                case 1:
                    whoseTurn.Text = "E' il turno di " + p1.playerName;
                    break;
                case -1:
                    whoseTurn.Text = "E' il turno di " + p2.playerName;
                    break;
            }
        }

        private void playAgain_Click(object sender, EventArgs e)
        {
            LoadImages();
            ChangeTurn();
            p1.ResetScore();
            p2.ResetScore();
            p1Score.Text = p1.ToString();
            p2Score.Text = p2.ToString();
        }

        void UpdateScore()
        {
            if (turn == 1)
            {
                p1.AddScore(1);
            }
            else
            {
                p2.AddScore(1);
            }
            p1Score.Text = p1.ToString();
            p2Score.Text = p2.ToString();
            matchedCards++;
        }
    }

    class Player
    {
        public string playerName { get; }
        int score = 0;
        public Player(string playerName)
        {
            this.playerName = playerName;
        }

        public void AddScore(int score)
        {
            this.score += score;
        }

        public void ResetScore()
        {
            score = 0;
        }

        public override string ToString()
        {
            return $"Punteggio di {this.playerName}: {this.score}";
        }

        public int CompareTo(Player player2)
        {
            if (this.score > player2.score)
            {
                return 1;
            }
            else if (this.score < player2.score)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
