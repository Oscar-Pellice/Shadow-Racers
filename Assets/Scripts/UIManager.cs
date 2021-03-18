using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int playing;
    public float timer;
    private GameManager gameManager;

    public bool tab = false;
    [SerializeField] private TMP_Text round_text;
    [SerializeField] private TMP_Text time_text;
    [SerializeField] private TMP_Text best_text;


    [SerializeField] private GameObject position_sprite;
    [SerializeField] private List<Sprite> list_position_sprite;

    [SerializeField] private GameObject semafor_struct;
    [SerializeField] private List<GameObject> semafor_state;
    [SerializeField] private List<Sprite> list_sem_sprite;


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
        time_text.text = minutes.ToString("00") + ":" + seconds.ToString("00") + ":" + milliseconds.ToString("00");

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

    public void setSemafor(int valor)
    {
        foreach (GameObject circle in semafor_state)
        {
            circle.GetComponent<Image>().sprite = list_sem_sprite[valor];
        }
    }

    public void SemSetActive(bool state)
    {
        semafor_struct.SetActive(state);
    }

    public void ChangeRound(int round)
    {
        round_text.text = (round+1).ToString() + " / 3";
    }

    public void ChangePosition(int pos)
    {
        position_sprite.GetComponent<Image>().sprite = list_position_sprite[pos];
    }

    public void StartRoundUI()
    {
        timer = 0f;
    }
}
