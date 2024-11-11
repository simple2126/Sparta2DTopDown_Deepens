using System;
using UnityEngine;

public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 아이템 효과
        OnPickedUp(other.gameObject);

        if (pickupSound) SoundManager.PlayClip(pickupSound);

        Destroy(gameObject);
    }

    protected abstract void OnPickedUp(GameObject receiver);
}