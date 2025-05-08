using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HapticFeedback : MonoBehaviour
{
    public static HapticFeedback Instance;

    public float hapticDuration = 0.1f;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip controllerTouchSFX;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
        }
        else
        {
            throw new System.Exception("Can't be two Haptic Feedback!");
        }
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void TriggerHapticFeedback(float amplitude, string controller)
    {
        if (controller == "LeftController")
        {
            StartCoroutine(ApplyHaptics(amplitude, OVRInput.Controller.LTouch));
        } else if (controller == "RightController")
        {
            StartCoroutine(ApplyHaptics(amplitude, OVRInput.Controller.RTouch));
        } else
        {
            throw new System.Exception("unknown controller string");
        }
    }

    public void TriggerControllerTouchFeedback(float amplitude)
    {
        _audioSource.PlayOneShot(controllerTouchSFX);
        TriggerHapticFeedback(amplitude, "LeftController");
        TriggerHapticFeedback(amplitude, "RightController");
    }

    private IEnumerator ApplyHaptics(float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(1, amplitude, controller);
        yield return new WaitForSeconds(hapticDuration);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
