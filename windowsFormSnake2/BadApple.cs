using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    [Serializable]
    internal class BadApple : Shape
    {
        public BadApple()
        {
        }
        public override void bonus(Snake s, List<Shape> shapes, int maxXpos, int maxYpos)
        {

            Setting.Score += Setting.Points; // increases the score for the game
            for(int i = 0; i < shapes.Count; i++)
            {
                Thread.Sleep(2);
                shapes[i].generateFood(maxXpos, maxYpos);
            }
            s.eat();
        }
        public override void paint(object sender, PaintEventArgs e)// draw food
        {
            Graphics canvas = e.Graphics;
            canvas.FillEllipse(Brushes.Purple, new Rectangle(X * Setting.Width, Y * Setting.Height,
            Setting.Width, Setting.Height));
        }

        //BadApple inherites the Shape destructor

    }
}
