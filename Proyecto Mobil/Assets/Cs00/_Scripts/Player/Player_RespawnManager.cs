using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_RespawnManager : MonoBehaviour
{
    private Player_Move _playerMove;
    private GameObject _adsManager;
    public static float Ink = 100f;
    private bool _inInk;
    [SerializeField] float inkLossRate;
    [SerializeField] float inkRechargeRate;
    [SerializeField] Slider inkSlider;
    
    private Vector3 _offset = new Vector3(0, 1f, 0);
    [HideInInspector] public Vector3 currentCheckPoint;

    private int _lives;
    private int _maxLives = 3;
    private Vector3 _initialCheckPoint;
    private bool _gotInitial;
    
    private void Start()
    {
        _lives = _maxLives;
        Prepare();
    }
    void Update()
    {
        Ink = Mathf.Clamp(Ink, 0, 100f);
        ChangeInk();
        UpdateSlider();
        DebugKill();
    }
    private void LateUpdate()
    {
        CheckInk();
    }
    private void UpdateSlider()
    {
        inkSlider.value = Ink;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Ink")) return;
        _inInk = true;
        if (!_gotInitial)
        {
            _initialCheckPoint = SetSpawn(other.transform.position);
            _gotInitial = true;
        }
        if (_gotInitial)
        {
            currentCheckPoint = SetSpawn(other.transform.position);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ink"))
        {
            _inInk = false;
        }
    }
    
    private Vector3 SetSpawn(Vector3 inkPuddle)
    {
        Vector3 respawnPoint =inkPuddle + _offset;
        return respawnPoint;
    }
    private void CheckInk()
    {
        if (Ink > 0) return;
        switch (_lives)
        {
            case int n when(n > 0):
                _playerMove.Respawn(currentCheckPoint);
                _lives--;
                break;
            case int n when(n <= 0):
                _playerMove.Respawn(_initialCheckPoint);
                _lives = _maxLives;
                break;
        }
    }
    private void ChangeInk()
    {
        switch (_inInk)
        {
            case true:
                Ink += inkRechargeRate * Time.fixedDeltaTime;
            break;
            
            case false:
                Ink -= inkLossRate * Time.fixedDeltaTime;
            break;
        }
    }
    private void Prepare()
    {
        if (_playerMove != null) return;
        try
        {
            _playerMove = GetComponent<Player_Move>();
        }
        catch{ Debug.LogWarning("Could not find Player_Move");}

        if (_adsManager != null) return;
        try
        {
            _adsManager = GameObject.Find("AdsManager");
        }
        catch {Debug.LogWarning("Could not find AdsManager"); }   
    }
    private void DebugKill()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Ink = 0;
        }
    }
}

