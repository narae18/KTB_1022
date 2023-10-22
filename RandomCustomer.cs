using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using System;

public class RandomCustomer : MonoBehaviour
{
    [System.Serializable]
    public class arr
    {
        public GameObject[] customer = new GameObject[3];
    }

    public arr[] MCCustomer = new arr[5];
    public arr[] WHCustomer = new arr[4];
    public arr[] MHCustomer = new arr[49];
    public arr[] BSCustomer = new arr[5];
    public arr[] DGCustomer = new arr[5];

    public int num1;
    public int num2;
    public int num3;
    public int num4;
    public int num5;

    public GameObject bread;
    public bool mybread = false;
    public bool[] customer_bread;
    public GameObject[] Delivery_bread;
    public int count = 0;

    public GameObject GameClear;

    // Start is called before the first frame update
    void Start()
    {
        num1 = Random.Range(0, 5);
        num2 = Random.Range(0, 4);
        num3 = Random.Range(0, 49);
        num4 = Random.Range(0, 5);
        num5 = Random.Range(0, 5);

        MCCustomer[num1].customer[0].SetActive(false);
        MCCustomer[num1].customer[1].SetActive(true);
        MCCustomer[num1].customer[1].tag = "mc";
        MCCustomer[num1].customer[2].SetActive(true);

        WHCustomer[num2].customer[0].SetActive(false);
        WHCustomer[num2].customer[1].SetActive(true);
        WHCustomer[num2].customer[1].tag = "wh";
        WHCustomer[num2].customer[2].SetActive(true);

        MHCustomer[num3].customer[0].SetActive(false);
        MHCustomer[num3].customer[1].SetActive(true);
        MHCustomer[num3].customer[1].tag = "mh";
        MHCustomer[num3].customer[2].SetActive(true);

        BSCustomer[num4].customer[0].SetActive(false);
        BSCustomer[num4].customer[1].SetActive(true);
        BSCustomer[num4].customer[1].tag = "bs";
        BSCustomer[num4].customer[2].SetActive(true);

        DGCustomer[num5].customer[0].SetActive(false);
        DGCustomer[num5].customer[1].SetActive(true);
        DGCustomer[num5].customer[1].tag = "dg";
        DGCustomer[num5].customer[2].SetActive(true);

        customer_bread[0] = false;
        customer_bread[1] = false;
        customer_bread[2] = false;
        customer_bread[3] = false;
        customer_bread[4] = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 1)
        {
            GameClear.SetActive(true);
            if (GameObject.Find("GameScore").GetComponent<GameScore>().time[0] < GameObject.Find("Canvas").GetComponent<Timer>().time)
                GameObject.Find("GameScore").GetComponent<GameScore>().time[0] = GameObject.Find("Canvas").GetComponent<Timer>().time;
            // IsPause = true;
            Time.timeScale = 0;
        }
    }

    public void SetBreadState(bool state)
    {
        mybread = state;
        bread.SetActive(state);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Bread")
        {
            if (mybread == false)
            {
                mybread = true;
                bread.SetActive(true);
            }
        }

        if (other.tag == "mc")
        {
            if (mybread == true)
            {
                mybread = false;
                bread.SetActive(false);
                customer_bread[0] = true;
                count++;
                MCCustomer[num1].customer[0].SetActive(true);
                MCCustomer[num1].customer[1].SetActive(false);
                MCCustomer[num1].customer[1].tag = "Untagged";
                MCCustomer[num1].customer[2].SetActive(false);
                Delivery_bread[0].SetActive(false);
            }
        }

        if (other.tag == "wh")
        {
            if (mybread == true)
            {
                mybread = false;
                bread.SetActive(false);
                customer_bread[1] = true;
                count++;
                WHCustomer[num2].customer[0].SetActive(true);
                WHCustomer[num2].customer[1].SetActive(false);
                WHCustomer[num2].customer[1].tag = "Untagged";
                WHCustomer[num2].customer[2].SetActive(false);
                Delivery_bread[1].SetActive(false);
            }
        }

        if (other.tag == "mh")
        {
            if (mybread == true)
            {
                mybread = false;
                bread.SetActive(false);
                customer_bread[2] = true;
                count++;
                MHCustomer[num3].customer[0].SetActive(true);
                MHCustomer[num3].customer[1].SetActive(false);
                MHCustomer[num3].customer[1].tag = "Untagged";
                MHCustomer[num3].customer[2].SetActive(false);
                Delivery_bread[2].SetActive(false);
            }
        }

        if (other.tag == "bs")
        {
            if (mybread == true)
            {
                mybread = false;
                bread.SetActive(false);
                customer_bread[3] = true;
                count++;
                BSCustomer[num4].customer[0].SetActive(true);
                BSCustomer[num4].customer[1].SetActive(false);
                BSCustomer[num4].customer[1].tag = "Untagged";
                BSCustomer[num4].customer[2].SetActive(false);
                Delivery_bread[3].SetActive(false);
            }
        }

        if (other.tag == "dg")
        {
            if (mybread == true)
            {
                mybread = false;
                bread.SetActive(false);
                customer_bread[4] = true;
                count++;
                DGCustomer[num5].customer[0].SetActive(true);
                DGCustomer[num5].customer[1].SetActive(false);
                DGCustomer[num5].customer[1].tag = "Untagged";
                DGCustomer[num5].customer[2].SetActive(false);
                Delivery_bread[4].SetActive(false);
            }
        }
    }
}
