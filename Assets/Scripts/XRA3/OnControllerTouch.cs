using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnControllerTouch : MonoBehaviour
{

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftController") || other.gameObject.layer == LayerMask.NameToLayer("Controller"))
        {
            HapticFeedback.Instance.TriggerControllerTouchFeedback(1f);
        }
    }
}
