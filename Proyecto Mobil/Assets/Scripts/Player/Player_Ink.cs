using UnityEngine;
using UnityEngine.UI;

public class Player_Ink : MonoBehaviour
{
    private Player_Move playerMove;
    public static float ink = 100f;
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
    }

    private void LateUpdate()
    {
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
        switch (inInk)
        {
            case true:
                ink += inkRechargeRate * Time.fixedDeltaTime;
            break;
            
            case false:
                ink -= inkLossRate * Time.fixedDeltaTime;
            break;
        }
    }
    private void CheckInk()
    {
        if (ink > 0) return;
        playerMove.Respawn();
    }
    private void Prepare()
    {
        if (playerMove != null) return;
        try
        {
            playerMove = GetComponent<Player_Move>();
        }
        catch{ Debug.LogWarning("Could not find Player_Move");}
    }
}

