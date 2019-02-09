using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIControl : MonoBehaviour
{

    [SerializeField]
    private GameObject _joysticks;
    [SerializeField]
    private FixedJoystick _moveJoystick;
    public FixedJoystick MoveJoystick { get { return _moveJoystick; } }
    [SerializeField]
    private FixedJoystick _aimJoystick;
    public FixedJoystick AimJoystick { get { return _aimJoystick; } }
    [SerializeField]
    private Text _messageText;

    private void Awake()
    {
        ClearMessageText();
    }
    // Use this for initialization
    void Start()
    {
        
        //only use touch joystick on android
#if UNITY_ANDROID
        _joysticks.SetActive(true);
#else
        joysticks.SetActive(false);
#endif

    }

    public void SetMessageText(string text)
    {
        _messageText.text = text;
    }

    public void SetMessageText(string text, float disableTime)
    {
        StopAllCoroutines();
        _messageText.text = text;
        StartCoroutine(DelayClearMessage(disableTime));
    }

    public void ClearMessageText()
    {
        _messageText.text = "";
    }
    IEnumerator DelayClearMessage(float duration)
    {
        float timeToDisable = Time.time + duration;
        while(timeToDisable > Time.time)
        {
            yield return null;
        }
        ClearMessageText();

    }
}
