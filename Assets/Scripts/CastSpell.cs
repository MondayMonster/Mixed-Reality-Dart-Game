using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    [SerializeField] OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
    [SerializeField] OVRInput.Controller controller = OVRInput.Controller.RTouch;

    [SerializeField] Transform _wandTip;
    [SerializeField] GameObject _spellProjectile;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool isGrabbed = GetComponentInChildren<Grabbable>().SelectingPointsCount > 0;
        if (OVRInput.GetDown(button, controller) && isGrabbed) {
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Instantiate(_spellProjectile, _wandTip.position, _wandTip.rotation);
    }
}
