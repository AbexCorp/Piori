using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableEnemy> _scriptableEnemies;


    private void Awake()
    {
        Instance = this;
        _scriptableEnemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList(); //Load all sunits from resource folder
    }

    


    //private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    //{
    //    //Finds all units from told faction, random shuffle and gets first, selects the prefab from scriptable object
    //    return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    //}
}
