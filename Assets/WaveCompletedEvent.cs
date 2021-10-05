using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCompletedEvent : GenericEvent
{

    public string WaveName { get; }
    public WaveCompletedEvent(string waveName)
    {
        this.WaveName = waveName;
    }

    public override string GetDescription()
    {
        return "Wave completed event";
    }
}
