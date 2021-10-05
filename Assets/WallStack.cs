using UnityEngine;

public class WallStack : MonoBehaviour
{
    private int wallsInStack;
    public int StartingWalls;
    public int maxWallsInStack;
    public int refreshRateSeconds;
    public bool HasWallAvailable { get { return wallsInStack > 0; } }

    private void Start()
    {
        wallsInStack = StartingWalls;
        //TODO game pause needs to be solved
        InvokeRepeating("AddWall", refreshRateSeconds, refreshRateSeconds);
    }

    public void AddWall()
    {
        if (wallsInStack < maxWallsInStack)
        {
            wallsInStack++;
        }
    }

    public bool RemoveWall()
    {
        if (wallsInStack > 0)
        {
            wallsInStack--;
            return true;
        }
        return false;
    }

}