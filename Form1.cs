using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tic_tac_toe
{
    public partial class Form1 : Form
    {
        bool turn = true; // true = X turn, false = O turn.
        int turn_count = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("By Sheen Lian", "About");
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button_click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (turn) {
                b.Text = "X";
            }
            else {
                b.Text = "O";
            }
            turn = !turn;
            b.Enabled = false;
            turn_count++;
            check_winner();
        }

        private void check_winner() {
            bool someone_win = false;

            //horizental 
            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && !(A1.Enabled))
                someone_win = true;
            else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && !(B1.Enabled))
                someone_win = true;
            else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && !(C1.Enabled))
                someone_win = true;
            //vertical
            if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && !(A1.Enabled))
                someone_win = true;
            else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && !(B2.Enabled))
                someone_win = true;
            else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && !(C3.Enabled))
                someone_win = true;
            //Cross
            if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && !(A1.Enabled))
                someone_win = true;
            else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && !A3.Enabled)
                someone_win = true;


            if (someone_win)
            {
                disable_buttons();
                string winner = "";
                if (turn)
                {
                    winner = "O";
                }
                else
                {
                    winner = "X";
                }
                MessageBox.Show(winner + " Wins", "Results");
            }
            else {
                if (turn_count == 9) {
                    MessageBox.Show("Draw", "Results");
                }
            }
                
        }

        private void disable_buttons() {
            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Enabled = false;
                }
            }
            catch { }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            turn = true;
            turn_count = 0;

            try
            {
                foreach (Control c in Controls)
                {
                    Button b = (Button)c;
                    b.Text = "";
                    b.Enabled = true;
                }
            }
            catch { }

        }
    }
}
