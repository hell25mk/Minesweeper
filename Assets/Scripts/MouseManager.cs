using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MouseManager : MonoBehaviour {
    public void OnUpdate() {
        if(!Input.GetMouseButtonDown((int)MouseButton.Left)
            && !Input.GetMouseButtonDown((int)MouseButton.Right)) {
            return;
        }

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D ray = Physics2D.Raycast(pos, Vector2.zero);

        if(ray.collider == null) {
            return;
        }

        IClickEvent clickEvent = ray.collider.GetComponent<IClickEvent>();

        if(clickEvent == null) {
            return;
        }

        if(Input.GetMouseButtonDown((int)MouseButton.Left)) {
            clickEvent.OnMouseLeftButtonDown();
        }
        else if(Input.GetMouseButtonDown((int)MouseButton.Right)) {
            clickEvent.OnMouseRightButtonDown();
        }
    }
}
