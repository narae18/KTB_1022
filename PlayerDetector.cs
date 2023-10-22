using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 감지되면 적(Enemy)에게 알리기
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어가 감지 범위에서 나가면 적(Enemy)에게 알리기
            EnemyController enemy = other.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.enabled = false;
            }
        }
    }
}
