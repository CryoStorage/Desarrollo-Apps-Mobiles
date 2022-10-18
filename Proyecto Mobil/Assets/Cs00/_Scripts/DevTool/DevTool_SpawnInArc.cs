using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DevTool_SpawnInArc : MonoBehaviour
{
    [Header("Arc Parameters")]
    [Tooltip("End of the arc")]
    [SerializeField] private GameObject endPos;
    [Tooltip("Height of the arc")]
    [SerializeField] private float arcHeight;
    [Tooltip("Density of objects along the arc")]
    [SerializeField] private int density;

    [Header("Prefabs")] 
    [Tooltip("Used to visualize the current arc")]
    [SerializeField] private GameObject marker;
    [Tooltip("Object that will be instantiated")]
    [SerializeField] private GameObject toInstantiate;
    private void Start()
    {
        Visualize();
    }
    private void Visualize()
    {
        SetNewPositions();
    }

    private List<GameObject> CreateObjects(int amount, GameObject obj)
    {
        List<GameObject> result = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            result.Add(Instantiate(obj));
        }
        return result;
    }

    private List<Vector3> SaveArcPositions()
    {
        List<Vector3> arcPositions = new List<Vector3>();

        for (int i = 0; i < density; i++)
        {
            // Compute the next position, with arc added in
            float x0 = transform.position.x;
            float x1 = endPos.transform.position.x;
            float dist = x1 - x0;
            float nextX = Mathf.MoveTowards(transform.position.x, x1,  i);
            float baseY = Mathf.Lerp(transform.position.y, endPos.transform.position.y, (nextX - x0) / dist);
            float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
            arcPositions.Add(new Vector3(nextX, baseY + arc, transform.position.z));
        }
        return arcPositions;
    }

    void SetNewPositions()
    {
        List<GameObject> objects = CreateObjects(density, marker);
        List<Vector3> arcPositions = SaveArcPositions();
        for (int i = 0; i < density; i++)
        {
            objects[i].transform.position = arcPositions[i];
            
        }
    }
}
