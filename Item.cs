using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//모든 아이템 부모
public abstract class Item : MonoBehaviour
{
    public abstract void DestroyAfterTime();
    public abstract void RunItem();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
