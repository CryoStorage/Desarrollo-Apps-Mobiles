using UnityEngine;
public class Player_CheckPointManager : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 1f, 0);
    public Vector3 currentCheckPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ink"))
        {
            currentCheckPoint = new Vector3(0f,0f,0f);
            currentCheckPoint = SetSpawn(other.transform.position);
            Debug.Log("Checkpoint Updated: " + currentCheckPoint);
        }
    }
    private Vector3 SetSpawn(Vector3 inkPuddle)
    {
        Vector3 result = inkPuddle + offset;
        return result;
    }
}
