using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : Singleton<SequenceManager>
{
    private Queue<IEnumerator> sequentialCoroutineQueue = new();
    private List<IEnumerator> parallelCoroutineList = new();

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    #region AddCoroutine(IEnumerator coroutine)
    // public void AddCoroutine(IEnumerator coroutine)
    // {
    //     if (coroutine == null)
    //     {
    //         Debug.LogWarning("Coroutine is null. Cannot add to the queue.");
    //         return;
    //     }

    //     sequentialCoroutineQueue.Enqueue(coroutine);

    //     if (!isPlaying)
    //     {
    //         StartCoroutine(PlaySequentialCoroutines());
    //     }
    // }
    #endregion

    public void AddCoroutine(params IEnumerator[] coroutines)
    {
        if (coroutines == null || coroutines.Length == 0)
        {
            Debug.LogWarning("No coroutines to add to the queue.");
            return;
        }

        if (parallelCoroutineList.Count > 0)
        {
            ApplyParallelCoroutine();
        }

        if (coroutines.Length == 1)
        {
            sequentialCoroutineQueue.Enqueue(coroutines[0]);
        }
        else
        {
            sequentialCoroutineQueue.Enqueue(PlayParallelCoroutines(coroutines));
        }

        if (!isPlaying)
        {
            StartCoroutine(PlaySequentialCoroutines());
        }
    }

    public void AddParallelCoroutine(IEnumerator coroutine)
    {
        parallelCoroutineList.Add(coroutine);
    }

    public void ApplyParallelCoroutine()
    {
        var parallelCoroutineArray = parallelCoroutineList.ToArray();
        parallelCoroutineList.Clear();
        AddCoroutine(parallelCoroutineArray);
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
