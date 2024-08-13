using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public interface IMouseInteractions
{
    public void OnMouseHoverEnter();
    public void OnMouseHoverLeave();
    public void OnMouseLeftClick(InputAction.CallbackContext context);
}
