using System;
using UnityEngine;

public class PlayerMouseManager : Singleton<PlayerMouseManager>
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask clickableLayerMask;
    [SerializeField] private LayerMask mouseOverLayerMask;

    public event Action<Dice> OnMouseOverDice;
    public event Action OnMouseExit;

    private GameObject lastHoveredObject;

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
                OnMouseExit?.Invoke();

                if (hoveredObject.TryGetComponent(out Dice dice))
                {
                    OnMouseOverDice?.Invoke(dice);
                }
            }
        }
        else
        {
            lastHoveredObject = null;
            OnMouseExit?.Invoke();
        }
    }
}
