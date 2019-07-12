using System;
using System.Collections.Generic;
using System.Text;

namespace Green.Chess
{
    public static class Extentions
    {
        public static Color GetOpponentColor(this Color color) => color == Color.White ? Color.Black : Color.White;
    }
}
