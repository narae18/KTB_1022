using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//일정시간동안 무적아이템
public class MushroomItem : Item
{
 
    public override void DestroyAfterTime()
    {
        Invoke("DestroyObject", 4.0f);
        gameObject.SetActive(true);
    }


    public override void RunItem()
    {

    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
