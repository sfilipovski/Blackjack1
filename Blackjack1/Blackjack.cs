using Blackjack1.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Blackjack1
{
    public partial class Blackjack : Form
    {
        List<Card> PlayerCards = new List<Card>() { new Card(0,0,null)};
        List<Card> BankerCards = new List<Card>() { new Card(0, 0, null)};
        List<PictureBox> playerCardImg = new List<PictureBox>();
        List<PictureBox> bankerCardImg = new List<PictureBox>();
        List<int> usedCards = new List<int>();
        Random rand = new Random();

        int balance = 1000;
        int bet = 0;
        int playerSum = 0;
        int bankerSum = 0;

        public Blackjack()
        {
            InitializeComponent();
            pictureBox4.ImageLocation = "deck.png";
            pictureBox4.SizeMode = PictureBoxSizeMode.AutoSize;
            label6.Text = balance.ToString();
            reset();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            reset();
            balance = 1000;
        }

        private void Start()
        {
            if (playerSum > 0)
            {
                MessageBox.Show("Match has already started", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (Bet.Text.Length == 0 || Convert.ToInt32(Bet.Text) > balance)
            {
                MessageBox.Show("Enter valid bet ammount!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                int randCard1 = pickRandCard();
                Card card1 = deck[randCard1];
                usedCards.Add(randCard1);

                int randCard2 = pickRandCard();
                Card card2 = deck[randCard2];
                usedCards.Add(randCard2);

                PlayerCards.Add(card1);
                PlayerCards.Add(card2);

                pictureBox2.ImageLocation = card1.Image;
                pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;

                pictureBox3.ImageLocation = card2.Image;
                pictureBox3.SizeMode = PictureBoxSizeMode.AutoSize;

                int randCard3 = pickRandCard();
                Card card3 = deck[randCard3];
                usedCards.Add(randCard3);

                BankerCards.Add(card3);

                pictureBox1.ImageLocation = card3.Image;
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;

                int sum = sumPlayerCards();
                label2.Text = String.Format("Player: " + sum);
                label1.Text = String.Format("Deaker: " + sumBankerCards());
                bet = Convert.ToInt32(Bet.Text);
                balance -= bet;
                if (sum == 21)
                {
                    MessageBox.Show(String.Format("You win "+Win()), "Congratulations!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Win();
                    reset();
                }
            }

        }

        private void Deal()
        {
            if (playerSum == 0)
            {
                MessageBox.Show("Click Start!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                playerSum = 0;
                int rand1 = pickRandCard();
                Card card1 = deck[rand1];
                PlayerCards.Add(card1);
                PictureBox pb4 = new PictureBox();
                pb4.Width = 90;
                pb4.Height = 123;
                pb4.Location = new Point((pictureBox3.Location.X + (pictureBox3.Location.X - pictureBox2.Location.X)*(playerCardImg.Count+1)), pictureBox2.Location.Y);
                pb4.SizeMode = PictureBoxSizeMode.AutoSize;
                pb4.ImageLocation = card1.Image;
                playerCardImg.Add(pb4);
                this.Controls.Add(pb4);

                sumPlayerCards();
                label2.Text = String.Format("Player: " + playerSum);
                if (playerSum > 21)
                {
                    MessageBox.Show("You lose!", "Defeat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    reset();
                }
                else if(playerSum == 21)
                {
                    MessageBox.Show("You win!", "Victory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Win();
                    reset();
                }
            }
        }
        private void Stand()
        {
            if (playerSum == 0)
            {
                MessageBox.Show("Click Start!", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                int sum = 0;
                while(bankerSum <= 16)
                {
                    int rand1 = pickRandCard();
                    Card card1 = deck[rand1];
                    BankerCards.Add(card1);

                    PictureBox pb5 = new PictureBox();
                    pb5.Width = 90;
                    pb5.Height = 123;
                    pb5.Location = new Point(pictureBox1.Location.X + (pictureBox3.Location.X - pictureBox2.Location.X) * (bankerCardImg.Count + 1), pictureBox1.Location.Y);
                    pb5.SizeMode = PictureBoxSizeMode.AutoSize;
                    pb5.ImageLocation = card1.Image;
                    playerCardImg.Add(pb5);
                    this.Controls.Add(pb5);

                    bankerCardImg.Add(pb5);
                    sum = sumBankerCards();
                    label1.Text = String.Format("Dealer: "+sum);
                    if(sum > playerSum)
                    {
                        break;
                    }
                }

                if(bankerSum > playerSum && bankerSum <=21)
                {
                    MessageBox.Show("You lose!", "Defeat", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    reset();
                }
                else if(bankerSum == playerSum)
                {
                    MessageBox.Show("Tie!", "Draw", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Tie();
                    reset();
                }
                else
                {
                    MessageBox.Show("You win!", "Victory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Win();
                    reset();
                }
            }
        }

        private int pickRandCard()
        {
            int random;
            random = rand.Next(1, deck.Count);
            return random;
        }
        private void reset()
        {
            resetCards(pictureBox1);
            resetCards(pictureBox2);
            resetCards(pictureBox3);

            foreach(PictureBox pb in playerCardImg)
            {
                this.Controls.Remove(pb);
            }
            playerCardImg = new List<PictureBox>();
            foreach (PictureBox pb in bankerCardImg)
            {
                this.Controls.Remove(pb);
            }
            bankerCardImg = new List<PictureBox>();

            label6.Text = String.Format("Balance: "+balance);
            Bet.Text = "";
            bet = 0;
            label2.Text = "Player: ";
            label1.Text = "Dealer: ";
            PlayerCards.Clear();
            BankerCards.Clear();
            usedCards.Clear();
            playerSum = 0;
            bankerSum = 0;
        }
        private void resetCards(PictureBox pic)
        {
            pic.ImageLocation = "deck.png";
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
        }
        private int sumPlayerCards()
        {
            playerSum = 0;
            for(int i = 0; i < PlayerCards.Count; i++)
            {
                playerSum += PlayerCards[i].Value;
            }
            return playerSum;
        }
        private int sumBankerCards()
        {
            bankerSum = 0;
            for (int i = 0; i < BankerCards.Count; i++)
            {
                bankerSum += BankerCards[i].Value;
            }
            return bankerSum;
        }

        private void PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Deal();
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Stand();
        }
        private int Win()
        {
            balance = balance + bet*2;
            return balance;
        }
        private int Lose()
        {
            balance = balance - bet;
            return balance;
        }
        private int Tie()
        {
            balance = balance + bet;
            return balance;
        }


        List<Card> deck = new List<Card>()
        {
            new Card(2, (Shape)1, "2D.png") ,
                new Card(3, (Shape)1, "3D.png") ,
                new Card(4, (Shape)1, "4D.png") ,
                new Card(5, (Shape)1, "5D.png") ,
                new Card(6, (Shape)1, "6D.png") ,
                new Card(7, (Shape)1, "7D.png") ,
                new Card(8, (Shape)1, "8D.png") ,
                new Card(9, (Shape)1, "9D.png") ,
                new Card(10, (Shape)1, "10D.png") ,
                new Card(10, (Shape)1, "JD.png") ,
                new Card(10, (Shape)1, "QD.png") ,
                new Card(10, (Shape)1, "KD.png") ,
                new Card(1, (Shape)1, "1D.png") ,

                new Card(2, (Shape)2, "2H.png") ,
                new Card(3, (Shape)2, "3H.png") ,
                new Card(4, (Shape)2, "4H.png") ,
                new Card(5, (Shape)2, "5H.png") ,
                new Card(6, (Shape)2, "6H.png") ,
                new Card(7, (Shape)2, "7H.png") ,
                new Card(8, (Shape)2, "8H.png") ,
                new Card(9, (Shape)2, "9H.png") ,
                new Card(10, (Shape)2, "10H.png") ,
                new Card(10, (Shape)2, "JH.png") ,
                new Card(10, (Shape)2, "QH.png") ,
                new Card(10, (Shape)2, "KH.png") ,
                new Card(1, (Shape)2, "1H.png") ,

                new Card(2, (Shape)3, "2S.png") ,
                new Card(3, (Shape)3, "3S.png") ,
                new Card(4, (Shape)3, "4S.png") ,
                new Card(5, (Shape)3, "5S.png") ,
                new Card(6, (Shape)3, "6S.png") ,
                new Card(7, (Shape)3, "7S.png") ,
                new Card(8, (Shape)3, "8S.png") ,
                new Card(9, (Shape)3, "9S.png") ,
                new Card(10, (Shape)3, "10S.png") ,
                new Card(10, (Shape)3, "JS.png") ,
                new Card(10, (Shape)3, "QS.png") ,
                new Card(10, (Shape)3, "KS.png") ,
                new Card(1, (Shape)3, "1S.png") ,

                new Card(2, (Shape)4, "2C.png") ,
                new Card(3, (Shape)4, "3C.png") ,
                new Card(4, (Shape)4, "4C.png") ,
                new Card(5, (Shape)4, "5C.png") ,
                new Card(6, (Shape)4, "6C.png") ,
                new Card(7, (Shape)4, "7C.png") ,
                new Card(8, (Shape)4, "8C.png") ,
                new Card(9, (Shape)4, "9C.png") ,
                new Card(10, (Shape)4, "10C.png") ,
                new Card(10, (Shape)4, "JC.png") ,
                new Card(10, (Shape)4, "QC.png") ,
                new Card(10, (Shape)4, "KC.png") ,
                new Card(1, (Shape)4, "1C.png") ,
        };
    }
}
