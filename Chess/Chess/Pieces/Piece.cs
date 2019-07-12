using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Chess.Pieces
{
    public abstract class Piece
    {
        public Square CurrentPosition { get; set; }
        public Color Color { get; }
        public Color OpponentColor => Color == Color.White ? Color.Black : Color.White;
        public Piece(Color color)
        {
            Color = color;
        }
        protected List<Square> _attackableSquares;
        protected List<Square> _reachableSquares;
        public List<Square> AttackableSquares => _attackableSquares;
        public List<Square> ReachableSquares => _reachableSquares;
        public void PopulateAttackableSquares(Board board)
        {
            _attackableSquares = GetAttackableSquares(board);
        }

        public void PopulateReachableSquares(Board board)
        {
            _reachableSquares = GetReachableSquares(board);
        }
        public abstract List<Square> GetReachableSquares(Board board);

        public abstract List<Square> GetAttackableSquares(Board board);

    }
}
