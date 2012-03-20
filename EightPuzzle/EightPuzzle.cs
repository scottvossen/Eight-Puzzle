using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EightPuzzle
{
    public partial class EightPuzzle : Form
    {
        int difficulty = 0;
        bool started = false;
        bool finished = false;

        #region Constants
        public int[] pos = new int[9] {0, 1, 2, 3, 4, 5, 6, 7, 8};
        public int[][] actualPos = new int[][]
        {
            new int[] {75, 85},     new int[] {122, 85},    new int[] {169, 85}, 
            new int[] {75, 132},     new int[] {122, 132},    new int[] {169, 132},
            new int[] {75, 179},    new int[] {122, 179},   new int[] {169, 179}
        };
        public int[][] neighbors = new int[][]
        {
            new int[] {1, 3},       new int[] {0, 2, 4},        new int[] {1, 5}, 
            new int[] {0, 4, 6},    new int[] {1, 3, 5, 7},     new int[] {2, 4, 8},
            new int[] {3, 7},       new int[] {6, 4, 8},        new int[] {5, 7} 
        };
        #endregion

        public EightPuzzle()
        {
            InitializeComponent();
        }

        #region Private Utility Functions
        private void stdButtonAction(object sender)
        {
            if (started && !finished)
                move(sender);
        }

        private void move(object sender)
        {
            performMove(sender);

            #region Check for solution
            finished = true;
            int count = 0;
            
            foreach (int i in pos)
            {
                if (count.CompareTo(i) == 0) { count++; }
                else { finished = false; }
            }

            if (finished)
            {
                this.lblWinMsg.Visible = true;
                solutionButton.Enabled = false;
            }
            #endregion
        }

        private void performMove(object sender)
        {
            Button b = (System.Windows.Forms.Button)sender;
            int currPos = 0;
            int newPos = -1;

            #region find currPos
            foreach (int[] coord in actualPos)
            {
                if (coord[0] == b.Location.X && coord[1] == b.Location.Y)
                    break;
                currPos++;
            }
            #endregion

            #region find value of neighbors, if 0 set as newPos
            foreach (int i in neighbors[currPos])
            {
                int neighVal = pos[i];
                if (neighVal.CompareTo(0) == 0)
                {
                    newPos = i;
                    break;
                }
            }
            #endregion

            #region if newPos found, swap places, move buttons
            if (newPos > -1 && newPos < 9)
            {
                pos[newPos] = pos[currPos];
                pos[currPos] = 0;

                #region Move buttons
                b.Location = new System.Drawing.Point(actualPos[newPos][0], actualPos[newPos][1]);
                this.button0.Location = new System.Drawing.Point(actualPos[currPos][0], actualPos[currPos][1]);
                #endregion
            }
            #endregion
        }

        private void randomize()
        {
            int numMoves = (difficulty + 1) * 40;
            System.Random RandNum = new System.Random();
            Button[] buttons = new Button[]
            {
                button0,    button1,    button2,
                button3,    button4,    button5,
                button6,    button7,    button8
            };

            while (numMoves > 0)
            {
                int rand = RandNum.Next(0, 9);
                performMove(buttons[rand]);
                numMoves--;
            }
            this.Refresh();
        }
        #endregion

        #region Standard Buttons
        private void button1_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            stdButtonAction(sender);
        }
        #endregion

        private void randButton_Click(object sender, EventArgs e)
        {
            started = true;
            lblInstruction.Visible = false;
            randomize();
            Button b = (System.Windows.Forms.Button)sender;

            #region Disable Options
            b.Enabled = false;
            randomDegree.Enabled = false;
            #endregion
        }

        private void randomDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox selector = (System.Windows.Forms.ComboBox)sender;
            difficulty = selector.SelectedIndex;
        }

        private void solutionButton_Click(object sender, EventArgs e)
        {
            Solver s = new Solver(pos, neighbors);
            // 36 char max
            dialogBox.Text = s.getSolution();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            #region move each button back to home
            button0.Location = new System.Drawing.Point(actualPos[0][0], actualPos[0][1]);
            button1.Location = new System.Drawing.Point(actualPos[1][0], actualPos[1][1]);
            button2.Location = new System.Drawing.Point(actualPos[2][0], actualPos[2][1]);
            button3.Location = new System.Drawing.Point(actualPos[3][0], actualPos[3][1]);
            button4.Location = new System.Drawing.Point(actualPos[4][0], actualPos[4][1]);
            button5.Location = new System.Drawing.Point(actualPos[5][0], actualPos[5][1]);
            button6.Location = new System.Drawing.Point(actualPos[6][0], actualPos[6][1]);
            button7.Location = new System.Drawing.Point(actualPos[7][0], actualPos[7][1]);
            button8.Location = new System.Drawing.Point(actualPos[8][0], actualPos[8][1]);
            #endregion

            #region reset pos[] to reflect the moves
            for (int i = 0; i <= 8; i++) { pos[i] = i; }
            #endregion

            #region reset difficulty, options, enable buttons, & dialogBox
            solutionButton.Enabled = true;
            dialogBox.Text = string.Empty;
            lblInstruction.Visible = true;
            lblWinMsg.Visible = false;
            started = false;
            finished = false;
            randButton.Enabled = true;
            randomDegree.Enabled = true;
            #endregion
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Instructions inst = new Instructions();
            inst.ShowDialog();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About abt = new About();
            abt.ShowDialog();
        }
    }
}
