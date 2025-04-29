using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRoundListSO", menuName = "Scriptable Objects/BossRounds/BossRoundListSO")]
public class BossRoundListSO : ScriptableObject
{
    public List<BossRoundSO> bossRounds;

    public BossRoundSO GetRandomBossRoundSO()
    {
        if (bossRounds.Count == 0) return null;
        return bossRounds[Random.Range(0, bossRounds.Count)];
    }
}