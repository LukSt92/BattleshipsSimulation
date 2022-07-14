using BattleshipsSimulation.Models;
using BattleshipsSimulation.Models.Ships;

namespace BattleshipsSimulation.GameMechanics
{
    public class DataGenerator
    {

        // This task first creates a new empty List of Cells for the specified player. After that it creates a cell one after another for specifed coordinates (i = rows, j = columns) up to 10, which gives us a total number of cells equal to 100, where each cell has its own unique ID.
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

        // First creates a new List of Ships with each individual ship. After that this task is placing ships one by one with random coords (X, Y) and in random configuration (horizontal or vertical).
        // How does it works?
        // Create new List with ships >> foreach loop takes one ship from List >> isHorizontal draws configuration >> nextX and nextY draws random coords >> if Length of current ship is larger than current coords (X for horizontal and Y for Vertical) it increase current coords by 1 in lastX or lastY >> it checks if current coords exists (if they are smaller than 10) if not, it starts again with new randoms >> It adds every single coord to the list >> another check, if any coord added to the list is already occupied it starts again >> If everything is ok this task add current ship to the board and repeat this process for every single ship. 
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

                    if (!isHorizontal)
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

        // This task takes the cell that was chosen in Shot task and checks if there is a ship on it.
        // if there is a ship then it changes values of IsHit and IsEmpty(both of them are essential for the board) and decrease player live.
        // after that thanks to IsHit property it looks for possible targets to shots more accurate. If there is no ship it returns miss.
        // How does it works?
        // Checks if there is a ship (IsShip) >> if false, change values of IsMiss and IsEmpty and return >> if true, change values of IsHit and IsEmpty, decrease live >>
        // find and assign coordinates to variables (int x, int y) >> increment and decrement coordinates to another variables (there is a bit of mess here but I did not find other solution, this is the only way it works)  >> find possible target cells and change their values of IsTarget (there are 4 possibilites Up, Down, Left, Right, it needs to check all of them) and return 
        public static async Task SelectCell(Models.Cell selectedCell, Player player)
        {
            if (selectedCell.IsShip)
            {
                player.Cells.Find(x => x.Id == selectedCell.Id).IsHit = true;
                player.Cells.Find(x => x.Id == selectedCell.Id).IsEmpty = false;
                player.Lives -= 1;

                int x = player.Cells.Find(x => x.Id == selectedCell.Id).X;
                int y = player.Cells.Find(x => x.Id == selectedCell.Id).Y;

                int addX = x += 1;
                int minX = x -= 1;
                int minusX = minX -= 1;
                int addY = y += 1;
                int minY = y -= 1;
                int minusY = minY -= 1;

                if (x > 1)
                {
                    player.Cells.Find(x => x.X == minusX && x.Y == y).IsTarget = true;

                }
                if (x < 10)
                {
                    player.Cells.Find(x => x.X == addX && x.Y == y).IsTarget = true;
                }
                if (y > 1)
                {
                    player.Cells.Find(i => i.X == x && i.Y == minusY).IsTarget = true;
                }
                if (y < 10)
                {
                    player.Cells.Find(i => i.X == x && i.Y == addY).IsTarget = true;
                }

            }

            if (!selectedCell.IsShip)
            {
                player.Cells.Find(x => x.Id == selectedCell.Id).IsMiss = true;
                player.Cells.Find(x => x.Id == selectedCell.Id).IsEmpty = false;
            }

            return;
        }
    }
}
