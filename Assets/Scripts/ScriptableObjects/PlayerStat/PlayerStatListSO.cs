using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatListSO", menuName = "Scriptable Objects/PlayerStatListSO", order = 0)]
public class PlayerStatListSO : ScriptableObject
{
    public List<PlayerStatSO> playerStatSOs;
}