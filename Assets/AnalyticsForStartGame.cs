using Analytics;
using UnityEngine;

public class AnalyticsForStartGame : MonoBehaviour
{
    [SerializeField] private CheckPointController _checkPointController;

    private void Start()
    {
        GameAnalytics.gameAnalytics.LogEvent("session_start");
    }
    private void OnApplicationQuit()
    {
        if (PlayerPrefs.GetInt(PlayerPrefsParametrs.FirstGameStart) == 0)
            GameAnalytics.gameAnalytics.LogEvent("close_game_start");
        else
            GameAnalytics.gameAnalytics.LogEvent($"close_game_point_{_checkPointController.CurrentCheckPointIndex + 1}");
    }
}
