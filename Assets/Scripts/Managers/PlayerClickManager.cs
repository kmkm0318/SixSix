using UnityEngine;

public class PlayerClickManager : Singleton<PlayerClickManager>
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit && hit.collider.TryGetComponent(out IClickable clickable))
            {
                clickable.OnClick();
            }
        }
    }
}
