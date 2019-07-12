using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Green.Chess.Pieces;

namespace Green.Chess
{
    public class Player
    {
        public Player(string name, Color color)
        {
            Name = name;
            Color = color;
        }
        public Color Color { get; }
        public string Name { get; }

        public MoveCommand GetMoveCommand(Board board,int moveCount)
        {
            Console.WriteLine($"{Name}'s move ({moveCount}): ");
            Console.ReadKey();
            var moveablePiece = board.Pieces[Color].FirstOrDefault(p => p.ReachableSquares.Count > 0);
            return new MoveCommand(moveablePiece, moveablePiece.ReachableSquares.First());
        }

        public PieceType GetPromotedToPiece()
        {
            return PieceType.Queen;
        }
    }
}
