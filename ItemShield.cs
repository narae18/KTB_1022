using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShield : MonoBehaviour
{

    public EnemyController enemyController; // EnemyController 스크립트에 액세스하기 위한 변수
    public float rotateSpeed;
    //public float itemDuration = 15.0f; // 아이템 지속 시간 (15초)
    private bool isActive = true;

    /*
    public int numberOfItemsToGenerate = 5; // You can set the number of items in the Inspector

    void Start()
    {
        for (int i = 0; i < numberOfItemsToGenerate; i++)
        {
            GameObject newItem = Instantiate(gameObject, new Vector3(Random.Range(-5, 4) + 0.5f, Random.Range(-5, 4) + 0.5f, 0), Quaternion.identity);
            // Make sure to attach the ItemCan script to the instantiated GameObject
        }
    }*/
    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Destroy(this.gameObject);
            gameObject.SetActive(false);
        }
    }

}