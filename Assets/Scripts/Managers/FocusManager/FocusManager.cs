/// <summary>
/// 모바일 환경에서 PC 환경의 마우스 Enter, Exit, Click 이벤트를 보조하기 위한 장치
/// </summary>
public class FocusManager : Singleton<FocusManager>
{
    private IFocusable _target;

    public void OnClick(IFocusable target)
    {
        if (target == _target)
        {
            _target?.OnInteract();
        }
        else
        {
            _target?.OnUnfocus();
            _target = target;
            _target?.OnFocus();
        }
    }
}