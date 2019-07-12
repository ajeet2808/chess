using System.Collections.Generic;
using System.Linq;

namespace Green.Chess.Pieces
{
    public class King : Piece
    {
        public King(Color color, int number) : base(color, PieceType.King, 0, number)
        {
        }

        public override List<Square> GetAttackableSquares(Board board)
        {
            var paths = new List<Square>();
            var rank = CurrentPosition.Rank + 1;
            var column = CurrentPosition.Column;
            if (rank <= Board.RANK_MAX)
            {
                paths.Add(board.GetSquare(rank, column));
            }
            rank = CurrentPosition.Rank - 1;
            if (rank >= Board.RANK_MIN)
            {
                paths.Add(board.GetSquare(rank, column));
            }

            rank = CurrentPosition.Rank;
            column = CurrentPosition.Column + 1;
            if (column <= Board.COLUMN_MAX)
            {
                paths.Add(board.GetSquare(rank, column));
            }

            rank = CurrentPosition.Rank;
            column = CurrentPosition.Column - 1;
            if (column >= Board.COLUMN_MIN)
            {
                paths.Add(board.GetSquare(rank, column));
            }

            return paths;
        }

        public override List<Square> GetReachableSquares(Board board)
        {
            var attackingSquares = GetAttackableSquares(board);
            var validDestinationSquares = attackingSquares.Where(sq => (!sq.IsOccupied || sq.Piece.Color != Color)).ToList();
            return validDestinationSquares;
        }

        public override string ToString()
        {
            return "+";
        }
    }
}
