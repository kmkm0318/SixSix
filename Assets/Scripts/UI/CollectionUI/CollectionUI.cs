using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class CollectionUI : BaseUI
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Transform _collectionSingleUIParent;
    [SerializeField] private CollectionSingleUI _collectionSingleUIPrefab;

    private void Start()
    {
        RegisterEvents();
        _closeButton.onClick.AddListener(() => Hide());

        InitCollectionUI();

        gameObject.SetActive(false);
    }

    private void InitCollectionUI()
    {
        var abilityDiceList = new List<AbilityDiceSO>();

        var normalAbilityDiceList = DataContainer.Instance.NormalAbilityDiceListSO.abilityDiceSOList;
        var rareAbilityDiceList = DataContainer.Instance.RareAbilityDiceListSO.abilityDiceSOList;
        var epicAbilityDiceList = DataContainer.Instance.EpicAbilityDiceListSO.abilityDiceSOList;
        var legendaryAbilityDiceList = DataContainer.Instance.LegendaryAbilityDiceListSO.abilityDiceSOList;

        abilityDiceList.AddRange(normalAbilityDiceList);
        abilityDiceList.AddRange(rareAbilityDiceList);
        abilityDiceList.AddRange(epicAbilityDiceList);
        abilityDiceList.AddRange(legendaryAbilityDiceList);

        foreach (var abilityDice in abilityDiceList)
        {
            var collectionSingleUI = Instantiate(_collectionSingleUIPrefab, _collectionSingleUIParent);

            collectionSingleUI.Init(abilityDice);
        }
    }

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region RegisterEvents
    private void RegisterEvents()
    {
        CollectionUIEvents.OnCollectionButtonClicked += OnCollectionButtonClicked;
    }

    private void UnregisterEvents()
    {
        CollectionUIEvents.OnCollectionButtonClicked -= OnCollectionButtonClicked;
    }

    private void OnCollectionButtonClicked()
    {
        Show();
    }
    #endregion
}