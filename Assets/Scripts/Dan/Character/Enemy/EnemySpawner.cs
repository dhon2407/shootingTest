using System.Collections;
using Dan.Helper.Utils;
using Dan.Manager;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dan.Character.Enemy
{
    [AddComponentMenu("Dan/Enemy Spawner")]
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField]
        private int poolCount = 5;
        [SerializeField]
        private float horizontalRange;
        [SerializeField]
        private PoolableMonobehaviour[] enemyPrefabList;
        [SerializeField]
        private Transform endTarget;
        [SerializeField]
        private float spawningInterval = 0.2f;
        [SerializeField]
        private float startSpawnDelay;

        private GameObjectPool<BaseEnemy> _enemyPool;
        private Player _player;
        private bool _spawning;
        private bool _gameStarted;

        private void Spawn()
        {
            if (_spawning)
                return;
            
            RandomizePosition();
            StartCoroutine(SpawnEnemies(Random.Range(1, 2)));
        }

        private IEnumerator SpawnEnemies(int enemyCount)
        {
            _spawning = true;
            
            for (int i = 0; i < enemyCount; i++)
            {
                var enemy = _enemyPool.GetObject();
                if (enemy == null)
                {
                    Debug.LogWarning("Enemy pool exhausted!");
                    continue;
                }

                enemy.transform.position = transform.position;
                enemy.SetMoveDirection((endTarget.position - transform.position).normalized);
                enemy.SetPlayer(_player);
                enemy.StartMoving();
                yield return new WaitForSeconds(spawningInterval);
            }

            _spawning = false;
        }

        private void RandomizePosition()
        {
            var horizontalOffset = Random.Range(-horizontalRange, horizontalRange);
            var randomPosition = transform.localPosition;
            randomPosition.x = horizontalOffset;
            transform.localPosition = randomPosition;
        }

        private void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        private void Start()
        {
            _enemyPool = GameObjectPool<BaseEnemy>.CreateInstance(enemyPrefabList, poolCount, FreeObjectPool.Transform);
            GameFlowManager.OnGameStart += StartSpawning;
            GameFlowManager.OnGameEnd += GameEnds;
        }

        private void OnDestroy()
        {
            GameFlowManager.OnGameStart -= StartSpawning;
            GameFlowManager.OnGameEnd -= GameEnds;
        }

        private void StartSpawning()
        {
            _gameStarted = true;
            StartCoroutine(ContinuousSpawning());
        }

        private IEnumerator ContinuousSpawning()
        {
            yield return new WaitForSeconds(startSpawnDelay);
            while (_gameStarted)
            {
                Spawn();
                yield return new WaitForSeconds(Random.Range(1f, 3f));
            }
        }
        
        private void GameEnds()
        {
            _gameStarted = false;
        }

        private void OnDrawGizmos()
        {
            var startRangePosition = transform.position;
            var endRangePosition = transform.position;

            startRangePosition.x -= horizontalRange;
            endRangePosition.x += horizontalRange;

            Gizmos.color = Color.white;
            Gizmos.DrawLine(startRangePosition, endRangePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startRangePosition, 1);
            Gizmos.DrawWireSphere(endRangePosition, 1);
        }
    }
}