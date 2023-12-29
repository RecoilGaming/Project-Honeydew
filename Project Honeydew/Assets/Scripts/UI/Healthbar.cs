<<<<<<< Updated upstream
using System.Collections;
=======
>>>>>>> Stashed changes
using UnityEngine;

public class Healthbar : MonoBehaviour
{
<<<<<<< Updated upstream
    // private variables
    [SerializeField] private Transform healthOverlay;

    public void SetHealthPercent(float health)
    {
        Vector2 scale = healthOverlay.localScale;
        scale.x = health;
        healthOverlay.localScale = scale;
=======
    public void SetHealthPercent(float health)
    {
        Vector2 scale = transform.localScale;
        scale.x = health;
        transform.localScale = scale;
>>>>>>> Stashed changes
    }
}