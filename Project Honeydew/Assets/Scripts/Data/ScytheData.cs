using UnityEngine;

[CreateAssetMenu(menuName = "Scythe Data")]
public class ScytheData : ScriptableObject
{
    // variables
    [Header("Scythe")]
    public float gravityClamp;
    public float gravityAccel;
    public float scytheRange;
    public float scytheSpeed;
    public float scytheAccel;
    public float returnSpeed;
    public float recoveryTime;
}