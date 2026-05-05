using Leon.Singleton;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : Singleton<CheckPointManager>
{
    public int lastCheckPointKey = 0;

    public List<CheckPoint> checkPoints;

    protected override void Awake()
    {
        base.Awake();
        SaveCheckPoint(SaveManager.Instance.saveSetup.lastCheckpoint);
    }

    public void SaveCheckPoint(int i)
    {
        if (i > lastCheckPointKey)
        {
            lastCheckPointKey = i;
        }
    }

    public Vector3 GetPositionFromLastCheckpoint()
    {
        var checkpoint = checkPoints.Find(i => i.key == lastCheckPointKey);
        checkpoint.TurnItOn();
        return checkpoint.transform.position;
    }

    public bool HasCheckPoint()
    {
        return lastCheckPointKey > 0;
    }
}
