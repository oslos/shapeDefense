using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject _towerGameObject;
    [SerializeField] private GameObject _shieldGameObject;
    private Tower _tower;
    private Shield _shield;

    private static GameManager _instance;

    public static GameManager Instance()
    {
        return _instance;
    }

    private void Awake()
    {
        if (!_instance)
            _instance = this;

        Initialize();
    }

    void OnDestroy()
    {
        EventManager.Instance.UnRegisterListener(GameEventActions.WAVE_COMPLETED, OnWaveCompleted);
    }

    private void Initialize()
    {

        if (_towerGameObject != null)
        {
            _tower = _towerGameObject.GetComponent<Tower>();
        }

        if (_shieldGameObject != null)
        {
            _shield = _shieldGameObject.GetComponent<Shield>();
        }

          EventManager.Instance.RegisterListener(GameEventActions.WAVE_COMPLETED, OnWaveCompleted);
    }


    private void OnWaveCompleted(GenericEvent waveCompletedEvent)
    {
        if (waveCompletedEvent is WaveCompletedEvent)
        {
            WaveCompletedEvent waveCompleted = (WaveCompletedEvent)waveCompletedEvent;

            Debug.Log("Game manager wave completed: " + waveCompleted.WaveName);

            // Start counter and then next wave;
        }

    }

    public Tower GetTower()
    {
        return _tower;
    }

    public GameObject GetTowerGameObject()
    {
        return _towerGameObject;
    }

    public Shield GetShield()
    {
        return _shield;
    }

    public GameObject GetShieldGameObject()
    {
        return _shieldGameObject;
    }
}
