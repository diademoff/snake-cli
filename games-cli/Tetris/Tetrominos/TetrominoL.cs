using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class TetrominoL : Tetromino
    {
        public TetrominoL(int field_width, int init_y_location) : base(field_width, init_y_location)
        {
        }

        protected override List<Point[]> structureRotate => new List<Point[]>{
            //  *
            //***
            //
            new Point[]{
                new Point(1,2),
                new Point(2,2),
                new Point(3,2),
                new Point(3,1)
            },
            // *
            // *
            // **
            new Point[]{
                new Point(2,1),
                new Point(2,2),
                new Point(2,3),
                new Point(3,1)
            },
            //
            //***
            //*
            new Point[]{
                new Point(1,3),
                new Point(1,2),
                new Point(2,2),
                new Point(3,2)
            },
            //**
            // *
            // *
            new Point[]{
                new Point(1,1),
                new Point(2,1),
                new Point(2,2),
                new Point(2,3)
            }
        };
    }
}