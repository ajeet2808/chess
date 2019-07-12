using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Chess.Pieces
{
    public class PieceFactory
    {
        public PieceFactory()
        {

        }

        private static IReadOnlyDictionary<PieceType, Type> _pieceClassByType = new Dictionary<PieceType, Type>()
        {
            [PieceType.Pawn] = typeof(Pawn),
            [PieceType.Rook] = typeof(Rook),
            [PieceType.Knight] = typeof(Knight),
            [PieceType.Bishop] = typeof(Bishop),
            [PieceType.Queen] = typeof(Queen),
            [PieceType.King] = typeof(King),
        };

        public static Piece CreatePiece<T>(Color color) where T : Piece => Activator.CreateInstance(typeof(T), color) as Piece;

        public static Piece CreatePiece(Color color, PieceType pieceType) => Activator.CreateInstance(_pieceClassByType[pieceType], color) as Piece;


        public static IReadOnlyDictionary<Color, List<Piece>> CreatePieces()
        {
            var piecesByColor = new Dictionary<Color, List<Piece>>();
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                var pieces = new List<Piece>();
                for (int i = 0; i < 8; i++)
                {
                    pieces.Add(CreatePiece<Pawn>(color));
                }

                for (int i = 0; i < 2; i++)
                {
                    pieces.Add(CreatePiece<Rook>(color));

                    pieces.Add(CreatePiece<Knight>(color));

                    pieces.Add(CreatePiece<Bishop>(color));
                }

                pieces.Add(CreatePiece<King>(color));
                pieces.Add(CreatePiece<Queen>(color));

                piecesByColor.Add(color, pieces);
            }
            return piecesByColor;
        }
    }
}
