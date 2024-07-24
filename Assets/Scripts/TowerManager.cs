using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public BaseTower SpawnTower(BaseTower towerPrefab)
    {
        return Instantiate(towerPrefab);
    }
}
