using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    [SerializeField] private GameObject enemy;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            CloneEnemy();
        }
    }
    void CloneEnemy()
    {
        GameObject enemyClone = Instantiate(enemy, this.transform);
        //enemyClone.AddComponent<EnemyMove>(); Depricated move style
    }
}
