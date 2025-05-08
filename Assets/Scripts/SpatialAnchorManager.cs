using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;  

public class SpatialAnchorManager : MonoBehaviour
{
    public OVRInput.Controller _controller;
    public OVRInput.Button _addButton;
    public string anchorName = "frame";
    public GameObject anchorPrefab;

    List<OVRSpatialAnchor.UnboundAnchor> _unboundAnchors = new();

    // Start is called before the first frame update
    void Start()
    {
        LoadAnchor();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(_addButton, _controller))
        {
            AddAnchor();
        }
    }

    public void AddAnchor()
    {
        Vector3 controllerPos = OVRInput.GetLocalControllerPosition(_controller);
        Quaternion controllerRotation = OVRInput.GetLocalControllerRotation(_controller);
        GameObject spatialPrefab = Instantiate(anchorPrefab, controllerPos, controllerRotation);

        OVRSpatialAnchor _anchor = spatialPrefab.AddComponent<OVRSpatialAnchor>();

        StartCoroutine(AnchorAdded(_anchor));
    }

    IEnumerator AnchorAdded(OVRSpatialAnchor anchor)
    {
        yield return new WaitUntil(() => anchor.Created && anchor.Localized);

        Debug.Log($"Created anchor {anchor.Uuid}");

        SaveAnchor(anchor);
    }

    public async void SaveAnchor(OVRSpatialAnchor anchor)
    {
        var result = await anchor.SaveAnchorAsync();

        if (result.Success)
        {
            Debug.Log($"Anchor {anchor.Uuid} saved successfully");
            SaveAnchorToPlayerPrefs(anchor.Uuid.ToString());
        } else
        {
            Debug.LogError($"Anchor {anchor.Uuid} failed to save with error {result.Status}");
        }
    }

    public async void LoadAnchorsByUuid(IEnumerable<Guid> uuids)
    {
        var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(uuids, _unboundAnchors);

        if (result.Success)
        {
            foreach (var unboundAnchor in result.Value)
            {
                unboundAnchor.LocalizeAsync().ContinueWith((success, anchor) =>
                {
                    if (success)
                    {
                        var spatialAnchor = transform.gameObject.AddComponent<OVRSpatialAnchor>();

                        unboundAnchor.BindTo(spatialAnchor);

                    }
                    else
                    {
                        Debug.LogError($"anchor {unboundAnchor.Uuid} FAILED");
                    }
                }, unboundAnchor);
            }
        } else
        {
            Debug.LogError(result.Value);
        }
    }

    public void LoadAnchor()
    {
        if (PlayerPrefs.HasKey(anchorName))
        {
            string uuidString = PlayerPrefs.GetString(anchorName);
            var uuids = new Guid[1];
            uuids[0] = new Guid(uuidString);
            LoadAnchorsByUuid(uuids);
        }
    }
 
    public void SaveAnchorToPlayerPrefs(string uuid)
    {
        PlayerPrefs.SetString(anchorName, uuid);
        Debug.Log(anchorName + " save with " + uuid);
    }
}
