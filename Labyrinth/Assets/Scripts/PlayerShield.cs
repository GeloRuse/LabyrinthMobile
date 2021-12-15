using UnityEngine;

public class PlayerShield : MonoBehaviour
{
    [SerializeField]
    private float shieldTime = 2f; //допустимое время использования щита

    [SerializeField]
    private Material shieldMaterial; //цвет щита
    private Material playerMaterial; //цвет игрока

    private Renderer playerRend;
    private PlayerCollision playerCol; //коллизия игрока

    private float counter; //счетчик времени
    private bool isHeld; //кнопка удерживается

    private void Update()
    {
        ShieldLoop();
    }

    /// <summary>
    /// Цикл щита
    /// </summary>
    private void ShieldLoop()
    {
        if (playerRend != null)
        {
            if (isHeld)
            {
                counter += Time.deltaTime;
                TurnOnShield();
                if (counter >= shieldTime)
                {
                    counter = 0;
                    isHeld = false;
                }
            }
            else if (counter > 0)
            {
                counter = 0;
            }
            else
                TurnOffShield();
        }
    }

    /// <summary>
    /// Получение цветовых компонентов игрока
    /// </summary>
    /// <param name="player">игрок</param>
    public void AssignPlayer(Transform player)
    {
        playerRend = player.GetComponent<Renderer>();
        playerCol = playerRend.GetComponent<PlayerCollision>();
        playerMaterial = playerRend.material;
    }

    /// <summary>
    /// Включить щит
    /// </summary>
    private void TurnOnShield()
    {
        playerCol.isShielded = true;
        playerRend.material = shieldMaterial;
    }

    /// <summary>
    /// Выключить щит
    /// </summary>
    private void TurnOffShield()
    {
        playerCol.isShielded = false;
        playerRend.material = playerMaterial;
    }

    /// <summary>
    /// Кнопка зажата
    /// </summary>
    public void ButtonDown()
    {
        isHeld = true;
    }

    /// <summary>
    /// Кнопка отпущена
    /// </summary>
    public void ButtonUp()
    {
        isHeld = false;
    }
}
