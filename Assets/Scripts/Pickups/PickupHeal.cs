using UnityEngine;

public class PickupHeal : PickupItem
{
    [SerializeField] int healValue = 10;

    protected override void OnPickedUp(GameObject receiver)
    {
        HealthSystem healthSystem = receiver.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(healValue);
    }
}