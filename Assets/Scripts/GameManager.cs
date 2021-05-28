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
    public PathReader pathReader;
    public static GameManager Instance;

    public Camera mainCamera;

    private PhotonView PV;

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
                                            { new Vector3(120, 0.5f, 10), new Vector3(128, 0.5f, 10), new Vector3(120, 0.5f, 0), new Vector3(128, 0.5f, 0), new Vector3(120, 0.5f, -10), new Vector3(128, 0.5f, -10) } };

    public List<Material> car_list;
    public List<GameObject> map_list;

    public Canvas winnerCanvas;
    public Canvas canvas;
    public TMP_Text winner_text;
    public string winnerString;

    public AudioMixer audioMixer;


    // ----------------------------- AWAKE ------------------------------------
    private void Awake()
    {
        // Generem instancia de gameManager
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    // ----------------------------- START ------------------------------------
    void Start()
    {
        // Assignem el audiomixer
        audioMixer.SetFloat("volume", InfoSaver.Instance.volume);
        // Assignem el PhotonView
        PV = GetComponent<PhotonView>();

        // Mirem quin mapa s'ha seleccionat per posarlo actiu
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
        // Setejem els temps 
        times = new float[12];
        //Setejem el pathreader
        pathReader = GetComponent<PathReader>();
    }

    // ----------------------------- FUNCIONS ------------------------------------
    public void CreatePlayer(int id)
    {
        // Serveix per crear i inicialitzar el jugador
        player = new Player(id);
        // Començem la ronda
        StartRound();
    }

    private void StartPlayer()
    {
        // Destruim si existeix un jugador
        if (playerGameObject != null) Destroy(playerGameObject);

        // Si es el masterClient
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

        // Afegim el jugador al pathreader
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

        // Iniciem els cotxes
        StartCoroutine(StartCars());
    }

    // Serveix per iniciar els phantoms al començament de la ronda
    IEnumerator StartCars()
    {
        // Creem el jugador
        StartPlayer();
        // Canviem la camara
        ChangeCamara(false);
        // Desactivem moviment
        playerGameObject.GetComponent<PlayerController>().SetMovement(false);

        //Generem fanasmes
        GameObject phantom;
        string prefabName = "Phantom Player2";
        if (PhotonNetwork.IsMasterClient && InfoSaver.Instance.CarSelected == 0 || !PhotonNetwork.IsMasterClient && InfoSaver.Instance.CarSelected != 0)
        {
            prefabName = "Phantom Player";
        }
        for (int r = 0; r < round; r++)
        {
            phantom = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", prefabName), startingPosition[InfoSaver.Instance.mapSelected,r * 2 + player.player_id], Quaternion.identity);
            phantom.name = prefabName + " - " + r.ToString();
            phantomCars.Add(phantom);
            phantom.GetComponent<IA_Car>().AssignRace(pathReader.getRace(r));
            phantom.GetComponent<IA_Car>().AssignPowerUp(pathReader.getPowerups(r));
            phantomCars[r].GetComponent<IA_Car>().SetMovement(false);
            yield return new WaitForSecondsRealtime(1);
        }

        //Activació del semafor
        UIManager.Instance.SemSetActive(true);
        UIManager.Instance.setSemafor(0);
        yield return new WaitForSecondsRealtime(1);
        UIManager.Instance.setSemafor(1);
        yield return new WaitForSecondsRealtime(1);
        UIManager.Instance.setSemafor(2);

        // Activem el moviment dels jugadors
        for (int r = 0; r < round; r++)
        {
            phantomCars[r].GetComponent<IA_Car>().SetMovement(true);
        }
        playerGameObject.GetComponent<PlayerController>().SetMovement(true);
        // Activem el reader
        pathReader.ActivateReader(true);
        // Reiniciem el timer
        playerGameObject.GetComponent<PlayerController>().ResetTimer();
        UIManager.Instance.StartRoundUI();
        roundFlag = 0;

        // Desactivem el semafor
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

    /*[PunRPC]
    void RPC_EndRound()
    {
        foreach (GameObject obj in phantomCars) Destroy(obj);
        phantomCars = new List<GameObject>();
        PhotonNetwork.Destroy(this.playerGameObject);
        Debug.Log(InfoSaver.Instance.winnerString);
    }*/

    IEnumerator EndRound()
    {
        roundFlag = 1;
        round++;
        // Delay time
        yield return new WaitForSecondsRealtime(5);
        // Destruim objectes
        foreach (GameObject obj in phantomCars) Destroy(obj);
        phantomCars = new List<GameObject>();
        PhotonNetwork.Destroy(this.playerGameObject);
        Debug.Log(this.winnerString);
        /*if (PV.IsMine)
        {
            PV.RPC("RPC_EndRound", RpcTarget.OthersBuffered);
        }*/
        UIManager.Instance.PUSetActive(false);

        // Mirem si es ultima ronda.
        if (round < MaxRounds)
        {
            StartRound();
        } 
        else 
        {
            //winnerString = InfoSaver.Instance.winnerString;

            winnerCanvas.gameObject.SetActive(true);
            if (winnerString.Contains("Player2"))
            {
                winner_text.text = "Winner\n RED";
            }
            else
            {
                winner_text.text = "Winner\n BLUE";
            }
            
            canvas.gameObject.SetActive(false);
        }
    }

    //------------------------------DEBUG-----------------------------------------------
    public void GoBack()
    {
        System.Diagnostics.Process.Start(Application.dataPath + "/../Shadow-Racers.exe");
        Application.Quit();
    }

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

