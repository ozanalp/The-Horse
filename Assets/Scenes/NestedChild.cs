using System.Collections.Generic;
using UnityEngine;

public class NestedChild : MonoBehaviour
{
    public List<Transform> childs = new List<Transform>();

    private void Start()
    {
        FindEveryChild(gameObject.transform);
        for (int i = 0; i < childs.Count; i++)
        {
            FindEveryChild(childs[i]);
            Debug.Log(childs.Count);
        }
    }

    public void FindEveryChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            childs.Add(parent.GetChild(i));
        }
    }
}