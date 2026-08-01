using UnityEngine;
using UnityEngine.InputSystem;

public class walk_deplacement : MonoBehaviour
{
    [Header("Déplacement")]
    public float vitesseDeplacement = 5f;

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

        Vector3 direction = new Vector3(mouvement.x, 0, mouvement.y).normalized;

        transform.Translate(direction * vitesseDeplacement * Time.deltaTime, Space.World);
    }
}