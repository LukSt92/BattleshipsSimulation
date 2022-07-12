namespace BattleshipsSimulation.Models
{
    public class Cell
    {
        public string Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsShip { get; set; }
        public bool IsHit { get; set; }
        public bool IsEmpty { get; set; }
        public bool IsMiss { get; set; }
    }
}
