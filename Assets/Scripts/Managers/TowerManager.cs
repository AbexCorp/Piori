using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;


    public bool IsSelling { get; private set; }
    public bool IsBuying => SelectedTowerPrefabToBuy == true;

    //Building
    public BaseTower SelectedTowerPrefabToBuy;
    public BaseTower SelectedTowerOnMap;

    private void Awake()
    {
        Instance = this;
    }

    public BaseTower SpawnTower(BaseTower towerPrefab)
    {
        return Instantiate(towerPrefab);
    }


    public void ClearAllInstructions()
    {
        ClearTowerToBuy();
        ClearSelectedTowerOnMap();
        StopSelling();
    }

    //Buying
    public void SelectTowerToBuy(BaseTower towerPrefab)
    {
        SelectedTowerPrefabToBuy = towerPrefab;
        ClearSelectedTowerOnMap();
        StopSelling();
    }
    public void ClearTowerToBuy()
    {
        SelectedTowerPrefabToBuy = null;
    }

    //Selecting
    public void SelectTowerOnMap(BaseTower tower)
    {
        SelectedTowerOnMap = tower;
        ClearTowerToBuy();
        StopSelling();
    }
    public void ClearSelectedTowerOnMap()
    {
        SelectedTowerOnMap = null;
    }

    //Selling
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
