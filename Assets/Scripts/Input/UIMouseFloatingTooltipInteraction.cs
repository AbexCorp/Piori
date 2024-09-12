using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIMouseFloatingToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    [TextArea]
    private string _hoverTooltipText;
    public void OnPointerEnter(PointerEventData eventData)
    {
        MenuManager.Instance.ShowMouseTooltip(_hoverTooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MenuManager.Instance.HideMouseTooltip();
    }
}
