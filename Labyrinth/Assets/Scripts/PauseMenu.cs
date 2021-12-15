using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenu; //меню паузы
    [SerializeField]
    private GameLoop gameLoop; //основной игровой цикл

    [SerializeField]
    private FadeScript fadeScript; //затухание экрана

    /// <summary>
    /// Приостановить игру и показать меню паузы
    /// </summary>
    public void PauseGame()
    {
        fadeScript.DimIn();
        pauseMenu.SetActive(true);
        gameLoop.gamePaused = true;
    }

    /// <summary>
    /// Возобновить игру
    /// </summary>
    public void ContinueGame()
    {
        fadeScript.DimOut();
        pauseMenu.SetActive(false);
        gameLoop.gamePaused = false;
    }
}
