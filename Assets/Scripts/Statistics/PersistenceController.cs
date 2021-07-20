using System.IO;
using UnityEngine;

namespace TestInvaders.Statistics
{
    public class PersistenceController
    {
        private string _fileName = "high_scores.json";
        
        public void SaveScore(ScoreData scoreData)
        {
            var persistentData = LoadScore();
            persistentData.Scores.Add(scoreData);

            var scoreDataJson = JsonUtility.ToJson(persistentData);

            var fullPath = Path.Combine(Application.persistentDataPath, _fileName);
            
            File.WriteAllText(fullPath, scoreDataJson);
        }

        public PersistentData LoadScore()
        {
            var result = new PersistentData();
            
            var fullPath = Path.Combine(Application.persistentDataPath, _fileName);

            var strRes = string.Empty;
            if (File.Exists(fullPath))
            {
                strRes = File.ReadAllText(fullPath);
            }

            if (!string.IsNullOrEmpty(strRes))
            {
                result = JsonUtility.FromJson<PersistentData>(strRes);
            }

            return result;
        }
    }
}