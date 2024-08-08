using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Unit")]
public class ScriptableEnemy : ScriptableObject
{
    public BaseEnemy UnitPrefab;


    public string Name = "NOT SET";
    public int PointCost = 1;

    public float Speed = 1;
    public int Health = 1;

    public int Damage = 1;
    public float AttackSpeed = 1;
}