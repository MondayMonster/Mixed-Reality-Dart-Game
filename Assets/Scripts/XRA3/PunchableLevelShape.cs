using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchableLevelShape : MonoBehaviour
{
    [SerializeField] private float hoverSpeed = 0.2f;
    [SerializeField] private float hoverAmplitude = 0.3f;

    [SerializeField] private float explosionForce = 10f;
    [SerializeField] private float explosionRadius = 3f;

    [SerializeField] private GameObject brokenIsoShape;

    [SerializeField] private bool reloadLevel = false;
    [SerializeField] private string sceneName;

    private Material parentMaterial;

    private AudioSource _audioSource;
    private float _offset;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _offset = this.transform.position.y;
        parentMaterial = GetComponent<MeshRenderer>().material;
        ChangeBrokenIsoShapeColor();
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3(this.transform.position.x, _offset + Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude, this.transform.position.z); 
    }

    public void ChangeBrokenIsoShapeColor()
    {
        foreach (MeshRenderer rend in brokenIsoShape.GetComponentsInChildren<MeshRenderer>())
        {
            rend.material = parentMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LeftController") || other.CompareTag("RightController"))
        {
            _audioSource.Play();
            GameObject mediumVersion = Instantiate(brokenIsoShape, this.transform.position, this.transform.rotation);

            foreach(Rigidbody rb in mediumVersion.GetComponentsInChildren<Rigidbody>())
            {
                rb.useGravity = true;
                rb.AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
            }

            Destroy(mediumVersion, 0.3f);
            
            if (reloadLevel)
            {
                XRA3SceneManager.Instance.RestartLevel();
            } else
            {
                XRA3SceneManager.Instance.LoadScene(sceneName);
            }
            this.gameObject.SetActive(false);
        }
    }
}
