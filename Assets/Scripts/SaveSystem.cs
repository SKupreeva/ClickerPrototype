using Logic;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;
using System.Globalization;

namespace Saves
{
    public static class SaveSystem
    {
        public static void SaveGameData(SaveData data)
        {
            var builder = new StringBuilder();
            foreach(var item in data.Businesses)
            {
                builder.AppendLine(JsonUtility.ToJson(item));
            }
            builder.AppendLine(data.Balance.ToString());
            File.WriteAllText(Application.persistentDataPath + "/businesses.json", builder.ToString());
        }

        public static SaveData LoadGameData()
        {
            var path = Application.persistentDataPath + "/businesses.json";

            if (File.Exists(path))
            {
                var text = File.ReadAllLines(path);
                if(text.Length == 0)
                {
                    return null;
                }

                var businesses = new List<Business>();
                for (var i = 0; i < text.Length - 1; i++)
                {
                    businesses.Add(JsonUtility.FromJson<Business>(text[i]));
                }
                var balance = float.Parse(text[text.Length - 1], CultureInfo.InvariantCulture.NumberFormat);

                var data = new SaveData()
                {
                    Balance = balance,
                    Businesses = businesses
                };

                return data;
            }
            else
            {
                File.Create(path);
                Debug.LogWarning($"File not foud in {path}. Creating new file.");
                return null;
            }
        }
    }
}
