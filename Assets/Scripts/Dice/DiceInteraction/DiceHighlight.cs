using System.Collections;
using UnityEngine;

/// <summary>
/// 다이스 상호작용 내용을 표시하기 위한 하이라이트 클래스
/// </summary>
public class DiceHighlight : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 1.125f;
    [SerializeField] private float scaleSpeed = 1f;

    private Coroutine _highlightCoroutine;

    public void SetColor(Color color)
    {
        if (_spriteRenderer != null)
        {
            _spriteRenderer.color = color;
        }
    }

    #region 하이라이트 코루틴
    private IEnumerator HighlightCoroutine()
    {
        while (true)
        {
            var targetScale = Mathf.PingPong(Time.time * scaleSpeed, 1) * (maxScale - minScale) + minScale;
            transform.localScale = new Vector3(targetScale, targetScale, 1);
            yield return null;
        }
    }

    public void StartHighlightCoroutine()
    {
        StopHighlightCoroutine();
        gameObject.SetActive(true);
        _highlightCoroutine = StartCoroutine(HighlightCoroutine());
    }

    public void StopHighlightCoroutine()
    {
        if (_highlightCoroutine != null)
        {
            StopCoroutine(_highlightCoroutine);
            _highlightCoroutine = null;
            gameObject.SetActive(false);
        }
    }
    #endregion
}