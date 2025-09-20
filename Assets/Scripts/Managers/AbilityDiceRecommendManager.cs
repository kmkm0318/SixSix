using System.Collections.Generic;
using System.Linq;

public class AbilityDiceRecommendManager : Singleton<AbilityDiceRecommendManager>
{
    public bool IsRecommended(List<int> diceIds, int targetId, int targetRound)
    {
        var combinationList = GetDiceCombinationList(diceIds, targetId);
        foreach (var combination in combinationList)
        {
            int clearedRound = DatabaseManager.Instance.GetCombinationClearedRound(combination);
            if (clearedRound >= targetRound) return true;
        }

        return false;
    }

    private List<List<int>> GetDiceCombinationList(List<int> diceIds, int targetId)
    {
        int count = diceIds.Count();

        return Enumerable.Range(0, 1 << count).Select(i =>
            Enumerable.Range(0, count).Where(j => (i & (1 << j)) != 0).Select(j => diceIds[j]).Concat(new[] { targetId }).ToList()
        ).ToList();
    }
}