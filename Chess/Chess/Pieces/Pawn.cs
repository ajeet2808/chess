using System.Collections.Generic;
using System.Linq;

namespace Green.Chess.Pieces
{
    public class Pawn : Piece
    {
        public Pawn(Color color) : base(color)
        {
        }
        private int MarchDirection => Color == Color.Black ? -1 : +1;

        public override List<Square> GetAttackableSquares(Board board)
        {
            var attackingSquares = new List<Square>();
            if (CurrentPosition.Rank == Board.RANK_MAX) return attackingSquares;

            if (CurrentPosition.Column != Board.Columns.ONE)
            {
                var square = board.GetSquare(CurrentPosition.Rank + 1 * MarchDirection, CurrentPosition.Column - 1);
                attackingSquares.Add(square);
            }

            if (CurrentPosition.Column != Board.Columns.EIGHT)
            {
                var square = board.GetSquare(CurrentPosition.Rank + 1 * MarchDirection, CurrentPosition.Column + 1);
                attackingSquares.Add(square);
            }
            return attackingSquares;
        }

        public override List<Square> GetReachableSquares(Board board)
        {
            var validDestinationSquares = new List<Square>();
            if (CurrentPosition.Rank == Board.RANK_MAX) return validDestinationSquares;

            var square = board.GetSquare(CurrentPosition.Rank + 1 * MarchDirection, CurrentPosition.Column);

            if (!square.IsOccupied)
            {
                validDestinationSquares.Add(square);
                if (CurrentPosition.Rank == Board.Ranks.TWO)
                {
                    square = board.GetSquare(CurrentPosition.Rank + 2 * MarchDirection, CurrentPosition.Column);
                    if (!square.IsOccupied)
                    {
                        validDestinationSquares.Add(square);
                    }
                }
            }

            if (CurrentPosition.Column != Board.Columns.ONE)
            {
                square = board.GetSquare(CurrentPosition.Rank + 1 * MarchDirection, CurrentPosition.Column - 1);
                if (square.IsOccupied && square.Piece.Color != Color)
                {
                    validDestinationSquares.Add(square);
                }
            }

            if (CurrentPosition.Column != Board.Columns.EIGHT)
            {
                square = board.GetSquare(CurrentPosition.Rank + 1 * MarchDirection, CurrentPosition.Column + 1);
                if (square.IsOccupied && square.Piece.Color != Color)
                {
                    validDestinationSquares.Add(square);
                }
            }
            validDestinationSquares = validDestinationSquares.Where(sq => !board.IsKingInCheckOnMove(this, sq)).ToList();
            return validDestinationSquares;
        }

        public override string ToString()
        {
            return "P";
        }
    }
}
