using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarLevel : MonoBehaviour
{
    public GameObject[] Level1_star;
    public GameObject[] Level2_star;
    public GameObject[] Level3_star;

    public GameObject Button_2;
    public GameObject Button_3;
    public GameObject Lock_2;
    public GameObject Lock_3;

    // Start is called before the first frame update
    void Start()
    {
        Level1_star[0] = GameObject.Find("Level1_Star").transform.Find("Level1_star1").gameObject;
        Level1_star[1] = GameObject.Find("Level1_Star").transform.Find("Level1_star2").gameObject;
        Level1_star[2] = GameObject.Find("Level1_Star").transform.Find("Level1_star3").gameObject;

        Level2_star[0] = GameObject.Find("Level2_Star").transform.Find("Level2_star1").gameObject;
        Level2_star[1] = GameObject.Find("Level2_Star").transform.Find("Level2_star2").gameObject;
        Level2_star[2] = GameObject.Find("Level2_Star").transform.Find("Level2_star3").gameObject;

        Level3_star[0] = GameObject.Find("Level3_Star").transform.Find("Level3_star1").gameObject;
        Level3_star[1] = GameObject.Find("Level3_Star").transform.Find("Level3_star2").gameObject;
        Level3_star[2] = GameObject.Find("Level3_Star").transform.Find("Level3_star3").gameObject;

        Button_2 = GameObject.Find("LevelButton").transform.Find("Level2").gameObject;
        Button_3 = GameObject.Find("LevelButton").transform.Find("Level3").gameObject;
        Lock_2 = GameObject.Find("Lock2");
        Lock_3 = GameObject.Find("Lock3");

        if (GameObject.Find("GameScore").GetComponent<GameScore>().time[0] != 0)
        {
            if (GameObject.Find("GameScore").GetComponent<GameScore>().time[0] >= 290)
            {
                Level1_star[0].SetActive(true);
                Level1_star[1].SetActive(true);
                Level1_star[2].SetActive(true);
            }
            else if(GameObject.Find("GameScore").GetComponent<GameScore>().time[0] >= 280)
            {
                Level1_star[0].SetActive(true);
                Level1_star[1].SetActive(true);
            }
            else
            {
                Level1_star[0].SetActive(true);
            }
            Button_2.SetActive(true);
            Lock_2.SetActive(false);
        }
        if (GameObject.Find("GameScore").GetComponent<GameScore>().time[1] != 0)
        {
            if (GameObject.Find("GameScore").GetComponent<GameScore>().time[1] >= 120)
            {

            }
            else if(GameObject.Find("GameScore").GetComponent<GameScore>().time[1] >= 60)
            {

            }
            else
            {
                
            }
            Button_3.SetActive(true);
            Lock_3.SetActive(false);
        }
        if (GameObject.Find("GameScore").GetComponent<GameScore>().time[2] != 0)
        {
            if (GameObject.Find("GameScore").GetComponent<GameScore>().time[2] >= 120)
            {

            }
            else if(GameObject.Find("GameScore").GetComponent<GameScore>().time[2] >= 60)
            {

            }
            else
            {
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
