using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{
    [Serializable]
    class Block:Shape
    {


        public override void paint(object sender, PaintEventArgs e)// draw food
        {
            Graphics canvas = e.Graphics;
            Rectangle fillRect = new Rectangle(X * Setting.Width, Y * Setting.Height,
                            Setting.Width, Setting.Height);

            canvas.FillEllipse(Brushes.Black, fillRect);
            // Create solid brush.
            SolidBrush blueBrush = new SolidBrush(Color.Black);

            // Create region for fill.
            Region fillRegion = new Region(fillRect);

            // Fill region to screen.
            e.Graphics.FillRegion(blueBrush, fillRegion);
        
        }
        public override void bonus(Snake S, List<Shape> shapes, int maxXpos, int maxYpos)
        {
            Setting.GameOver = true;
        }

        //BadApple inherites the Shape destructor

    }
}
