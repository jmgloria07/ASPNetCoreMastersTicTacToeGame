using TicTacToeGame.DomainsModels;

namespace TicTacToeGame.Services.Dto
{
    public class CellDto
    {        
        public Cell.Row RowNum { get; set; }
        public Cell.Column ColumnField { get; set; }
        public Cell.State CellState { get; set; }
    }
}