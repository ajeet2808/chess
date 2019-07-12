using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Green.Chess.Pieces
{
    public class Knight : Piece
    {
        public Knight(Color color) : base(color)
        {
        }

        public override List<Square> GetAttackableSquares(Board board)
        {
            var paths = new List<Square>();

            var rank = CurrentPosition.Rank + 2;
            var column = CurrentPosition.Column + 1;
            if (rank <= Board.RANK_MAX)
            {
                if (column <= Board.COLUMN_MAX)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
                column = CurrentPosition.Column - 1;
                if (column >= Board.COLUMN_MIN)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
            }

            rank = CurrentPosition.Rank - 2;
            column = CurrentPosition.Column + 1;
            if (rank >= Board.RANK_MIN)
            {
                if (column <= Board.COLUMN_MAX)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
                column = CurrentPosition.Column - 1;
                if (column >= Board.COLUMN_MIN)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
            }

            rank = CurrentPosition.Rank + 1;
            column = CurrentPosition.Column + 2;
            if (rank <= Board.RANK_MAX)
            {
                if (column <= Board.COLUMN_MAX)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
                column = CurrentPosition.Column - 2;
                if (column >= Board.COLUMN_MIN)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
            }

            rank = CurrentPosition.Rank - 1;
            column = CurrentPosition.Column + 2;
            if (rank >= Board.RANK_MAX)
            {
                if (column <= Board.COLUMN_MAX)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
                column = CurrentPosition.Column - 2;
                if (column >= Board.COLUMN_MIN)
                {
                    paths.Add(board.GetSquare(rank, column));
                }
            }
            return paths;
        }

        public override List<Square> GetReachableSquares(Board board)
        {
            var attackingSquares = GetAttackableSquares(board);
            var validDestinationSquares = attackingSquares.Where(sq => (!sq.IsOccupied || sq.Piece.Color != Color) && !board.IsKingInCheckOnMove(this, sq)).ToList();
            return validDestinationSquares;
        }
        public override string ToString()
        {
            return "K";
        }
    }
}
