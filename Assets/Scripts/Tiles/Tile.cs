using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    //Components
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected GameObject _highlight;


    //Inspector References
    public ScriptableTile ScriptableTile;


    //Code References
    public BaseTower OccuppyingTower;


    //Fields & Properties
    public string TileName => ScriptableTile.TileName;
    public bool IsWalkable => ScriptableTile.IsWalkable && OccuppyingTower == null;
    public bool IsBuildable => ScriptableTile.IsBuildable && OccuppyingTower == null;
    public bool BlocksBullets => ScriptableTile.BlocksBullets;


    [SerializeField]
    protected bool _isWalkable;





    public virtual void Init(int x, int y)
    {
        //bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        //_spriteRenderer.color = isOffset ? _primaryColor : _secondaryColor;
    }

    public void SetTower(BaseTower tower) //Move tower
    {
        //if (tower.OccupiedTile != null) //If tile was set somwhere already, will be used when moving unit
            //tower.OccupiedTile.OccupiedUnit = null;

        tower.transform.position = transform.position;
        OccuppyingTower = tower;
        tower.OccupiedTile = this;
    }
    public void BuildTower(BaseTower towerPrefab)
    {
        BaseTower tower = TowerManager.Instance.SpawnTower(towerPrefab);
        tower.transform.position = transform.position;
        OccuppyingTower = tower;
        tower.OccupiedTile = this;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }
    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    private void OnMouseDown()
    {
        //if (GameManager.Instance.GameState != GameState.BuyPhase)
        //    return;
        if(TowerManager.Instance.IsSelling == true) //Selling
        {
            if (IsBuildable || OccuppyingTower == null)
                return;

            OccuppyingTower.OccupiedTile = null;
            Destroy(OccuppyingTower.gameObject);
            OccuppyingTower = null;
            MenuManager.Instance.StopSelling();
        }
        else if(TowerManager.Instance.SelectedTowerPrefabToBuy != null) //Building
        {
            if (OccuppyingTower != null || !IsBuildable)
                return;

            BuildTower(TowerManager.Instance.SelectedTowerPrefabToBuy);
            MenuManager.Instance.ClearTower();
        }
    }
}
