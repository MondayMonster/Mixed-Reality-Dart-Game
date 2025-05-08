using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CustomSpatialAnchor : MonoBehaviour
{
    public string anchorName = "frame";
    List<OVRSpatialAnchor.UnboundAnchor> _unboundAnchors = new();

    // Start is called before the first frame update
    void Start()
    {
        LoadAnchor();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddAnchor()
    {
        OVRSpatialAnchor _anchor = this.transform.gameObject.AddComponent<OVRSpatialAnchor>();

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
        }
        else
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
                        Debug.LogError(result.Status);
                    }
                }, unboundAnchor);
            }
        }
        else
        {
            Debug.LogError(result.Status);
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

    public void EraseAnchor()
    {
        if (GetComponent<OVRSpatialAnchor>()) {
            var anchor = GetComponent<OVRSpatialAnchor>();
            EraseAnchorAsync(anchor);
        }
        
    }

    public async void EraseAnchorAsync(OVRSpatialAnchor _spatialAnchor)
    {
        var result = await _spatialAnchor.EraseAnchorAsync();

        if (result.Success)
        {
            Debug.Log("Successfuly erased anchor.");
            PlayerPrefs.DeleteKey(anchorName);
        } else
        {
            Debug.LogError($"Failed to erase anchor {anchorName}");
        }
    }
}
