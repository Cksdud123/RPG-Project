using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

[System.Serializable]
public class WeightedRandomList<T>
{
    // �����۰� ����ġ�� ���ÿ� �ޱ� ���� Pair�� ����
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

    // Pair�� ����Ʈ�� ����
    public List<Pair> list = new List<Pair>();

    // �������� ������� ���� Ȯ��
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

        // ����ġ�� ��� ����
        foreach (Pair p in list)
        {
            totalWeight += p.weight;
        }

        // value�� ����ġ�� ���ؼ� ������ ���� ����
        float randomValue = Random.value * totalWeight;

        float sumWeight = 0;

        // ����ġ�� ��� ���ϸ� ���� value���� ũ�ٸ� �� �������� ����
        foreach (Pair p in list)
        {
            sumWeight += p.weight;

            if (sumWeight >= randomValue)
            {
                return p.item;
            }
        }

        // �������� ��ӵ��� �ʴ� ���
        if (randomValue < nondropRate)
        {
            return default(T);
        }

        return default(T);
    }
}