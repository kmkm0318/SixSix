public class AbilityDiceContext
{
    public AbilityDice currentAbilityDice;
    public PlayDice playDice;
    public AbilityDice abilityDice;
    public GambleDice gambleDice;
    public HandSO handSO;
    public bool isRetriggered;

    public AbilityDiceContext(AbilityDice currentAbilityDice = null, PlayDice playDice = null, AbilityDice abilityDice = null, GambleDice gambleDice = null, HandSO handSO = null, bool isRetriggered = false)
    {
        this.currentAbilityDice = currentAbilityDice;
        this.playDice = playDice;
        this.abilityDice = abilityDice;
        this.gambleDice = gambleDice;
        this.handSO = handSO;
        this.isRetriggered = isRetriggered;
    }
}