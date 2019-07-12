using System;
using Green.Chess.Pieces;
using Green.Collections;

namespace Green.Chess
{
    public class MoveCommand : IRankable
    {
        public MoveCommand(Piece piece, Square destination)
        {
            Piece = piece;
            Destination = destination;
        }
        public Piece Piece { get; set; }
        public Square Destination { get; set; }

        public int Rank => 9999 - GetMoveValue();

        private int GetMoveValue()
        {
            int rank = 0;
            if (Destination.IsOccupied && Destination.Piece.Color != Piece.Color)
            {
                rank = Destination.Piece.Points * 1000 + rank;
            }
            rank = rank + (8-Math.Abs(Destination.Rank - 4) )* 100;
            rank = rank + (8-Math.Abs(Destination.Column - 4)) * 10;
            rank = rank + Piece.Points;
            
            return rank;
        }
    }
}
