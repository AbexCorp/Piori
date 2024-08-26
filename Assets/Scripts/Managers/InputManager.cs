using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    static InputManager Instance;

    //Components

    //Inspector References
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _mouseInteractable;
    [SerializeField]
    private GraphicRaycaster _graphicRaycaster;

    //Code References

    //Fields & Properties
    private Vector2 _mousePosition;
    private IMouseInteractions _objectUnderCursor;
    private PointerEventData _pointerEventData;


    private void Awake()
    {
        //if(Instance != null && Instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        Instance = this;
        //DontDestroyOnLoad(gameObject);

        if(_camera == null)
            _camera = Camera.main;
        GetComponent<PlayerInput>().camera = _camera;
        _pointerEventData = new(null);
    }

    private void Update()
    {
        HandleMouseHover();
    }

    private void HandleMouseHover()
    {
        Vector2 worldMousePosition = _camera.ScreenToWorldPoint(_mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin:worldMousePosition, direction:Vector2.zero, distance:1, layerMask:_mouseInteractable); //Change this to raycast all if problems with clicking towers

        if (hit)
        {
            hit.collider.gameObject.TryGetComponent<IMouseInteractions>(out IMouseInteractions clickable);
            if(clickable != null)
            {
                if(clickable != _objectUnderCursor)
                {
                    _objectUnderCursor?.OnMouseHoverLeave();
                    _objectUnderCursor = clickable;
                    clickable?.OnMouseHoverEnter();
                }
            }
            else
                _objectUnderCursor?.OnMouseHoverLeave();
        }
        else
            _objectUnderCursor?.OnMouseHoverLeave();
    }


    public void MousePositionAction(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }
    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        _pointerEventData.position = _mousePosition;
        List<RaycastResult> results = new();
        _graphicRaycaster.Raycast(_pointerEventData, results);
        if (results.Count > 0 || results == null)
            return;

        Vector2 worldMousePosition = _camera.ScreenToWorldPoint(_mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin:worldMousePosition, direction:Vector2.zero, distance:1, layerMask:_mouseInteractable); //Change this to raycast all if problems with clicking towers

        if (hit)
        {
            hit.collider.gameObject.TryGetComponent<IMouseInteractions>(out IMouseInteractions clickable);
            if(clickable != null)
            {
                clickable?.OnMouseLeftClick(context);
            }
        }
    }
}
