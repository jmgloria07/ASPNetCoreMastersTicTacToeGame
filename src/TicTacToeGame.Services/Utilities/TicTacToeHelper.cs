using System;
using System.Collections.Generic;
using System.Text;
using TicTacToeGame.Services.Dto;
using TicTacToeGame.DomainsModels;
using TicTacToeGame.Services.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace TicTacToeGame.Services.Utilities
{
    public class TicTacToeHelper
    {
        public IEnumerable<Cell> InitializeCells(string ownerId)
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
                        CellState = Cell.State.BLANK,
                        OwnerId = ownerId
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

            bool isCrossTurnAndCastedByCross = game.GameState == Game.State.CROSS_TURN
                && castedBy.Equals(game.PlayerCross);
            bool isNaughtTurnAndCastedByNaught = game.GameState == Game.State.NAUGHT_TURN
                && castedBy.Equals(game.PlayerNaught);

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
            throw new CellNotFoundException();
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

        public Game.State GetGameStateWon(Cell.State mark)
        {
            if (mark == Cell.State.CROSS) return Game.State.CROSS_WON;
            else if (mark == Cell.State.NAUGHT) return Game.State.NAUGHT_WON;

            throw new TicTacToeStateException(mark);
        }

        public Game.State CalculateGameState(TicTacToe ticTacToe, Cell.State mark)
        {
            if (mark == Cell.State.BLANK) throw new TicTacToeStateException(mark);

            bool isTicTacToeWon = CheckIfALineOfSameMarksIsCreated(ticTacToe.Cells, mark);
            if (isTicTacToeWon) return GetGameStateWon(mark);

            bool isDraw = CheckIfBoardIsAlreadyFull(ticTacToe.Cells);
            if (isDraw) return Game.State.DRAW;

            return ToggleGameStateForNextTurn(mark);
        }

        public string GenerateTokenAsync(IdentityUser user, SecurityKey securityKey)
        {
            IList<Claim> userClaims = new List<Claim>
            {
                new Claim("Id", user.Id),
                new Claim("Email", user.Email)
            };

            return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMonths(1),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            ));
        }

        //-------------private methods----------------//

        private bool CheckIfALineOfSameMarksIsCreated(IEnumerable<Cell> cells, Cell.State mark)
        {
            for (int i = 0; i < TicTacToeConstants.LINE_TABLE_DEF.GetLength(0); i++)
            {
                Cell[] cellsInOneLine = GetCellsInOneLine(cells, i);
                
                int sameCellsCount = 0;
                foreach (Cell cell in cellsInOneLine)
                {
                    if (cell.CellState == mark) sameCellsCount++;
                    else break;
                }

                if (sameCellsCount == 3) return true;
            }

            return false;
        }

        private Cell[] GetCellsInOneLine(IEnumerable<Cell> cells, int lineIndex) 
        {
            int cellCountInOneLine = TicTacToeConstants.LINE_TABLE_DEF.GetLength(1);

            Cell[] result = new Cell[cellCountInOneLine];

            for (int i = 0; i < cellCountInOneLine; i++)
            {
                int row = TicTacToeConstants.LINE_TABLE_DEF[lineIndex, i, 0];
                int column = TicTacToeConstants.LINE_TABLE_DEF[lineIndex, i, 1];
                result[i] = GetCellFromRowAndCol(cells, row, column);
            }

            return result;
        }
        
        //a single blank mark means that that the board isn't full
        private bool CheckIfBoardIsAlreadyFull(IEnumerable<Cell> cells)
        {
            foreach (Cell cell in cells)
            {
                if (cell.CellState == Cell.State.BLANK) return false;
            }

            return true;
        }
        private Game.State ToggleGameStateForNextTurn(Cell.State mark)
        {
            if (mark == Cell.State.CROSS) return Game.State.NAUGHT_TURN;
            else if (mark == Cell.State.NAUGHT) return Game.State.CROSS_TURN;

            throw new TicTacToeStateException(mark);
        }
    }
}
