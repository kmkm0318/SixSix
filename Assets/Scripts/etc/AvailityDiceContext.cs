public readonly struct AvailityDiceContext
{
    public readonly AvailityDice availtiyDice;
    public readonly PlayDice playDice;
    public readonly HandSO handSO;

    public AvailityDiceContext(AvailityDice availtiyDice = null, PlayDice playDice = null, HandSO handSO = null)
    {
        this.availtiyDice = availtiyDice;
        this.playDice = playDice;
        this.handSO = handSO;
    }
}