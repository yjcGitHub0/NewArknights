using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heap
{
    private List<Vector3> a = new List<Vector3>
    {
        new Vector3(0,0,0)
    };
    
    public int count = 0;//堆中元素个数，指向末尾元素
    
    public void Push(Vector3 tmp)
    {
        count++;
        a.Add(tmp);
        int i, j;
        for (i = count >> 1, j = count; i > 0 && a[i].z > tmp.z; j = i, i = i >> 1)
        {
            a[j] = a[i];
        }
        a[j] = tmp;
    }

    public void Pop()
    {
        Vector3 tmp = a[count];
        count--;
        int i, j;
        for (i = 1; i <= count;)
        {
            j = (i << 1);
            if (j > count) break;
            if ((j | 1) <= count && a[(j | 1)].z < a[j].z) j = j | 1;
            if (tmp.z <= a[j].z) break;
            a[i] = a[j];
            i = j;
        }
        a[i] = tmp;
        a.RemoveAt(count+1);
    }
    
    public Vector3 Top()
    {
        return a[1];
    }

    public void Clear()
    {
        count = 0;
        a.Clear();
        a.Add(new Vector3(0, 0, 0));
    }
    
}
