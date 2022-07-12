using BattleshipsSimulation.Models;

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
                    player.Cells.Add(new Cell() { Id = Guid.NewGuid().ToString(), X = i, Y = j });
                }
            }
            return Task.FromResult(true);
        }

    }
}
