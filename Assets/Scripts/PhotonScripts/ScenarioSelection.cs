using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioSelection : MonoBehaviour
{
    public static ScenarioSelection Instance;

    public List<Sprite> scenario_list;
    public int counter;

    public GameObject image_scenario;
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    public void Go_right()
    {
        counter = (counter + 1) % scenario_list.Count;
        image_scenario.GetComponent<Image>().sprite = scenario_list[counter];
    }

    public void Go_left()
    {
        counter = (counter - 1 + scenario_list.Count) % scenario_list.Count;
        image_scenario.GetComponent<Image>().sprite = scenario_list[counter];
    }
}
