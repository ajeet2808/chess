using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Green.Chess.Pieces;
using Green.Collections;

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

        public MoveCommand GetMoveCommand(IPriorityQueue<MoveCommand> validMoves, int moveCount)
        {
            var moveCommand = validMoves.Peek();
            return moveCommand;
        }

        public PieceType GetPromotedToPiece()
        {
            return PieceType.Queen;
        }
    }
}
