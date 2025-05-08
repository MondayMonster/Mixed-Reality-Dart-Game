using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomSpatialAnchor_XRA4 : MonoBehaviour
{
    [SerializeField] private string anchorName = "dartboard";
    List<OVRSpatialAnchor.UnboundAnchor> _unboundAnchors = new();


    // Start is called before the first frame update
    void Start()
    {
        LoadBoardAnchor();
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

    private void SaveAnchorToPlayerPrefs(string uuid)
    {
        PlayerPrefs.SetString(anchorName, uuid);
        Debug.Log(anchorName + " save with " + uuid);
    }

    public void LoadBoardAnchor()
    {
        if (PlayerPrefs.HasKey(anchorName))
        {
            string uuidString = PlayerPrefs.GetString(anchorName);
            var uuids = new Guid[1];
            uuids[0] = new Guid(uuidString);
            LoadAnchorsByUuid(uuids);
        } else if (anchorName == "dartboard")
        {
            this.transform.position = new Vector3(0f, 1.5f, 0.5f);
        }
    }

    private async void LoadAnchorsByUuid(IEnumerable<Guid> uuids)
    {
        var result = await OVRSpatialAnchor.LoadUnboundAnchorsAsync(uuids, _unboundAnchors);

        if (result.Success)
        {
            Debug.Log("Anchors loaded successfully!");
            foreach (var unboundAnchor in result.Value)
            {
                unboundAnchor.LocalizeAsync().ContinueWith((success, anchor) =>
                {
                    if (success)
                    {
                        var spatialAnchor = transform.gameObject.AddComponent<OVRSpatialAnchor>();
                        Debug.Log("anchor loaded " + unboundAnchor.Uuid);

                        unboundAnchor.BindTo(spatialAnchor);

                    }
                    else
                    {
                        Debug.LogError($"Localization failed for anchor {unboundAnchor.Uuid}");
                    }
                }, unboundAnchor);
            }
        }
        else
        {
            Debug.LogError($"Load failed with error {result.Status}");
        }
    }

    public void DeleteAnchor()
    {
        if (GetComponent<OVRSpatialAnchor>())
        {
            var anchor = GetComponent<OVRSpatialAnchor>();
            DeleteAnchorAsync(anchor);
            Destroy(GetComponent<OVRSpatialAnchor>());
        }
    }

    private async void DeleteAnchorAsync(OVRSpatialAnchor _spatialAnchor)
    {
        var result = await _spatialAnchor.EraseAnchorAsync();

        if (result.Success)
        {
            Debug.Log("Successfuly erased anchor.");
            PlayerPrefs.DeleteKey(anchorName);
        }
        else
        {
            Debug.LogError($"Failed to erase anchor {anchorName}");
        }
    }
    
}
