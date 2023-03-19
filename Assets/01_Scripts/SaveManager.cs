using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveManager
{
    public static SaveManager Instance;

    private string _dataPath = $"{Application.persistentDataPath}/savedata.data";

    private CryptoModule _cryptoModule;


    public SaveManager()
    {
        _cryptoModule = new CryptoModule();
    }
    public void Save(SaveData data)
    {

        Debug.Log(_dataPath);

        string json = JsonUtility.ToJson(data);
        json = _cryptoModule.AESEncrypt256(json);
        Debug.Log(json);

        StreamWriter writer = null;
        try
        {
            writer = new StreamWriter(_dataPath);


            writer.Write(json);
            writer.Flush();
            writer.Close();
        }
        catch(Exception ex)
        {
            writer?.Close();
            Debug.LogError($"Saving Error Exception : {ex.Message}");
        }
    }

    public SaveData Load()
    {
        SaveData returnData = null;
        StreamReader reader = null;
        try
        {
            reader = new StreamReader(_dataPath);
            string loadData = reader.ReadToEnd();

            loadData = _cryptoModule.Decrypt(loadData);
            Debug.Log(loadData);

            reader.Close();
            returnData = JsonUtility.FromJson<SaveData>(loadData);
        }
        catch(Exception ex)
        {
            reader?.Close();
            Debug.Log($"error occured dajfnao : {ex.Message}");
        }


        return returnData;
    }
}
