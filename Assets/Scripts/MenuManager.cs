using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField]
    private GameObject _selectedHeroObject;
    [SerializeField]
    private GameObject _tileObject;

    [SerializeField]
    private GameObject _tower1Button;
    [SerializeField]
    private GameObject _clearTowerButton;
    public BaseTower _selectedTowerToBuy;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowSelectedHero(BaseHero hero)
    {
        if(hero == null)
        {
            _selectedHeroObject.SetActive(false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
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

    public void SelectTower()
    {
        _selectedTowerToBuy = Resources.LoadAll<ScriptableTower>("Towers").ToList().FirstOrDefault().TowerPrefab;
    }
    public void ClearTower()
    {
        _selectedTowerToBuy = null;
    }
}
