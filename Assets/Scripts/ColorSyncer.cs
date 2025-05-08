using Normal.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSyncer : RealtimeComponent<ColorSyncModel>
{
    [SerializeField] private Color randomColor;

    [SerializeField] private MeshRenderer[] _meshRenderers;

    public OVRInput.Controller _controller = OVRInput.Controller.RTouch;
    public OVRInput.Button _button = OVRInput.Button.PrimaryIndexTrigger;

    // Start is called before the first frame update
    void Start()
    {
        AssignRandomColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(_button, _controller))
        {
            AssignRandomColor();
        }
    }

    private void AssignRandomColor()
    {
        //Color randColor = new Color(Random.value, Random.value, Random.value);
        randomColor = Random.ColorHSV();

        SetColor();
    }

    private void SetColor()
    {
        model.color = randomColor;
    }

    private void UpdateMeshColor()
    {
        foreach(var renderer in _meshRenderers)
        {
            renderer.material.color = model.color;
        }
    }
}
