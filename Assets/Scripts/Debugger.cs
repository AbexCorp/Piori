using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Debugger : MonoBehaviour
{
    private List<ScriptableEnemy> _scriptableEnemies;
    int counter = 0;
    int cooldown = 1020;

    private void Awake()
    {
        _scriptableEnemies = Resources.LoadAll<ScriptableEnemy>("Units/Enemies").ToList();
    }

    void Update()
    {
        counter++;
        if (counter > cooldown)
        {
            counter = 0;
            if(cooldown - 50 > 0)
                cooldown -= 50;
            Spawn();
        }
    }
    private void Spawn()
    {
        BaseEnemy enemy = Instantiate(_scriptableEnemies.FirstOrDefault().EnemyPrefab);
        enemy.transform.position = GridManager.Instance.GetRandomBorderTileWalkable().TileCoordinates.WorldPosition;
    }
    //public void BuildTower(BaseTower towerPrefab)
    //{
    //    BaseTower tower = TowerManager.Instance.SpawnTower(towerPrefab);
    //    tower.transform.position = transform.position;
    //    OccuppyingTower = tower;
    //    tower.OccupiedTile = this;
    //    EnableCollisions(fullObstacle: BlocksBullets);
    //}
}
