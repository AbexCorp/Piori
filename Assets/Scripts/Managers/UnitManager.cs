using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableEnemy> _scriptableEnemies;
    public List<BaseEnemy> SpawnedEnemies { get; private set; }



    private void Awake()
    {
        Instance = this;
        _scriptableEnemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList();
        SpawnedEnemies = new();
    }

    public BaseEnemy SpawnEnemy()
    {
        BaseEnemy enemy = Instantiate(_scriptableEnemies.FirstOrDefault().EnemyPrefab);
        enemy.transform.position = GridManager.Instance.GetRandomBorderTileWalkable().TileCoordinates.WorldPosition;
        SpawnedEnemies.Add(enemy);
        return enemy;
    }

    


    //private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    //{
    //    //Finds all units from told faction, random shuffle and gets first, selects the prefab from scriptable object
    //    return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    //}
}
