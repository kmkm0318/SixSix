using UnityEngine;

public class CollectionSingleUI : UIFocusHandler
{
    private AbilityDiceSO _abilityDiceSO;

    public void Init(AbilityDiceSO abilityDiceSO)
    {
        // AbilityDiceSO 저장
        _abilityDiceSO = abilityDiceSO;

        //다이스 스프라이트 리스트 가져오기
        var defaultDiceSpriteList = DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO.spriteList;

        //해금 상태에 따른 색 지정
        Color color = abilityDiceSO.IsUnlcoked() ? Color.white : new Color(1f, 1f, 1f, 0.5f);

        //이벤트 등록
        OnFocused += HandleFocused;
        OnUnfocused += HandleUnfocused;

        //이미지 설정
        Image.sprite = defaultDiceSpriteList[abilityDiceSO.MaxDiceValue - 1];
        Image.material = abilityDiceSO.shaderDataSO.imageMaterial;
        Image.color = color;
    }

    // 포커스 되었을 때 툴팁 표시
    private void HandleFocused()
    {
        string descriptionText = _abilityDiceSO.IsUnlcoked() ? _abilityDiceSO.GetDescriptionText() : _abilityDiceSO.abilityUnlock.GetDescriptionText();

        ToolTipUIEvents.TriggerOnToolTipShowRequested(
            RectTransform,
            Vector2.left,
            _abilityDiceSO.DiceName,
            descriptionText,
            ToolTipTag.AbilityDice,
            _abilityDiceSO.rarity
        );
    }

    // 포커스 해제 되었을 때 툴팁 숨기기
    private void HandleUnfocused()
    {
        ToolTipUIEvents.TriggerOnToolTipHideRequested(RectTransform);
    }
}