using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;


    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private RectTransform _canvasRectTransform;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        InputManager.Instance.OnMouseLeftClickSubscribe(OnLeftClick);
    }
    private void Update()
    {
        UpdateTooltipPosition();
    }



    //Shows information about the tile under the cursor. Shows terrain type, tile info, and tower built on it
    #region TileHoverInfo

    [Space]
    [Space]
    [SerializeField]
    private TileHoverObject _tileHoverObject;
    [Serializable]
    public struct TileHoverObject
    {
        public GameObject TileHoverInfoGameObject;
        public RectTransform TileHoverInfoGameObjectTransform;

        public TMP_Text TileName;
        public TMP_Text BuiltTowerType;

        public GameObject checkboxWalking;
        public GameObject checkboxBuilding;
        public GameObject checkboxBlocksBullets;
    }
    public void ShowTileHoverInfo(Tile tile)
    {
        if(tile == null)
        {
            _tileHoverObject.TileName.text = "None";
            _tileHoverObject.BuiltTowerType.text = "Empty";
            _tileHoverObject.checkboxWalking.GetComponent<Image>().color = Color.red;
            _tileHoverObject.checkboxBuilding.GetComponent<Image>().color = Color.red;
            _tileHoverObject.checkboxBlocksBullets.GetComponent<Image>().color = Color.red;

            return;
        }
        _tileHoverObject.TileHoverInfoGameObject.SetActive(true);


        _tileHoverObject.TileName.text = tile.TileName;
        _tileHoverObject.BuiltTowerType.text = tile.OccuppyingTower == null ? "Empty" : tile.OccuppyingTower.ScriptableTower.TowerName;

        _tileHoverObject.checkboxWalking.GetComponent<Image>().color = tile.IsWalkable ? Color.green : Color.red;
        _tileHoverObject.checkboxBuilding.GetComponent<Image>().color = tile.IsBuildable ? Color.green : Color.red;
        _tileHoverObject.checkboxBlocksBullets.GetComponent<Image>().color = tile.BlocksBullets ? Color.green : Color.red;
    }
    public void HideTileHoverInfo()
    {
        if(_tileHoverObject.TileHoverInfoGameObjectTransform.anchoredPosition.x != 0)
            _tileHoverObject.TileHoverInfoGameObjectTransform.anchoredPosition = new Vector3(0, 0);
        else
            _tileHoverObject.TileHoverInfoGameObjectTransform.anchoredPosition = new Vector3(_tileHoverObject.TileHoverInfoGameObjectTransform.rect.width, 0);
    }

    #endregion


    //Shows tower buy menu as well as cancel buttons.
    #region ButtonBars

    [Space]
    [Space]
    [SerializeField]
    private ButtonBarObject _buttonBarObject;
    [Serializable]
    public struct ButtonBarObject
    {
        public GameObject TowerBar;
        public RectTransform TowerBarTransform;

        public GameObject CancelBar;
        public RectTransform CancelBarTransform;
        public Image Prewiev;
    }
    public void ShowTowerBuying()
    {
        if (TowerManager.Instance.IsSelling)
        {
            _buttonBarObject.TowerBar.SetActive(false);
            _buttonBarObject.CancelBar.SetActive(true);
            //_buttonBarObject.Prewiev = sell.png
        }
        else if (TowerManager.Instance.IsBuying)
        {
            _buttonBarObject.TowerBar.SetActive(false);
            _buttonBarObject.CancelBar.SetActive(true);
            //_buttonBarObject.Prewiev = tower.png
        }
        else
        {
            _buttonBarObject.TowerBar.SetActive(true);
            _buttonBarObject.CancelBar.SetActive(false);
        }
    }
    public void HideTowerButtonBar()
    {
        if(_buttonBarObject.TowerBarTransform.anchoredPosition.y != 0)
            _buttonBarObject.TowerBarTransform.anchoredPosition = new Vector3(0, 0);
        else
            _buttonBarObject.TowerBarTransform.anchoredPosition = new Vector3(0, _buttonBarObject.TowerBarTransform.rect.height);
    }
    public void HideCancelButtonBar()
    {
        if(_buttonBarObject.CancelBarTransform.anchoredPosition.y != 0)
            _buttonBarObject.CancelBarTransform.anchoredPosition = new Vector3(0, 0);
        else
            _buttonBarObject.CancelBarTransform.anchoredPosition = new Vector3(0, _buttonBarObject.CancelBarTransform.rect.height);
    }

    #endregion


    #region MouseFloatingTooltip

    [Space]
    [Space]
    [Header("Mouse Floating Tooltip")]
    [SerializeField]
    private RectTransform _tooltipObjectTransform;
    [SerializeField]
    private TMP_Text _tooltipText;
    public void ShowMouseTooltip(string text)
    {
        _tooltipText.text = text;
        _tooltipObjectTransform.gameObject.SetActive(true);
    }
    public void HideMouseTooltip()
    {
        _tooltipObjectTransform.gameObject.SetActive(false);
    }
    private void UpdateTooltipPosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRectTransform, InputManager.Instance.MousePosition, null, out Vector2 pos);
        _tooltipObjectTransform.anchoredPosition = pos + new Vector2((_tooltipObjectTransform.rect.width/2) + 6, (-_tooltipObjectTransform.rect.height/2) - 6);

        Vector3[] corners = new Vector3[4];
        Vector3[] canvasCorners = new Vector3[4];
        // [0] bottom left, [1] top left, [2] top right, [3] bottom right
        _tooltipObjectTransform.GetWorldCorners(corners);
        _canvasRectTransform.GetWorldCorners(canvasCorners);

        float newX = corners[3].x > canvasCorners[3].x ? (-_tooltipObjectTransform.rect.width / 2) - 6 : (_tooltipObjectTransform.rect.width/2) + 6;
        float newY = corners[3].y < canvasCorners[3].y ? (_tooltipObjectTransform.rect.height / 2) + 6 : (-_tooltipObjectTransform.rect.height/2) - 6;
        _tooltipObjectTransform.anchoredPosition = pos + new Vector2(newX, newY);
    }
    private void OnLeftClick(InputAction.CallbackContext context)
    {
        HideMouseTooltip();
    }

    #endregion


    #region Other

    [Space]
    [Space]
    [Header("Other")]
    [SerializeField]
    private GameObject _currencyGameObject;
    [SerializeField]
    private TMP_Text _currencyText;

    public void UpdateCurrency()
    {
        _currencyText.text = $"$:{TowerManager.Instance.Currency}";
    }



    [Space]
    [Space]
    [SerializeField]
    private GameObject _warningWindowGameObject;
    [SerializeField]
    private TMP_Text _warningText;
    private Coroutine _warningTimerCoroutine;
    public void ShowWarning(string warning)
    {
        if (_warningTimerCoroutine != null)
            StopCoroutine(_warningTimerCoroutine);
        _warningWindowGameObject.SetActive(true);
        _warningText.text = warning;
        _warningTimerCoroutine = StartCoroutine(WarningTimer());
    }
    private IEnumerator WarningTimer()
    {
        yield return new WaitForSeconds(2.5f);
        _warningWindowGameObject.SetActive(false);
    }



    [Space]
    [Space]
    [SerializeField]
    private UnityEngine.UI.Image _playerHealth;
    [SerializeField]
    private TMP_Text _healthBarText;
    public void UpdatePlayerHealth()
    {
        _playerHealth.fillAmount = PlayerController.Instance.CurrentHealth / (float)PlayerController.Instance.MaxHealth;
        _healthBarText.text = $"{PlayerController.Instance.CurrentHealth}/{PlayerController.Instance.MaxHealth}";
    }

    #endregion


    //ui commands

    //Cancels all instructions like buying and returns ui to normal
    public void StopAllCommands()
    {
        TowerManager.Instance.ClearAllInstructions();
        StopBuying();
        ShowTowerBuying();
    }


    //Called by buttons
    public void StartBuyingTower(BaseTower towerPrefab)
    {
        if(towerPrefab.Cost > TowerManager.Instance.Currency)
        {
            //Debug.LogWarning("Not enough money");
            ShowWarning("Not enough money");
            return;
        }
        TowerManager.Instance.SelectTowerToBuy(towerPrefab);
        ShowTowerBuying();
    }
    public void StopBuying()
    {
        TowerManager.Instance.ClearTowerToBuy();
        ShowTowerBuying();
    }

    public void StartSelling()
    {
        TowerManager.Instance.StartSelling();
        ShowTowerBuying();
    }
    public void StopSelling()
    {
        TowerManager.Instance.StopSelling();
        ShowTowerBuying();
    }
}
