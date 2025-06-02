using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : Singleton<SequenceManager>
{
    private Queue<IEnumerator> sequentialCoroutineQueue = new();
    private List<IEnumerator> parallelCoroutineList = new();

    private bool isPlaying = false;

    public event Action OnAnimationStarted;
    public event Action OnAnimationFinished;

    public void AddCoroutine(IEnumerator coroutine, bool isParallel = false)
    {
        if (coroutine == null)
        {
            Debug.LogWarning("Coroutine is null. Cannot add to the queue.");
            return;
        }

        if (isParallel)
        {
            parallelCoroutineList.Add(coroutine);
            return;
        }

        if (parallelCoroutineList.Count > 0)
        {
            ApplyParallelCoroutine();
        }

        sequentialCoroutineQueue.Enqueue(coroutine);

        if (!isPlaying)
        {
            StartCoroutine(PlaySequentialCoroutines());
        }
    }

    public void AddCoroutine(Action action, bool isParallel = false)
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null. Cannot execute.");
            return;
        }

        AddCoroutine(ExecuteAction(action), isParallel);
    }

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

    private IEnumerator ExecuteAction(Action action)
    {
        action.Invoke();
        yield break;
    }

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
}
