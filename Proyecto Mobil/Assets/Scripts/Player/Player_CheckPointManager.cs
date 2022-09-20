using System;
using UnityEngine;
public class Player_CheckPointManager : MonoBehaviour
{
    private Vector3 offset = new Vector3(0, 1f, 0);
    [HideInInspector] public Vector3 currentCheckPoint;

    private void Update()
    {
        Vector3 difference = currentCheckPoint - transform.position;
        Debug.DrawRay(transform.position,difference,Color.magenta);
        Debug.DrawLine(transform.position,difference,Color.cyan);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ink")) return;
        currentCheckPoint = SetSpawn(other.transform.position);
    }
    private Vector3 SetSpawn(Vector3 inkPuddle)
    {
        Vector3 respawnPoint =inkPuddle + offset;
        Vector3 result = respawnPoint - transform.position;
        return result;
    }
}
