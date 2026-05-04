using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    public int key = 1;

    private bool _activeCheckpoint = false;

    private void OnTriggerEnter(Collider other)
    {
        Player P = other.GetComponent<Player>();

        if (!_activeCheckpoint && P != null)
        {
            CheckCheckpoint(P.health.currentLife);
        }
    }

    private void CheckCheckpoint(int playerHealth)
    {
        TurnItOn();
        SaveManager.Instance.SaveCheckpoint(key, playerHealth);

        if (SaveManager.Instance.saveSetup.lastCheckpoint >= key) return;

        _activeCheckpoint = true;
        CheckPointManager.Instance.SaveCheckPoint(key);
    }



    public void TurnItOn()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.white);
    }
    private void TurnItOff()
    {
        meshRenderer.material.SetColor("_EmissionColor", Color.black);
    }
}
