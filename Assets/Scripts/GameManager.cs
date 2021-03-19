using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    private PathReader pathReader;
    public static GameManager Instance;

    public Camera mainCamera;

    public struct Player
    {
        public int player_id;
        public int checkPoint;
        public int lap;

        public Player(int id)
        {
            lap = 1;
            checkPoint = 0;
            player_id = id;
        }

        public void Reset()
        {
            lap = 1;
            checkPoint = 0;
        }
    }

    public float[] times;

    private Player player;

    private List<GameObject> phantomCars = new List<GameObject>();

    public GameObject playerGameObject = null;

    private int round = 0;
    public int roundFlag = 0;
    private const int MaxRounds = 3;
    private Vector3[,] startingPosition = { { new Vector3(20, 199, -370), new Vector3(28, 199, -370), new Vector3(20, 199, -380), new Vector3(28, 199, -380), new Vector3(20, 199, -390), new Vector3(28, 199, -390) },
                                            { new Vector3(120, 0, 10), new Vector3(128, 0, 10), new Vector3(120, 0, 0), new Vector3(128, 0, 0), new Vector3(120, 0, -10), new Vector3(128, 0, -10) } };

    public List<Material> car_list;
    public List<GameObject> map_list;

    public Canvas winnerCanvas;
    public Canvas canvas;
    public TMP_Text winner_text;
    public string winnerString;

    public AudioMixer audioMixer;

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
        audioMixer.SetFloat("volume", InfoSaver.Instance.volume);

        if (InfoSaver.Instance.mapSelected == 0)
        {
            map_list[1].SetActive(false);
            map_list[0].SetActive(true);
        }
        else
        {
            map_list[0].SetActive(false);
            map_list[1].SetActive(true);
        }
        times = new float[12];
        pathReader = GetComponent<PathReader>();
    }

    public void CreatePlayer(int id)
    {
        // Serveix per crear i inicialitzar el jugador
        player = new Player(id);

        StartRound();
    }

    private void StartPlayer()
    {
        if (playerGameObject != null) Destroy(playerGameObject);

        if (PhotonNetwork.IsMasterClient)
        {
            if (InfoSaver.Instance.CarSelected == 0)
            {
                playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), startingPosition[InfoSaver.Instance.mapSelected, round * 2 + player.player_id], Quaternion.identity);
            }
            else
            {
                playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), startingPosition[InfoSaver.Instance.mapSelected, round * 2 + player.player_id], Quaternion.identity);
            }
            
            playerGameObject.name = "Player";
            //playerGameObject.transform.Find("Car/Body").GetComponent<MeshRenderer>().materials[0] = car_list[InfoSaver.Instance.CarSelected];
            //Debug.Log("Change:" + playerGameObject.transform.Find("Car/Body").GetComponent<MeshRenderer>().materials[0].ToString() + "--->" + car_list[InfoSaver.Instance.CarSelected].ToString());
        }
        else
        {
            if (InfoSaver.Instance.CarSelected == 0)
            {
                playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player2"), startingPosition[InfoSaver.Instance.mapSelected, round * 2 + player.player_id], Quaternion.identity);
            }
            else
            {
                playerGameObject = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player"), startingPosition[InfoSaver.Instance.mapSelected, round * 2 + player.player_id], Quaternion.identity);
            }
            playerGameObject.name = "Player2";
            //playerGameObject.transform.Find("Car").transform.Find("Body").GetComponent<MeshRenderer>().materials[0] = car_list[InfoSaver.Instance.CarSelected];
        }

        pathReader.AddRoundPlayer(playerGameObject);
        pathReader.ActivateReader(false);

        //Creem i setejem la camera
        GameObject camera = playerGameObject.transform.Find("Camera").gameObject;
        camera.GetComponent<CameraFollow>().SetTarget(playerGameObject.transform.GetChild(0));
    }

    public void StartRound()
    {
        //Iniciar tots el jugadors
        UIManager.Instance.ChangeRound(round);

        StartCoroutine(StartCars());
    }

    // Serveix per iniciar els phantoms al començament de la ronda
    IEnumerator StartCars()
    {
        StartPlayer();
        ChangeCamara(false);
        playerGameObject.GetComponent<PlayerController>().SetMovement(false);

        GameObject phantom;
        string prefabName = "Phantom Player2";
        if (player.player_id == 0 && InfoSaver.Instance.CarSelected == 0 || player.player_id == 1 && InfoSaver.Instance.CarSelected == 1)
        {
            prefabName = "Phantom Player";
        }

        for (int r = 0; r < round; r++)
        {
            phantom = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), startingPosition[InfoSaver.Instance.mapSelected,r * 2 + player.player_id], Quaternion.identity);
            phantom.name = prefabName + " - " + r.ToString();
            phantomCars.Add(phantom);
            phantom.GetComponent<IA_Car>().AssignRace(pathReader.getRace(r));
            phantomCars[r].GetComponent<IA_Car>().SetMovement(false);
            yield return new WaitForSecondsRealtime(1);
        }

        //semafor
        UIManager.Instance.SemSetActive(true);
        UIManager.Instance.setSemafor(0);
        yield return new WaitForSecondsRealtime(1);
        UIManager.Instance.setSemafor(1);
        yield return new WaitForSecondsRealtime(1);
        UIManager.Instance.setSemafor(2);

        for (int r = 0; r < round; r++)
        {
            phantomCars[r].GetComponent<IA_Car>().SetMovement(true);
        }
        playerGameObject.GetComponent<PlayerController>().SetMovement(true);
        pathReader.ActivateReader(true);
        playerGameObject.GetComponent<PlayerController>().ResetTimer();
        UIManager.Instance.StartRoundUI();
        roundFlag = 0;


        yield return new WaitForSecondsRealtime(2);
        UIManager.Instance.SemSetActive(false);

        //Powerups
        UIManager.Instance.PUSetActive(true);
    }

    public void FinishRound()
    {
        //SaveInfo.Instance.SaveIntoJson(pathReader.getRace(0));

        StartCoroutine(EndRound());
    }

    public void ChangeCamara(bool principal)
    {
        mainCamera.enabled = principal;
    }

    IEnumerator EndRound()
    {
        roundFlag = 1;
        round++;
        yield return new WaitForSecondsRealtime(5);
        foreach (GameObject obj in phantomCars) Destroy(obj);
        phantomCars = new List<GameObject>();
        Destroy(playerGameObject);
        if (round < MaxRounds)
        {
            StartRound();
        } else
        {
            InfoSaver.Instance.BroadcastWinner(winnerString);
            winnerString = InfoSaver.Instance.winnerString;

            winnerCanvas.gameObject.SetActive(true);
            if (winnerString == "Player" || winnerString == "Phantom Player - 1" || winnerString == "Phantom Player - 2")
            {
                winner_text.text = "Winner\nPlayer 1";
            }
            else
            {
                winner_text.text = "Winner\nPlayer 2";
            }
            
            canvas.gameObject.SetActive(false);
        }
    }

    public void GoBack()
    {
        PhotonNetwork.LoadLevel(0);
    }

    //------------------------------DEBUG-----------------------------------------------

    float deltaTime = 0.0f;
 
	void Update()
	{
		deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
	}
 
	void OnGUI()
	{
		int w = Screen.width, h = Screen.height;
 
		GUIStyle style = new GUIStyle();
 
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
		GUI.Label(rect, text, style);
	}
}

