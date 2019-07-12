using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Green.Chess.Pieces;

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


        public void Play()
        {
            var count = 0;
            Board.Pieces[Turn].ForEach(p => p.PopulateReachableSquares(Board));
            do
            {
                if (Turn == Color.White)
                {
                    count++;
                }
                var moveCommand = Players[Turn].GetMoveCommand(Board, count);
                
                var move = MakeMove(Board, moveCommand.Piece, moveCommand.Destination);
                Turn = Turn.GetOpponentColor();
                Board.Pieces[Turn].ForEach(p => p.PopulateReachableSquares(Board));
                UpdateGameStatus();
                UpdateDisplay();
            } while (GameStatus == GameStatus.InPlay);
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
            if (IsCheckMate())
            {
                GameStatus = GameStatus.CheckMate;
            }

            if (InSufficientMaterials())
            {
                GameStatus = GameStatus.Draw;
            }

            return GameStatus;
        }

        public bool IsCheckMate()
        {
            var moveablePiece = Board.Pieces[Turn].FirstOrDefault(p => p.ReachableSquares.Count > 0);
            return moveablePiece == null && Board.IsKingInCheck(Turn);
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

            RemovePiece(destination.Piece);
            destination.Place(piece.CurrentPosition.Pick());
            if (IsPromotionalMove(piece, destination))
            {
                move.PromotedToPiece = PieceFactory.CreatePiece(Turn, Players[Turn].GetPromotedToPiece());
                RemovePiece(piece);
                destination.Place(move.PromotedToPiece);
            }
            return move;
        }

        //public void UndoMove(Move move)
        //{
        //    //var from = Board.GetSquare(move.From.Rank, move.From.Column);
        //    //var to = Board.GetSquare(move.To.Rank, move.To.Column);
        //    //var captured = move.CapturedPiece;
        //    //var piece = to.Pick();
        //}

        private void RemovePiece(Piece piece)
        {
            if (piece != null)
            {
                Pieces[piece.Color].Remove(Board.RemovePiece(piece));
            }
        }
    }
    public enum GameStatus
    {
        InPlay,
        Draw,
        CheckMate
    }
}
