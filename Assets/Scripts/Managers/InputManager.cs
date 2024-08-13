using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    static InputManager Instance;

    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask _mouseInteractable;

    private Vector2 mousePosition;

    private IMouseInteractions objectUnderCursor;

    private void Awake()
    {
        Instance = this;

        if(_camera == null)
            _camera = Camera.main;
        GetComponent<PlayerInput>().camera = _camera;
    }

    private void Update()
    {
        HandleMouseHover();
    }

    private void HandleMouseHover()
    {
        Vector2 worldMousePosition = _camera.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin:worldMousePosition, direction:Vector2.zero, distance:1, layerMask:_mouseInteractable); //Change this to raycast all if problems with clicking towers

        if (hit)
        {
            hit.collider.gameObject.TryGetComponent<IMouseInteractions>(out IMouseInteractions clickable);
            if(clickable != null)
            {
                if(clickable != objectUnderCursor)
                {
                    objectUnderCursor?.OnMouseHoverLeave();
                    objectUnderCursor = clickable;
                    clickable?.OnMouseHoverEnter();
                }
            }
            else
                objectUnderCursor?.OnMouseHoverLeave();
        }
        else
            objectUnderCursor?.OnMouseHoverLeave();
    }


    public void MousePositionAction(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
    public void OnMouseLeftClick(InputAction.CallbackContext context)
    {
        Vector2 worldMousePosition = _camera.ScreenToWorldPoint(mousePosition);
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
