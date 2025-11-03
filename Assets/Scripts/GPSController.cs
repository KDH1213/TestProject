using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class GPSController : MonoBehaviour
{
    private Vector2 lastGPSPos;

    public float TotalDistacne {  get; private set; }
    private bool isTracking = false;

    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

    public UnityEvent<float> onChangeMeter;
    public UnityEvent onFailedGPS;

    private void Start()
    {
        if (Input.location.isEnabledByUser)
        {
            StartCoroutine(CoGps());
        }
    }

    private IEnumerator CoGps()
    {
        // GPS 시작
        Input.location.Start(1f, 0.1f); // (accuracy, minDistance)

        // 초기화 대기
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("GPS 초기화 실패");
            onFailedGPS?.Invoke();
            yield break;
        }

        isTracking = true;
        lastGPSPos = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);

        StartCoroutine(UpdateDistance());
    }


    private IEnumerator UpdateDistance()
    {
        TotalDistacne = 0f;
        onChangeMeter?.Invoke(TotalDistacne);

        while (isTracking)
        {
            var current = new Vector2(Input.location.lastData.latitude, Input.location.lastData.longitude);
            float delta = Utills.HaversineDistance(lastGPSPos, current);

            // GPS 노이즈 제거 (예: 1m 이하 이동은 무시)
            if (delta > 1f)
            {
                TotalDistacne += delta;
                onChangeMeter?.Invoke(TotalDistacne);
            }

            lastGPSPos = current;

            yield return waitForSeconds; // 1초마다 업데이트
        }
    }


}