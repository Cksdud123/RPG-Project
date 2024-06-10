using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class WeightedRandomList<T>
{
    // 아이템과 가중치를 동시에 받기 위해 Pair로 저장
    [System.Serializable]
    public struct Pair
    {
        public T item;
        public float weight;

        public Pair(T item, float weight)
        {
            this.item = item;
            this.weight = weight;
        }
    }

    // Pair를 리스트로 보관
    public List<Pair> list = new List<Pair>();

    // 아이템을 드롭하지 않을 확률
    private float nondropRate;

    public int Count
    {
        get => list.Count;
    }

    public float Value
    {
        set => this.nondropRate = value;
        get => nondropRate;
    }

    public void Add(T item, float weight)
    {
        list.Add(new Pair(item, weight));
    }

    public T GetRandom()
    {
        float totalWeight = nondropRate;

        // 가중치를 모두 더함
        foreach (Pair p in list)
        {
            totalWeight += p.weight;
        }

        // value에 가중치를 곱해서 랜덤한 값을 만듬
        float randomValue = Random.value * totalWeight;

        float sumWeight = 0;

        // 가중치를 계속 더하며 현재 value보다 크다면 그 아이템을 리턴
        foreach (Pair p in list)
        {
            sumWeight += p.weight;

            if (sumWeight >= randomValue)
            {
                return p.item;
            }
        }

        // 아이템이 드롭되지 않는 경우
        if (randomValue < nondropRate)
        {
            return default(T);
        }

        return default(T);
    }
}