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
        private int startAtLevel = 1;
        [SerializeField]
        private int poolCount = 5;
        [SerializeField]
        private float spawnRange;
        [SerializeField]
        private PoolableMonobehaviour[] enemyPrefabList;
        [SerializeField]
        private Transform endTarget;
        [SerializeField]
        private float spawningInterval = 0.2f;
        [SerializeField]
        private float startSpawnDelay;
        [SerializeField]
        private int maxGroupSpawnLimit;
        [SerializeField]
        private bool isOnHorizontalAxis = true;

        private GameObjectPool<BaseEnemy> _enemyPool;
        private Player _player;
        private bool _spawning;
        private bool _gameStarted;
        private int _currentLevel = 1;
        private Coroutine _spawningRoutine;

        private void Spawn()
        {
            if (_spawning)
                return;
            
            RandomizePosition();
            StartCoroutine(SpawnEnemies(Mathf.Min(maxGroupSpawnLimit,Random.Range(1, 2 * _currentLevel - startAtLevel))));
        }

        private IEnumerator SpawnEnemies(int enemyCount)
        {
            _spawning = true;
            
            for (int i = 0; i < enemyCount; i++)
            {
                if (!_gameStarted)
                    yield break;
                
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
            var rangeOffset = Random.Range(-spawnRange, spawnRange);
            var randomPosition = transform.localPosition;

            if (isOnHorizontalAxis)
                randomPosition.x = rangeOffset;
            else
                randomPosition.y = rangeOffset;
            
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
            GameFlowManager.OnLevelChange += LevelChange;
            GameFlowManager.OnGameEnd += GameEnds;
        }

        private void OnDestroy()
        {
            GameFlowManager.OnGameStart -= StartSpawning;
            GameFlowManager.OnLevelChange += LevelChange;
            GameFlowManager.OnGameEnd -= GameEnds;
        }

        private void LevelChange(int currentLevel)
        {
            _currentLevel = currentLevel;
            if (_spawningRoutine != null)
                StopCoroutine(_spawningRoutine);
            
            if (startAtLevel <= GameFlowManager.CurrentLevel)
                _spawningRoutine = StartCoroutine(ContinuousSpawning());
        }

        private void StartSpawning()
        {
            _gameStarted = true;
            
            if (startAtLevel <= GameFlowManager.CurrentLevel)
                _spawningRoutine = StartCoroutine(ContinuousSpawning());
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
            _currentLevel = 1;
        }

        private void OnDrawGizmos()
        {
            var startRangePosition = transform.position;
            var endRangePosition = transform.position;

            if (isOnHorizontalAxis)
            {
                startRangePosition.x -= spawnRange;
                endRangePosition.x += spawnRange;
            }
            else
            {
                startRangePosition.y -= spawnRange;
                endRangePosition.y += spawnRange;
            }

            Gizmos.color = Color.white;
            Gizmos.DrawLine(startRangePosition, endRangePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(startRangePosition, 1);
            Gizmos.DrawWireSphere(endRangePosition, 1);
        }
    }
}