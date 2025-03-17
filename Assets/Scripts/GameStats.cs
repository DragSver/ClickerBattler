using UnityEngine;

namespace ClickRPG
{
    public class GameStats
    {
        public float SaveBestTime(float newTime)
        {
            var bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
            if (newTime >= bestTime) return bestTime;
            
            PlayerPrefs.SetFloat("BestTime", newTime);
            PlayerPrefs.Save();
            return newTime;
        }

        public int AddKills()
        {
            var totalKills = PlayerPrefs.GetInt("TotalKills", 0);
            totalKills++;
            PlayerPrefs.SetInt("TotalKills", totalKills);
            PlayerPrefs.Save();
            return totalKills;
        }

        public int AddDeaths()
        {
            var totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
            totalDeaths++;
            PlayerPrefs.SetInt("TotalDeaths", totalDeaths);
            PlayerPrefs.Save();
            return totalDeaths;
        }
        
        public void ResetStats()
        {
            PlayerPrefs.DeleteKey("BestTime");
            PlayerPrefs.DeleteKey("TotalKills");
            PlayerPrefs.DeleteKey("TotalDeaths");
            PlayerPrefs.Save();
        }
    }
}