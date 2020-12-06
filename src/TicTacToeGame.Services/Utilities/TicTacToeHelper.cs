using System;
using System.Collections.Generic;
using System.Text;
using TicTacToeGame.Services.Dto;
using TicTacToeGame.DomainsModels;
using TicTacToeGame.Services.Exceptions;

namespace TicTacToeGame.Services.Utilities
{
    public class TicTacToeHelper
    {
        public IEnumerable<Cell> InitializeCells()
        {
            IList<Cell> result = new List<Cell>();
            
            foreach(Cell.Row row in Enum.GetValues(typeof(Cell.Row))) 
            {
                foreach (Cell.Column column in Enum.GetValues(typeof(Cell.Column)))
                {
                    Cell cell = new Cell()
                    {
                        ColumnField = column,
                        RowNum = row,
                        CellState = Cell.State.BLANK
                    };
                    result.Add(cell);
                }
            }
            
            return result;
        }

        public IEnumerable<CellDto> PopulateCellDtos(IEnumerable<Cell> cells)
        {
            IList<CellDto> result = new List<CellDto>();
            foreach (Cell cell in cells)
            {
                CellDto cellDto = new CellDto()
                {
                    RowNum = cell.RowNum,
                    ColumnField = cell.ColumnField,
                    CellState = cell.CellState
                };
                result.Add(cellDto);
            }
            return result;
        }

        public bool ShouldSetCell(Game game, string castedBy, Cell cell)
        {
            if (game.GameState == Game.State.NAUGHT_WON
                || game.GameState == Game.State.CROSS_WON
                || game.GameState == Game.State.DRAW)
                throw new TicTacToeStateException(game.GameState);

            if (cell.CellState != Cell.State.BLANK) 
                throw new TicTacToeStateException(cell.CellState);

            bool isCrossTurnAndCastedByCross = game.GameState == Game.State.CROSS_TURN;
            //TODO && castedBy.Equals(game.PlayerCross);
            bool isNaughtTurnAndCastedByNaught = game.GameState == Game.State.NAUGHT_TURN;
                //TODO && castedBy.Equals(game.PlayerNaught);

            return isCrossTurnAndCastedByCross || isNaughtTurnAndCastedByNaught;
        }

        public Cell GetCellFromRowAndCol(IEnumerable<Cell> cells, int row, int column)
        {
            foreach (Cell cell in cells)
            {
                if ((Cell.Row) row == cell.RowNum 
                    && (Cell.Column) column == cell.ColumnField)
                    return cell;
            }
            return null;
        }

        public TicTacToe CastTurn(TicTacToe ticTacToe, Cell cell, Game.State gameState)
        {
            if (ticTacToe == null) throw new NullParameterException();

            IList<Cell> castedTicTacToe = new List<Cell>();
            foreach (Cell ticTacToeCell in ticTacToe.Cells)
            {
                if (ticTacToeCell.RowNum == cell.RowNum 
                    && ticTacToeCell.ColumnField == cell.ColumnField)
                {
                    ticTacToeCell.SetDate = DateTime.UtcNow;
                    ticTacToeCell.CellState = GetCellState(gameState);
                    ticTacToeCell.OwnerId = cell.OwnerId;
                }
                castedTicTacToe.Add(ticTacToeCell);
            }
            ticTacToe.Cells = castedTicTacToe;
            return ticTacToe;
        }

        public Cell.State GetCellState(Game.State gameState)
        {
            if (gameState == Game.State.CROSS_TURN) 
                return Cell.State.CROSS;
            else if (gameState == Game.State.NAUGHT_TURN) 
                return Cell.State.NAUGHT;

            throw new TicTacToeStateException(gameState);
        }

        public Game.State GetGameStateWon(Cell.State cellState)
        {
            if (cellState == Cell.State.CROSS) return Game.State.CROSS_WON;
            else if (cellState == Cell.State.NAUGHT) return Game.State.NAUGHT_WON;

            throw new TicTacToeStateException(cellState);
        }

        public Game.State GetGameStateNextTurn(Cell.State cellState)
        {
            if (cellState == Cell.State.CROSS) return Game.State.NAUGHT_TURN;
            else if (cellState == Cell.State.NAUGHT) return Game.State.CROSS_TURN;

            throw new TicTacToeStateException(cellState);
        }

        public Game.State CalculateGameState(TicTacToe ticTacToe, Cell.State cellState)
        {
            if (cellState == Cell.State.BLANK) throw new TicTacToeStateException(cellState);

            bool isTicTacToeWon = false;
            for (int i = 0; i < TicTacToeConstants.LINE_TABLE_DEF.GetLength(0); i++)
            {
                Cell[] cellsForChecking = new Cell[3];
                for (int j = 0; j < TicTacToeConstants.LINE_TABLE_DEF.GetLength(1); j++)
                {
                    int row = TicTacToeConstants.LINE_TABLE_DEF[i, j, 0];
                    int column = TicTacToeConstants.LINE_TABLE_DEF[i, j, 1];
                    foreach (Cell cell in ticTacToe.Cells)
                    {
                        if (cell.RowNum == (Cell.Row) row
                            && cell.ColumnField == (Cell.Column) column)
                        {
                            cellsForChecking[j] = cell;
                            break;
                        }
                    }                    
                }
                int winCount = 0;
                foreach (Cell cell in cellsForChecking)
                {
                    if (cell.CellState == cellState) winCount++;
                    else break;
                }
                if (winCount == 3)
                {
                    isTicTacToeWon = true;
                    break;
                }
            }

            if (isTicTacToeWon) return GetGameStateWon(cellState);

            bool isDraw = true;
            foreach (Cell cell in ticTacToe.Cells)
            {
                if (cell.CellState == Cell.State.BLANK)
                {
                    isDraw = false;
                    break;
                }
            }
            if (isDraw) return Game.State.DRAW;

            return GetGameStateNextTurn(cellState);
        }
    }
}
