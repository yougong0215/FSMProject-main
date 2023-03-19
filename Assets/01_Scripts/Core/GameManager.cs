using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Transform _playerTrm;
    public Transform PlayerTrm => _playerTrm;

    public static GameManager Instance;

    [SerializeField]
    private Transform _mapTrm;

    private void Awake()
    {
        _playerTrm = GameObject.Find("Player").transform;

        if (Instance != null)
        {
            Debug.LogError("Multiple Gamemanager is running!");
        }
        Instance = this;

        MapManager.Instance = new MapManager(_mapTrm);

        SaveManager.Instance = new SaveManager();
    }

    #region 技捞宏 包府 康开

    [SerializeField]
    private List<string> _achievements;
    [SerializeField]
    private string _name;
    [SerializeField]
    private int _maxScore;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveData data = new SaveData
            { 
                Achievements = _achievements,
                MaxSccore = _maxScore,
                Name = _name
            };
            SaveManager.Instance.Save(data);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveData data = SaveManager.Instance.Load();

            if(data != null)
            {
                _achievements = data.Achievements;
                _maxScore = data.MaxSccore;
                _name = data.Name;
            }
        }
    }

    #endregion

}
