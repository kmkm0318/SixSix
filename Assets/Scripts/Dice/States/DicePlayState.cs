public class DicePlayState : IDiceState
{
    private Dice dice;

    public void Init(Dice dice)
    {
        this.dice = dice;
    }

    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void OnClick()
    {
        throw new System.NotImplementedException();
    }

    public void OnCollide()
    {
        throw new System.NotImplementedException();
    }
}
