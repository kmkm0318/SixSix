using System;
using System.Collections.Generic;

[Serializable]
public struct Pair<Key, Value>
{
    public Key key;
    public Value value;
}

[Serializable]
public struct PairList<Key, Value>
{
    public List<Pair<Key, Value>> pairs;

    public Dictionary<Key, Value> GetDict()
    {
        Dictionary<Key, Value> res = new();

        foreach (var pair in pairs)
        {
            res[pair.key] = pair.value;
        }

        return res;
    }
}