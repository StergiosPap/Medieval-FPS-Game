using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateEnemies : MonoBehaviour
{
    public GameObject Enemy;
    public GameObject enemySpawnPoint1, enemySpawnPoint2, enemySpawnPoint3;

    public Transform playerTransform;

    private bool isInsideEnemyArea, enemyHasSpawned;

    void Start()
    {
        isInsideEnemyArea = false;
        enemyHasSpawned = false;
    }

    void Update()
    {
        SpawnEnemy();
    }


    //Εαν ο παίκτης βρίσκεται μέσα στο δάσος των ορκ και δεν υπάρχει ήδη εχθρός, γίνεται spawn.
    void SpawnEnemy()
    {
        if (transform.position.x < -145 && transform.position.z >= 121 && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Transform enemy;
            GameObject enemySpawnPoint;
            int roll = Random.Range(1, 4);
            switch (roll)
            {
                case 1:
                    enemySpawnPoint = enemySpawnPoint1;
                    break;
                case 2:
                    enemySpawnPoint = enemySpawnPoint2;
                    break;
                case 3:
                    enemySpawnPoint = enemySpawnPoint3;
                    break;
                default:
                    enemySpawnPoint = enemySpawnPoint1;
                    break;
            }

            enemy = Instantiate(Enemy, enemySpawnPoint.transform.position, enemySpawnPoint.transform.rotation).transform;
            enemyHasSpawned = true;
        }
    }


}
