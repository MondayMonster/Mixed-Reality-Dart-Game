using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DartboardColliderHandler_XRA4 : MonoBehaviour
{
    [SerializeField] private int dartValue = -1;
    [SerializeField] private bool isOuterRim = false;

    [SerializeField] private AudioSource _audioSource;

    private bool dartCollided = false;

    // Start is called before the first frame update
    void Start()
    {
        if (dartValue == -1)
        {
            Debug.LogError("Dart value should not be -1.");
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit dartboard");
        if (other.CompareTag("DartTip"))
        {
            if (isOuterRim)
            {
                Debug.Log("Collided with outer board!");
            } else
            {
                Debug.Log($"Collided with {dartValue} space!");
            }

            _audioSource.Play();
            if (ScoreManager_XRA4.Instance.timerStarted)
            {
                DartReference_XRA4 dartRef = other.GetComponent<DartReference_XRA4>();

                if (other.transform.parent != this.transform && !dartRef.isAttachedToBoard)
                {
                    //other.GetComponentInParent<DartBehavior_XRA4>().SetHitDartBoard();
                    //other.gameObject.GetComponentInParent<DartBehavior_XRA4>().SetHitDartBoard();
                    dartRef._dartBehaviorRef.SetHitDartBoard();
                    dartRef.isAttachedToBoard = true;
                    other.gameObject.GetComponentInParent<Rigidbody>().useGravity = false;
                    other.gameObject.GetComponentInParent<Rigidbody>().isKinematic = true;
                    other.transform.SetParent(this.transform);
                    StartCoroutine(StartRespawnCounter());

                    if (!isOuterRim)
                    {
                        Debug.Log($"Adding {dartValue} to score");
                        ScoreManager_XRA4.Instance.UpdateCurrentScore(dartValue);
                    }
                }
                
            }
            
        }
    }

    private IEnumerator StartRespawnCounter()
    {
        yield return new WaitForSeconds(5f);
        DartSpawner_XRA4.activeDartCount--;
    }
}
