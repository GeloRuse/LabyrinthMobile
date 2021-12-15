using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    [SerializeField]
    private FadeScript fadeScript; //затемнение экрана

    private PlayerCollision playerScript; //коллизия игрока
    [SerializeField]
    private PlayerShield playerShield; //щит игрока

    [SerializeField]
    private MazeRenderer mazeScript; //лабиринт

    [SerializeField]
    private Transform playerPrefab; //префаб игрока
    [SerializeField]
    private Transform playerPiecesPrefab; //префаб осколков игрока

    [SerializeField]
    private Transform finishPrefab; //префаб финиша

    [SerializeField]
    private Transform deathPrefab; //префаб смертельной зоны
    [SerializeField]
    [Range(1, 10)]
    private float deathZoneChance = 5f; //шанс появления смертельной зоны (чем выше значение, тем ниже шанс)

    [SerializeField]
    private float playerSpeed = 1f; //скорость передвижения игрока

    private WallState[,] maze; //лабиринт
    private List<Position> pathList = new List<Position>(); //путь до финиша
    [SerializeField]
    private Transform pathPrefab;

    public bool gamePaused; //состояние паузы

    [SerializeField]
    private float time = 2f;
    private float counter;


    private void Awake()
    {
        maze = mazeScript.PrepareeMaze();
        SpawnEntities(maze);
        PreparePath();
    }

    private void FixedUpdate()
    {
        if (counter < time)
        {
            counter += Time.deltaTime;
        }
        else
        {
            MovePlayer();
        }
    }

    /// <summary>
    /// Движение игрока к финишу
    /// </summary>
    private void MovePlayer()
    {
        if (pathList.Count > 0 && playerScript != null && !gamePaused)
        {
            Position pos = pathList[0];
            Vector3 target = new Vector3((-maze.GetLength(0) / 2) + pos.X, playerScript.transform.position.y, (-maze.GetLength(1) / 2) + pos.Y);
            playerScript.transform.position = Vector3.MoveTowards(playerScript.transform.position, target, Time.deltaTime * playerSpeed);
            if (Vector3.Distance(playerScript.transform.position, target) < 0.001f)
            {
                pathList.RemoveAt(0);
            }
        }
    }

    /// <summary>
    /// Подготовить путь к финишу
    /// </summary>
    private void PreparePath()
    {
        pathList.Clear();
        Stack<Position> path = MazeSolver.SolveMaze(maze);
        Stack<Position> pathRev = new Stack<Position>();
        while (path.Count > 0)
        {
            pathRev.Push(path.Pop());
        }
        while (pathRev.Count > 0)
        {
            Position pos = pathRev.Pop();
            pathList.Add(pos);
            if (pos.Equals(new Position { X = maze.GetLength(0) - 1, Y = maze.GetLength(1) - 1 }))
                pathRev.Clear();
        }

        DrawPath();
    }

    /// <summary>
    /// Отображение пути к финишу
    /// </summary>
    private void DrawPath()
    {
        for (int i = 1; i < pathList.Count; i++)
        {
            Vector3 pA = new Vector3((-maze.GetLength(0) / 2) + pathList[i - 1].X, 0.1f, (-maze.GetLength(1) / 2) + pathList[i - 1].Y);
            Vector3 pB = new Vector3((-maze.GetLength(0) / 2) + pathList[i].X, 0.1f, (-maze.GetLength(1) / 2) + pathList[i].Y);
            Transform pathTr = Instantiate(pathPrefab, transform);
            Vector3 between = pB - pA;
            float distance = between.magnitude;
            pathTr.localScale = new Vector3(pathTr.localScale.x, pathTr.localScale.y, distance);
            pathTr.position = pA + (between / 2f);
            pathTr.LookAt(pB);
        }
    }

    /// <summary>
    /// Создание игровых объектов
    /// </summary>
    /// <param name="maze">лабиринт</param>
    private void SpawnEntities(WallState[,] maze)
    {
        SpawnPlayer();
        SpawnFinish();
        SpawnDeathZones();
    }

    /// <summary>
    /// Создание игрока
    /// </summary>
    private void SpawnPlayer()
    {
        playerScript = Instantiate(playerPrefab, transform).GetComponent<PlayerCollision>();
        SubEvents();
        playerScript.transform.position = new Vector3(-maze.GetLength(0) / 2, playerScript.transform.position.y, -maze.GetLength(1) / 2);
        playerShield.AssignPlayer(playerScript.transform);
    }

    /// <summary>
    /// Создание финиша
    /// </summary>
    private void SpawnFinish()
    {
        var finish = Instantiate(finishPrefab, transform);
        finish.position = new Vector3((-maze.GetLength(0) / 2) + maze.GetLength(0) - 1, finish.position.y, (-maze.GetLength(1) / 2) + maze.GetLength(1) - 1);
    }

    /// <summary>
    /// Создание смертельных зон
    /// </summary>
    private void SpawnDeathZones()
    {
        var rng = new System.Random();
        for (int i = 1; i < maze.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < maze.GetLength(1) - 1; j++)
            {
                if (rng.Next(10) > deathZoneChance)
                {
                    var deathZone = Instantiate(deathPrefab, transform);
                    deathZone.position = new Vector3((-maze.GetLength(0) / 2) + i, deathZone.position.y, (-maze.GetLength(1) / 2) + j);
                }
            }
        }
    }

    /// <summary>
    /// Завершение игры
    /// </summary>
    private void FinishGame()
    {
        playerScript.GetComponentInChildren<ParticleSystem>().Play();
        fadeScript.FadeToScene();
    }

    /// <summary>
    /// Проигрыш
    /// </summary>
    private void EndGame()
    {
        Transform playerPieces = Instantiate(playerPiecesPrefab, transform);
        playerPieces.position = playerScript.gameObject.transform.position;
        UnsubEvents();
        Destroy(playerScript.gameObject);
        StartCoroutine(Restart(playerPieces));
    }

    /// <summary>
    /// Перезапуск игры
    /// </summary>
    /// <param name="pieces">осколки игрока</param>
    /// <returns></returns>
    private IEnumerator Restart(Transform pieces)
    {
        yield return new WaitForSeconds(2f);
        Destroy(pieces.gameObject);
        SpawnPlayer();
        PreparePath();
        counter = 0;
    }

    private void SubEvents()
    {
        playerScript.OnFinish += FinishGame;
        playerScript.OnDeath += EndGame;
    }

    private void UnsubEvents()
    {
        playerScript.OnFinish -= FinishGame;
        playerScript.OnDeath -= EndGame;
    }
}
