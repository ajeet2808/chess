using System;
using Green.Chess.Pieces;

namespace Green.Chess
{
    public class Square : IEquatable<Square>
    {
        public int Id => (Rank - 1) * Board.RANK_MAX + Column;
        public Color Color { get; }
        public Square(int rank, int column)
        {
            if (rank > 8 || column > 8)
            {
                throw new Exception($"Invalid Square coordinates. (Rank,Column):({rank},{column})");
            }
            Rank = rank;
            Column = column;
            Color = (rank + column) % 2 == 1 ? Color.White : Color.Black;

        }

        public int Column { get; }
        public int Rank { get; }

        public bool Equals(Square other)
        {
            return other != null && other.Column == Column && other.Rank == Rank;
        }

        public Piece Piece { get; set; }

        public bool IsOccupied => Piece != null;

        public override int GetHashCode()
        {
            return Id;
        }

        public void Place(Piece piece)
        {
            if (piece == null) return;
            Piece = piece;
            piece.CurrentPosition = this;
        }

        public Piece Pick()
        {
            if (Piece == null) return null;
            Piece.CurrentPosition = null;
            var piece = Piece;
            Piece = null;
            return piece;
        }

        public override bool Equals(object obj)
        {
            if (obj is Square square)
            {
                return square.Rank == Rank && square.Column == Column;
            }
            return false;
        }

        public override string ToString()
        {
            return $"{Rank},{Column}";
        }
    }
}