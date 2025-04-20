public interface IState<T>
{
    T Value { get; set; }
    void Enter(T data);
    void Exit();
}
