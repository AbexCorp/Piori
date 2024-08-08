using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy
{
    //Components

    //Inspector References
    public ScriptableEnemy ScriptableEnemy;

    //Code References


    //Fields & Properties
    public string Name => ScriptableEnemy.Name;
    public int PointCost => ScriptableEnemy.PointCost;
    public float Speed => ScriptableEnemy.Speed;
    public int Health => ScriptableEnemy.Health;
    public int Damage => ScriptableEnemy.Damage;
    public float AttackSpeed => ScriptableEnemy.AttackSpeed;

    protected Tile destination;
    protected float distanceToPlayer;


    protected void Start()
    {
        
    }

    // Update is called once per frame
    protected void Update()
    {
        
    }
}
