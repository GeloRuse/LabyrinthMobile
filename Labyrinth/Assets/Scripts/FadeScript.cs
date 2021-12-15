using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
    [SerializeField]
    private Animator anim; //анимация затухания и затемнения

    /// <summary>
    /// Обратный эффект затухания экрана
    /// </summary>
    private void Start()
    {
        anim.SetTrigger("FadeIn");
    }

    /// <summary>
    /// Затухание экрана
    /// </summary>
    public void FadeToScene()
    {
        anim.SetTrigger("FadeOut");
    }

    /// <summary>
    /// Затемнение экрана во время паузы
    /// </summary>
    public void DimIn()
    {
        anim.SetTrigger("DimIn");
    }

    /// <summary>
    /// Обратный эффект затемнения
    /// </summary>
    public void DimOut()
    {
        anim.SetTrigger("DimOut");
    }

    /// <summary>
    /// По завершению затухания перезагрузить уровень
    /// </summary>
    public void OnFadeComplete()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
