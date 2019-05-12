using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class DataPanel : MonoBehaviour
{
    Text[] Text;
    Planet Planet;
    // Start is called before the first frame update
    void Start()
    {
        Text = this.GetComponentsInChildren<Text>();
        Planet = GameObject.Find("Planet").GetComponent<Planet>();


    }

    // Update is called once per frame
    void Update()
    {
        Text[2].text =Planet.Cities.Count().ToString();
        Text[3].text =Planet.Cities.Count().ToString();

    }
}
