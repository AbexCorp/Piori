using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Scriptable Tower")]
public class ScriptableTower : ScriptableObject
{
    public BaseTower TowerPrefab;

    public BaseTower UpgradedFrom;
    public BaseTower[] UpgradesTo;

    public TowerType TowerType;
    public TowerTier TowerTier;

    //public bool ShootsProjectile;
    //public Projectile ProjectilePrefab
}


public enum TowerType
{
    Bullet = 0,
    Destruction = 1,
    Debuff = 2
}

public enum TowerTier
{
    TierOne = 0,
    TierTwo = 1,
    TierThree = 2
}
