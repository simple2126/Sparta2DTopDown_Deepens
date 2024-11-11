using System;
using UnityEngine;

public abstract class PickupItem : MonoBehaviour
{
    [SerializeField] private AudioClip pickupSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 아이템 효과
        OnPickedUp(collision.gameObject);
        
        if(pickupSound != null) SoundManager.PlayClip(pickupSound);

        Destroy(gameObject);
    }

    protected abstract void OnPickedUp(GameObject gameObject);
}