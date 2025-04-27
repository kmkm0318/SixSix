using UnityEngine;

public abstract class BossRoundSO : ScriptableObject
{
    [SerializeField] private string bossName;
    public string BossName => bossName;
    [SerializeField] private string bossDescription;
    public string BossDescription => bossDescription;

    public abstract void OnEnter();
    public abstract void OnExit();
}