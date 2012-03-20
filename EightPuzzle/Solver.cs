using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EightPuzzle
{
    class Solver
    {
        private int[] pos;
        private int[][] neighbors;
        
        public Solver(int[] pos, int[][] neighbors)
        {
            this.pos = (int[]) pos.Clone();
            this.neighbors = neighbors;
        }

        public string getSolution()
        {
            string solution = displayStatus(); // string.Empty;
            int[,] pos3D = new int[3, 3];
            int freeSpace = -1; // Location of button0 in pos
            bool solved = false;
            bool moved = false;
            int optEntropy = calcEntropy(pos);
            string optMove = string.Empty;
            int[] optPos = new int[9]; // Stores best move
            int[] simPos = new int[9]; // Stores simulated move

            while (!solved)
            {
                #region Find location of button0
                for (int i = 0; i <= 8; i++)
                {
                    if (pos[i] == 0)
                    {
                        freeSpace = i;
                        break;
                    }
                }

                if (freeSpace == -1)
                    return "ERROR: Cannot find button0";
                #endregion

                #region One Move Lookahead
                foreach (int n in neighbors[freeSpace])
                {
                    int nVal = pos[n]; // The value of neighbor n
                    simPos = swap(pos, freeSpace, n);
                    int entropy = calcEntropy(simPos);
                    if (entropy < optEntropy)
                    {
                        optPos = simPos;
                        moved = true;
                        optMove = "move " + nVal + "\n";
                        optEntropy = entropy;
                    }
                }
                #endregion

                if (moved)
                {
                    pos = optPos;
                    solution += optMove;
                    moved = false;
                }
                else
                {
                    solution += "Incomplete Solution.";
                    break;
                }

                #region Check for solution
                solved = true;
                for (int i = 0; i <= 8; i++)
                {
                    if (i.CompareTo(pos[i]) != 0) { solved = false; break; }
                }
                #endregion
            }
            if (solved) { solution += "Complete Solution!"; }
            return solution;
        }

        private int[] swap(int[] pos, int a, int b)
        {
            int[] POS = (int[])pos.Clone();
            int temp = POS[a];
            POS[a] = POS[b];
            POS[b] = temp;
            return POS;
        }

        private string displayStatus()
        {
            string status = string.Empty;
            /*status = "pos: " + pos[0] + ", " + pos[1] + ", " + pos[2] + ", " +
                                      pos[3] + ", " + pos[4] + ", " + pos[5] + ", " +
                                      pos[6] + ", " + pos[7] + ", " + pos[8] + ", \n";*/
            status += "Current Entropy: " + calcEntropy(pos) + "\n";
            return status;
        }

        private int calcEntropy(int[] pos)
        {
            int entropy = 0;
            # region Load pos3D[,]
            int[,] pos3D =
            {
                { pos[0], pos[3], pos[6] },
                { pos[1], pos[4], pos[7] },
                { pos[2], pos[5], pos[8] }
            };
            #endregion
            #region Define home[][]
            int[][] home = new int[9][]
            {
                new int [] { 0, 0 },
                new int [] { 1, 0 },
                new int [] { 2, 0 },
                new int [] { 0, 1 },
                new int [] { 1, 1 },
                new int [] { 2, 1 },
                new int [] { 0, 2 },
                new int [] { 1, 2 },
                new int [] { 2, 2 }
            };
            #endregion

            for (int i = 1; i <= 8; i++)
            {
                int i_xVal = 0; // button(i)'s x coordinate in pos3D
                int i_yVal = 0; // button(i)'s y coordinate in pos3D

                #region find button(i)
                bool found = false;
                for (int y = 0; y <= 2; y++)
                {
                    for (int x = 0; x <= 2; x++)
                    {
                        if (pos3D[x, y] == i)
                        {
                            i_xVal = x;
                            i_yVal = y;
                            found = true;
                            break;
                        }
                    }
                    if (found) { break; }
                }
                #endregion

                // find how far away from home it is for x
                entropy += Math.Abs(i_xVal - home[i][0]);

                // find how far away from home it is for y
                entropy += Math.Abs(i_yVal - home[i][1]);
            }
            return entropy;
        }
    }
}
