using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CSVLoader : MonoBehaviour
{
    [HideInInspector] public TextAsset textFile;
    private char lineSeparator = '\n';
    private string[] fieldSeparator = {"\",\""};
    [HideInInspector] public string path = string.Empty;

    public void LoadCSV(string newPath)
    {
        path = newPath;
        textFile = Resources.Load<TextAsset>(newPath);
        Debug.Log("File: "+textFile);
    }

    public Dictionary<string, string> GetDictionaryValues(string attributeId)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();
        string[] lines = textFile.text.Split(lineSeparator);

        int attrIndex = -1;

        string[] headers = lines[0].Split(fieldSeparator, System.StringSplitOptions.None);

        for (int i = 0; i < headers.Length; i++)
        {
            if (headers[i].Contains(attributeId))
            {
                attrIndex = i;
                /*Debug.Log("Index: "+attrIndex);
                Debug.Log("Attribute Id: " + attributeId);*/
                break;
            }
        }

        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

        for (int j = 1; j < lines.Length; j++)
        {
            string line = lines[j];

            string[] fields = CSVParser.Split(line);
            /*for (int f = 0; f < fields.Length; f++)
            {
                fields[f] = fields[f].TrimStart(' ', surround);
                fields[f] = fields[f].TrimEnd(surround);
            }*/

            if (fields.Length > attrIndex)
            {
                var key = fields[0];
                if (dictionary.ContainsKey(key)) { continue; }
                var value = fields[1];
                //Debug.Log("Fields : "+ fields[1]+" Key: "+key);
                dictionary.Add(key, value);
            }
        }
        return dictionary;
    }

    public string GetKey()
    {
        string[] lines = textFile.text.Split(lineSeparator);
        string line = lines[0];
        Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
        string[] fields = CSVParser.Split(line);
        //Debug.Log("Fields: " + fields[1]);
        return fields[1];
    }
}
