using System;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public event Action OnFinish; //событие финиша
    public event Action OnDeath; //событие проигрыша

    public bool isShielded; //игрок использует щит

    private void OnTriggerStay(Collider other)
    {
        if (!isShielded && other.CompareTag("DeathZone"))
        {
            OnDeath?.Invoke();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            OnFinish?.Invoke();
        }
    }
}
