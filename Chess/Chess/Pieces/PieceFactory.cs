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

        private static IDictionary<(Color, PieceType), int> _pieceCounter = new Dictionary<(Color, PieceType), int>()
        {
            [(Color.White, PieceType.Pawn)] = 0,
            [(Color.White, PieceType.Rook)] = 0,
            [(Color.White, PieceType.Knight)] = 0,
            [(Color.White, PieceType.Bishop)] = 0,
            [(Color.White, PieceType.Queen)] = 0,
            [(Color.White, PieceType.King)] = 0,
            [(Color.Black, PieceType.Pawn)] = 0,
            [(Color.Black, PieceType.Rook)] = 0,
            [(Color.Black, PieceType.Knight)] = 0,
            [(Color.Black, PieceType.Bishop)] = 0,
            [(Color.Black, PieceType.Queen)] = 0,
            [(Color.Black, PieceType.King)] = 0,
        };
        
        public static Piece CreatePiece(Color color, PieceType type)
        {
            var key = (color, type);
            _pieceCounter[key]++;
            return Activator.CreateInstance(_pieceClassByType[type], color, _pieceCounter[key]) as Piece;
        }


        public static IReadOnlyDictionary<Color, List<Piece>> CreatePieces()
        {
            var piecesByColor = new Dictionary<Color, List<Piece>>();
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                var pieces = new List<Piece>();
                for (int i = 0; i < 8; i++)
                {
                    pieces.Add(CreatePiece(color, PieceType.Pawn));
                }

                for (int i = 0; i < 2; i++)
                {
                    pieces.Add(CreatePiece(color, PieceType.Rook));

                    pieces.Add(CreatePiece(color, PieceType.Knight));

                    pieces.Add(CreatePiece(color, PieceType.Bishop));
                }

                pieces.Add(CreatePiece(color, PieceType.King));
                pieces.Add(CreatePiece(color, PieceType.Queen));

                piecesByColor.Add(color, pieces);
            }
            return piecesByColor;
        }
    }
}
