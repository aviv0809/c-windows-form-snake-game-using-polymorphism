using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    //this is the snake head class and the apple that the snake eat class
    internal class Circle
    {
        public int X { get; set; } // this is a public int class called X
        public int Y { get; set; } // this is a public int class called Y

        public Circle()
        {
            // this function is resetting the X and Y to 0
            X = 0;
            Y = 0;
        }
    }
}
