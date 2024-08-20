using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    //Components
    [SerializeField]
    private GameObject _tileObject;
    [SerializeField]
    private GameObject _selectedTowerToBuyObject;

    //Inspector References
    //Code References


    private void Awake()
    {
        Instance = this;
    }

    public void ShowTileInfo(Tile tile)
    {
        if(tile == null)
        {
            _tileObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.ScriptableTile.TileName;
        _tileObject.SetActive(true);
    }
    public void ShowTowerToBuy()
    {
        if(TowerManager.Instance.SelectedTowerPrefabToBuy == null)
        {
            _selectedTowerToBuyObject.SetActive(false);
            return;
        }

        _selectedTowerToBuyObject.GetComponentInChildren<Text>().text = $"Buying: {TowerManager.Instance.SelectedTowerPrefabToBuy.ScriptableTower.TowerName}";
        _selectedTowerToBuyObject.SetActive(true);
    }

    //Spawn button
    public void SelectTower()
    {
        BaseTower selectedPrefab = Resources.LoadAll<ScriptableTower>("Towers").ToList().FirstOrDefault().TowerPrefab;
        TowerManager.Instance.SelectTowerToBuy(selectedPrefab);
        ShowTowerToBuy();
    }
    public void ClearTower()
    {
        TowerManager.Instance.ClearAllInstructions(); /////


        TowerManager.Instance.ClearTowerToBuy();
        ShowTowerToBuy();
    }

    //Selling button
    public void StartSelling()
    {
        if(TowerManager.Instance.IsSelling == true)
        {
            StopSelling();
            return;
        }
        TowerManager.Instance.StartSelling();
        _selectedTowerToBuyObject.GetComponentInChildren<Text>().text = $"Selling";
        _selectedTowerToBuyObject.SetActive(true);
    }
    public void StopSelling()
    {
        TowerManager.Instance.StopSelling();
        _selectedTowerToBuyObject.SetActive(false);
    }
}
