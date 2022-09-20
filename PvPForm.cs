using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mankala
{
    public partial class PvPForm : Form
    {
        int round = 1; // round 1 means player 1 turn, round 2 means player 2 turn
        int pitNumber = 14;
        Label[] pitsLabel;
        PictureBox[] pits; // Pit number 6 and 13 are store pits
        Random rand = new Random();
        int lastRockIndex; // Where it landed the last rock

        public PvPForm()
        {
            InitializeComponent();
            pitsLabel = new Label[14] { labelPit0, labelPit1, labelPit2, labelPit3, labelPit4, labelPit5, labelPit6, labelPit7, labelPit8,
                                   labelPit9, labelPit10, labelPit11, labelPit12, labelPit13};
            pits = new PictureBox[14] { pictureBoxPit0, pictureBoxPit1, pictureBoxPit2, pictureBoxPit3, pictureBoxPit4, pictureBoxPit5,
                                        pictureBoxPit6, pictureBoxPit7, pictureBoxPit8, pictureBoxPit9, pictureBoxPit10, pictureBoxPit11,
                                        pictureBoxPit12, pictureBoxPit13};
        }

        private void PvPForm_Load(object sender, EventArgs e)
        {
            InitialiazePitCount();
        }

        // Initialize the pit counter
        private void InitialiazePitCount()
        {
            foreach(Label lab in pitsLabel)
            {
                // index 18 and 27 are pits 6 and 13 that are store pits
                if (lab.TabIndex == 18 || lab.TabIndex == 27)
                    continue;
                lab.Text = "4";
            }
        }

        // Start the game and draw the rocks
        private void buttonStart_Click(object sender, EventArgs e)
        {
            DrawInitialRocks();
            buttonStart.Enabled = false;
            buttonStart.Hide();

            EnablePitsPlayer1();
        }

        // Enable pits click for player 1
        private void EnablePitsPlayer1 ()
        {
            for(int i = 0; i< 6; i++)
            {
                pits[i].Click += new EventHandler(PitsClick);
            }
        }

        // Disable pits click for player 1
        private void DisablePitsPlayer1()
        {
            for (int i = 0; i < 6; i++)
            {
                pits[i].Click -= new EventHandler(PitsClick);
            }
        }

        // Enable pits click for player 2
        private void EnablePitsPlayer2()
        {
            for (int i = 7; i < 13; i++)
            {
                pits[i].Click += new EventHandler(PitsClick);
            }
        }

        // Disable pits click for player 2
        private void DisablePitsPlayer2()
        {
            for (int i = 7; i < 13; i++)
            {
                pits[i].Click -= new EventHandler(PitsClick);
            }
        }

        private void PitsClick(object sender, EventArgs e)
        {
            DrawRocks(sender as PictureBox);
            CheckMoves();
        }

        private Label GetPitLabel(string name)
        {
            return this.Controls.Find(name, true).FirstOrDefault() as Label;
        }

        private PictureBox GetPictureBox(string name)
        {
            return this.Controls.Find(name, true).FirstOrDefault() as PictureBox;
        }

        // Draws the rocks after you click on a enabled pit
        private void DrawRocks(PictureBox pit)
        {
            int numberOfRocks;
            // We get eg: Pit10
            string labelName = pit.Name.Remove(0, 10);
            Label pitLabel = GetPitLabel("label" + labelName);
            numberOfRocks = Int32.Parse(pitLabel.Text);

            // Set the current pit label to 0
            pitLabel.Text = "0";

            //First we clear the pit
            Rectangle rect;
            Graphics g = ClearPit(pit);

            // Check the number pit so we can know where to start from to draw
            int currentPitNumber = Int32.Parse(pit.Name.Remove(0, 13));

            // Draw the rocks on the other pits
            for (int j = 0; j < numberOfRocks; j++)
            {
                // Coordinates
                int x = 25, y = 20;
                int step = 15, start = 25;
                int counter = 0;

                // If we are at the last pit we reset on 0
                if (currentPitNumber == 13)
                {
                    currentPitNumber = 0;
                }
                else
                    currentPitNumber++;

                // Take the new pit and label to draw there
                pit = GetPictureBox("PictureBoxPit" + currentPitNumber);
                pitLabel = GetPitLabel("LabelPit" + currentPitNumber);
                pitLabel.Text = (Int32.Parse(pitLabel.Text) + 1).ToString();

                // Check if it's the last rock that we draw
                if (j == numberOfRocks - 1)
                {
                    lastRockIndex = currentPitNumber;
                }

                // Clear the pit before we draw the new rocks
                rect = new Rectangle(0, 0, 100, 100);
                g.FillRectangle(new SolidBrush(Color.Transparent), rect);
                pit.Refresh();

                // Draw the rocks on the new pit
                for (int r = 0; r < Int32.Parse(pitLabel.Text); r++)
                {
                    // Draw the rocks
                    g = System.Drawing.Graphics.FromHwnd(pit.Handle);
                    rect = new Rectangle(x, y, 11, 11);
                    Color color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                    g.DrawEllipse(Pens.Black, rect);
                    g.FillEllipse(new SolidBrush(color), rect);
                    counter++;
                    x += step;

                    if (counter == 3)
                    {
                        x = start;
                        y += step;
                        counter = 0;
                    }
                }

            }
        }

        // Clears the pit
        private static Graphics ClearPit(PictureBox pit)
        {
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(pit.Handle);
            Rectangle rect = new Rectangle(0, 0, 100, 100);
            g.FillRectangle(new SolidBrush(Color.Transparent), rect);
            pit.Refresh();
            return g;
        }

        // Draws the pits in their initial place
        private void DrawInitialRocks()
        {
            for (int i = 0; i < pitNumber; i++)
            {
                int numberOfRocks = Int32.Parse(pitsLabel[i].Text);

                System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(pits[i].Handle);
                DrawAPit(numberOfRocks, g);
            }
        }

        private void DrawAPit(int numberOfRocks, Graphics g)
        {
            // Coordinates
            int x = 25, y = 20;
            int step = 15, start = 25;
            int counter = 0;

            for (int j = 0; j < numberOfRocks; j++)
            {

                Rectangle rect = new Rectangle(x, y, 11, 11);
                Color color = Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256));
                g.DrawEllipse(Pens.Black, rect);
                g.FillEllipse(new SolidBrush(color), rect);
                counter++;
                x += step;

                if (counter == 3)
                {
                    x = start;
                    y += step;
                    counter = 0;
                }

            }
        }

        private void CheckMoves()
        {
            int emptyPits = 0;

            // If the last rock hits an empty pit you get both the pits rocks
            Label lastRockHit = GetPitLabel("labelPit" + lastRockIndex);
            if (Int32.Parse(lastRockHit.Text) == 1 && round == 1 && lastRockIndex < 6 && Int32.Parse(GetPitLabel("labelPit" + (12 - lastRockIndex)).Text) != 0)
            {
                int myPit = 1, myStore = 6;
                MankalaTrick(myPit, myStore);
            }
            else
              if (Int32.Parse(lastRockHit.Text) == 1 && round == 2 && lastRockIndex > 6 && lastRockIndex != 13 && Int32.Parse(GetPitLabel("labelPit" + (12 - lastRockIndex)).Text) != 0)
            {
                int myPit = 1, myStore = 13;
                MankalaTrick(myPit, myStore);
            }

            // If the last rock hits your store then you can move again
            if (round == 1 && lastRockIndex != 6 || round ==2 && lastRockIndex != 13)
            {
                if (round == 1)
                {
                    DisablePitsPlayer1();
                    EnablePitsPlayer2();
                    round = 2;
                }
                else
                {
                    DisablePitsPlayer2();
                    EnablePitsPlayer1();
                    round = 1;
                }
            }

            foreach (Label lab in pitsLabel)
            {
                // index 18 and 27 are pits 6 and 13 that are store pits
                if (lab.TabIndex == 18 || lab.TabIndex == 27)
                    continue;
                if (lab.Text == "0")
                    emptyPits++;
            }
            EndOfGame(emptyPits);

        }

        // Take both pits if the last rock hits an empty pit
        private void MankalaTrick(int myPit, int myStore)
        {
            int enemypit;
            ClearPit(GetPictureBox("pictureBoxPit" + lastRockIndex));
            GetPitLabel("labelPit" + lastRockIndex).Text = "0";
            // The oposite pit
            int opositePit = 12 - lastRockIndex;
            ClearPit(GetPictureBox("pictureBoxPit" + opositePit));
            enemypit = Int32.Parse(GetPitLabel("labelPit" + opositePit).Text);
            GetPitLabel("labelPit" + opositePit).Text = "0";

            // Update the store label
            GetPitLabel("labelPit" + myStore).Text = (Int32.Parse(GetPitLabel("labelPit" + myStore).Text) + myPit + enemypit).ToString();

            // Draw the rocks on the store pit
            System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd(GetPictureBox("pictureBoxPit" + myStore).Handle);
            DrawAPit(Int32.Parse(GetPitLabel("labelPit" + myStore).Text), g);
        }

        private void EndOfGame(int emptyPits)
        {
            if (emptyPits == 12)
            {
                DisablePitsPlayer1();
                DisablePitsPlayer2();
                if (Int32.Parse(pitsLabel[6].Text) > Int32.Parse(pitsLabel[13].Text))
                {
                    MessageBox.Show("Player 1 wins!");
                }
                else if (Int32.Parse(pitsLabel[6].Text) < Int32.Parse(pitsLabel[13].Text))
                {
                    MessageBox.Show("Player 2 wins!");
                }
                else
                {
                    MessageBox.Show("It's a tie!");
                }
                this.Close();
            }
        }
    }
}
