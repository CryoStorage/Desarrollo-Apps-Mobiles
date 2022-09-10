using UnityEngine;
using UnityEngine.UI;

public class Player_Ink : MonoBehaviour
{
    private Player_Move playerMove;
    [HideInInspector]static public float ink = 100f;
    private bool inInk;
    [SerializeField] float inkLossRate;
    [SerializeField] float inkRechargeRate;
    [SerializeField] Slider inkSlider;

    private void Start()
    {
        Prepare();
        
    }
    void Update()
    {
        ink = Mathf.Clamp(ink, 0, 100f);
        ChangeInk();
        UpdateSlider();
        CheckInk();
    }
    private void UpdateSlider()
    {
        inkSlider.value = ink;

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ink"))
        {
            inInk = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ink"))
        {
            inInk = false;
        }
    }
    private void ChangeInk()
    {
        if (inInk)
        {
            ink += inkRechargeRate * Time.fixedDeltaTime;
        }
        if (!inInk)
        {
            ink -= inkLossRate * Time.fixedDeltaTime;
        }
    }
    private void CheckInk()
    {
        if (ink > 0) return;
        playerMove.Respawn();
        
    }
    private void Prepare()
    {
        if (playerMove == null)
        {
            try
            {
                playerMove = GetComponent<Player_Move>();
            }
            catch{ Debug.LogWarning("Could not find Player_Move");}
        }
    }
}

