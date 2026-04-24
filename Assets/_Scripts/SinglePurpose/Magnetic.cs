using UnityEngine;

public class Magnetic : MonoBehaviour
{
    public Transform objectToMove;
    public float dist = 0.2f;
    public float itemSpeed = 3f;
    public string tagToFollow = "Player";

    [HideInInspector] public Transform targetObj;
    [HideInInspector] public bool canFollow = false;

    void Update()
    {
        if (!canFollow) return;
        if ((objectToMove == null) || (targetObj == null)) return;

        Vector3 tarPos = targetObj.position + Vector3.up;
        if ((Vector3.Distance(objectToMove.position, tarPos) > dist))
        {
            objectToMove.position = Vector3.MoveTowards(objectToMove.position, tarPos, Time.deltaTime * itemSpeed);
            itemSpeed += 1f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagToFollow))
        {
            targetObj = other.transform;
            canFollow = true;
        }
    }
}
