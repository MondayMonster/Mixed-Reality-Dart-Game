using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Meta.XR.MRUtilityKit;

public class BuildSurfaceRealtime : MonoBehaviour
{

    private NavMeshSurface _surface;

    private void Awake()
    {
        _surface = GetComponent<NavMeshSurface>();
        

        MRUK.Instance.RegisterSceneLoadedCallback(BakeSurface);
        
    }

    private void BakeSurface()
    {
        StartCoroutine(BuildNavMesh());
    }

    private IEnumerator BuildNavMesh()
    {
        yield return new WaitForEndOfFrame();
        _surface.BuildNavMesh();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
