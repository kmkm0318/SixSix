using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// 파티클 매니저 클래스
/// </summary>
public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem _diceCollideEffect;
    [SerializeField] private ParticleSystem _handSuccessEffect;

    #region 오브젝트 풀
    private ObjectPool<ParticleSystem> _diceCollideEffectPool;
    private ObjectPool<ParticleSystem> _handSuccessEffectPool;
    #endregion

    private void Awake()
    {
        InitPool();
        RegisterEvents();
    }

    #region 오브젝트 풀링
    private void InitPool()
    {
        _diceCollideEffectPool = new(
            createFunc: () => Instantiate(_diceCollideEffect),
            actionOnGet: (effect) => effect.gameObject.SetActive(true),
            actionOnRelease: (effect) => effect.gameObject.SetActive(false),
            actionOnDestroy: (effect) => Destroy(effect.gameObject),
            collectionCheck: false,
            defaultCapacity: 10,
            maxSize: 20
        );

        _handSuccessEffectPool = new(
            createFunc: () => Instantiate(_handSuccessEffect),
            actionOnGet: (effect) => effect.gameObject.SetActive(true),
            actionOnRelease: (effect) => effect.gameObject.SetActive(false),
            actionOnDestroy: (effect) => Destroy(effect.gameObject),
            collectionCheck: false,
            defaultCapacity: 5,
            maxSize: 10
        );
    }
    #endregion

    private void OnDestroy()
    {
        UnregisterEvents();
    }

    #region 이벤트 구독, 해제
    private void RegisterEvents()
    {
        ParticleEvents.OnDiceCollide += SpawnDiceCollideEffect;
        ParticleEvents.OnHandSuccess += SpawnHandSuccessEffect;
    }

    private void UnregisterEvents()
    {
        ParticleEvents.OnDiceCollide -= SpawnDiceCollideEffect;
        ParticleEvents.OnHandSuccess -= SpawnHandSuccessEffect;
    }
    #endregion

    #region 파티클 스폰 함수
    private void SpawnDiceCollideEffect(Vector3 pos, Vector3 dir)
    {
        var rot = Quaternion.FromToRotation(Vector3.right, dir);

        var ps = _diceCollideEffectPool.Get();
        ps.transform.SetPositionAndRotation(pos, rot);
        ps.Play();

        StartCoroutine(DelayReleaseEffect(ps, _diceCollideEffectPool));
    }

    private void SpawnHandSuccessEffect(Vector3 pos)
    {
        var ps = _handSuccessEffectPool.Get();
        ps.transform.position = pos;
        ps.Play();

        StartCoroutine(DelayReleaseEffect(ps, _handSuccessEffectPool));
    }
    #endregion

    #region 파티클 지연 해제 코루틴
    private IEnumerator DelayReleaseEffect(ParticleSystem ps, ObjectPool<ParticleSystem> pool)
    {
        yield return new WaitUntil(() => !ps.isPlaying);
        pool.Release(ps);
    }
    #endregion
}