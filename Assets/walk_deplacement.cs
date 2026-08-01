using UnityEngine;
using UnityEngine.InputSystem;

public class walk_deplacement : MonoBehaviour
{
    [Header("Déplacement")]
    public float vitesseDeplacement = 5f;

    [Header("Référence")]
    public Transform cameraTransform;

    void Update()
    {
        Vector2 mouvement = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
            mouvement.y += 1;

        if (Keyboard.current.sKey.isPressed)
            mouvement.y -= 1;

        if (Keyboard.current.aKey.isPressed)
            mouvement.x -= 1;

        if (Keyboard.current.dKey.isPressed)
            mouvement.x += 1;

        // Directions de la caméra
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // On ignore l'inclinaison verticale
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Direction finale
        Vector3 direction = (forward * mouvement.y + right * mouvement.x).normalized;

        transform.position += direction * vitesseDeplacement * Time.deltaTime;
    }
}