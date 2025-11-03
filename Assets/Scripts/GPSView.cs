using TMPro;
using UnityEngine;

public class GPSView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI meterText;

    [SerializeField]
    private TextMeshProUGUI activeTimeText;

    public void OnChangeMeter(float meter)
    {
        meterText.text = meter.ToString();
    }

    public void OnChangeActiveTimer(float timer)
    {
        activeTimeText.text = timer.ToString();
    }
}
