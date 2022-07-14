using BattleshipsSimulation.Models;
using BattleshipsSimulation.Models.Ships;

namespace BattleshipsSimulation.GameMechanics
{
    public class DataGenerator
    {
        public static Task<bool> GenerateBoard(Player player)
        {
            player.Cells = new List<Cell>();


            for (int i = 1; i <= 10; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    player.Cells.Add(new Cell() { Id = Guid.NewGuid().ToString(), X = i, Y = j, IsEmpty = true });
                }
            }
            return Task.FromResult(true);
        }

        public static async Task<bool> GenerateShips(Player player)
        {
            player.Ships = new List<Ship>()
            {
                new Destroyer(),
                new Submarine(),
                new Cruiser(),
                new Battleship(),
                new Carrier(),
            };

            var rnd = new Random();
            foreach (var ship in player.Ships)
            {
                bool isPlaced = false;
                while (!isPlaced)
                {
                    var nextX = rnd.Next(1, 10);
                    var nextY = rnd.Next(1, 10);
                    var lastX = nextX;
                    var lastY = nextY;
                    var isHorizontal = rnd.NextDouble() >= 0.5;

                    if (isHorizontal)
                    {
                        for (int i = 1; i < ship.Length; i++)
                            lastX++;
                    }
                    else
                    {
                        for (int i = 1; i < ship.Length; i++)
                            lastY++;
                    }

                    if (lastX > 10 || lastY > 10)
                        continue;

                    var occupiedCells = player.Cells.Where(x =>
                        x.X >= nextX &&
                        x.Y >= nextY &&
                        x.X <= lastX &&
                        x.Y <= lastY).ToList();

                    if (occupiedCells.Any(x => x.IsShip))
                        continue;

                    foreach (var cell in occupiedCells)
                        cell.IsShip = true;

                    isPlaced = true;
                }
            }
            return true;

        }

        public static async Task SelectCell(Models.Cell selectedcell, Player player)
        {
            if (selectedcell.IsShip)
            {
                player.Cells.Find(x => x.Id == selectedcell.Id).IsHit = true;
                player.Cells.Find(x => x.Id == selectedcell.Id).IsEmpty = false;
                player.Lives -= 1;

                int x = player.Cells.Find(x => x.Id == selectedcell.Id).X;
                int y = player.Cells.Find(x => x.Id == selectedcell.Id).Y;

                int addx = x += 1;
                int minx = x -= 1;
                int minusx = minx -= 1;
                int addy = y += 1;
                int miny = y -= 1;
                int minusy = miny -= 1;

                if (x > 1)
                {
                    player.Cells.Find(x => x.X == minusx && x.Y == y).IsTarget = true;

                }
                if (x < 10)
                {
                    player.Cells.Find(x => x.X == addx && x.Y == y).IsTarget = true;
                }
                if (y > 1)
                {
                    player.Cells.Find(i => i.X == x && i.Y == minusy).IsTarget = true;
                }
                if (y < 10)
                {
                    player.Cells.Find(i => i.X == x && i.Y == addy).IsTarget = true;
                }

            }

            if (!selectedcell.IsShip)
            {
                player.Cells.Find(x => x.Id == selectedcell.Id).IsMiss = true;
                player.Cells.Find(x => x.Id == selectedcell.Id).IsEmpty = false;
            }

            return;
        }
    }
}
