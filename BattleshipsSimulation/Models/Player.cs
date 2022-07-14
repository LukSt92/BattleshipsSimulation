using BattleshipsSimulation.Models.Ships;

namespace BattleshipsSimulation.Models
{
    public class Player
    {
        public int Id { get; set; }

        public List<Cell> Cells { get; set; }
        public List<Ship> Ships { get; set; }
        public int Lives { get; set; }
        public bool IsMyTurn { get; set; }

        public int WinCount { get; set; }
    }
}
