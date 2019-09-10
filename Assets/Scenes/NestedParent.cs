using System.Collections.Generic;
using UnityEngine;

public class NestedParent : MonoBehaviour
{
    public List<Transform> childs = new List<Transform>();

    private void Start()
    {
        FindEveryChild(gameObject.transform);
    }

    public void FindEveryChild(Transform parent)
    {
        int count = parent.childCount;
        for (int i = 0; i < count; i++)
        {
            Transform child = parent.GetChild(i);

            childs.Add(child);

            if (child.childCount > 0)
            {
                FindEveryChild(child);
            }        
        }
    }
}
