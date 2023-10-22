using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuxxeStudio;
using UnityEngine.UI;

public class ItemCan : MonoBehaviour
{
    public float rotateSpeed;
    public float itemDuration = 15.0f; // 아이템 지속 시간 (15초)

    private bool isActive = true;
    public Text messageText; // UI Text 오브젝트를 연결할 변수
    public Text timerText; // 아이템 제한 시간을 표시할 UI Text 오브젝트를 연결할 변수

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
        if (isActive && other.name == "Player")
        {
            PuxxeStudio.PlayerControllDemo playerspeed = other.GetComponent<PuxxeStudio.PlayerControllDemo>();

            playerspeed.moveSpeed *= 3;
            gameObject.SetActive(false);

            // 15초 후에 아이템을 파괴
            Invoke("DestroyItem", itemDuration);
        }
    }

    // 아이템을 파괴하는 메서드
    void DestroyItem()
    {
        isActive = false;
        Destroy(gameObject);

        // UI Text 오브젝트를 다시 비활성화
        messageText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }
}
