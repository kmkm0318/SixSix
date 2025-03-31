using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : Singleton<SequenceManager>
{
    private Queue<(IEnumerator, bool)> coroutineQueue = new();
    private List<IEnumerator> parallelCoroutineList = new();
    private Action onSequenceCompleted;

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    public void AddCoroutine(IEnumerator coroutine, bool isParallel = false, Action onComplete = null)
    {
        coroutineQueue.Enqueue((coroutine, isParallel));
        if (onComplete != null)
        {
            onSequenceCompleted += onComplete;
        }

        if (!isPlaying)
        {
            StartCoroutine(PlaySequentialCoroutines());
        }
    }

    private IEnumerator PlaySequentialCoroutines()
    {
        isPlaying = true;

        while (coroutineQueue.Count > 0)
        {
            var (coroutine, isParallel) = coroutineQueue.Dequeue();
            if (isParallel)
            {
                parallelCoroutineList.Add(coroutine);
            }
            else
            {
                if (parallelCoroutineList.Count > 0)
                {
                    yield return StartCoroutine(PlayParallelMethods());
                }

                yield return StartCoroutine(coroutine);
            }
        }

        if (parallelCoroutineList.Count > 0)
        {
            yield return StartCoroutine(PlayParallelMethods());
        }

        isPlaying = false;

        onSequenceCompleted?.Invoke();
        onSequenceCompleted = null;
    }

    private IEnumerator PlayParallelMethods()
    {
        List<Coroutine> runningCoroutines = new();

        foreach (var coroutine in parallelCoroutineList)
        {
            runningCoroutines.Add(StartCoroutine(coroutine));
        }

        foreach (var coroutine in runningCoroutines)
        {
            yield return coroutine;
        }

        parallelCoroutineList.Clear();
    }
}
