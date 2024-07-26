using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;

    private List<ScriptableUnit> _units;

    public BaseHero SelectedHero;

    private void Awake()
    {
        Instance = this;
        _units = Resources.LoadAll<ScriptableUnit>("Units").ToList(); //Load all sunits from resource folder
    }

    //public void SpawnHeroes()
    //{
    //    var heroCount = 1;
    //    for(int i = 0; i < heroCount; i++)
    //    {
    //        var randomPrefab = GetRandomUnit<BaseHero>(Faction.Hero);
    //        var randomSpawnedHero = Instantiate(randomPrefab);
    //        var randomSpawnTile = GridManager.Instance.GetHeroSpawnTile();

    //        randomSpawnTile.SetUnit(randomSpawnedHero);
    //    }

    //    //GameManager.Instance.ChangeState(GameState.SpawnEnemies);
    //}

    private T GetRandomUnit<T>(Faction faction) where T : BaseUnit
    {
        //Finds all units from told faction, random shuffle and gets first, selects the prefab from scriptable object
        return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
    }

    //public void SetSelectedHero(BaseHero hero)
    //{
    //    SelectedHero = hero;
    //    MenuManager.Instance.ShowSelectedHero(hero);
    //}
}
