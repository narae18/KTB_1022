using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunHealth : MonoBehaviour
{
    public Slider run;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        run.value = PuxxeStudio.PlayerControllDemo.health;
    }
}
