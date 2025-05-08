using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRA3BlockCollider : MonoBehaviour
{
    [SerializeField] private AudioClip playerHitSFX;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BlockCollider") || other.CompareTag("LeftController") || other.CompareTag("RightController") || other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Hit Block");
            AudioSource.PlayClipAtPoint(playerHitSFX, XRA3LevelManager.Instance.SFXPlayerLocation.position);
            HapticFeedback.Instance.TriggerHapticFeedback(1, "LeftController");
            HapticFeedback.Instance.TriggerHapticFeedback(1, "RightController");
            XRA3LevelManager.Instance.UpdateScoreFromBlockHit();
            Destroy(this.gameObject, 0.2f);
        }
    }
}
