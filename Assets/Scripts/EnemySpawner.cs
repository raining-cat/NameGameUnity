using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemyAI enemyPrefab;
    public PlayerController player;
    public List<Transform> patrolPoints;

    public int enemiesMaxCount = 5;
    public float delay = 3;
    public float increaseEnemiesCountDelay = 30;

    public List<Transform> spawnerPoints;
    private List<EnemyAI> _enemies;

    private float _timeLastSpawned;

    private void Start()
    {
        spawnerPoints = new List<Transform>(transform.GetComponentsInChildren<Transform>());
        _enemies = new List<EnemyAI>();

        Invoke("IncreaseEnemiesMaxCount", increaseEnemiesCountDelay);
    }

    private void IncreaseEnemiesMaxCount()
    {
        enemiesMaxCount++;
        Invoke("IncreaseEnemiesMaxCount", increaseEnemiesCountDelay);
    }

    private void Update()
    {
        CheckForDeathEnemies();
        CreateEnemy();
    }

    private void CheckForDeathEnemies()
    {
        for (var i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i].IsAlive()) continue;
            _enemies.RemoveAt(i);
            i--;
        }
    }

    private void CreateEnemy()
    {
        if (_enemies.Count >= enemiesMaxCount) return;
        if (Time.time - _timeLastSpawned < delay) return;

        var enemy = Instantiate(enemyPrefab);
        enemy.transform.position = spawnerPoints[Random.Range(0, spawnerPoints.Count)].position;
        enemy.player = player;
        enemy.patrolPoints = patrolPoints;
        _enemies.Add(enemy);
        _timeLastSpawned = Time.time;
    }
}
