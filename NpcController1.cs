using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController1 : MonoBehaviour
{
    public Transform player;             // 플레이어를 따라가기 위한 플레이어의 Transform
    public float moveSpeed = 3.0f;       // 이동 속도
    public float roamingRadius = 20.0f;  // 로밍 반경
    public float roamingDuration = 3.0f; // 로밍 지속 시간

    private Vector3 startPosition;       // 초기 위치
    private bool isChasing = false;     // 플레이어를 추격 중인지 여부

    private Vector3 randomRoamingPosition; // 로밍 위치를 저장하기 위한 변수
    private float roamingTimer = 0.0f;    // 로밍 타이머

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        startPosition = transform.position;
        SetRandomRoamingPosition();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        // 추적 중이 아닐 때도 이동 방향으로 얼굴을 맞추기
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
