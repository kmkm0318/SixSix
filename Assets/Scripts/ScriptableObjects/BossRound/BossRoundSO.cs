using UnityEngine;

// [CreateAssetMenu(fileName = "BossRoundSO", menuName = "Scriptable Objects/BossRoundSO")]
public abstract class BossRoundSO : ScriptableObject
{
    [SerializeField] private string bossName;
    [SerializeField] private string bossDescription;

    public string BossName => bossName;
    public string BossDescription => bossDescription;

    public abstract void OnEnter();
    public abstract void OnExit();
}