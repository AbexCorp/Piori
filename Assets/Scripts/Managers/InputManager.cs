using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;


    //Inspector References
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _mouseInteractable;


  

    private void Awake()
    {
        Instance = this;

        if(_camera == null)
            _camera = Camera.main;
        GetComponent<PlayerInput>().camera = _camera;
        _pointerEventData = new(null);
    }

    private void Update()
    {
        HandleMouseHover();
    }


    #region MousePositionAction

    private Vector2 _mousePosition;
    public Vector2 MousePosition => _mousePosition;
    public void MousePositionAction(InputAction.CallbackContext context)
    {
        _mousePosition = context.ReadValue<Vector2>();
    }

    #endregion

    #region OnMouseLeftClick

    private event Action<InputAction.CallbackContext> _onMouseLeftClickEvent;
    
    [SerializeField]
    private GraphicRaycaster _graphicRaycaster;
    private PointerEventData _pointerEventData;
    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        _onMouseLeftClickEvent?.Invoke(context);

        if (context.performed)
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
    public void OnMouseLeftClickSubscribe(Action<InputAction.CallbackContext> sub)
    {
        _onMouseLeftClickEvent += sub;
    }

    #endregion

    #region OnMovement

    private event Action<InputAction.CallbackContext> _onMovementEvent;
    public void OnMovement(InputAction.CallbackContext context)
    {
        _onMovementEvent?.Invoke(context);
    }
    public void OnMovementSubscribe(Action<InputAction.CallbackContext> sub)
    {
        _onMovementEvent += sub;
    }

    #endregion


    private IMouseInteractions _objectUnderCursor;
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
}
