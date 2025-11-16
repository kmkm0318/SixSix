/// <summary>
/// FocusManager에서 관리하는 포커스 가능한 객체의 인터페이스
/// </summary>
public interface IFocusable
{
    public void OnFocus();
    public void OnUnfocus();
    public void OnInteract();
}