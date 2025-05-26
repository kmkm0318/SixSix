using UnityEngine;
using UnityEngine.Localization;

public abstract class BossRoundSO : ScriptableObject
{
    [SerializeField] private LocalizedString bossName;
    public string BossName => bossName.GetLocalizedString();
    [SerializeField] protected LocalizedString bossDescription;

    public abstract void OnEnter();
    public abstract void OnExit();
    public virtual string GetBossDescription()
    {
        return bossDescription.GetLocalizedString();
    }
}