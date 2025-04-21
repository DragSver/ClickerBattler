using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Configs;
using Configs.LevelConfigs;
using Datas.Game.EnemiesData;
using Datas.Global;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;

namespace Editor.Csv
{
    [CreateAssetMenu(menuName="Configs/ConfigsCsvFiller", fileName = "ConfigsCsvFiller")]
    public class ConfigsCsvFiller : ScriptableObject
    {
        [SerializeField] private string _csvLevelDataFilePath;
        [SerializeField] private LevelsConfig _levelsConfig;
        private List<List<string>> _levelsCsv;
        
        [SerializeField] private string _csvSkillDataFilePath;
        [SerializeField] private SkillConfig _skillConfig;
        private List<List<string>> _skillCsv;


        [ContextMenu("Get Levels CSV File")]
        private void GetLevelCsv() => _ = GetLevelCsvFileAsync();
        private async Task GetLevelCsvFileAsync()
        {
            _levelsCsv = await ReadCsvFromUrl(_csvLevelDataFilePath);
            Debug.Log("Completed getting levels csv file");
        }
        
        [ContextMenu("Get Skills CSV File")]
        private void GetSkillsCsv() => _ = GetSkillsCsvFileAsync();
        private async Task GetSkillsCsvFileAsync()
        {
            _skillCsv = await ReadCsvFromUrl(_csvSkillDataFilePath);
            Debug.Log("Completed getting skills csv file");
        }

        [ContextMenu("Fill Levels Data")]
        private void FillLevelData()
        {
            _levelsConfig.Clear();

            for (var i = 1; i < _levelsCsv.Count; i++)
            {
                var location = int.Parse(_levelsCsv[i][0]);
                var level = int.Parse(_levelsCsv[i][1]);
                var wive = int.Parse(_levelsCsv[i][2]);

                // Получаем или создаем LocationLevelData
                var locationIndex = _levelsConfig.Locations.FindIndex(data => data.Location == location);
                if (locationIndex == -1)
                {
                    _levelsConfig.Locations.Add(new LocationLevelData(location, new List<LevelData>()));
                    locationIndex = _levelsConfig.Locations.Count - 1;
                }

                var locationLevelData = _levelsConfig.Locations[locationIndex];

                // Получаем или создаем LevelData
                var levelIndex = locationLevelData.Levels.FindIndex(data => data.LevelNumber == level);
                if (levelIndex == -1)
                {
                    locationLevelData.Levels.Add(new LevelData
                    {
                        Location = location,
                        LevelNumber = level,
                        EnemiesWives = new List<EnemiesWiveData>(),
                        Rewards = new List<CollectedItemsData>()
                    });
                    levelIndex = locationLevelData.Levels.Count - 1;
                }

                var levelData = locationLevelData.Levels[levelIndex];

                // Добавляем награду
                var reward = _levelsCsv[i][8];
                if (reward != "" && reward != "-")
                {
                    if (int.TryParse(reward, out var rewardInt))
                        levelData.Rewards.Add(new CollectedItemsData { Count = rewardInt, Id = "coin" });
                }

                // Получаем или создаем EnemiesWiveData
                var wiveIndex = levelData.EnemiesWives.FindIndex(data => data.Wive == wive);
                if (wiveIndex == -1)
                {
                    levelData.EnemiesWives.Add(new EnemiesWiveData
                    {
                        Wive = wive,
                        LevelNumber = level,
                        Location = location,
                        Enemies = new List<EnemySpawnData>()
                    });
                    wiveIndex = levelData.EnemiesWives.Count - 1;
                }

                var enemyWiveData = levelData.EnemiesWives[wiveIndex];

                // Добавляем таймер
                var time = _levelsCsv[i][7];
                if (time != "" && time != "-")
                {
                    if (float.TryParse(time, out var timeFloat))
                        enemyWiveData.Time = timeFloat;
                }

                // Добавляем врага
                if (enemyWiveData.Enemies.Count < 4)
                {
                    var enemyId = _levelsCsv[i][4];
                    var isBoss = _levelsCsv[i][5] == "босс";
                    var hp = int.Parse(_levelsCsv[i][6]);

                    enemyWiveData.Enemies.Add(new EnemySpawnData
                    {
                        Wive = wive,
                        LevelNumber = level,
                        Location = location,
                        Id = enemyId,
                        IsBoss = isBoss,
                        Hp = hp
                    });
                }

                // Обновляем структуры обратно в коллекции
                levelData.EnemiesWives[wiveIndex] = enemyWiveData;
                locationLevelData.Levels[levelIndex] = levelData;
                _levelsConfig.Locations[locationIndex] = locationLevelData;
                
            }
            
            EditorUtility.SetDirty(_levelsConfig);
            AssetDatabase.SaveAssets();
            
            Debug.Log("Fill level data");
        }

        [ContextMenu("Fill Skills Data")]
        private void FillSkillData()
        {
            _skillConfig.Clear();

            foreach (var t in _skillCsv)
            {
                var skillId = t[0];
                var skillData = _skillConfig.SkillDatas.Find(data => data.Id == skillId);
                if (skillData == null)
                {
                    skillData = new SkillData
                    {
                    Id = skillId,
                    SkillsLevel = new List<SkillDataByLevel>() 
                    };
                    
                    _skillConfig.SkillDatas.Add(skillData);
                }

                var skillLevel = int.Parse(t[1]);
                var skillValue = float.Parse(t[2], CultureInfo.InvariantCulture);
                var skillPrice = int.Parse(t[3]);
                var skillDataByLevel = skillData.SkillsLevel.Find(level => level.Level == skillLevel); 
                if (skillDataByLevel == null)
                {
                    skillDataByLevel = new SkillDataByLevel();
                    skillData.SkillsLevel.Add(skillDataByLevel);
                }

                skillDataByLevel.Level = skillLevel;
                skillDataByLevel.Value = skillValue;
                skillDataByLevel.Price = skillPrice;
            }
        }
        
        private static async Task<List<List<string>>> ReadCsvFromUrl(string url)
        {
            var client = new HttpClient();
            var response = await client.GetStringAsync(url);

            var result = new List<List<string>>();
            var reader = new StringReader(response);

            while (await reader.ReadLineAsync() is { } line)
            {
                var values = line.Split(',');
                if (values.Length > 0)
                    result.Add(new List<string>(values));
            }

            return result;
        }
    }
}