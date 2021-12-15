using UnityEngine;

public class MazeRenderer : MonoBehaviour
{
    [SerializeField]
    [Range(1, 50)]
    private uint width = 5; //ширина лабиринта

    [SerializeField]
    [Range(1, 50)]
    private uint height = 5; //глубина лабиринта

    [SerializeField]
    private Transform wallPrefab; //префаб стены

    [SerializeField]
    private Transform floorPrefab; //префаб пола

    /// <summary>
    /// Подготовить лабиринт
    /// </summary>
    /// <returns>готовый лабиринт</returns>
    public WallState[,] PrepareeMaze()
    {
        var maze = MazeGenerator.Generate(width, height);
        Visualize(maze);
        return maze;
    }

    /// <summary>
    /// Установка стен лабиринта
    /// </summary>
    /// <param name="maze">лабиринт</param>
    private void Visualize(WallState[,] maze)
    {
        var floor = Instantiate(floorPrefab, transform);
        floor.localScale = new Vector3(width / 8f, 1, height / 8f);

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var cell = maze[i, j];
                var position = new Vector3((-width / 2) + i, 0, (-height / 2) + j);

                if (cell.HasFlag(WallState.UP))
                {
                    var topWall = Instantiate(wallPrefab, transform);
                    topWall.position = position + new Vector3(0, 0, .5f);
                    topWall.localScale = new Vector3(1, topWall.localScale.y, topWall.localScale.z);
                }

                if (cell.HasFlag(WallState.LEFT))
                {
                    var leftWall = Instantiate(wallPrefab, transform);
                    leftWall.position = position + new Vector3(-.5f, 0, 0);
                    leftWall.localScale = new Vector3(1, leftWall.localScale.y, leftWall.localScale.z);
                    leftWall.eulerAngles = new Vector3(0, 90, 0);
                }

                if (i == width - 1)
                {
                    if (cell.HasFlag(WallState.RIGHT))
                    {
                        var rightWall = Instantiate(wallPrefab, transform);
                        rightWall.position = position + new Vector3(.5f, 0, 0);
                        rightWall.localScale = new Vector3(1, rightWall.localScale.y, rightWall.localScale.z);
                        rightWall.eulerAngles = new Vector3(0, 90, 0);
                    }
                }

                if (j == 0)
                {
                    if (cell.HasFlag(WallState.DOWN))
                    {
                        var bottomWall = Instantiate(wallPrefab, transform);
                        bottomWall.position = position + new Vector3(0, 0, -.5f);
                        bottomWall.localScale = new Vector3(1, bottomWall.localScale.y, bottomWall.localScale.z);
                    }
                }
            }
        }
    }

    public Position GetWidthHeight()
    {
        return new Position { X = (int)width, Y = (int)height };
    }
}
