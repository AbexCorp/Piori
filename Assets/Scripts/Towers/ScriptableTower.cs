using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Scriptable Tower")]
public class ScriptableTower : ScriptableObject
{
    [SerializeField]
    protected BaseTower _towerPrefab;

    [SerializeField]
    protected BaseTower _upgradedFrom;
    [SerializeField]
    protected BaseTower[] _upgradesTo;

    [SerializeField]
    public TowerType _towerType;
    [SerializeField]
    public TowerTier _towerTier;


    [SerializeField] 
    protected string _towerName = "Building name is null";
    [SerializeField]
    private float _baseAttackSpeed;
    [SerializeField]
    private int _baseDamage;
    [SerializeField]
    private float _baseRange;

    [SerializeField]
    private int _cost = 20;

    //public bool ShootsProjectile;
    //public Projectile ProjectilePrefab


    public BaseTower TowerPrefab { get { return _towerPrefab; } }

    public BaseTower UpgradedFrom {  get { return _upgradedFrom; } }
    public BaseTower[] UpgradesTo { get { return _upgradesTo; } }

    public TowerType TowerType { get { return _towerType; } }
    public TowerTier TowerTier { get { return _towerTier; } }


    public string TowerName { get { return _towerName; } }
    public float BaseAttackSpeed { get { return _baseAttackSpeed; } }
    public int BaseDamage { get { return _baseDamage; } }
    public float BaseRange { get { return _baseRange; } }

    public int Cost { get { return _cost; } }
}


public enum TowerType
{
    Wall = 0,
    Bullet = 1,
    Destruction = 2,
    Debuff = 3
}

public enum TowerTier
{
    TierOne = 0,
    TierTwo = 1,
    TierThree = 2
}
