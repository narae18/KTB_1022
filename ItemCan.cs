using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PuxxeStudio;
using UnityEngine.UI;

public class ItemCan : MonoBehaviour
{
    public float rotateSpeed;
    public float itemDuration = 15.0f; // ������ ���� �ð� (15��)

    private bool isActive = true;
    public Text messageText; // UI Text ������Ʈ�� ������ ����
    public Text timerText; // ������ ���� �ð��� ǥ���� UI Text ������Ʈ�� ������ ����

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

            // 15�� �Ŀ� �������� �ı�
            Invoke("DestroyItem", itemDuration);
        }
    }

    // �������� �ı��ϴ� �޼���
    void DestroyItem()
    {
        isActive = false;
        Destroy(gameObject);

        // UI Text ������Ʈ�� �ٽ� ��Ȱ��ȭ
        messageText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }
}
