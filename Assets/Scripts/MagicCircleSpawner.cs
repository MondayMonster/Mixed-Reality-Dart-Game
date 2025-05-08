using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class MagicCircleSpawner : MonoBehaviour
{
    [SerializeField] GameObject magicCirclePrefab;

    [SerializeField] MRUKAnchor.SceneLabels labels;
    [SerializeField] MRUK.SurfaceType surfaceType;


    // Start is called before the first frame update
    void Start()
    {
        MRUK.Instance.RegisterSceneLoadedCallback(SpawnMagicCircle);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnMagicCircle()
    {
        Vector3 randomPos = Vector3.zero;
        Vector3 randomNormal = Vector3.zero;
        MRUKRoom room = MRUK.Instance.GetCurrentRoom();

        bool isFound = room.GenerateRandomPositionOnSurface(surfaceType, 1, LabelFilter.Included(labels),
            out randomPos, out randomNormal);

        if (isFound)
        {
            Instantiate(magicCirclePrefab, randomPos, Quaternion.identity);
        }
    }
}
