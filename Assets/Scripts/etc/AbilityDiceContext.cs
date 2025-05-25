public readonly struct AbilityDiceContext
{
    public readonly AbilityDice abilityDice;
    public readonly PlayDice playDice;
    public readonly HandSO handSO;

    public AbilityDiceContext(AbilityDice availtiyDice = null, PlayDice playDice = null, HandSO handSO = null)
    {
        this.abilityDice = availtiyDice;
        this.playDice = playDice;
        this.handSO = handSO;
    }
}