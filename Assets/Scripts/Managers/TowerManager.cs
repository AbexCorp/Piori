using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    //Building
    public BaseTower SelectedTowerPrefabToBuy;
    public BaseTower SelectedTowerOnMap;
    public bool IsSelling = false;

    private void Awake()
    {
        Instance = this;
    }

    public BaseTower SpawnTower(BaseTower towerPrefab)
    {
        return Instantiate(towerPrefab);
    }

    public void SelectTowerToBuy(BaseTower towerPrefab)
    {
        SelectedTowerPrefabToBuy = towerPrefab;
        ClearSelectedTowerOnMap();
    }
    public void ClearTowerToBuy()
    {
        SelectedTowerPrefabToBuy = null;
    }


    public void SelectTowerOnMap(BaseTower tower)
    {
        SelectedTowerOnMap = tower;
        ClearTowerToBuy();
    }
    public void ClearSelectedTowerOnMap()
    {
        SelectedTowerOnMap = null;
    }

    public void StartSelling()
    {
        IsSelling = true;
        ClearTowerToBuy();
        ClearSelectedTowerOnMap();
    }
    public void StopSelling()
    {
        IsSelling = false;
    }
}
