using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    [Tooltip("Specifies name of wave")]
    [SerializeField] private string name = "";
    [SerializeField] private List<WaveItem> waveItems = new List<WaveItem>();

    public string GetName()
    {
        return name;
    }

    public List<WaveItem> GetWaveItems()
    {
        return waveItems;
    }
}
