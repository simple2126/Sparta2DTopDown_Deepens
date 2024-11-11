using System.Collections.Generic;
using UnityEngine;

public class PickupStatModifiers : PickupItem
{
    [SerializeField] private List<CharacterStat> statsModifier;
    protected override void OnPickedUp(GameObject receiver)
    {
        CharacterStatsHandler statHandler = receiver.GetComponent<CharacterStatsHandler>();
        foreach (CharacterStat stat in statsModifier)
        {
            statHandler.AddStatModifier(stat);
        }

        HealthSystem healthSystem = receiver.GetComponent<HealthSystem>();
        healthSystem.ChangeHealth(0);
    }
}