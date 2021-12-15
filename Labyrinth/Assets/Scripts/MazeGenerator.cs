﻿using System;
using System.Collections.Generic;

[Flags]
public enum WallState
{
    // 0000 -> стены отсутствуют
    // 1111 -> ячейка окружена стенами
    LEFT = 1, // 0001
    RIGHT = 2, // 0010
    UP = 4, // 0100
    DOWN = 8, // 1000

    VISITED = 128, // 1000 0000
}

public struct Position
{
    public int X;
    public int Y;
}

/// <summary>
/// Соседняя ячейка с общей стеной
/// </summary>
public struct Neighbour
{
    public Position Position;
    public WallState SharedWall;
}

public static class MazeGenerator
{
    /// <summary>
    /// Получение стены со стороны соседней ячейки
    /// </summary>
    /// <param name="wall">стена</param>
    /// <returns>противоположная стена</returns>
    private static WallState GetOppositeWall(WallState wall)
    {
        switch (wall)
        {
            case WallState.RIGHT:
                return WallState.LEFT;
            case WallState.LEFT:
                return WallState.RIGHT;
            case WallState.UP:
                return WallState.DOWN;
            case WallState.DOWN:
                return WallState.UP;
            default:
                return WallState.LEFT;
        }
    }

    /// <summary>
    /// Создание лабиринта с помощью алгоритма Recursive Backtracker
    /// </summary>
    /// <param name="maze">лабиринт</param>
    /// <param name="width">ширина лабиринта</param>
    /// <param name="height">глубина лабиринта</param>
    /// <returns>готовый лабиринт</returns>
    private static WallState[,] ApplyRecursiveBacktracker(WallState[,] maze, uint width, uint height)
    {
        var rng = new Random();
        var positionStack = new Stack<Position>();
        var position = new Position { X = 0, Y = 0 }; //начальная точка

        maze[position.X, position.Y] |= WallState.VISITED;  // 1000 1111
        positionStack.Push(position);

        while (positionStack.Count > 0)
        {
            var current = positionStack.Pop();
            var neighbours = GetUnvisitedNeighbours(current, maze, width, height);

            if (neighbours.Count > 0)
            {
                positionStack.Push(current);

                var randIndex = rng.Next(0, neighbours.Count);
                var randomNeighbour = neighbours[randIndex];

                var nPosition = randomNeighbour.Position;
                maze[current.X, current.Y] &= ~randomNeighbour.SharedWall;
                maze[nPosition.X, nPosition.Y] &= ~GetOppositeWall(randomNeighbour.SharedWall);
                maze[nPosition.X, nPosition.Y] |= WallState.VISITED;

                positionStack.Push(nPosition);
            }
        }

        return maze;
    }

    /// <summary>
    /// Получение непосещенных соседних ячеек
    /// </summary>
    /// <param name="p">текущая ячейка</param>
    /// <param name="maze">лабиринт</param>
    /// <param name="width">ширина лабиринта</param>
    /// <param name="height">глубина лабиринта</param>
    /// <returns>список соседних ячеек</returns>
    private static List<Neighbour> GetUnvisitedNeighbours(Position p, WallState[,] maze, uint width, uint height)
    {
        var list = new List<Neighbour>();

        if (p.X > 0) // left
        {
            if (!maze[p.X - 1, p.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X - 1,
                        Y = p.Y
                    },
                    SharedWall = WallState.LEFT
                });
            }
        }

        if (p.Y > 0) // DOWN
        {
            if (!maze[p.X, p.Y - 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y - 1
                    },
                    SharedWall = WallState.DOWN
                });
            }
        }

        if (p.Y < height - 1) // UP
        {
            if (!maze[p.X, p.Y + 1].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X,
                        Y = p.Y + 1
                    },
                    SharedWall = WallState.UP
                });
            }
        }

        if (p.X < width - 1) // RIGHT
        {
            if (!maze[p.X + 1, p.Y].HasFlag(WallState.VISITED))
            {
                list.Add(new Neighbour
                {
                    Position = new Position
                    {
                        X = p.X + 1,
                        Y = p.Y
                    },
                    SharedWall = WallState.RIGHT
                });
            }
        }

        return list;
    }

    /// <summary>
    /// Генерация лабиринта
    /// </summary>
    /// <param name="width">ширина лабиринта</param>
    /// <param name="height">глубина лабиринта</param>
    /// <returns>сгенерированный лабиринт</returns>
    public static WallState[,] Generate(uint width, uint height)
    {
        WallState[,] maze = new WallState[width, height];
        WallState initial = WallState.RIGHT | WallState.LEFT | WallState.UP | WallState.DOWN;
        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                maze[i, j] = initial;  // 1111
            }
        }

        return ApplyRecursiveBacktracker(maze, width, height);
    }
}
