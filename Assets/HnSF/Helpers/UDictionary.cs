/*using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

// https://gist.github.com/Moe-Baker/e36610361012d586b1393994febeb5d2
[Serializable]
public class UDictionary
{
    public class SplitAttribute : PropertyAttribute
    {
        public float Key { get; protected set; }
        public float Value { get; protected set; }

        public SplitAttribute(float key, float value)
        {
            this.Key = key;
            this.Value = value;
        }
    }
}

[Serializable]
public class UDictionary<TKey, TValue> : UDictionary, IDictionary<TKey, TValue>
{
    [SerializeField]
    List<TKey> keys;
    public List<TKey> Keys => keys;
    ICollection<TKey> IDictionary<TKey, TValue>.Keys => keys;

    [SerializeField]
    List<TValue> values;
    public List<TValue> Values => values;
    ICollection<TValue> IDictionary<TKey, TValue>.Values => values;

    public int Count => keys.Count;

    public bool IsReadOnly => false;

    public Dictionary<TKey, TValue> cache;

    public bool Cached => cache != null;

    public Dictionary<TKey, TValue> Dictionary
    {
        get
        {
            if (cache == null)
            {
                cache = new Dictionary<TKey, TValue>();

                for (int i = 0; i < keys.Count; i++)
                {
                    if (keys[i] == null) continue;
                    if (cache.ContainsKey(keys[i])) continue;

                    cache.Add(keys[i], values[i]);
                }
            }

            return cache;
        }
    }

    public TValue this[TKey key]
    {
        get => Dictionary[key];
        set
        {
            var index = keys.IndexOf(key);

            if (index < 0)
            {
                Add(key, value);
            }
            else
            {
                values[index] = value;
                if (Cached) Dictionary[key] = value;
            }
        }
    }

    public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value);

    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);
    public bool Contains(KeyValuePair<TKey, TValue> item) => ContainsKey(item.Key);

    public void Add(TKey key, TValue value)
    {
        keys.Add(key);
        values.Add(value);

        if (Cached) Dictionary.Add(key, value);
    }
    public void Add(KeyValuePair<TKey, TValue> item) => Add(item.Key, item.Value);

    public bool Remove(TKey key)
    {
        var index = keys.IndexOf(key);

        if (index < 0) return false;

        keys.RemoveAt(index);
        values.RemoveAt(index);

        if (Cached) Dictionary.Remove(key);

        return true;
    }
    public bool Remove(KeyValuePair<TKey, TValue> item) => Remove(item.Key);

    public void Clear()
    {
        keys.Clear();
        values.Clear();

        if (Cached) Dictionary.Clear();
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => (Dictionary as IDictionary).CopyTo(array, arrayIndex);

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Dictionary.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => Dictionary.GetEnumerator();

    public UDictionary()
    {
        values = new List<TValue>();
        keys = new List<TKey>();
    }
}*/