using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkUIControl : MonoBehaviour
{

    [SerializeField]
    private GameObject joysticks;
    [SerializeField]
    private FixedJoystick moveJoystick;
    public FixedJoystick MoveJoystick { get { return moveJoystick; } }
    [SerializeField]
    private FixedJoystick aimJoystick;
    public FixedJoystick AimJoystick { get { return aimJoystick; } }
    [SerializeField]
    private Text messageText;

    private void Awake()
    {
        ClearMessageText();
    }
    // Use this for initialization
    void Start()
    {
        
        //only use touch joystick on android
#if UNITY_ANDROID
        joysticks.SetActive(true);
#else
        joysticks.SetActive(false);
#endif

    }

    public void SetMessageText(string text)
    {
        messageText.text = text;
    }

    public void SetMessageText(string text, float disableTime)
    {
        StopAllCoroutines();
        messageText.text = text;
        StartCoroutine(DelayClearMessage(disableTime));
    }

    public void ClearMessageText()
    {
        messageText.text = "";
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
