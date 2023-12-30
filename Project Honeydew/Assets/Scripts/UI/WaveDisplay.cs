using TMPro;
using UnityEngine;

public class WaveDisplay : MonoBehaviour
{
    private TextMeshProUGUI text;

    // runs before scene loads
    private void Awake()
	{
        text = transform.GetComponent<TextMeshProUGUI>();
    }

    public void SetWave(int wave)
    {
        text.text = "Wave " + wave;
    }
}
