using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Green.Chess.Pieces;
using Green.Collections;

namespace Green.Chess
{
    public class Game
    {
        public Color Turn { get; set; } = Color.White;

        public Dictionary<Color, Player> Players { get; set; }

        public Board Board { get; set; }

        public IReadOnlyDictionary<Color, List<Piece>> Pieces { get; }

        public IReadOnlyDictionary<Color, List<Piece>> CapturedPieces { get; }

        public Game()
        {
            Pieces = PieceFactory.CreatePieces();
            CapturedPieces = new Dictionary<Color, List<Piece>>() { [Color.White] = new List<Piece>(), [Color.Black] = new List<Piece>() };
            Players = new Dictionary<Color, Player>();
        }

        public void Init()
        {
            Board = new Board();
            Board.DeployPieces(Pieces);
            var whitePlayer = new Player("White", Color.White);
            var blackPlayer = new Player("Black", Color.Black);
            Players.Add(whitePlayer.Color, whitePlayer);
            Players.Add(blackPlayer.Color, blackPlayer);
            Turn = Color.White;
            UpdateDisplay();
        }

        private IPriorityQueue<MoveCommand> GetValidMoves(Color color)
        {
            var moves = new PriorityQueue<MoveCommand>();
            foreach (var piece in Pieces[color])
            {
                var validMoves = GetValidMoves(piece);
                if (validMoves.Count > 0)
                {
                    moves.Enqueue(validMoves);
                }
            }
            return moves;
        }

        private List<MoveCommand> GetValidMoves(Piece piece)
        {
            var moves = piece.GetReachableSquares(Board).Where(sq => !IsKingInCheckOnMove(piece, sq)).Select(sq => new MoveCommand(piece, sq)).ToList();
            return moves;
        }

        public void Play()
        {
            var count = 0;
            UpdateGameStatus();
            while (GameStatus == GameStatus.InPlay)
            {
                if (Turn == Color.White)
                {
                    count++;
                }
                Console.WriteLine($"{Players[Turn].Name}'s move ({count}): ");
                Console.ReadKey();
                var moves = GetValidMoves(Turn);
                var moveCommand = Players[Turn].GetMoveCommand(moves, count);

                var move = MakeMove(Board, moveCommand.Piece, moveCommand.Destination);
                Turn = Turn.GetOpponentColor();
                var pieces = Board.Pieces[Turn].Where(x => x.CurrentPosition == null).ToList();
                var pieces2 = Board.Pieces[Turn.GetOpponentColor()].Where(x => x.CurrentPosition == null).ToList();
                //Board.Pieces[Turn].ForEach(p => p.PopulateReachableSquares(Board));
                UpdateGameStatus();
                UpdateDisplay();
            };
            switch (GameStatus)
            {
                case GameStatus.Draw:
                    Console.WriteLine($"It's Draw!!");
                    break;
                case GameStatus.CheckMate:
                    Console.WriteLine($"{Players[Turn.GetOpponentColor()].Name} is winner");
                    break;
            }
        }

        private void UpdateDisplay()
        {
            Board.DisplayBoard();
        }

        private GameStatus UpdateGameStatus()
        {
            var validMoves = GetValidMoves(Turn);
            var moveablePiece = validMoves.Count > 0 ? validMoves.Peek().Piece : null;
            if (moveablePiece == null)
            {
                GameStatus = IsKingInCheck(Turn) ? GameStatus.CheckMate : GameStatus.Draw;
            }
            else if (InSufficientMaterials())
            {
                GameStatus = GameStatus.Draw;
            }
            return GameStatus;
        }

        public bool InSufficientMaterials()
        {
            if (Pieces[Turn].Count == 1 && Pieces[Turn.GetOpponentColor()].Count == 1)
            {
                return true;
            }

            return false;
        }

        public GameStatus GameStatus { get; set; } = GameStatus.InPlay;
        private bool IsPromotionalMove(Piece piece, Square destination)
        {
            var promotionalRank = Board.GetPromotionRank(piece.Color);
            return promotionalRank == destination.Rank && piece is Pawn;
        }

        public Move MakeMove(Board board, Piece piece, Square destination)
        {
            var currentPosition = piece.CurrentPosition;

            var move = new Move();
            move.From = new Square(piece.CurrentPosition.Rank, piece.CurrentPosition.Column);
            move.To = new Square(destination.Rank, destination.Column);
            move.CapturedPiece = destination.Piece;
            if (destination.IsOccupied)
            {
                RemovePiece(destination.Piece);
            }
            destination.Place(piece.CurrentPosition.Pick());
            if (IsPromotionalMove(piece, destination))
            {
                move.PromotedToPiece = PieceFactory.CreatePiece(Turn, Players[Turn].GetPromotedToPiece());
                RemovePiece(piece);
                destination.Place(move.PromotedToPiece);
            }
            return move;
        }

        public IReadOnlyDictionary<int, Square> GetAttackingSquares(Color color)
        {
            var map = new Dictionary<int, Square>();
            foreach (var piece in Pieces[color])
            {
                foreach (var square in piece.GetAttackableSquares(Board))
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
            Piece capturedPiece = null;
            if (destination.IsOccupied)
            {
                capturedPiece = RemovePiece(destination.Piece);
            }

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
                if (capturedPiece != null)
                {
                    AddPiece(capturedPiece, destination);
                }
            }
            return isKingInCheckOnMove;
        }


        private Piece RemovePiece(Piece piece)
        {
            if (piece == null) throw new InvalidOperationException("cannot remove null piece");
            piece = Board.RemovePiece(piece);
            Pieces[piece.Color].Remove(piece);
            return piece;
        }

        private void AddPiece(Piece piece, Square destination)
        {
            Pieces[piece.Color].Add(piece);
            destination.Place(piece);
        }
    }
    public enum GameStatus
    {
        InPlay,
        Draw,
        CheckMate
    }
}
