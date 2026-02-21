using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class GoogleSheetReader : MonoBehaviour
{
    private string apiKey = "AIzaSyBFcCNpO6JRtQLSM07GOhzsjkOw3PzkP1Y";


    #region GoogleDocsViewer 변수
    private string defaultUrlKey_GoogleDocs = "https://docs.google.com/spreadsheets/d/";
    private string sheetID = "885261910";
    private string sheetID2 = "2007133998";

    private string spreadID = "1P3CvL_0j1ww7MBHO4k1dn_Gjvv4ISgURNL0mDC_offM";
    private readonly string urlReadFormat = "{0}{1}/edit?gid={2}#gid={2}";
    private readonly string urlExportFormat = "{0}{1}/export?format=csv&gid={2}";

    #endregion

    #region RestAPI 변수
    private string googleapisUrl = "https://sheets.googleapis.com/v4/spreadsheets/";
    private string restApiFomat = "{0}{1}/values/{2}!{3}?key={4}";
    private string restAPISheetID = "시트2";

    [SerializeField]
    private string range = "A2:D";

    #endregion


    private void Awake()
    {
        StartCoroutine(CoReadSheet_RestAPI());
    }

    private IEnumerator CoReadSheet_GoogleDocsViewer()
    {
        string url = string.Format(urlExportFormat, defaultUrlKey_GoogleDocs, spreadID, sheetID2);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Raw JSON:\n" + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }
    }

    private IEnumerator CoReadSheet_RestAPI()
    {
        string url = string.Format(restApiFomat, googleapisUrl, spreadID, sheetID, range, apiKey);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Raw JSON:\n" + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError(request.error);
                Debug.LogError($"Result: {request.result}");
                Debug.LogError($"ResponseCode: {request.responseCode}");
                Debug.LogError($"Error: {request.error}");
                Debug.LogError($"Body: {request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator CoGetLastColumn()
    {
        string url = string.Format(restApiFomat, googleapisUrl, spreadID, sheetID, range, apiKey);

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                var res = JsonUtility.FromJson<GoogleSheetResponse>(request.downloadHandler.text);

                // 현재 열 개수
                int columnCount = res.values[0].Length;
                string nextColumn = ColumnNumberToName(columnCount + 1);
            }
            else
            {
                Debug.LogError(request.error);
                Debug.LogError($"Result: {request.result}");
                Debug.LogError($"ResponseCode: {request.responseCode}");
                Debug.LogError($"Error: {request.error}");
                Debug.LogError($"Body: {request.downloadHandler.text}");
            }
        }
    }

    private IEnumerator WriteToLastColumn(string sheetName, int row, int column, string value)
    {
        string columnName = ColumnNumberToName(column);
        string range = $"{sheetName}!{columnName}{row}";
        string url = string.Format(restApiFomat, googleapisUrl, spreadID, sheetID, range, apiKey);


        string jsonBody = "{\"values\": [[\"" + value + "\"]] }";

        UnityWebRequest request = UnityWebRequest.Put(url, jsonBody);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공적으로 마지막 열에 값 추가됨!");
        }
        else
        {
            Debug.LogError(request.error);
            Debug.LogError(request.downloadHandler.text);
        }
    }

    private static string ColumnNumberToName(int number)
    {
        string result = "";
        while (number > 0)
        {
            int mod = (number - 1) % 26;
            result = (char)(65 + mod) + result;
            number = (number - mod - 1) / 26;
        }
        return result;
    }
}

[System.Serializable]
public class GoogleSheetResponse
{
    public string range;
    public string majorDimension;
    public string[][] values;
}

[System.Serializable]
public class CharacterData
{
    public string Id;
    public string PrefabID;
    public string CharactorClassType;
    public string Damage;
    public string AttackRange;
    public string AttackSpeed;
}