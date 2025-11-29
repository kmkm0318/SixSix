using UnityEngine;


/// <summary>
/// 다이스 오브젝트 및 갬블 다이스 아이콘 UI의 상호작용 정보를 보여주는 UI
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class InteractionInfoUI : MonoBehaviour
{
    [SerializeField] private AnimatedText _infoText; //정보 텍스트 오브젝트
    [SerializeField] private DiceInteractionTypeDataList _interactionTypeDatas; //상호작용 타입 데이터 리스트
    [SerializeField] private float _offset = 25f; //UI 위치 오프셋

    private Camera _mainCamera; //메인 카메라
    private RectTransform _rectTransform; //자신의 RectTransform
    private Transform _target; //대상 트랜스폼
    private float _targetOffset; //대상 오프셋
    private bool _isUI = false; //대상이 UI인지 여부

    private void Start()
    {
        RegisterEvents();
        _mainCamera = Camera.main;
        _rectTransform = GetComponent<RectTransform>();
        gameObject.SetActive(false);
    }

    private void LateUpdate()
    {
        HandlePosition();
    }

    // UI 위치 처리
    private void HandlePosition()
    {
        if (_target == null) return;

        Vector3 targetPos = _target.position;

        if (_isUI)
        {
            //대상이 UI인 경우 그대로 오프셋 적용
            targetPos += new Vector3(0f, _targetOffset, 0f);
        }
        else
        {
            //대상이 월드 오브젝트인 경우 카메라 좌표로 변환하기 전 오프셋 적용
            targetPos = _mainCamera.WorldToScreenPoint(_target.position + new Vector3(0f, _targetOffset, 0f));
        }

        //공통 오프셋 적용
        targetPos.y += _offset;
        _rectTransform.position = targetPos;
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    private void RegisterEvents()
    {
        InteractionInfoUIEvents.OnShowInteractionInfoUI += OnShowInteractionInfoUI;
        InteractionInfoUIEvents.OnHideInteractionInfoUI += OnHideInteractionInfoUI;
    }

    private void UnregisterEvents()
    {
        InteractionInfoUIEvents.OnShowInteractionInfoUI -= OnShowInteractionInfoUI;
        InteractionInfoUIEvents.OnHideInteractionInfoUI -= OnHideInteractionInfoUI;
    }

    private void OnShowInteractionInfoUI(Transform target, DiceInteractionType type, int value)
    {
        _target = target;

        if (target is RectTransform rect)
        {
            _isUI = true;
            _targetOffset = rect.rect.height / 2f;
        }
        else
        {
            _isUI = false;
            _targetOffset = target.lossyScale.y / 2f;
        }

        if (_interactionTypeDatas.DataDict.TryGetValue(type, out var data))
        {
            gameObject.SetActive(true);
            string info = data.localizedText.GetLocalizedString(value);
            _infoText.SetText(info);
        }
    }

    private void OnHideInteractionInfoUI(Transform target)
    {
        if (_target != target) return;
        _target = null;

        _infoText.ClearText();
        gameObject.SetActive(false);
    }
}