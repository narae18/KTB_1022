using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController1 : MonoBehaviour
{
    public Transform player;             // �÷��̾ ���󰡱� ���� �÷��̾��� Transform
    public float moveSpeed = 3.0f;       // �̵� �ӵ�
    public float roamingRadius = 20.0f;  // �ι� �ݰ�
    public float roamingDuration = 3.0f; // �ι� ���� �ð�

    private Vector3 startPosition;       // �ʱ� ��ġ
    private bool isChasing = false;     // �÷��̾ �߰� ������ ����

    private Vector3 randomRoamingPosition; // �ι� ��ġ�� �����ϱ� ���� ����
    private float roamingTimer = 0.0f;    // �ι� Ÿ�̸�

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        SetRandomRoamingPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // ���� ���� �ƴ� ���� �̵� �������� ���� ���߱�
        Vector3 moveDirection = (randomRoamingPosition - transform.position).normalized;
        transform.forward = moveDirection;
        Roam();
    }


    void SetRandomRoamingPosition()
    {
        randomRoamingPosition = startPosition + new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)) * roamingRadius;
    }

    void Roam()
    {
        roamingTimer += Time.deltaTime;
        if (roamingTimer >= roamingDuration)
        {
            SetRandomRoamingPosition();
            roamingTimer = 0.0f;
        }

        transform.position = Vector3.MoveTowards(transform.position, randomRoamingPosition, moveSpeed * Time.deltaTime);
    }
}
