using UnityEngine;

public class GameManager : MonoBehaviour
{
    // serialized variables
    [Header("General")]
    [SerializeField] private PlayerController playerController;

    // runs when script loads
    private void Start()
    {
        // SETUP
    }

    // runs every frame
    private void Update()
    {
        if (playerController.Health <= 0)
        {
            Application.Quit();
        }
    }
}
