using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleEffect : MonoBehaviour
{
    [SerializeField] private GameObject reticle;
    [SerializeField] private float spellRange = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray reticleRay = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(reticleRay, out hit, spellRange))
        {
            Debug.DrawLine(reticleRay.origin, hit.point);
            Vector3 hitPosition = hit.point + hit.normal * 0.01f;
            var direction = transform.position - hit.point;
            //var orientation = Quaternion.LookRotation(hit.normal, Vector3.up);
            var orientation = Quaternion.FromToRotation(Vector3.forward, direction);
            reticle.transform.SetPositionAndRotation(hitPosition, orientation);
            reticle.SetActive(true);
        } else
        {
            reticle.SetActive(false);
        }
    }
}
