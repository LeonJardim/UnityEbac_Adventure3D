using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public int key = 1;

    private bool _activeCheckpoint = false;
    private string _checkcpointKey = "CheckpointKey";

    private void OnTriggerEnter(Collider other)
    {
        if (!_activeCheckpoint && other.CompareTag("Player"))
        {
            CheckCheckpoint();
        }
    }

    private void CheckCheckpoint()
    {
        TurnItOn();
        SaveCheckPoint();
    }

    private void SaveCheckPoint()
    {
        if (PlayerPrefs.GetInt(_checkcpointKey, 0) > key) return;

        PlayerPrefs.SetInt(_checkcpointKey, key);
        _activeCheckpoint = true;
        CheckPointManager.Instance.SaveCheckPoint(key);
    }

    private void TurnItOn()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.white);
    }
    private void TurnItOff()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
    }
}
