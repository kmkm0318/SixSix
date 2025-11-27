/// <summary>
/// 모바일 환경에서 PC 환경의 마우스 Enter, Exit, Click 이벤트를 보조하기 위한 장치
/// </summary>
public class FocusManager : Singleton<FocusManager>
{
    // 현재 포커스된 대상
    private IFocusable _target;

    /// <summary>
    /// 포커스 변경 및 클릭 처리
    /// </summary>
    public void OnClick(IFocusable target)
    {
        // 같은 대상 클릭 시 상호작용 처리
        if (target == _target)
        {
            _target?.OnInteract();
        }
        // 다른 대상 클릭 시 포커스 변경
        else
        {
            SetFocus(target);
        }
    }

    /// <summary>
    /// 포커스 변경
    /// </summary>
    public void SetFocus(IFocusable newTarget)
    {
        if (newTarget == _target) return;

        _target?.OnUnfocus();
        _target = newTarget;
        _target?.OnFocus();
    }

    /// <summary>
    /// 포커스 해제
    /// </summary>
    public void UnsetFocus(IFocusable target)
    {
        if (target == _target)
        {
            _target?.OnUnfocus();
            _target = null;
        }
    }
}