using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRA3ShapeBehavior : MonoBehaviour
{
    [SerializeField] private bool reactsToRight = true;
    [Header("Shape Fragment Types")]
    [SerializeField] private GameObject smallPieces;
    [SerializeField] private GameObject mediumPieces;
    [SerializeField] private GameObject largePieces;

    [SerializeField] private float moveSpeed = 15f;

    [Header("Controller")]
    [SerializeField] private OVRInput.Controller leftController;
    [SerializeField] private OVRInput.Controller rightController;

    [SerializeField] private float explosionForce = 1f;
    [SerializeField] private float explosionRadius = 2f;
    private Material parentMaterial;

    [Space(10)]
    [Header("SFX")]
    [SerializeField] private AudioClip correctControllerHitSFX;
    [SerializeField] private AudioClip incorrectControllerHitSFX;

    [Header("Incorrect Hit Rebound Components")]
    [SerializeField] private float reboundHeight = 2f;
    [SerializeField] private float reboundDuration = 2f;
    [SerializeField] private float reboundLength = 1.5f;

    // Start is called before the first frame update
    void Awake()
    {
        parentMaterial = GetComponent<MeshRenderer>().material;
        ChangeBrokenIsoShapesColor(smallPieces);
        ChangeBrokenIsoShapesColor(mediumPieces);
        ChangeBrokenIsoShapesColor(largePieces);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += moveSpeed * Time.deltaTime * transform.forward * -1;
    }

    public void ChangeBrokenIsoShapesColor(GameObject brokenPieceObject)
    {
        foreach (MeshRenderer rend in brokenPieceObject.GetComponentsInChildren<MeshRenderer>())
        {
            rend.material = parentMaterial;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (XRA3LevelManager.Instance.gameOver) return;

        if (other.CompareTag("RightController"))
        {
            Vector3 controllerVelocity = OVRInput.GetLocalControllerVelocity(rightController);
            float magnitude = controllerVelocity.magnitude;
            Debug.Log($"magnitude of right: {magnitude}");

            if (reactsToRight)
            {
                HapticFeedback.Instance.TriggerHapticFeedback(Mathf.Min(magnitude, 1), "RightController");
                HandleCorrectControllerHit(magnitude);
                
            } else
            {
                HapticFeedback.Instance.TriggerHapticFeedback(1, "RightController");
                HandleInCorrectControllerHit();
            }
        } else if (other.CompareTag("LeftController"))
        {
            Vector3 controllerVelocity = OVRInput.GetLocalControllerVelocity(leftController);
            float magnitude = controllerVelocity.magnitude;
            Debug.Log($"magnitude of left: {magnitude}");

            if (!reactsToRight)
            {
                HapticFeedback.Instance.TriggerHapticFeedback(Mathf.Min(magnitude, 1), "LeftController");
                HandleCorrectControllerHit(magnitude);
            }
            else
            {
                HapticFeedback.Instance.TriggerHapticFeedback(1, "LeftController");
                HandleInCorrectControllerHit();
            }
        }
    }

    private void HandleCorrectControllerHit(float strength)
    {
        if (strength > 0.5f && strength <= 1.0f)
        {
            GameObject smallVersion = Instantiate(smallPieces, this.transform.position, this.transform.rotation);
            Debug.Log("created small version");
            CreateExplosion(smallVersion);
            Destroy(smallVersion, 0.3f);

            AudioSource.PlayClipAtPoint(correctControllerHitSFX, XRA3LevelManager.Instance.SFXPlayerLocation.position);
            XRA3LevelManager.Instance.UpdateScore(strength, true);
            Destroy(this.gameObject);
        } else if (strength > 1.0f && strength <= 2.0f)
        {
            GameObject mediumVersion = Instantiate(mediumPieces, this.transform.position, this.transform.rotation);
            Debug.Log("created medium version");
            CreateExplosion(mediumVersion);
            Destroy(mediumVersion, 0.3f);

            AudioSource.PlayClipAtPoint(correctControllerHitSFX, XRA3LevelManager.Instance.SFXPlayerLocation.position);
            XRA3LevelManager.Instance.UpdateScore(strength, true);
            Destroy(this.gameObject);
        } else if (strength > 2.0f)
        {
            GameObject largeVersion = Instantiate(largePieces, this.transform.position, this.transform.rotation);
            Debug.Log("created large version");
            CreateExplosion(largeVersion);
            Destroy(largeVersion, 0.3f);

            AudioSource.PlayClipAtPoint(correctControllerHitSFX, XRA3LevelManager.Instance.SFXPlayerLocation.position);
            XRA3LevelManager.Instance.UpdateScore(strength, true);
            Destroy(this.gameObject);
        }
    }

    private void CreateExplosion(GameObject obj)
    {
        foreach (Rigidbody rb in obj.GetComponentsInChildren<Rigidbody>())
        {
            rb.useGravity = true;
            rb.AddExplosionForce(explosionForce, this.transform.position, explosionRadius);
        }
    }

    private void HandleInCorrectControllerHit()
    {
        AudioSource.PlayClipAtPoint(incorrectControllerHitSFX, XRA3LevelManager.Instance.SFXPlayerLocation.position);
        XRA3LevelManager.Instance.UpdateScore(0, false);
        StartCoroutine(IncorrectHitEffect());
    }

    private IEnumerator IncorrectHitEffect()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = startPos + new Vector3(0, reboundHeight, reboundLength);
        float timeElapsed = 0f;

        while (timeElapsed < reboundDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, timeElapsed / reboundDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
        
}
