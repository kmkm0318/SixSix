using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequenceManager : Singleton<AnimationSequenceManager>
{
    private Queue<(IEnumerator, bool)> animationQueue = new();
    private List<IEnumerator> parallelAnimations = new();

    private bool isPlaying = false;
    public bool IsPlaying => isPlaying;

    public void AddAnimation(IEnumerator animation, bool isParallel = false)
    {
        animationQueue.Enqueue((animation, isParallel));

        if (!isPlaying)
        {
            StartCoroutine(PlaySequentialAnimations());
        }
    }

    private IEnumerator PlaySequentialAnimations()
    {
        isPlaying = true;

        while (animationQueue.Count > 0)
        {
            var (animation, isParallel) = animationQueue.Dequeue();
            if (isParallel)
            {
                parallelAnimations.Add(animation);
            }
            else
            {
                if (parallelAnimations.Count > 0)
                {
                    yield return StartCoroutine(PlayParallelAnimations(parallelAnimations.ToArray()));
                    parallelAnimations.Clear();
                }

                yield return StartCoroutine(animation);
            }
        }

        if (parallelAnimations.Count > 0)
        {
            yield return StartCoroutine(PlayParallelAnimations(parallelAnimations.ToArray()));
            parallelAnimations.Clear();
        }

        isPlaying = false;
    }

    private IEnumerator PlayParallelAnimations(params IEnumerator[] animations)
    {
        List<Coroutine> runningCoroutines = new();

        foreach (var animation in animations)
        {
            runningCoroutines.Add(StartCoroutine(animation));
        }

        foreach (var coroutine in runningCoroutines)
        {
            yield return coroutine;
        }
    }
}
