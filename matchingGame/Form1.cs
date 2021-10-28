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
        PictureBox[] pictureBoxesList = new PictureBox[18]; //array globale delle picturebox
        Player p1 = new Player("player1");  //giocatore1 di nome "player1"
        Player p2 = new Player("player2"); //giocatore2 di nome "player2"
        int turn = 0;       //indica di chi è il turno corrente
        int card1, card2;   //contengono gli indici nell'array di picturebox delle tessere cliccate
        int matchedCards = 0;   //numero di coppie trovate
        bool isFirstCard = true;    //indica se la tessera cliccata è la prima della coppia
        bool canClick = true;       //se false, non permette al giocatore di cliccare altre tessere
        
        public Form1()
        {
            InitializeComponent();
        }

        //al caricamento del form vengono caricate le immagini e scelto il giocatore che inizia la partita
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadImages();
            ChangeTurn();
        }

        void LoadImages()
        {
            
            int[] imagesIndexes = { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9 };     //ai file delle immagini sono stati assegnati dei numeri. ogni numero è ripetuto due volte per ottenere sul tavolo 2 tessere con la stessa immagine.
            PictureBox[] pictureBoxes = { pictureBox1, pictureBox2, pictureBox3, pictureBox4, pictureBox5, pictureBox6, pictureBox7, pictureBox8, pictureBox9, pictureBox10, pictureBox11, pictureBox12, pictureBox13, pictureBox14, pictureBox15, pictureBox16, pictureBox17, pictureBox18 }; //array locale di carte
            
            for(int i = 0; i < pictureBoxes.Length; i++)
            {
                pictureBoxesList[i] = pictureBoxes[i];      //inserisce nell'array globale le picturebox presenti in quello locale
            }        
            
            Random random = new Random();
            imagesIndexes = imagesIndexes.OrderBy(x => random.Next()).ToArray();    //viene mescolato l'array con i numeri per avere ad ogni partita le tessere mescolate in modo casuale

            for(int i = 0; i < pictureBoxesList.Length; i++)    //viene assegnata a ogni picturebox l'immagine di default come immagine, il nome dell'immagine che corrisponde alla picturebox come nome e un evento click per scoprire la tessera 
            {                
                pictureBoxesList[i].Image = Image.FromFile(@"images/0.jpg");
                pictureBoxesList[i].Name = imagesIndexes[i].ToString() + ".jpg";
                pictureBoxesList[i].Click += new EventHandler(CheckMatch);
            }
            UpdateLabels();     //vengono aggiornate le etichette che mostrano il punteggio e di quale giocatore è il turno
        }

        //chiamata a ogni click di una carta
        async void CheckMatch(object sender, EventArgs e)
        {
            PictureBox clickedCard = sender as PictureBox;      //si assegna alla variabile "clickedCard" la picturebox che ha chiamato l'evento
            int index = Array.IndexOf(pictureBoxesList, clickedCard);   //si trova l'indice della picturebox nell'array globale
            if(canClick == true)
            {
                if (isFirstCard)
                {
                    card1 = index;  
                    pictureBoxesList[card1].Image = Image.FromFile(@"images/" + pictureBoxesList[card1].Name);  //viene "scoperta" la tessera in base al nome che le è stato dato, il quale corrisponde a un file immagine nella cartella "images"
                    isFirstCard = false;
                }
                else if(pictureBoxesList[card1].Location != pictureBoxesList[index].Location) //se non è la prima tessera cliccata e viene cliccata una tessera in una posizione diversa dalla prima
                {
                    card2 = index;
                    pictureBoxesList[card2].Image = Image.FromFile(@"images/" + pictureBoxesList[card2].Name); //viene "scoperta" la secoda tessera
                    canClick = false;   //viene impedito che si possa cliccare in modo da non permettere che, durante il momento in cui le carte vengono mostrate, non si possano cliccare altre carte
                    if (pictureBoxesList[card1].Name == pictureBoxesList[card2].Name) //se i nomi, e quindi le immagini, della tessera corrispondono
                    {
                        pictureBoxesList[card1].Click -= CheckMatch;    //si rimuovono gli eventi click e le immagini
                        pictureBoxesList[card2].Click -= CheckMatch;
                        await Task.Delay(1000); //dato che si tratta di una funzione asincrona si fa aspettare un secondo per mostrare quali carte sono state cliccate
                        pictureBoxesList[card1].Image = null;
                        pictureBoxesList[card2].Image = null;
                        matchedCards++; //incrementa il valore di coppie trovate
                        UpdateScore();  //viene aggiornato il punteggio
                    }
                    else
                    {
                        await Task.Delay(500);  //se le carte non coincidono, si mostrano per mezzo secondo e poi vengono "ricoperte", impostando la loro immagine a quella di default
                        pictureBoxesList[card1].Image = Image.FromFile(@"images/0.jpg");
                        pictureBoxesList[card2].Image = Image.FromFile(@"images/0.jpg");
                        ChangeTurn();   //viene cambiato il turno
                    }
                    isFirstCard = true;     //sia in caso di coppia trovata che non, vengono reimpostate le variabili al valore iniziale
                    canClick = true;
                }
            }
            if(matchedCards == 9)   //se le coppie trovate sono 9 (il numero totale di coppie sul tavolo)
            {
                matchedCards = 0;   //viene reimpostato il numero di coppie a 0
                EndGame(0);     //e la partita viene terminata con codice 0
            }
        }

        //codice 0 = partita svolta normalmente, codice 1 = partita terminata tramite resa
        void EndGame(int code)
        {
            if(code == 0)
            { 
                switch (p1.CompareTo(p2))   //vengono comparati i punteggi dei due giocatori e viene modificata una label per comunicare il vincitore
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
                if(turn == 1)   //se il turno è del giocatore 1 vince il giocatore 2, se no il contrario
                {
                    whoseTurn.Text = "Ha vinto " + p2.playerName;
                } else
                {
                    whoseTurn.Text = "Ha vinto " + p1.playerName;
                }
            }
            canClick = false;   //viene impedito di cliccare
            surrender.Visible = false;  //vengono nascosti i bottoni per arrendersi e per cambiare turno e viene mostrato quello per giocare ancora
            skipTurn.Visible = false;
            playAgain.Visible = true;
        }

        //permette al giocatore nel turno corrente di arrendersi e far vincere il giocatore avversario, anche se colui che si arrende è in vantaggio
        private void surrender_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Attenzione! Se ti arrendi, sarai il giocatore perdente anche se sei hai il punteggio più alto. Vuoi arrenderti lo stesso?", "Vuoi veramente arrenderti?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                EndGame(1); //la partita viene conclusa con codice 1 (resa)
            }
        }

        //bottone che cambia il turno
        private void skipTurn_Click(object sender, EventArgs e)
        {
            ChangeTurn();
        }

        void ChangeTurn()
        {
            while (turn == 0) //se ci si trova all'inizio della partita viene generato un numero random tra -1 e 2 (2 escluso), e finchè esce 0 viene estratto di nuovo
            {
                Random random = new Random();
                turn = random.Next(-1, 2);
            }
            turn = turn * -1; //per cambiare il turno viene moltiplicato il numero che indica di chi è il turno per -1
            switch (turn)
            {
                case 1: //se il valore è 1 il turno è del giocatore 1
                    whoseTurn.Text = "E' il turno di " + p1.playerName;
                    break;
                case -1: //se il valore è 2 il turno è del giocatore 2
                    whoseTurn.Text = "E' il turno di " + p2.playerName;
                    break;
            }
        }

        //al clic del bottone "gioca ancora" viene resettato tutto alla situazione iniziale
        private void playAgain_Click(object sender, EventArgs e)
        {
            turn = 0; 
            LoadImages();
            ChangeTurn();
            p1.ResetScore();
            p2.ResetScore();
            UpdateLabels();
            playAgain.Visible = false;
            skipTurn.Visible = true;
            surrender.Visible = true;
        }

        //aggiunge un punto in base a di chi sia il turno
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
            UpdateLabels();
        }

        //aggiorna le label in base al punteggio dei giocatori
        void UpdateLabels()
        {
            p1Score.Text = p1.ToString();
            p2Score.Text = p2.ToString();
        }
    }

    //il singolo giocatore, ha un nome e un punteggio come attributi, gli si può aggiungere un punteggio,
    //resettare il punteggio, ottenere una stringa che comunica il punteggio e compararlo a un altro giocatore in base al punteggio
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
            if (this.score > player2.score) //compara i punteggio
            {
                return 1;   //se maggiore ritorna 1
            }
            else if (this.score < player2.score)    
            {
                return -1; //se minore ritorna 2
            }
            else
            {
                return 0;   //se uguale ritorna 0
            }
        }
    }
}
