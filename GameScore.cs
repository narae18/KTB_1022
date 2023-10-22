using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScore : MonoBehaviour
{
    public float[] time;  //각 레벨 별로 남은 시간 저장

    private void    Awake()
    {
        var objs = FindObjectsOfType<GameScore>();
        if (objs.Length == 1)
            DontDestroyOnLoad(gameObject);
        else
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
