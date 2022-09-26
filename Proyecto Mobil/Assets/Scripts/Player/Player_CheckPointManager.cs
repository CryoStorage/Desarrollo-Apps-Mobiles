using Unity.VisualScripting;
using UnityEngine;
public class Player_CheckPointManager : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 1f, 0);
    [HideInInspector] public Vector3 currentCheckPoint;

    private void Update()
    {
        Debug.DrawLine(transform.position,currentCheckPoint,Color.magenta);
        // Debug.DrawLine(transform.position,currentCheckPoint,Color.cyan);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ink")) return;
        currentCheckPoint = SetSpawn(other.transform.position);
    }
    private Vector3 SetSpawn(Vector3 inkPuddle)
    {
        Vector3 respawnPoint =inkPuddle + offset; ;
        return respawnPoint;
    }

}
