using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Scriptable Enemy")]
public class ScriptableEnemy : ScriptableObject
{
    [SerializeField]
    private BaseEnemy _enemyPrefab;

    [Space]
    [SerializeField]
    private string _name = "NOT SET";
    [SerializeField]
    private int _pointCost = 1;
    [SerializeField]
    private int _currencyOnKill = 1;

    [Space]
    [SerializeField]
    private float _speed = 1;
    [SerializeField]
    private int _health = 1;

    [Space]
    [SerializeField]
    private int _damage = 1;
    [SerializeField]
    private float _attackSpeed = 1;
    [SerializeField]
    private float _attackRange = 1;
    [SerializeField]
    private bool _attackIsRanged = false;
    [SerializeField]
    private bool _stopsMovingOnAttackCooldown = false;


    public BaseEnemy EnemyPrefab => _enemyPrefab;


    public string Name => _name;
    public int PointCost => _pointCost;
    public int CurrencyOnKill => _currencyOnKill;

    public float Speed => _speed;
    public int Health => _health;

    public int Damage => _damage;
    public float AttackSpeed => _attackSpeed;
    public float AttackRange => _attackRange;
    public bool AttackIsRanged => _attackIsRanged;
    public bool StopsMovingOnAttackCooldown => _stopsMovingOnAttackCooldown;
}