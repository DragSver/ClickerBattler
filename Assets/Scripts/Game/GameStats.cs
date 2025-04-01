using UnityEngine;

namespace Game
{
    public static class GameStats
    {
        public static float SaveBestTime(float newTime)
        {
            var bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
            if (newTime >= bestTime) return bestTime;
            
            PlayerPrefs.SetFloat("BestTime", newTime);
            PlayerPrefs.Save();
            return newTime;
        }

        public static float SaveBestTimeEnemy(float newTime, string idEnemy)
        {
            var bestTime = PlayerPrefs.GetFloat("BestTimeEnemy" + idEnemy, float.MaxValue);
            if (newTime >= bestTime) return bestTime;
            
            PlayerPrefs.SetFloat("BestTimeEnemy" + idEnemy, newTime);
            PlayerPrefs.Save();
            return newTime;
        }

        public static int AddKills()
        {
            var totalKills = PlayerPrefs.GetInt("TotalKills", 0);
            totalKills++;
            PlayerPrefs.SetInt("TotalKills", totalKills);
            PlayerPrefs.Save();
            return totalKills;
        }
        public static int GetKills() => PlayerPrefs.GetInt("TotalKills", 0);

        public static int AddDeaths()
        {
            var totalDeaths = PlayerPrefs.GetInt("TotalDeaths", 0);
            totalDeaths++;
            PlayerPrefs.SetInt("TotalDeaths", totalDeaths);
            PlayerPrefs.Save();
            return totalDeaths;
        }
        
        public static void ResetStats()
        {
            PlayerPrefs.DeleteKey("BestTime");
            PlayerPrefs.DeleteKey("TotalKills");
            PlayerPrefs.DeleteKey("TotalDeaths");
            PlayerPrefs.Save();
        }
    }
}