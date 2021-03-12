using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    public TMP_Text text;
    public int playing;
    public float timer;
    private GameManager gameManager;

    public bool tab = false;
    [SerializeField] private GameObject tabContainer;
    [SerializeField] private TMP_Text playerName;
    [SerializeField] private TMP_Text Time1_1_1;
    [SerializeField] private TMP_Text Time1_2_1;
    [SerializeField] private TMP_Text Time1_2_2;
    [SerializeField] private TMP_Text Time1_3_1;
    [SerializeField] private TMP_Text Time1_3_2;
    [SerializeField] private TMP_Text Time1_3_3;
    [SerializeField] private TMP_Text playerName2;
    [SerializeField] private TMP_Text Time2_1_1;
    [SerializeField] private TMP_Text Time2_2_1;
    [SerializeField] private TMP_Text Time2_2_2;
    [SerializeField] private TMP_Text Time2_3_1;
    [SerializeField] private TMP_Text Time2_3_2;
    [SerializeField] private TMP_Text Time2_3_3;

    [SerializeField] private GameObject background;
    [SerializeField] private GameObject winnerPage;


    public static UIManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int milliseconds = Mathf.FloorToInt((timer * 100f) % 100f);
        text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        
        if (tab)
        {
            showTab();
        } else
        {
            tabContainer.SetActive(false);
        }
    }

    private void showTab()
    {
        float[] infoTimes = (float[])GameManager.Instance.times.Clone();
        setTime(Time1_1_1, infoTimes[0]);
        setTime(Time1_2_1, infoTimes[2]);
        setTime(Time1_2_2, infoTimes[4]);
        setTime(Time1_3_1, infoTimes[6]);
        setTime(Time1_3_2, infoTimes[8]);
        setTime(Time1_3_3, infoTimes[10]);
        setTime(Time2_1_1, infoTimes[1]);
        setTime(Time2_2_1, infoTimes[3]);
        setTime(Time2_2_2, infoTimes[5]);
        setTime(Time2_3_1, infoTimes[7]);
        setTime(Time2_3_2, infoTimes[9]);
        setTime(Time2_3_3, infoTimes[11]);
        tabContainer.SetActive(true);
    }

    private void setTime(TMP_Text text, float time)
    {
        if (time != 0f)
        {
            int minutes = Mathf.FloorToInt(time / 60f);
            int seconds = Mathf.FloorToInt(time % 60f);
            int milliseconds = Mathf.FloorToInt((time * 100f) % 100f);
            text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");
        }
        else
        {
            text.text = "Not Set";
        }
    }

    public void StartRoundUI()
    {
        timer = 0f;
    }

    public void ShowResults()
    {
        background.SetActive(true);
        winnerPage.SetActive(true);
    }
}
