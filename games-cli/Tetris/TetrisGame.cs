using System;
using System.Collections.Generic;
using System.Drawing;

namespace Games
{
    public class TetrisGame : Game
    {
        public override bool IsFocused { get => true; set => throw new NotImplementedException(); }

        public override int DelayBetweenFrames => 200;

        public override bool IsGameOver => isGameOver;
        bool isGameOver = false;
        /*
        Блок, который падает в данный момент
        */
        Tetromino falling_tetromino;
        /*
        Следующий блок после текущего
        */
        Tetromino next_tetromino;
        /*
        Фактически нарисованный падающий тетрамино
        */
        IDrawable[] tetromino_drawn = new IDrawable[0];
        /*
        Упавшие блоки
        */
        List<IDrawable> tetromino_fallen = new List<IDrawable>();
        Random rnd = new Random();
        int falling_blocks_field_width = 20;
        // Координата левого края
        int left_border;
        // Координата правого края
        int right_border;

        RightInfo rf;

        public TetrisGame(int FIELD_SIZE_WIDTH, int FIELD_SIZE_HEIGHT, Padding p) : base(FIELD_SIZE_WIDTH, FIELD_SIZE_HEIGHT, p)
        {
            left_border = padding.Left + (FIELD_SIZE_WIDTH / 2) - (falling_blocks_field_width / 2);
            right_border = FIELD_SIZE_WIDTH - (padding.Right + (FIELD_SIZE_WIDTH / 2) - (falling_blocks_field_width / 2));

            falling_tetromino = GetRandomTetromino();
            next_tetromino = GetRandomTetromino();

            this.rf = new RightInfo(right_border, next_tetromino, padding);
        }

        Tetromino GetRandomTetromino()
        {
            int field_width = FIELD_SIZE_WIDTH;
            int init_y_location = padding.Top;
            switch (rnd.Next(1, 8))
            {
                case 1:
                    return new TetrominoI(field_width, init_y_location);
                case 2:
                    return new TetrominoJ(field_width, init_y_location);
                case 3:
                    return new TetrominoL(field_width, init_y_location);
                case 4:
                    return new TetrominoO(field_width, init_y_location);
                case 5:
                    return new TetrominoS(field_width, init_y_location);
                case 6:
                    return new TetrominoT(field_width, init_y_location);
                case 7:
                    return new TetrominoZ(field_width, init_y_location);
            }
            throw new Exception("Unreal exception GetRandomTetromino");
        }

        public override void HandleKey(ConsoleKey key)
        {
            if (key == ConsoleKey.A || key == ConsoleKey.LeftArrow)
            {
                MoveFallingLeft();
            }
            else if (key == ConsoleKey.D || key == ConsoleKey.RightArrow)
            {
                MoveFallingRight();
            }
            else if (key == ConsoleKey.W || key == ConsoleKey.UpArrow)
            {
                RotateFalling();
            }
        }

        void MoveFallingLeft()
        {
            if (IsIntersectsWithFallen(falling_tetromino.TryMoveLeft()))
            {
                return;
            }
            falling_tetromino.MoveLeft();
        }

        void MoveFallingRight()
        {
            if (IsIntersectsWithFallen(falling_tetromino.TryMoveRight()))
            {
                return;
            }
            falling_tetromino.MoveRight();
        }

        void RotateFalling()
        {
            if (IsIntersectsWithFallen(falling_tetromino.TryRotate()))
            {
                return;
            }
            falling_tetromino.Rotate();
        }

        public override void NextFrame(Drawer d)
        {
            d.CreateBorder('·', new Padding(
                left_border,
                FIELD_SIZE_WIDTH - right_border,
                padding.Top,
                padding.Buttom));

            if (IsOnButtom(falling_tetromino) ||
                IsIntersectsWithFallen(falling_tetromino.TryMoveDown()))
            {
                SwitchToNextTetromino();
            }
            else
            {
                falling_tetromino.MoveDown();
                tetromino_drawn = falling_tetromino.ElementContent; // сохранить, чтобы потом удалить
                d.Create(falling_tetromino);
            }

            d.Create(rf);
            d.Create(tetromino_fallen.ToArray());
        }

        void AddCurrentTetrominoToFallen()
        {
            foreach (IDrawable i in falling_tetromino.ElementContent)
            {
                tetromino_fallen.Add(i);
            }
        }

        void SwitchToNextTetromino()
        {
            AddCurrentTetrominoToFallen();
            falling_tetromino = next_tetromino;
            next_tetromino = GetRandomTetromino();
            rf.ChangeNextTetromino(next_tetromino);
        }

        /*
        Пересекается ли текущий блок с упавшими
        */
        bool IsIntersectsWithFallen(Point[] t)
        {
            foreach (var i in tetromino_fallen)
            {
                foreach (var j in t)
                {
                    if (i.Location.X == j.X &&
                        i.Location.Y == j.Y)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        bool IsOnButtom(Tetromino t)
        {
            foreach (IDrawable i in t.ElementContent)
            {
                if (i.Location.Y >= FIELD_SIZE_HEIGHT - padding.Buttom - 2)
                {
                    return true;
                }
            }
            return false;
        }

        bool IsCollidesBorder(Tetromino t)
        {
            foreach (IDrawable i in t.ElementContent)
            {
                if (i.Location.X <= left_border || i.Location.X + 1 >= right_border)
                {
                    return true;
                }
            }
            return false;
        }

        public override void PrepareForNextFrame(Drawer d)
        {
            d.Remove(tetromino_drawn);
            d.Remove(rf);
        }
    }
}