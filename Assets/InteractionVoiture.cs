using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionVoiture : MonoBehaviour
{
    [Header("Interaction")]
    public float distanceInteraction = 3f;

    [Header("Joueur")]
    public GameObject player;
    public Camera cameraJoueur;
    public MonoBehaviour walkScript;
    public MonoBehaviour mouseLookScript;

    [Header("Voiture")]
    public Camera cameraVoiture;
    public MonoBehaviour carController;

    [Header("Sortie")]
    public Transform exitLeft;
    public Transform exitRight;
    public Transform exitFront;
    public Transform exitBack;

    public float rayonJoueur = 0.4f;
    public float hauteurJoueur = 2f;

    private bool dansLaVoiture = false;
    private Renderer[] playerRenderers;
    private Collider[] playerColliders;
    private bool[] playerRendererStates;
    private bool[] playerColliderStates;

    void Start()
    {
        if (cameraJoueur == null)
        {
            Debug.LogWarning("cameraJoueur n'est pas assignée sur InteractionVoiture.");
        }

        cameraVoiture.gameObject.SetActive(false);
        carController.enabled = false;

        if (player != null)
        {
            playerRenderers = player.GetComponentsInChildren<Renderer>(true);
            playerColliders = player.GetComponentsInChildren<Collider>(true);
            playerRendererStates = new bool[playerRenderers.Length];
            playerColliderStates = new bool[playerColliders.Length];
        }
    }

    void Update()
    {
        // Si le joueur est dans la voiture
        if (dansLaVoiture)
        {
            if (Keyboard.current.eKey.wasPressedThisFrame)
            {
                Debug.Log(">>> CIBLE TOUCHEE !");
                SortirVoiture();
            }

            return;
        }

        // Détection du véhicule
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distanceInteraction))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red);

            if (hit.collider.CompareTag("C15"))
            {
                if (Keyboard.current.eKey.wasPressedThisFrame)
                {
                    EntrerVoiture();
                }
            }
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * distanceInteraction, Color.green);
        }
    }

    void EntrerVoiture()
    {
        dansLaVoiture = true;

        cameraJoueur.gameObject.SetActive(false);
        cameraVoiture.gameObject.SetActive(true);

        walkScript.enabled = false;
        mouseLookScript.enabled = false;

        SavePlayerState();
        SetPlayerVisible(false);
        SetPlayerCollisions(false);

        carController.enabled = true;
    }

    void SortirVoiture()
    {
        Transform pointLibre = TrouverPointLibre();

        if (pointLibre == null)
        {
            Debug.Log("Aucun point de sortie disponible.");
            return;
        }

        dansLaVoiture = false;

        carController.enabled = false;

        player.transform.position = pointLibre.position;
        player.transform.rotation = Quaternion.Euler(0, pointLibre.eulerAngles.y, 0);

        RestorePlayerState();

        walkScript.enabled = true;
        mouseLookScript.enabled = true;

        cameraVoiture.gameObject.SetActive(false);
        cameraJoueur.gameObject.SetActive(true);
    }

    Transform TrouverPointLibre()
    {
        Transform[] points =
        {
            exitLeft,
            exitRight,
            exitFront,
            exitBack
        };

        foreach (Transform point in points)
        {
            Vector3 bas = point.position + Vector3.up * rayonJoueur;
            Vector3 haut = point.position + Vector3.up * (hauteurJoueur - rayonJoueur);

            bool occupe = Physics.CheckCapsule(bas, haut, rayonJoueur);

            if (!occupe)
            {
                return point;
            }
        }

        return null;
    }

    void SavePlayerState()
    {
        if (playerRenderers != null)
        {
            for (int i = 0; i < playerRenderers.Length; i++)
            {
                playerRendererStates[i] = playerRenderers[i].enabled;
            }
        }

        if (playerColliders != null)
        {
            for (int i = 0; i < playerColliders.Length; i++)
            {
                playerColliderStates[i] = playerColliders[i].enabled;
            }
        }
    }

    void RestorePlayerState()
    {
        if (playerRenderers != null && playerRendererStates != null)
        {
            for (int i = 0; i < playerRenderers.Length; i++)
            {
                playerRenderers[i].enabled = playerRendererStates[i];
            }
        }

        if (playerColliders != null && playerColliderStates != null)
        {
            for (int i = 0; i < playerColliders.Length; i++)
            {
                playerColliders[i].enabled = playerColliderStates[i];
            }
        }
    }

    void SetPlayerVisible(bool visible)
    {
        if (playerRenderers == null) return;

        foreach (Renderer renderer in playerRenderers)
        {
            renderer.enabled = visible;
        }
    }

    void SetPlayerCollisions(bool enabled)
    {
        if (playerColliders == null) return;

        foreach (Collider collider in playerColliders)
        {
            collider.enabled = enabled;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;

        if (exitLeft) Gizmos.DrawWireSphere(exitLeft.position, rayonJoueur);
        if (exitRight) Gizmos.DrawWireSphere(exitRight.position, rayonJoueur);
        if (exitFront) Gizmos.DrawWireSphere(exitFront.position, rayonJoueur);
        if (exitBack) Gizmos.DrawWireSphere(exitBack.position, rayonJoueur);
    }
}