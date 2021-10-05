using UnityEngine;

[System.Serializable]
public class WaveItem
{
    [Tooltip("Spawned enemy type")]
    [SerializeField] private GameObject enemyType = null;

    [Tooltip("Specifies count of enemy type")]
    [SerializeField] private int count = 0;

    [Tooltip("Delay in sec between each enemy spawn")]
    [SerializeField] private float delay = -1f;


    public GameObject GetEnemyType()
    {
        return enemyType;
    }

    public int GetCount()
    {
        return count;
    }
    public float GetDelay()
    {
        return delay;
    }


}
