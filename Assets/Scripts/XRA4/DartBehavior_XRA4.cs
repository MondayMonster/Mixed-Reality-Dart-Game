using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartBehavior_XRA4 : MonoBehaviour
{
    [SerializeField] private float hapticDuration = 0.25f;
    [SerializeField] private float expireDuration = 5f;
    [SerializeField] private float dartForce = 25f;
    [SerializeField] private OVRInput.Controller rightController;
    
    private AudioSource _audioSource;
    private Rigidbody _rb;

    public bool hitDartBoard = false;
    private bool dartThrown = false;

    private Coroutine timedDestroyCoroutine;

    //private DartSpawner_XRA4 _dartSpawnerRef;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        //_dartSpawnerRef = GameObject.FindGameObjectWithTag("DartSpawner").GetComponent<DartSpawner_XRA4>();
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dartThrown && !hitDartBoard)
        {
            TriggerDartExpire(true);
        } else if (dartThrown && hitDartBoard)
        {
            TriggerDartExpire(false);
        }
    }

    private void TriggerDartExpire(bool status)
    {
        if (status)
        {
            if (timedDestroyCoroutine == null)
            {
                Debug.Log("Starting destroy timer for dart");
                timedDestroyCoroutine = StartCoroutine(TimedDestroy());
            }
        } else
        {
            if (timedDestroyCoroutine != null)
            {
                Debug.Log("Stopping destroy timer");
                StopCoroutine(timedDestroyCoroutine);
                timedDestroyCoroutine = null;
            }
        }  
    }

    private IEnumerator TimedDestroy()
    {
        yield return new WaitForSeconds(expireDuration);
        DartSpawner_XRA4.activeDartCount--;
        Destroy(this.gameObject);
    }

    public void SetHitDartBoard()
    {
        hitDartBoard = true;
    }

    public void OnDartGrabbed()
    {
        TriggerHapticFeedback(1f);
        _rb.useGravity = true;
        _rb.isKinematic = false;
    }

    public void OnDartReleased()
    {
        _audioSource.Play();
        this.transform.parent = null;
        Vector3 controllerVelocity = OVRInput.GetLocalControllerVelocity(rightController);
        _rb.AddForce(controllerVelocity * dartForce, ForceMode.Impulse);
        dartThrown = true;
    }

    public void TriggerHapticFeedback(float amplitude)
    {
        StartCoroutine(ApplyHaptics(amplitude, OVRInput.Controller.RTouch));
    }

    private IEnumerator ApplyHaptics(float amplitude, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(1, amplitude, controller);
        yield return new WaitForSeconds(hapticDuration);
        OVRInput.SetControllerVibration(0, 0, controller);
    }
}
