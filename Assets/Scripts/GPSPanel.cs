using UnityEngine;

public class GPSPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject initCanvas;
    [SerializeField]
    private GameObject startCanvas;
    [SerializeField]
    private GameObject drivingCanvas;

    private void Start()
    {
        // GPS 권한 확인
        if (!Input.location.isEnabledByUser)
        {
            initCanvas.SetActive(true);
            startCanvas.SetActive(false);
            drivingCanvas.SetActive(false);
        }
        else
        {
            initCanvas.SetActive(false);
            startCanvas.SetActive(true);
            drivingCanvas.SetActive(false);
        }
    }

    public void OnStartDriving()
    {
        initCanvas.SetActive(false);
        startCanvas.SetActive(false);
        drivingCanvas.SetActive(true);
    }

    public void OnEndDriving()
    {
        initCanvas.SetActive(false);
        startCanvas.SetActive(true);
        drivingCanvas.SetActive(false);
    }
}
