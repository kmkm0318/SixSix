using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : Singleton<SequenceManager>
{
    private Queue<IEnumerator> sequentialCoroutineQueue = new();
    private List<IEnumerator> parallelCoroutineList = new();

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

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

    public void ExecuteLater(Action action)
    {
        if (action == null)
        {
            Debug.LogWarning("Action is null. Cannot add to the queue.");
            return;
        }

        StartCoroutine(ExecuteOneFrameLater(action));
    }

    private IEnumerator ExecuteOneFrameLater(Action action)
    {
        yield return null;
        action?.Invoke();
    }

    public void ApplyParallelCoroutine()
    {
        if (parallelCoroutineList.Count == 0)
        {
            Debug.LogWarning("No parallel coroutines to apply.");
            return;
        }

        var parallelCoroutineArray = parallelCoroutineList.ToArray();
        parallelCoroutineList.Clear();
        AddCoroutine(PlayParallelCoroutines(parallelCoroutineArray));
    }

    private IEnumerator PlaySequentialCoroutines()
    {
        isPlaying = true;

        yield return null;

        while (sequentialCoroutineQueue.Count > 0)
        {
            var coroutine = sequentialCoroutineQueue.Dequeue();

            yield return StartCoroutine(coroutine);
        }

        isPlaying = false;
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
