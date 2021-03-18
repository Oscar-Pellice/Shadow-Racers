using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarSelection : MonoBehaviour
{
    public List<Sprite> car_list;
    public int counter;

    public GameObject image_car;


    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
    }

    public void Go_right()
    {
        counter = (counter + 1) % car_list.Count;
        image_car.GetComponent<Image>().sprite = car_list[counter];
    }

    public void Go_left()
    {
        counter = (counter - 1 + car_list.Count) % car_list.Count;
        image_car.GetComponent<Image>().sprite = car_list[counter];
    }
}
