using UnityEngine;

public class Camera_Move : MonoBehaviour
{

    [SerializeField] GameObject[] cameraPositions;

    [SerializeField] private GameObject player; 
    private float playerY;
    // Update is called once per frame
    void Update()
    {
        playerY = player.transform.position.y;
        ChangeScreen();
    }

    void ChangeScreen()
    {
        switch (playerY)
        {
            case float n when(n < 20.01f):
                transform.position = cameraPositions[0].transform.position;    
            break;
            
            case float n when(n > 20.01f && n < 60.01f):
                transform.position = cameraPositions[1].transform.position;    
            break;
            
            case float n when(n > 60.01f && n < 100.01f):
                transform.position = cameraPositions[2].transform.position;    
            break;
            
            case float n when(n > 100.1f):
                transform.position = cameraPositions[3].transform.position;    
            break;
            
            default:
            transform.position = cameraPositions[0].transform.position;
            break;
        }
    }
}
