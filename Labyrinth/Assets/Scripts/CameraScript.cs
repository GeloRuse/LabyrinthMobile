using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField]
    private MazeRenderer maze; //лабиринт

    /// <summary>
    /// Назначение размера камеры в зависимости от размера лабиринта
    /// </summary>
    private void Start()
    {
        Position wh = maze.GetWidthHeight();
        float size = (wh.X + wh.Y) / 2f;
        GetComponent<Camera>().orthographicSize = size;
    }
}
