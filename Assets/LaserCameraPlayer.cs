using UnityEngine;
using UnityEngine.InputSystem;

public class LaserDebug : MonoBehaviour
{
    [Header("Réglages")]
    public float distanceMax = 100f;

    public Color couleurLaser = Color.green;
    public Color couleurImpact = Color.red;

    void Update()
    {
        // Le laser n'est actif que lorsque E est maintenue
        if (!Keyboard.current.eKey.isPressed)
            return;

        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, distanceMax))
        {
            // Laser jusqu'au point d'impact
            Debug.DrawLine(ray.origin, hit.point, couleurImpact);

            Debug.Log("Touché : " + hit.collider.name);

            if (hit.collider.CompareTag("C15"))
            {
                Debug.Log(">>> CIBLE TOUCHEE !");
            }
        }
        else
        {
            // Laser sur toute la distance
            Debug.DrawRay(ray.origin, ray.direction * distanceMax, couleurLaser);
        }
    }
}