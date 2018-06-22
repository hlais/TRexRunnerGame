using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TRexRunnerGame
{
    public partial class Form1 : Form
    {
        bool jumping = false;
        int jumpSpeed = 10;
        int force = 12; // force of the jump
        int score = 0;
        int obstacleSpeed = 10;
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            resetGame(); // run the reset game function
        }

        private void gameEvent(object sender, EventArgs e)
        {
            //.Top gets/sets distance of image pixels(trex), we link it to its jumpSpeed
            trex.Top += jumpSpeed;

            //update score;
            scoreText.Text = $"Score: {score}";
            //if jumping is true and force less than zero
            if (jumping && force < 0)
            {
                jumping = false;
            }

            //if jumping change jump speed to -12, reduce force by 1
            if (jumping)
            {
                jumpSpeed = -12;
                force -= 1;
            }
            else
            {
                //revert to original sprite position 
                jumpSpeed = 12;

            }

            foreach ( Control x in this.Controls)
            {
                //if element is picture box and it has the tag obstacle
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {
                    x.Left -= obstacleSpeed;// move obstacle towards the left

                    if (x.Left + x.Width < -120) //when image moves off screen
                    {
                        x.Left = this.ClientSize.Width + rnd.Next(200, 800); //ClientSize gets length of form  + random added distance
                        score++;
                    }
                    //if player collides with 'obstacle'
                    if (trex.Bounds.IntersectsWith(x.Bounds))
                    {
                        //setting game over state
                        gameTimer.Stop();
                        trex.Image = Properties.Resources.dead;
                        scoreText.Text = $"Press R to Restart" + $" || Score: {score} ";
                    }
                }
            }
            if (trex.Top >= 380 && !jumping)
            {
                force = 12;
                trex.Top = floor.Top - trex.Height;
            }
            if (score >= 10)
            {
                obstacleSpeed = 15;
            }
        }
        

        private void keyisdown(object sender, KeyEventArgs e)
        {
            //make space bar jump, if player is not jumping then allow jumping
            if (e.KeyCode == Keys.Space && !jumping)
            {
                jumping = true;
            }
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            //R to run reset method
            if (e.KeyCode == Keys.R)
            {
                resetGame();
            }
            if (jumping)
            {
                jumping = false;
            }

        }
        public void resetGame()
        {
            force = 12;
            //put player on top of floor
            trex.Top = floor.Top - trex.Height;
            jumpSpeed = 0;
            jumping = false;
            score = 0;
            obstacleSpeed = 10;
            scoreText.Text += $"Score {score}";
            trex.Image = Properties.Resources.running;

            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && (string)x.Tag == "obstacle")
                {   
                    int position = rnd.Next(600, 1000);
                    //643 is the form's width
                    x.Left = 640 + (x.Left + position + x.Width * 3);//change left position randomly at the begining of the game
                }
            }
            gameTimer.Start();
        }
    }
}
