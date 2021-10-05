using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("Array of waves objects")]
    [SerializeField] private Wave[] waves;
    private Queue<WaveWrapper> _wavesQueue = new Queue<WaveWrapper>();
    private Dictionary<string, WaveWrapper> _currentlyRunningWaves = new Dictionary<string, WaveWrapper>();
    private float waveCompletedSearchInterval = 1f;
    private float _currentWaveCompletedSearchInterval = 0f;
    private SpawnerState _state = SpawnerState.WAITING;
    private float _r = 6f;
    private static EnemySpawner _instance;

    public static EnemySpawner Instance { get { return _instance; } }

    private static long _waveId = 0;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;

            // To persist singleton across scenes.
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            addWave(waves[i]);
        }
    }


    public void addWave(Wave wave)
    {
        EnemySpawner._waveId++;
        _wavesQueue.Enqueue(new WaveWrapper(wave, EnemySpawner._waveId.ToString()));
    }


    public void NextWave()

    {
        if (_wavesQueue.Count > 0)
        {
            StartCoroutine(SpawnEnemyWave(_wavesQueue.Dequeue()));
        }
        else
        {
            // TODO fire "NO_WAVE_LEFT"
            Debug.Log("No wave left.");
        }
    }


    public void CheckIfSomeWaveIsCompleted()
    {
        _currentWaveCompletedSearchInterval += Time.deltaTime;

        if (_currentWaveCompletedSearchInterval >= waveCompletedSearchInterval)
        {
            _currentWaveCompletedSearchInterval = 0f;


            Dictionary<string, int> runningWavesWithEnemies = new Dictionary<string, int>();
            foreach (var waveId in _currentlyRunningWaves.Keys)
            {
                runningWavesWithEnemies.Add(waveId, 0);
            }


            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("enemy");

            for (int i = 0; i < gameObjects.Length; i++)
            {
                EnemyWaveMetaData enemyWaveMetaData = gameObjects[i].GetComponent<EnemyWaveMetaData>();

                if (enemyWaveMetaData != null && runningWavesWithEnemies.ContainsKey(enemyWaveMetaData.WaveId))
                {
                    runningWavesWithEnemies[enemyWaveMetaData.WaveId] += 1;
                }
            }


            foreach (string waveId in runningWavesWithEnemies.Keys)
            {
                if (runningWavesWithEnemies[waveId] == 0)
                {

                    // Fire event WAVE_COMPLETED
                    EventManager.Instance.FireEvent(GameEventActions.WAVE_COMPLETED,
                     new WaveCompletedEvent(_currentlyRunningWaves[waveId].GetWave().GetName()));

                    _currentlyRunningWaves.Remove(waveId);
                }
            }
        }
    }

    IEnumerator SpawnEnemyWave(WaveWrapper waveWrapper)
    {

        _state = SpawnerState.SPAWNING;

        _currentlyRunningWaves.Add(waveWrapper.GetId(), waveWrapper);

        foreach (var waveItem in waveWrapper.GetWave().GetWaveItems())
        {
            for (int i = 0; i < waveItem.GetCount(); i++)
            {

                if (waveItem.GetEnemyType() != null && waveItem.GetCount() > 0 && waveItem.GetDelay() >= 0)
                {
                    SpawnWaveEnemy(waveItem.GetEnemyType(), waveWrapper.GetId());
                    yield return new WaitForSeconds(waveItem.GetDelay());
                }

            }
        }

        _state = SpawnerState.WAITING;

        yield break;
    }

    private void SpawnWaveEnemy(GameObject enemy, string waveId)
    {
        Debug.Log("Spawning enemy: " + enemy.GetInstanceID().ToString() + "from wave: " + waveId);

        float angle = Random.Range(0, Mathf.PI * 2);
        var position = new Vector3(Mathf.Sin(angle) * _r, Mathf.Cos(angle) * _r, 0);

        EnemyWaveMetaData enemyWaveMetaData = enemy.AddComponent<EnemyWaveMetaData>() as EnemyWaveMetaData;
        enemyWaveMetaData.WaveId = waveId;

        Instantiate(enemy, position, Quaternion.identity);
    }


    // Update is called once per frame
    void Update()
    {

        // TODO for testing purposes, will be removed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextWave();
        }
        if (_state == SpawnerState.WAITING)
        {

            CheckIfSomeWaveIsCompleted();
        }
    }


    private enum SpawnerState
    {
        SPAWNING, WAITING
    }

    private class WaveWrapper
    {
        Wave wave;
        string id;

        public WaveWrapper(Wave wave, string id)
        {
            this.wave = wave;
            this.id = id;
        }

        public Wave GetWave()
        {
            return wave;
        }

        public string GetId()
        {
            return id;
        }
    }
}
