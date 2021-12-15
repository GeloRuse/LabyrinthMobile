using System.Collections.Generic;

public class MazeSolver
{
    /// <summary>
    /// Очистка посещенных ячеек
    /// </summary>
    /// <param name="maze">лабиринт</param>
    /// <returns>лабиринт с непосещенными ячейками</returns>
    private static WallState[,] ClearVisited(WallState[,] maze)
    {
        for (int i = 0; i < maze.GetLength(0); i++)
        {
            for (int j = 0; j < maze.GetLength(1); j++)
            {
                maze[i, j] &= ~WallState.VISITED;
            }
        }
        return maze;
    }

    /// <summary>
    /// Поиск пути к финишу
    /// </summary>
    /// <param name="maze">лабиринт</param>
    /// <returns>путь к финишу</returns>
    public static Stack<Position> SolveMaze(WallState[,] maze)
    {
        maze = ClearVisited(maze);

        Position endPos = new Position { X = maze.GetLength(0) - 1, Y = maze.GetLength(1) - 1 };
        Stack<Position> path = new Stack<Position>();
        path.Push(new Position { X = 0, Y = 0 });
        maze[0, 0] |= WallState.VISITED;

        Solve(maze, path, endPos);

        return path;
    }

    /// <summary>
    /// Рекурсивный метод поиска пути
    /// </summary>
    /// <param name="maze">лабиринт</param>
    /// <param name="path">текущий путь</param>
    /// <param name="endPos">финиш</param>
    private static void Solve(WallState[,] maze, Stack<Position> path, Position endPos)
    {
        Position curPos = path.Peek();
        if (curPos.Equals(endPos))
        {
            return;
        }
        List<Position> neighbours = GetNeighbours(maze, curPos);
        if (neighbours.Count == 0)
        {
            path.Pop();
        }
        foreach (Position chld in neighbours)
        {
            path.Push(chld);
            Solve(maze, path, endPos);
        }
    }

    /// <summary>
    /// Получение ячеек, в которые можно попасть
    /// </summary>
    /// <param name="maze">лабиринт</param>
    /// <param name="pos">текущая ячейка</param>
    /// <returns>список ячеек</returns>
    private static List<Position> GetNeighbours(WallState[,] maze, Position pos)
    {
        var list = new List<Position>();
        if (!maze[pos.X, pos.Y].HasFlag(WallState.UP) && !maze[pos.X, pos.Y + 1].HasFlag(WallState.VISITED))
        {
            list.Add(new Position
            {
                X = pos.X,
                Y = pos.Y + 1
            });
            maze[pos.X, pos.Y + 1] |= WallState.VISITED;
        }
        if (!maze[pos.X, pos.Y].HasFlag(WallState.RIGHT) && !maze[pos.X + 1, pos.Y].HasFlag(WallState.VISITED))
        {
            list.Add(new Position
            {
                X = pos.X + 1,
                Y = pos.Y
            });
            maze[pos.X + 1, pos.Y] |= WallState.VISITED;
        }
        if (!maze[pos.X, pos.Y].HasFlag(WallState.DOWN) && !maze[pos.X, pos.Y - 1].HasFlag(WallState.VISITED))
        {
            list.Add(new Position
            {
                X = pos.X,
                Y = pos.Y - 1
            });
            maze[pos.X, pos.Y - 1] |= WallState.VISITED;
        }
        if (!maze[pos.X, pos.Y].HasFlag(WallState.LEFT) && !maze[pos.X - 1, pos.Y].HasFlag(WallState.VISITED))
        {
            list.Add(new Position
            {
                X = pos.X - 1,
                Y = pos.Y
            });
            maze[pos.X - 1, pos.Y] |= WallState.VISITED;
        }
        return list;
    }
}
