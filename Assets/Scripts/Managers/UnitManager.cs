using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableEnemy> _scriptableEnemies;

    public int EnemyCount { get; set; }


    private void Awake()
    {
        Instance = this;
        _scriptableEnemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList();
        EnemyCount = 0;
    }

    public BaseEnemy SpawnEnemy()
    {
        BaseEnemy enemy = Instantiate(_scriptableEnemies.FirstOrDefault().EnemyPrefab);
        enemy.transform.position = GridManager.Instance.GetRandomBorderTileWalkable().TileCoordinates.WorldPosition;
        EnemyCount++;
        return enemy;
    }

    


    //private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    //{
    //    //Finds all units from told faction, random shuffle and gets first, selects the prefab from scriptable object
    //    return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    //}
}
