using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus.Interaction;
using Normal.Realtime;

public class RealtimeCastSpell : MonoBehaviour
{
    [SerializeField] OVRInput.Button button = OVRInput.Button.PrimaryIndexTrigger;
    [SerializeField] OVRInput.Controller controller = OVRInput.Controller.RTouch;

    [SerializeField] Transform _wandTip;
    [SerializeField] string _spellProjectileName;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isGrabbed = GetComponentInChildren<Grabbable>().SelectingPointsCount > 0;
        if (OVRInput.GetDown(button, controller) && isGrabbed)
        {
            SpawnProjectile();
        }
    }

    private void SpawnProjectile()
    {
        Realtime.Instantiate(_spellProjectileName, _wandTip.position, _wandTip.rotation, Realtime.InstantiateOptions.defaults);
    }
}
