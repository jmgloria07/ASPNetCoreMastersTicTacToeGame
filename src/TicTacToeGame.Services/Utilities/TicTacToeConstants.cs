using System;
using System.Collections.Generic;
using System.Text;
using TicTacToeGame.Services.Dto;

namespace TicTacToeGame.Services.Utilities
{
    public class TicTacToeConstants
    {
        public static readonly int[,,] LINE_TABLE_DEF = new int[,,] { 
            //ROW, COLUMN
            { { 1, 1 }, { 1, 2 }, { 1, 3 } }, //top horizontal line
            { { 2, 1 }, { 2, 2 }, { 2, 3 } }, //middle horizontal line
            { { 3, 1 }, { 3, 2 }, { 3, 3 } }, //bottom horizontal line
            { { 1, 1 }, { 2, 2 }, { 3, 3 } }, //diagonal from top-left
            { { 1, 3 }, { 2, 2 }, { 3, 1 } }, //diagonal from top-right
            { { 1, 1 }, { 2, 1 }, { 3, 1 } }, //left vertical line
            { { 1, 2 }, { 2, 2 }, { 3, 2 } }, //middle vertical line
            { { 1, 3 }, { 2, 3 }, { 3, 3 } }  //right vertical line
        };
        public const int UNHANDLED_ERROR_CODE = 0;
    }
}
