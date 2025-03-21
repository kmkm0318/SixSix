using System.Collections.Generic;

public class PlayerDiceManager : Singleton<PlayerDiceManager>
{
    private List<Dice> playDiceList = new();
    public List<Dice> PlayDiceList => playDiceList;
    private List<Dice> availityDiceList = new();
    public List<Dice> AvailityDiceList => availityDiceList;

    public void AddPlayDice(Dice playDice)
    {
        playDiceList.Add(playDice);
    }

    public void AddAvailityDice(Dice availityDice)
    {
        availityDiceList.Add(availityDice);
    }

    public bool AreAllDiceStopped()
    {
        return playDiceList.TrueForAll(dice => !dice.IsRolling) && availityDiceList.TrueForAll(dice => !dice.IsRolling);
    }

    public List<int> GetPlayDiceValues()
    {
        List<int> playDiceValues = new();
        foreach (Dice dice in playDiceList)
        {
            playDiceValues.Add(dice.FaceIndex + 1);
        }
        return playDiceValues;
    }
}
