using System;
using System.Collections.Generic;
using System.Text;
using Green.Chess.Pieces;

namespace Green.Chess
{
    public class Move
    {
        public Piece Piece { get; set; }

        public Square From { get; set; }

        public Square To { get; set; }

        public Piece CapturedPiece { get; set; }

        public bool IsPromotionalMove => PromotedToPiece != null;

        public Piece PromotedToPiece { get; set; }

        public bool IsDoublePawnPush => Piece is Pawn && (To.Rank - From.Rank) * (Piece.Color == Color.White ? +1 : -1) == 2;
    }
}
