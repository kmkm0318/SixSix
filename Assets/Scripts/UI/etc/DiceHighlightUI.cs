using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 다이스 UI의 하이라이트를 위한 클래스
/// </summary>
public class DiceHighlightUI : MonoBehaviour
{
    [SerializeField] private Image[] _highlightImages;
    [SerializeField] private float _minScale = 1f;
    [SerializeField] private float _maxScale = 1.125f;
    [SerializeField] private float _scaleSpeed = 1f;

    private Coroutine _highlightCoroutine;

    public void SetColor(Color color)
    {
        if (_highlightImages != null)
        {
            foreach (var image in _highlightImages)
            {
                image.color = color;
            }
        }
    }

    #region 하이라이트 코루틴
    private IEnumerator HighlightCoroutine()
    {
        while (true)
        {
            var targetScale = Mathf.PingPong(Time.time * _scaleSpeed, 1) * (_maxScale - _minScale) + _minScale;
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