using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Chess.Pieces
{
    public abstract class Piece
    {
        public int Id { get; }
        public int Points { get; }
        public PieceType Type { get; }
        public Square CurrentPosition { get; set; }
        public Color Color { get; }
        public Color OpponentColor => Color == Color.White ? Color.Black : Color.White;
        public Piece(Color color, PieceType type, int points, int number)
        {
            Color = color;
            Type = type;
            Points = points;
            Id = ((int)Color + 1) * 100 + ((int)type + 1) * 10 + number;
        }
        public abstract List<Square> GetReachableSquares(Board board);

        public abstract List<Square> GetAttackableSquares(Board board);

        public override int GetHashCode()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            if (obj is Piece piece)
            {
                return piece.Id == Id;
            }
            return false;
        }
    }
}
