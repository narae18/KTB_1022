using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVoice : MonoBehaviour
{
    public float rotateSpeed;

    private bool isActive = true;
    public EnemyController enemyController; // EnemyController ½ºÅ©

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
            enemyController.isChasing = false;
            gameObject.SetActive(false);
        }
    }

}
