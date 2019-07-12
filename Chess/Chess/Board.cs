using Green.Chess.Pieces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Green.Chess
{
    public class Board
    {
        public const int RANK_MAX = 8;
        public const int RANK_MIN = 1;
        public const int COLUMN_MAX = 8;
        public const int COLUMN_MIN = 1;
        public Board()
        {
            Squares = new List<List<Square>>();
            for (int rank = 1; rank <= 8; rank++)
            {
                var row = new List<Square>();
                for (int column = 1; column <= 8; column++)
                {
                    row.Add(new Square(rank, column));
                }
                Squares.Add(row);
            }
        }

        public void DisplayBoard()
        {
            Console.WriteLine();
            for (int rank = 8; rank >= 1; rank--)
            {
                for (int column = 1; column <= 8; column++)
                {
                    var sq = GetSquare(rank, column);
                    WriteWithColor(sq);
                }
                Console.WriteLine();
            }
        }
        private void WriteWithColor(Square square, bool printEmpty = false)
        {
            Console.ResetColor();
            Console.BackgroundColor = square.Color == Color.White ? ConsoleColor.White : ConsoleColor.DarkYellow;
            if (!printEmpty && square.IsOccupied)
            {
                Console.ForegroundColor = square.Piece.Color == Color.White ? ConsoleColor.Green : ConsoleColor.DarkRed;
                Console.Write($" {square.Piece} ");
            }
            else
            {
                Console.Write("   ");
            }

            Console.ResetColor();
        }

        public IReadOnlyDictionary<Color, List<Piece>> Pieces { get; set; }
        public List<List<Square>> Squares { get; }
        public Square GetSquare(int rank, int column)
        {
            if (rank > RANK_MAX || rank < RANK_MIN || column > COLUMN_MAX || column < COLUMN_MIN)
            {
                throw new Exception("Invalid coordinates");
            }
            return Squares[rank - 1][column - 1];
        }
        public Piece RemovePiece(Piece piece)
        {
            var square = GetSquare(piece.CurrentPosition.Rank, piece.CurrentPosition.Column);
            return square.Pick();
        }


        private void Deploy<T>(Color color)
        {
            var pieces = Pieces[color].Where(p => p is T).ToList();
            int rank = 0;
            int column = 0;
            if (typeof(T) == typeof(Pawn))
            {
                rank = (color == Color.White ? Ranks.TWO : Ranks.SEVEN);
                for (column = COLUMN_MIN; column <= COLUMN_MAX; column++)
                {
                    var sqaure = GetSquare(rank, column);
                    sqaure.Piece = pieces[column - 1];
                    pieces[column - 1].CurrentPosition = sqaure;
                }
            }

            if (typeof(T) == typeof(Rook))
            {
                rank = color == Color.White ? Ranks.ONE : Ranks.EIGHT;
                column = Columns.ONE;
                var sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[0];
                sqaure.Piece.CurrentPosition = sqaure;

                column = Columns.EIGHT;
                sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[1];
                sqaure.Piece.CurrentPosition = sqaure;
            }

            if (typeof(T) == typeof(Knight))
            {
                rank = color == Color.White ? Ranks.ONE : Ranks.EIGHT;
                column = Columns.TWO;
                var sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[0];
                sqaure.Piece.CurrentPosition = sqaure;

                column = Columns.SEVEN;
                sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[1];
                sqaure.Piece.CurrentPosition = sqaure;
            }

            if (typeof(T) == typeof(Bishop))
            {
                rank = color == Color.White ? Ranks.ONE : Ranks.EIGHT;
                column = Columns.THREE;
                var sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[0];
                sqaure.Piece.CurrentPosition = sqaure;

                column = Columns.SIX;
                sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[1];
                sqaure.Piece.CurrentPosition = sqaure;
            }

            if (typeof(T) == typeof(Queen))
            {
                rank = color == Color.White ? Ranks.ONE : Ranks.EIGHT;
                column = Columns.FOUR;
                var sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[0];
                sqaure.Piece.CurrentPosition = sqaure;
            }

            if (typeof(T) == typeof(King))
            {
                rank = color == Color.White ? Ranks.ONE : Ranks.EIGHT;
                column = Columns.FIVE;
                var sqaure = GetSquare(rank, column);
                sqaure.Piece = pieces[0];
                sqaure.Piece.CurrentPosition = sqaure;
            }
        }

        public void DeployPieces(IReadOnlyDictionary<Color, List<Piece>> pieces)
        {
            Pieces = pieces;
            foreach (Color color in Enum.GetValues(typeof(Color)))
            {
                Deploy<Pawn>(color);
                Deploy<Rook>(color);
                Deploy<Knight>(color);
                Deploy<Bishop>(color);
                Deploy<Queen>(color);
                Deploy<King>(color);
            }
        }

        public IReadOnlyDictionary<int, Square> GetAttackingSquares(Color color)
        {
            var map = new Dictionary<int, Square>();
            foreach (var piece in Pieces[color])
            {
                foreach (var square in piece.GetAttackableSquares(this))
                {
                    map.TryAdd(square.Id, square);
                }
            }
            return map;
        }

        public bool IsKingInCheck(Color color)
        {
            var kingsPosition = Pieces[color].Single(p => p is King).CurrentPosition;
            var opponentAttackingSquares = GetAttackingSquares(color.GetOpponentColor());
            var isInCheck = opponentAttackingSquares.ContainsKey(kingsPosition.Id);
            return isInCheck;
        }

        public bool IsKingInCheckOnMove(Piece piece, Square destination)
        {
            var isKingInCheckOnMove = false;
            var currentSquare = piece.CurrentPosition;
            var capturedPiece = destination.Pick();
            piece = piece.CurrentPosition.Pick();
            try
            {
                destination.Place(piece);
                isKingInCheckOnMove = IsKingInCheck(piece.Color);
            }
            finally
            {
                piece = destination.Pick();
                currentSquare.Place(piece);
                destination.Place(capturedPiece);
            }
            return isKingInCheckOnMove;
        }

        public int GetPromotionRank(Color color) => color == Color.White ? RANK_MAX : RANK_MIN;

        public static class Ranks
        {
            public const int ONE = 1;
            public const int TWO = 2;
            public const int THREE = 3;
            public const int FOUR = 4;
            public const int FIVE = 5;
            public const int SIX = 6;
            public const int SEVEN = 7;
            public const int EIGHT = 8;
        }

        public static class Columns
        {
            public const int ONE = 1;
            public const int TWO = 2;
            public const int THREE = 3;
            public const int FOUR = 4;
            public const int FIVE = 5;
            public const int SIX = 6;
            public const int SEVEN = 7;
            public const int EIGHT = 8;
        }
    }
}
