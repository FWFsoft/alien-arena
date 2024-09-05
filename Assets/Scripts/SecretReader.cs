
using System;
using System.Collections.Generic;
using UnityEngine;


public class SecretReader
{
    private static Dictionary<string, string> secrets = new();
    static SecretReader()
    {
        string filePath = System.IO.Path.Combine(Application.dataPath, ".env");
        string data = System.IO.File.ReadAllText(filePath);
        foreach (var line in data.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
        {
            Debug.Log("line" + line);
            var parts = line.Split("=");
            if (parts.Length != 2)
            {
                Debug.LogWarning("Line length was not an even 2");
            }
            else
            {
                secrets.Add(parts[0].ToUpper(), parts[1]);
            }
            Debug.Log(secrets);
        }
    }
    public static string GetSecret(string key)
    {
        secrets.TryGetValue(key, out string value);
        return value;
    }

}