using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Combat Action", menuName = "New Combat Action")]

public class CombatAction : ScriptableObject
{
    public enum Type
    {
        Attack,
        Heal

    }

    public string DisplayName;
    public Type ActionType;

    [Header("Damage")]
    public int Damage;
    public GameObject ProjectilePrefab;

    [Header("Heal")]
    public int HealAmount;
}
