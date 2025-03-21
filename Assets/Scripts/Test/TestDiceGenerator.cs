using System.Collections;
using UnityEngine;

public class TestDiceGenerator : MonoBehaviour
{
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Playboard playboard;

    private void Start()
    {
        StartCoroutine(DiceGenerate());
    }

    IEnumerator DiceGenerate()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.25f);
            var dice = Instantiate(dicePrefab, playboard.DiceGeneratePosition, Quaternion.identity);
            dice.Init(6, playboard);

            PlayerDiceManager.Instance.AddPlayDice(dice);
        }
    }
}
