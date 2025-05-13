using UnityEngine;

public class GambleDice : Dice
{
    [SerializeField] private GambleDiceSO gambleDiceSO;

    public void Init(GambleDiceSO gambleDiceSO, Playboard playboard)
    {
        base.Init(gambleDiceSO.MaxDiceValue, gambleDiceSO.diceSpriteListSO, gambleDiceSO.diceMaterialSO, playboard);

        this.gambleDiceSO = gambleDiceSO;
    }

    public void TriggerEffect()
    {
        gambleDiceSO.TriggerEffect(this);
    }

    public override void ShowToolTip()
    {
        string name = $"{gambleDiceSO.diceName}";
        string description = $"{gambleDiceSO.GetDescriptionText()}";

        ToolTipUI.Instance.ShowToolTip(this, transform, Vector3.down, name, description);
    }
}
