using System;
using UnityEngine;

public class MouseManager : Singleton<MouseManager>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask clickableLayerMask;
    [SerializeField] private LayerMask mouseOverLayerMask;

    private GameObject lastHoveredObject;

    public event Action OnMouseExited;
    public event Action OnMouseClicked;

    private void Update()
    {
        HandleMouseClick();
        HandleMouseOver();
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, 0f, clickableLayerMask);

            if (hit && hit.collider.TryGetComponent(out IClickable clickable))
            {
                clickable.OnClick();
            }

            OnMouseClicked?.Invoke();
        }
    }

    private void HandleMouseOver()
    {
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, mouseOverLayerMask);

        if (hit.collider != null)
        {
            GameObject hoveredObject = hit.collider.gameObject;

            if (hoveredObject != lastHoveredObject)
            {
                lastHoveredObject = hoveredObject;
                OnMouseExited?.Invoke();

                if (hoveredObject.TryGetComponent(out IHighlightable highlightable))
                {
                    highlightable.ShowHighlight();
                }

                if (hoveredObject.TryGetComponent(out IToolTipable toolTipable))
                {
                    toolTipable.ShowToolTip();
                }
            }
        }
        else if (lastHoveredObject != null)
        {
            lastHoveredObject = null;
            OnMouseExited?.Invoke();
        }
    }
}
