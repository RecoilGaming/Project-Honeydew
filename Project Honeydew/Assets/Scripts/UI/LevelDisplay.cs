using TMPro;
using UnityEngine;

public class LevelDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    // runs before scene loads
    private void Awake()
	{
        text = transform.GetComponent<TextMeshProUGUI>();
    }

    public void SetLevel(int level)
    {
        text.text = "Level: " + level;
    }
}
