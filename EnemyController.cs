using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;             // 플레이어를 따라가기 위한 플레이어의 Transform
    public Transform grandma;            // 비둘기 할머니를 따라가기 위한 Grandma의 Transform
    public float moveSpeed = 3.0f;       // 이동 속도
    public float detectionRange = 7.0f;  // 플레이어 감지 범위
    public float roamingRadius = 20.0f;  // 로밍 반경
    public float roamingDuration = 3.0f; // 로밍 지속 시간

    private Vector3 startPosition;       // 초기 위치
    public bool isChasing = false;     // 플레이어를 추격 중인지 여부
    private bool grandmaChasing = false;  // grandma를 추격 중인지 여부

    private Vector3 randomRoamingPosition; // 로밍 위치를 저장하기 위한 변수
    private float roamingTimer = 0.0f;    // 로밍 타이머

    private RandomCustomer randomCustomerScript; // RandomCustomer 스크립트 참조

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        grandma = GameObject.FindGameObjectWithTag("Grandma").transform;
        startPosition = transform.position;
        SetRandomRoamingPosition();

        // BreadCheck 오브젝트를 찾아서 그 아래에 있는 RandomCustomer 스크립트를 가져오도록 수정
        GameObject breadCheckObject = player.Find("BreadCheck").gameObject;
        randomCustomerScript = breadCheckObject.GetComponent<RandomCustomer>();
    }

    void Update()
    {
        float distanceToGrandma = Vector3.Distance(transform.position, grandma.position);
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (enabled) // Enemy가 활성화된 경우에만 실행
        {
            // Player와 Grandma의 위치 관련 거리 계산
            bool playerInRange = distanceToPlayer <= detectionRange && randomCustomerScript.mybread == true;
            bool grandmaInRange = distanceToGrandma <= detectionRange;

            if (playerInRange && !grandmaInRange)
            {
                // Player만 감지되었을 때 플레이어 추적
                isChasing = true;
                grandmaChasing = false;

                // 바라보는 로직
                Vector3 lookDirection = (player.position - transform.position).normalized;
                transform.forward = lookDirection;
            }
            else if (!playerInRange && grandmaInRange)
            {
                // Grandma만 감지되었을 때 Grandma 추적
                isChasing = false;
                grandmaChasing = true;

                Vector3 lookDirection = (grandma.position - transform.position).normalized;
                transform.forward = lookDirection;
            }
            else if (playerInRange && grandmaInRange)
            {
                // Player와 Grandma 모두 감지되었을 때 Player 추적
                isChasing = true;
                grandmaChasing = false;

                Vector3 lookDirection = (player.position - transform.position).normalized;
                transform.forward = lookDirection;
            }
            else
            {
                // 모두 감지 범위에 들어오지 않을 때 랜덤 로밍
                isChasing = false;
                grandmaChasing = false;
                Vector3 moveDirection = (randomRoamingPosition - transform.position).normalized;
                transform.forward = moveDirection;
                Roam();
            }

            if (isChasing) // 쫓아가는 로직
            {
                PlayerChasing();

                if (distanceToPlayer >= detectionRange)
                {
                    isChasing = false;
                    SetRandomRoamingPosition();
                }
            }
            else if (grandmaChasing)
            {
                GrandmaChasing();

                if (distanceToGrandma >= detectionRange)
                {
                    grandmaChasing = false;
                    SetRandomRoamingPosition();
                }
            }
            else
            {
                // 추적 중이 아닐 때도 이동 방향으로 얼굴을 맞추기
                Vector3 moveDirection = (randomRoamingPosition - transform.position).normalized;
                transform.forward = moveDirection;
                Roam();
            }
        }
    }

    void PlayerChasing()
    {
        Vector3 moveDirection = (player.position - transform.position).normalized;
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }

    void GrandmaChasing()
    {
        float distanceToGrandma = Vector3.Distance(transform.position, grandma.position);

        // Grandma와의 거리가 일정 값 이상인 경우에만 Grandma를 추적
        if (distanceToGrandma > 0.5f)
        {
            Vector3 moveDirection = (grandma.position - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") // Assuming the player has the "Player" tag
        {
            randomCustomerScript.mybread = false;
            randomCustomerScript.bread.SetActive(false);
        }
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
