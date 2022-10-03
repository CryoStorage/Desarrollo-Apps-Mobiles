using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Win : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Goal")) return;
        Win();
    }

    void Win()
    {
        sceneManager.GetComponent<SceneChanger>().WinScreen();

    }
}
