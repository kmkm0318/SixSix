using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossRoundListSO", menuName = "Scriptable Objects/BossRounds/BossRoundListSO")]
public abstract class BossRoundListSO : ScriptableObject
{
    public List<BossRoundSO> bossRounds;
}