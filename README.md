# SixSix

## 게임 설명

![image.png](Assets/Sprites/images/1.png)

![image.png](Assets/Sprites/images/2.png)

![image.png](Assets/Sprites/images/3.png)

![image.png](Assets/Sprites/images/4.png)

SixSix는 주사위를 사용한 포커 룰을 통해 점수와 재화를 획득하고, 상점에서 아이템과 강화를 통해 높은 라운드를 목표로 하는 로그 라이크 게임입니다.

---

## 플레이 영상

[![Video Label](http://img.youtube.com/vi/MpYB58EnZWI/0.jpg)](https://youtu.be/MpYB58EnZWI?t=0s)

---

## 게임 진행 순서

![image.png](Assets/Sprites/images/5.png)

게임 시작 시 보이는 메인 메뉴 화면입니다. 메인 메뉴에서는 시작, 퀘스트, 컬렉션, 옵션, 나가기가 선택 가능하고 좌측 하단에서 점수 리더 보드를 확인할 수 있습니다.

![image.png](Assets/Sprites/images/6.png)

게임 화면에서는 롤 버튼을 통해 주사위를 굴릴 수 있습니다. 굴린 주사위의 조합을 통해서 핸드를 판단하며 제출 가능한 핸드가 좌측에 점수와 함께 표시됩니다. 플레이어는 해당 점수를 클릭하는 것으로 한 번의 플레이를 소모하여 점수를 얻을 수 있습니다.

![image.png](Assets/Sprites/images/7.png)

라운드를 클리어하면 상점에 진입하게 됩니다. 상점에서는 게임을 통해 획득한 머니를 사용해 점수를 얻는 데 도움을 주는 어빌리티 다이스, 갬블 다이스를 구매하거나 강화를 진행할 수 있습니다.

![image.png](Assets/Sprites/images/8.png)

라운드를 진행함에 따라 달성해야 하는 점수가 계속 높아집니다. 주사위를 굴리면 현재 주사위 조합에 따라 달성 가능한 조합을 조언으로 알려줍니다. 이를 통해서 플레이어는 굴리고 싶지 않은 주사위는 ‘킵’ 기능을 통해 주사위 눈이 변경되지 않도록 할 수 있습니다.

![image.png](Assets/Sprites/images/9.png)

6라운드마다 보스 라운드가 되어 플레이어는 하나의 디버프를 받고 라운드를 진행하게 됩니다. 예시에서는 각 플레이 당 최대 한 번의 롤만 가능한 디버프가 적용된 모습입니다.

---

## 주요 기능 소개

### 어빌리티 다이스

어빌리티 다이스는 점수 획득, 재화 획득 등의 효과를 통해 플레이에 도움을 주는 요소입니다.

어빌리티 다이스는 상점에 랜덤하게 등장하며 발동 조건과 효과가 다양하기 때문에 플레이어가 게임을 질리지 않고 플레이할 수 있도록 합니다.

수많은 다이스를 효율적으로 관리하고 기획자가 코드 수정 없이도 새로운 다이스를 생성할 수 있도록 ScriptableObject 기반의 전략 패턴을 사용하여 시스템을 모듈화 하였습니다.

- AbilityDiceSO
    
    ```csharp
    [CreateAssetMenu(fileName = "AbilityDiceSO", menuName = "Scriptable Objects/AbilityDiceSO")]
    public class AbilityDiceSO : ScriptableObject
    {
        [Header("Dice Info")]
        public int abilityDiceID;
        public LocalizedString diceNameLocalized;
        public string DiceName => diceNameLocalized.GetLocalizedString();
        public AbilityDiceRarity rarity;
        public AbilityDiceAutoKeepType autoKeepType;
        public int price;
        public int SellPrice => price / 2;
        public ShaderDataSO shaderDataSO;
        public int maxDiceValue;
        public int MaxDiceValue => Mathf.Min(maxDiceValue, DataContainer.Instance.CurrentPlayerStat.diceSpriteListSO.DiceFaceCount);
    
        [Header("Dice Trigger, Effect, Unlock")]
        public AbilityTriggerSO abilityTrigger;
        public AbilityEffectSO abilityEffect;
        public AbilityDiceUnlockSO abilityUnlock;
    
        public bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
        {
            return abilityTrigger.IsTriggered(triggerType, context);
        }
    
        public void TriggerEffect(AbilityDiceContext context)
        {
            abilityEffect.TriggerEffect(context);
        }
    
        public string GetDescriptionText(int effectValue = 0)
        {
            return abilityTrigger.GetTriggerDescription(this) + "\n" + abilityEffect.GetEffectDescription(this, effectValue);
        }
    
        public bool IsUnlcoked()
        {
            return abilityUnlock.IsUnlocked();
        }
    
        public string GetUnlockDescriptionText()
        {
            return abilityUnlock.GetDescriptionText();
        }
    }
    ```

어빌리티 다이스는 세 가지 주요 요소를 가지고 있습니다.

1. 어빌리티 다이스가 발동하는 조건인 트리거
2. 발동했을 때의 효과인 이펙트
3. 상점에 나오도록 해금하는 방법인 언락

세 요소도 ScriptableObject 형식으로 만들어졌으며, 기본 클래스를 추상 클래스로 정의하고 세부 클래스에서 내용을 정의하도록 하였습니다.

중요한 부분은 트리거와 이펙트를 분리하여, 새로운 트리거 혹은 이펙트를 하나만 만들어도 기존의 것들과 모두 조합할 수 있어 높은 재사용성을 확보한 부분입니다.

트리거는 현재 상태를 통해서 해당 어빌리티 다이스의 효과가 발동해야 하는지 정의하는 요소입니다.

EffectTriggerType을 통해 어떤 상태를 바라보는지 정하고 IsTriggered 함수에서 context로 들어온 현재 상태를 확인하여 발동 여부를 정합니다.

- AbilityTriggerSO
    
    ```csharp
    public abstract class AbilityTriggerSO : ScriptableObject
    {
        [SerializeField] protected EffectTriggerType triggerType;
        [SerializeField] protected LocalizedString triggerDescription;
        public EffectTriggerType TriggerType => triggerType;
    
        public abstract bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context);
        public virtual string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
        {
            return triggerDescription.GetLocalizedString();
        }
    }
    ```
    

예시로 특정 숫자의 플레이 다이스가 발동되었을 때 발동하는 트리거는 아래와 같습니다.

context로 들어온 현재 상태에서 playDice가 targetValues에 포함되었는지에 따라 발동 여부를 반환합니다.

- AbilityTriggerDiceSO
    
    ```csharp
    [CreateAssetMenu(fileName = "AbilityTriggerDiceSO", menuName = "Scriptable Objects/AbilityTriggerSO/AbilityTriggerDiceSO")]
    public class AbilityTriggerDiceSO : AbilityTriggerSO
    {
        [SerializeField] private List<int> targetValues;
        public List<int> TargetValues => targetValues;
    
        public override bool IsTriggered(EffectTriggerType triggerType, AbilityDiceContext context)
        {
            return triggerType == TriggerType && context.playDice != null && targetValues.Contains(context.playDice.DiceValue);
        }
    
        public override string GetTriggerDescription(AbilityDiceSO abilityDiceSO)
        {
            if (triggerDescription == null)
            {
                Debug.LogError("Trigger description is not set for " + name);
                return string.Empty;
            }
            triggerDescription.Arguments = new object[] { string.Join(", ", targetValues) };
            triggerDescription.RefreshString();
            return triggerDescription.GetLocalizedString();
        }
    }
    ```
    

이펙트는 해당 어빌리티 다이스가 발동했을 때 플레이어가 얻는 효과를 정의하는 요소입니다.

TriggerEffect 함수를 통해서 현재 context에 따라 효과를 주게 됩니다.

- AbilityEffectSO
    
    ```csharp
    public abstract class AbilityEffectSO : ScriptableObject
    {
        [SerializeField] protected EffectCalculateType calculateType;
        [SerializeField] protected LocalizedString effectDescription;
        public abstract void TriggerEffect(AbilityDiceContext context);
    
        public virtual string GetEffectDescription(AbilityDiceSO abilityDiceSO, int effectValue = 0)
        {
            return effectDescription.GetLocalizedString();
        }
    }
    ```
    

이때, 단순하게 효과를 주지 않고 현재 어빌리티 다이스의 값에 따라서 효과의 크기가 달라집니다.

예를 들어 아래와 같이 점수를 획득하는 효과의 경우 ‘기본 점수’와 ‘주사위의 값’을 calculateType에 따라 계산하여 주사위의 값이 높을수록 더 높은 효과를 얻을 수 있도록 되어 있습니다. 대부분의 calculateType은 곱셉입니다.

- AbilityEffectScorePairSO
    
    ```csharp
    [CreateAssetMenu(fileName = "AbilityEffectScorePairSO", menuName = "Scriptable Objects/AbilityEffectSO/AbilityEffectScorePairSO")]
    public class AbilityEffectScorePairSO : AbilityEffectSO
    {
        [SerializeField] private ScorePair scorePair;
    
        public override void TriggerEffect(AbilityDiceContext context)
        {
            ScorePair resultScorePair = DiceEffectCalculator.GetCalculatedEffectValue(scorePair, context.currentAbilityDice.DiceValue, calculateType);
    
            TriggerManager.Instance.ApplyTriggerEffect(context.currentAbilityDice.transform, Vector3.down, resultScorePair);
        }
    
        public override string GetEffectDescription(AbilityDiceSO abilityDiceSO, int effectValue = 0)
        {
            if (effectDescription == null)
            {
                Debug.LogError("Effect description is not set for AbilityEffectScorePairSO.");
                return string.Empty;
            }
            effectDescription.Arguments = new object[] { scorePair, DiceEffectCalculator.GetCalculateDescription(abilityDiceSO.MaxDiceValue, calculateType) };
            effectDescription.RefreshString();
            return effectDescription.GetLocalizedString();
        }
    }
    ```
    

어빌리티 다이스의 값에 따라서 효과의 크기가 바뀌기 때문에 어빌리티 다이스의 값에도 신경을 써서 킵을 하거나 해야 합니다. 이는 플레이어에게 조금 피로한 작업이 될 것입니다.

이러한 플레이어의 피로도를 줄이기 위해서 AbilityDiceAutoKeepType을  도입했습니다. 각 어빌리티 다이스가 높은 값과 낮은 값 중 어떨 때에 더 좋은 지 정해두고 플레이어가 롤을 완료하면 결과에 따라 자동으로 킵을 할 수 있도록 했습니다. 자동으로 킵을 해주는 기능은 옵션에서 선택할 수 있도록 했습니다.

언락은 해당 어빌리티 다이스가 어떤 경우에 상점에 나타나는지 해금 방법을 정의하는 요소입니다.
IsUnlocked를 통해 해금된 상태인지를 확인할 수 있습니다. 해금 상태인 경우에만 상점에 등장합니다.

- AbilityDiceUnlockSO
    
    ```csharp
    public abstract class AbilityDiceUnlockSO : ScriptableObject
    {
        [SerializeField] protected LocalizedString unlockDescription;
        public abstract bool IsUnlocked();
        public virtual string GetDescriptionText()
        {
            return unlockDescription.GetLocalizedString();
        }
    }
    ```
    

처음 게임을 시작했을 때에 기본적인 어빌리티 다이스는 상점에 등장해야 하기 때문에 항상 해금된 상태인 언락이 필요합니다.

- AbilityDiceUnlockAlwaysSO
    
    ```csharp
    [CreateAssetMenu(fileName = "AbilityDiceUnlockAlwaysSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockAlwaysSO", order = 0)]
    public class AbilityDiceUnlockAlwaysSO : AbilityDiceUnlockSO
    {
        public override bool IsUnlocked()
        {
            return true;
        }
    }
    ```
    

이외의 경우에는 PlayerRecordManager를 통해 플레이어의 기록을 확인하고 그에 따라서 해금된 상태인지 반환합니다.

예시의 경우는 플레이어의 최대 점수가 특정 점수를 넘었는지를 확인하여 해금 상태를 반환합니다.

- AbilityDiceUnlockMaxScoreSO
    
    ```csharp
    [CreateAssetMenu(fileName = "AbilityDiceUnlockMaxScoreSO", menuName = "Scriptable Objects/AbilityDiceUnlockSO/AbilityDiceUnlockMaxScoreSO", order = 0)]
    public class AbilityDiceUnlockMaxScoreSO : AbilityDiceUnlockSO
    {
        [SerializeField] private double value = 1e6;
    
        public override bool IsUnlocked()
        {
            var maxScore = PlayerRecordManager.Instance.PlayerRecordData.maxScore;
    
            return maxScore >= value;
        }
    
        public override string GetDescriptionText()
        {
            if (unlockDescription == null)
            {
                Debug.LogError($"There is no Description for Ability Unlock : {typeof(AbilityDiceUnlockMoneyGainedSO)}");
                return string.Empty;
            }
    
            unlockDescription.Arguments = new object[] { UtilityFunctions.FormatNumber(value) };
            unlockDescription.RefreshString();
            return unlockDescription.GetLocalizedString();
        }
    }
    ```
    

### 순차적 애니메이션

주사위를 굴리거나 핸드를 선택했을 때 각 플레이 다이스의 점수 표시, 어빌리티 다이스의 효과 발동 등의 애니메이션을 실행합니다. 이때 각각의 애니메이션이 한번에 실행하는 것이 아니라 앞선 애니메이션이 끝나면 그 다음의 애니메이션이 실행되어야 합니다.

이러한 순차적 애니메이션을 위해서 SequenceManager를 구성했습니다. 각 애니메이션을 코루틴으로 만들어 해당 코루틴이 끝날 때까지 대기하고 그 다음 코루틴을 실행하는 식으로 구현했습니다.

- PlaySequentialCoroutines
    
    ```csharp
    private Queue<IEnumerator> sequentialCoroutineQueue = new();
    
        private IEnumerator PlaySequentialCoroutines()
        {
            isPlaying = true;
            OnAnimationStarted?.Invoke();
    
            yield return null;
    
            while (sequentialCoroutineQueue.Count > 0)
            {
                var coroutine = sequentialCoroutineQueue.Dequeue();
    
                yield return StartCoroutine(coroutine);
            }
    
            isPlaying = false;
            OnAnimationFinished?.Invoke();
        }
    
    ```
    

하지만 모든 애니메이션이 하나씩만 실행되진 않습니다. 예를 들어 주사위의 점수 애니메이션이 실행될 때 해당 주사위의 흔들림 애니메이션도 같이 실행될 수 있습니다. 이런 동시적인 애니메이션을 위해 ParallelCoroutine 시스템을 만들었습니다. 리스트에 있는 모든 코루틴을 동시에 실행하고 저장한 후에 모든 코루틴이 종료될 때까지 기다리는 하나의 코루틴입니다.

- PlayParallelCoroutines
    
    ```csharp
    private List<IEnumerator> parallelCoroutineList = new();
    
        public void ApplyParallelCoroutine()
        {
            if (parallelCoroutineList.Count == 0)
            {
                return;
            }
    
            var parallelCoroutineArray = parallelCoroutineList.ToArray();
            parallelCoroutineList.Clear();
            AddCoroutine(PlayParallelCoroutines(parallelCoroutineArray));
        }
    
        private IEnumerator PlayParallelCoroutines(params IEnumerator[] coroutines)
        {
            List<Coroutine> runningCoroutines = new();
    
            foreach (var coroutine in coroutines)
            {
                runningCoroutines.Add(StartCoroutine(coroutine));
            }
    
            foreach (var coroutine in runningCoroutines)
            {
                yield return coroutine;
            }
        }
    ```
    

추가적으로 각 애니메이션 사이에 애니메이션이 아닌 함수를 실행하는 기능도 구현했습니다. 특정 애니메이션이 끝난 후에 다음 로직을 진행하기 위한 기능입니다. Action으로 들어온 일반적인 함수를 코루틴으로 감싸는 방식입니다.

- ExecuteAction
    
    ```csharp
        public void AddCoroutine(Action action, bool isParallel = false)
        {
            if (action == null)
            {
                Debug.LogWarning("Action is null. Cannot execute.");
                return;
            }
    
            AddCoroutine(ExecuteAction(action), isParallel);
        }
    
        private IEnumerator ExecuteAction(Action action)
        {
            action.Invoke();
            yield break;
        }
    ```
    

### 핸드 추천 기능

주사위를 사용하는 포커 룰에 익숙하지 않은 플레이어들을 위해 핸드 추천 기능을 만들었습니다. 현재 플레이어의 주사위 조합에서 달성할 수 있는 더 좋은 핸드들을 확률과 함께 알려주는 기능입니다.

확률을 계산하기 위해 현재 주사위 중에서 일부 주사위를  굴렸을 때의 결과를 재귀적으로 탐색했습니다. 각 주사위 조합이 핸드를 달성했는지 여부를 확인하고 달성 횟수를 저장합니다.

- DFS
    
    ```csharp
    private static void DFS(List<int> currentValues, int pickCount, int idx, Dictionary<Hand, int> successCounts)
        {
            // 인덱스 초과, 고를 수 있는 수가 0 이하이면 중지
            if (idx >= currentValues.Count || pickCount <= 0)
            {
                // 고를 수 있는 수가 0이 아니면 패스
                if (pickCount != 0) return;
    
                // 핸드 체크
                GetHandCheckResultsNonAlloc(currentValues);
    
                // 성공 핸드 카운트 증가
                foreach (var hand in HandCheckResultsCache.Keys)
                {
                    if (HandCheckResultsCache[hand])
                    {
                        if (!successCounts.ContainsKey(hand))
                        {
                            successCounts[hand] = 0;
                        }
                        successCounts[hand]++;
                    }
                }
    
                return;
            }
    
            // 고를 수 있는 수가 남은 주사위보다 많으면 종료
            if (pickCount > currentValues.Count - idx) return;
    
            // 현재 인덱스의 주사위를 고르는 경우 탐색
    
            // 값 저장
            var originalValue = currentValues[idx];
    
            // 1~6까지 값으로 변경 후 DFS 호출
            for (int i = 1; i <= 6; i++)
            {
                currentValues[idx] = i;
                DFS(currentValues, pickCount - 1, idx + 1, successCounts);
            }
    
            // 값 복구
            currentValues[idx] = originalValue;
    
            // 현재 인덱스의 주사위를 고르지 않는 경우 탐색
            DFS(currentValues, pickCount, idx + 1, successCounts);
        }
    ```
    

각각의 핸드에 대해서 달성 가능한 최대 확률을 얻어 플레이어에게 조언을 제공합니다. 확률 계산은 (달성 횟수)/(주사위 조합의 경우의 수)가 됩니다.

- CalculateHandProbabilities
    
    ```csharp
        private static Dictionary<Hand, float> CalculateHandProbabilities(List<int> diceValues, int maxRollDiceNum)
        {
            Dictionary<Hand, float> res = new();
    
            foreach (var hand in DataContainer.Instance.TotalHandListSO.handList)
            {
                res[hand.hand] = 0;
            }
    
            for (int i = 0; i <= maxRollDiceNum; i++)
            {
                Dictionary<Hand, float> tmp = new();
    
                var handCounts = GetHandSuccessCounts(diceValues, i);
                int totalCount = (int)Mathf.Pow(6, i);
    
                foreach (var hand in handCounts.Keys)
                {
                    tmp[hand] = handCounts[hand] / (float)totalCount;
                }
    
                foreach (var pair in tmp)
                {
                    if (!res.ContainsKey(pair.Key))
                    {
                        res[pair.Key] = pair.Value;
                    }
                    else
                    {
                        res[pair.Key] = Mathf.Max(res[pair.Key], pair.Value);
                    }
                }
            }
    
            return res;
        }
    ```