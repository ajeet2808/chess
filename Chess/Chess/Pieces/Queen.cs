using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Green.Chess.Pieces
{
    public class Queen : Piece
    {
        public Queen(Color color, int number) : base(color, PieceType.Queen, 9, number)
        {
        }

        private List<Square> BuildPath(Board board, int rankSeed, int columnSeed)
        {
            var path = new List<Square>();
            int rank = CurrentPosition.Rank + rankSeed;
            int column = CurrentPosition.Column + columnSeed;
            while (rank <= Board.RANK_MAX && rank >= Board.RANK_MIN && column <= Board.COLUMN_MAX && column >= Board.COLUMN_MIN)
            {
                var square = board.GetSquare(rank, column);
                path.Add(square);
                if (square.IsOccupied) break;
                rank += rankSeed;
                column += columnSeed;
            }
            return path;
        }

        public override List<Square> GetReachableSquares(Board board)
        {
            var attackingSquares = GetAttackableSquares(board);
            var validDestinationSquares = attackingSquares.Where(sq => (!sq.IsOccupied || sq.Piece.Color != Color)).ToList();
            return validDestinationSquares;
        }

        public override List<Square> GetAttackableSquares(Board board)
        {
            var attackingSquares = new List<Square>();

            attackingSquares.AddRange(BuildPath(board, +1, +1));
            attackingSquares.AddRange(BuildPath(board, -1, -1));
            attackingSquares.AddRange(BuildPath(board, +1, -1));
            attackingSquares.AddRange(BuildPath(board, -1, +1));

            attackingSquares.AddRange(BuildPath(board, +1, 0));
            attackingSquares.AddRange(BuildPath(board, -1, 0));
            attackingSquares.AddRange(BuildPath(board, 0, +1));
            attackingSquares.AddRange(BuildPath(board, 0, -1));

            return attackingSquares;
        }

        public override string ToString()
        {
            return "Q";
        }
    }
}
