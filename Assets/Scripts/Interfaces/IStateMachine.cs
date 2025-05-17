public interface IStateMachine<T>
{
    void ChangeState(T t);
}

public interface IStateMachine<T1, T2>
{
    void ChangeState(T1 t1);
}