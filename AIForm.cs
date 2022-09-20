using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Mankala
{
    public partial class AIForm : Form
    {

        int round = 1; // round 1 means player 1 turn, round 2 means player 2 turn
        int pitNumber = 14;
        Label[] pitsLabel;
        PictureBox[] pits; // Pit number 6 and 13 are store pits
        Random rand = new Random();
        int lastRockIndex; // Where it landed the last rock
        TreeNode root;

        public AIForm()
        {
            InitializeComponent();
            pitsLabel = new Label[14] { labelPit0, labelPit1, labelPit2, labelPit3, labelPit4, labelPit5, labelPit6, labelPit7, labelPit8,
                                   labelPit9, labelPit10, labelPit11, labelPit12, labelPit13};
            pits = new PictureBox[14] { pictureBoxPit0, pictureBoxPit1, pictureBoxPit2, pictureBoxPit3, pictureBoxPit4, pictureBoxPit5,
                                        pictureBoxPit6, pictureBoxPit7, pictureBoxPit8, pictureBoxPit9, pictureBoxPit10, pictureBoxPit11,
                                        pictureBoxPit12, pictureBoxPit13};
        }

        private void AIForm_Load(object sender, EventArgs e)
        {
            InitialiazePitCount();
        }

        // Initialiaze the pit counter
        private void InitialiazePitCount()
        {
            foreach (Label lab in pitsLabel)
            {
                // Pit6 and Pit 13 are stores
                if (lab.Name == "labelPit6" || lab.Name == "labelPit13")
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

        private void PitsClick(object sender, EventArgs e)
        {
            DrawRocks(sender as PictureBox);
            CheckMoves();
        }

        private void AIPitsClick(PictureBox aiPitClick)
        {
            DrawRocks(aiPitClick);
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
            // If the AI dosen't have no more moves we need this
            if (pit == null)
                return;

            int numberOfRocks;
            // We get eg: Pit10
            string labelName = pit.Name.Remove(0, 10);
            Label pitLabel = GetPitLabel("label" + labelName);
            numberOfRocks = Int32.Parse(pitLabel.Text);

            // Set the current pit label to 0
            pitLabel.Text = "0";
            pitLabel.Update();

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
                pitLabel.Update();

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
            if (round == 1 && lastRockIndex != 6 || round == 2 && lastRockIndex != 13)
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
            int enemyPit;
            ClearPit(GetPictureBox("pictureBoxPit" + lastRockIndex));
            GetPitLabel("labelPit" + lastRockIndex).Text = "0";
            // The oposite pit
            int opositePit = 12 - lastRockIndex;
            ClearPit(GetPictureBox("pictureBoxPit" + opositePit));
            enemyPit = Int32.Parse(GetPitLabel("labelPit" + opositePit).Text);
            GetPitLabel("labelPit" + opositePit).Text = "0";

            // Update the store label
            GetPitLabel("labelPit" + myStore).Text = (Int32.Parse(GetPitLabel("labelPit" + myStore).Text) + myPit + enemyPit).ToString();

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
            // If it's ai round we let him pick
            if (round == 2)
            {
                Thread.Sleep(500);
                AIMove();
            }
        }

        int GetPitCount(Label label)
        {
            return int.Parse(label.Text);
        }

        // Board state is represented as an int array of 14 values
        int[] GetActualBoardValue()
        {
            int[] values = new int[14];
            for (int i = 0; i < pitsLabel.Length; i++)
            {
                values[i] = GetPitCount(pitsLabel[i]);
            }
            return values;
        }

        // This function creates the tree in first state with a depth of 2
        // On root we have the actual state of the game
        // On the first level we have the possible moves of AI
        // On the second level we have the possbile moves of the player
        private void NewGameTree()
        {
            int[] board = GetActualBoardValue();
            root = new TreeNode(board);

            for(int i = 7; i < 13; i++)
            {
                
                if(board[i] != 0)
                {
                    root.AddChild(CalculateAINodes(board, i, 2), i, 1);
                }
            }
            foreach(TreeNode child in root._children)
            {
                for (int i = 0; i <= 5; i++)
                {
                    if(board[i] != 0)
                    {
                        child.AddChild(CalculateAINodes(board, i, 1), i, 2);
                    }
                }
            }
        }

        // This function calculates from the given position the output of the move
        // and it will return the board as an output, turn it's used to set the player turn that moves
        private int[] CalculateAINodes(int[] board, int position, int turn)
        {
            int[] copyBoard = new int[14];
            Array.Copy(board, copyBoard, 14);

            // We need this variable to distribute the rocks corectly
            int currentNumberOfRocks = copyBoard[position];
            int nextPitIndex = position + 1;
            int lastPitIndex;

            // Set the clicked board to 0
            copyBoard[position] = 0;

            // When currentNumberOfRocks reaches 0 then it means all the rocks are distributed
            while(currentNumberOfRocks != 0)
            {
                nextPitIndex = nextPitIndex % 14;

                // We increment the next pit count of the rocks
                copyBoard[nextPitIndex]++;
                nextPitIndex++;

                currentNumberOfRocks--;
            }

            // We need this for the special rules
            lastPitIndex = nextPitIndex - 1;

            // If the last pit is 1
            // Then get the opposite pit's rocks and put them in your store
            if (copyBoard[lastPitIndex] == 1 && !(lastPitIndex == 6 || lastPitIndex == 13))
            {
                // Calculate the opposite pit
                int oppositeIndex = 12 - lastPitIndex;

                // If there are rocks in the opposite pit, add them to your store
                if (copyBoard[oppositeIndex] != 0)
                {
                    if (round % 2 == 1)
                    {
                        // Add the rocks to player 1 store
                        copyBoard[6] = copyBoard[6] + copyBoard[oppositeIndex] + copyBoard[lastPitIndex];
                    }
                    else
                    {
                        // Add the rocks to player 2 store
                        copyBoard[13] = copyBoard[13] + copyBoard[oppositeIndex] + copyBoard[lastPitIndex];
                    }
                    //set the pits to 0 rocks
                    copyBoard[oppositeIndex] = 0;
                    copyBoard[lastPitIndex] = 0;
                }
            }

            return copyBoard;
        }

        // This function parses the tree created and it returns the best move
        // Exemplu: pe nivelul 0 o sa fie maximul pe care AI-ul il alege
        //          pe nivelul 1 o sa se afle miscarile noastre
        //          pe nivelul 2 o sa se afle miscarile player-ului
        //          nivelul 1 o ia minimul de pe nivelul 2, pentru ca algoritmul minimax presupune ca
        //          player-ul stie sa joace la fel de bine ca si el, iar de aceea ia minimul.
        private int MiniMax(TreeNode root, int depth)
        {
            if(depth == 0)
            {
                return root.value;
            }
            if(root.type == 2)
            {
                int bestValue = -100;
                foreach(TreeNode child in root._children)
                {
                    int val = MiniMax(child, depth - 1);
                    bestValue = Math.Max(bestValue, val);
                }
                root.value = bestValue;
                return bestValue;
            }
            else if(root.type == 1)
            {
                int bestValue = 100;
                foreach(TreeNode child in root._children)
                {
                    int val = MiniMax(child, depth - 1);
                    bestValue = Math.Min(bestValue, val);
                }
                root.value = bestValue;
                return bestValue;
            }
            return 0;
        }

        // AI turn to move
        private void AIMove()
        {
            #region randomMoves
            //int nextMove;
            //string pitClick = "pictureBoxPit";
            //PictureBox aiPitClick; // This is the pit that AI will choose

            //nextMove = rand.Next(7,13);

            //pitClick = pitClick + nextMove.ToString();
            //aiPitClick = GetPictureBox(pitClick);

            #endregion
            int bestMove = -100;
            PictureBox pictureBoxAiPick = new PictureBox();
            // Firstly we create the game tree
            NewGameTree();

            //Look in the current game tree for the best move and build the tree till the root has the best value;
            bestMove = MiniMax(root, 2);
            int bestPos = -1;

            //We have the best value of the move, but we do not have the move position to select
            foreach (TreeNode child in root._children)
            {
                if (child.value == bestMove)
                {
                    bestPos = child.pos;
                }
            }
            pictureBoxAiPick = GetPictureBox("pictureBoxPit" + bestPos);

            AIPitsClick(pictureBoxAiPick);
        }

        // Enable pits click for player 1
        private void EnablePitsPlayer1()
        {
            for (int i = 0; i < 6; i++)
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

    }
}
