using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace snake
{
    
    public partial class Form1 : Form
    {
        private Snake Snake = new Snake();
        // creating an list array for the snake
        private GoodApple food = new GoodApple(); // creating a single Circle class called food
        private BadApple poison = new BadApple();
        private Block block = new Block();
        private List<Shape> shapes = new List<Shape>();
        private bool pause = false;
        private int maxXpos;
        private int maxYpos;
        private int flag=0;
        public Form1()
        {
            InitializeComponent();

            new Setting(); // linking the Settings Class to this Form

            gameTimer.Interval = 1000 / Setting.Speed; // Changing the game time to settings speed
            gameTimer.Tick += updateScreen; // linking an updateScreen function to the timer
            gameTimer.Start(); // starting the timer

            startGame(); // running the start game function
        }
        ~Form1()// the destructor is mereley formal since in C# , objects are being deleted by the
            // the system itself  ( and the garbage collector) , and , GameOver function does the same effect
        {
            Snake = null;
            food = null;
            poison = null;
            shapes = null;
            pause = true;
            maxXpos = 0;
            maxYpos = 0;
            flag = 0;
            label4.Visible = true;
            label3.Visible = true;
            Setting.GameOver = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void keyisdown(object sender, KeyEventArgs e)
        {
            // the key down event will trigger the change state from the Input class
            Input.changeState(e.KeyCode, true);
        }

        private void keyisup(object sender, KeyEventArgs e)
        {
            // the key up event will trigger the change state from the Input class
            Input.changeState(e.KeyCode, false);
        }

        private void updateGraphics(object sender, PaintEventArgs e)
        {
            // this is where we will see the snake and its parts moving

            Graphics canvas = e.Graphics; // creates a new graphics class called canvas

            if (Setting.GameOver == false)
            {
                if (pause)
                {
                    label4.Visible = true;
                }
                else
                {
                    label4.Visible = false;
                }
                // if the game is not over then we do the following
                Snake.paint( sender, e);
                for (int i = 0; i < shapes.Count; i++)
                {
                    shapes[i].paint(sender, e);
                }

            }
            else
            {
                // this part will run when the game is over
                // it will show the game over text and make the label 3 visible on the screen

                string gameOver = "Game Over \n" + "Final Score is " + Setting.Score + "\n Press enter to Restart \n";
                label3.Text = gameOver;
                label3.Visible = true;
            }
            
            if(Setting.Score % 500 == 0)
            {
                if (flag != 1)
                {
                    Thread.Sleep(2);
                    Block b = new Block { };
                    b.generateFood(maxXpos, maxYpos);
                    shapes.Add(b);
                    flag = 1;
                }
            }
            else
            {
                flag = 0;
            }
            Snake.checkIfeat(shapes,maxXpos, maxYpos);
        }
        private void startGame()
        {
            maxXpos = pbCanvas.Size.Width / Setting.Width;
            maxYpos = pbCanvas.Size.Height / Setting.Height;
            shapes.Add(poison);
            shapes.Add(food);
            shapes.Add(block);
            // this is the start game function
            label4.Visible = false;
            label3.Visible = false; // set label 3 to be invisible
            new Setting(); // create a new instance of settings
            Snake.startGame();
            label2.Text = Setting.Score.ToString(); // show the score via label 2
            for(int i = 0; i < shapes.Count; i++)
            {
                Thread.Sleep(2);
                shapes[i].generateFood(maxXpos, maxYpos);
            }
            
        }

        private void updateScreen(object sender, EventArgs e)
        {
            gameTimer.Interval = 1000 / Setting.Speed;
            label2.Text = Setting.Score.ToString(); // show the score on the label 2
            // this is the Timers update screen function. 
            // each tick will run this function

            if (Setting.GameOver == true)
            {
                // if the game over is true and player presses enter
                // we run the start game function
                shapes.Clear();
                if (Input.KeyPress(Keys.Enter))
                {     
                    startGame();
                }
            }
            else
            {
                //if the game is not over, then the following commands will be executed

                if(Input.KeyPress(Keys.Escape))
                {
                    pause = !(pause);// pausing the game
                }

                // moving the snake to different directions
                if (Input.KeyPress(Keys.Right) && Setting.direction != Directions.Left)
                {
                    Setting.direction = Directions.Right;
                }
                else if (Input.KeyPress(Keys.Left) && Setting.direction != Directions.Right)
                {
                    Setting.direction = Directions.Left;
                }
                else if (Input.KeyPress(Keys.Up) && Setting.direction != Directions.Down)
                {
                    Setting.direction = Directions.Up;
                }
                else if (Input.KeyPress(Keys.Down) && Setting.direction != Directions.Up)
                {
                    Setting.direction = Directions.Down;
                }

                Snake.MovePlayer(pause, maxXpos, maxYpos); 
            }
            pbCanvas.Invalidate(); // refresh the picture box and update the graphics on it
        }

        private void save1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();// + "..\\myModels";
            saveFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    //saving the snake , score and the board situation
                    formatter.Serialize(stream, Snake);
                    formatter.Serialize(stream, Setting.Score);
                    formatter.Serialize(stream, shapes);
                    formatter.Serialize(stream, Setting.GameOver);
                }
            }
        }



        private void save2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();// + "..\\myModels";
            openFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                Snake = (Snake)binaryFormatter.Deserialize(stream);
                Setting.Score = (int)binaryFormatter.Deserialize(stream);
                shapes = (List<Shape>)binaryFormatter.Deserialize(stream);
                Setting.GameOver = (bool)binaryFormatter.Deserialize(stream);
                pbCanvas.Invalidate();
            }
        }

    }
}
