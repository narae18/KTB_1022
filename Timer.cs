using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timer;
    public float time;
    private int min;
    private int sec;

    public GameObject Gameover;
    bool IsPause;

    // Start is called before the first frame update
    void Start()
    {
        time = 300f;
        IsPause = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (time > 0)
            time -= Time.deltaTime;
        else
        {
            Gameover.SetActive(true);
            IsPause = true;
            Time.timeScale = 0;
        }
            // SceneManager.LoadScene("SelectLevel");
        
        min = (int)(Mathf.Ceil(time) / 60);
        sec = (int)(Mathf.Ceil(time) % 60);
        if (min < 10)
        {
            if (sec < 10)
                timer.text = "   " + "0" + min.ToString() + " : " + "0" + sec.ToString();
            else
                timer.text = "   " + "0" + min.ToString() + " : " + sec.ToString();
        }
        else
        {
            if (sec < 10)
                timer.text = "   " + min.ToString() + " : " + "0" + sec.ToString();
            else
                timer.text = "   " + min.ToString() + " : " + sec.ToString();
        }
    }
}
