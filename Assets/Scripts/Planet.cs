using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Assets.Scripts.HexLogic;

public class Planet : MonoBehaviour
{

    List<HexCell> Cells, Equator = new List<HexCell>();
    // Start is called before the first frame update
    HexCell NorthPole, SouthPole;
    public Material OceanMaterial, SeaMaterial, IceMaterial, FlatlandMaterial, DesertMaterial, TundraMaterial, MountainsMaterial, RainforestMaterial;
    public GameObject Huts;
    public GameObject Houses;
    public GameObject Blocks;
    public GameObject SpawnEffect;
    public GameObject Selection;
    public List<HexCell> Cities;
    public float ExpansionTimer = 3f;
    public float UpgradeTimer = 5f;

    void Start()
    {
        Cells = GetComponentsInChildren<HexCell>().ToList();

        NorthPole = Cells.Where(obj => obj.name == "Field.164").SingleOrDefault();
        SouthPole = Cells.Where(obj => obj.name == "Field.598").SingleOrDefault();

        Equator.Add(Cells.Where(obj => obj.name == "Field.1202").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1206").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1238").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1272").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1293").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1300").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1304").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1338").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1343").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1367").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1400").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1404").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1444").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1448").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1450").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1477").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1505").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1510").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1550").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1556").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1583").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1584").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1601").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1609").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1613").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1656").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1664").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1681").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1691").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1707").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1712").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1739").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1760").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1783").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1790").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1797").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1824").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1842").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1870").SingleOrDefault());
        Equator.Add(Cells.Where(obj => obj.name == "Field.1913").SingleOrDefault());

        Generate();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonUp("Jump"))
        {
            Reset();
            Generate();
        }

        transform.Rotate(-0.5f * Vector3.up * Time.deltaTime);
        
        
    }

    void Generate()
    {

        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.9f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.85f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.80f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.75f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.70f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.65f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 2f, 0.60f);


        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);
        Cells[Random.Range(0, Cells.Count)].RandomAreaSet(EHexState.Flatland, 0.5f, 0.5f);



          foreach (HexCell hex in Equator)
          { hex.RandomAreaSetOnLand(EHexState.Desert, 20f, 0.5f); }


          foreach (HexCell hex in Equator)
          { hex.RandomAreaSetOnLand(EHexState.Rainforest, 4.0f, 0.5f); }

        var Land = Cells.FindAll(
            delegate (HexCell cell) {
                if (cell.State == EHexState.Flatland)
                return true;
                return false;
            });

        Land[Random.Range(0, Land.Count)].RandomAreaSetOnLand(EHexState.Mountains, 0.4f, 0.7f);
        Land[Random.Range(0, Land.Count)].RandomAreaSetOnLand(EHexState.Mountains, 0.4f, 0.7f);
        Land[Random.Range(0, Land.Count)].RandomAreaSetOnLand(EHexState.Mountains, 0.4f, 0.7f);
        Land[Random.Range(0, Land.Count)].RandomAreaSetOnLand(EHexState.Mountains, 0.4f, 0.7f);
        Land[Random.Range(0, Land.Count)].RandomAreaSetOnLand(EHexState.Mountains, 0.4f, 0.7f);
        Land[Random.Range(0, Land.Count)].RandomAreaSetOnLand(EHexState.Mountains, 0.4f, 0.7f);

        NorthPole.RandomAreaSet(EHexState.Ice, 20f, 0.3f);
        SouthPole.RandomAreaSet(EHexState.Ice, 20f, 0.3f);

        NorthPole.RandomAreaSetOnLandNotIce(EHexState.Tundra, 40f, 0.5f);
        SouthPole.RandomAreaSetOnLandNotIce(EHexState.Tundra, 40f, 0.5f);

 

        foreach (HexCell hex in Cells)
        { if (hex.GetComponent<MeshRenderer>().material == TundraMaterial) hex.State = EHexState.Tundra; }

        foreach (HexCell hex in Cells)
        { hex.GenerateSea(); }

    }

    void Reset()
    {
        foreach (HexCell hex in Cells)
        { hex.Reset(); }
        Cities.Clear();

    }

    public void Regenerate() {
        Reset();
        Generate();
    }

    public void RemoveTowns()
    {
        foreach (HexCell hex in Cells)
        { hex.RemoveTowns(); }
        Cities.Clear();
    }

    public void ExitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit ();
    #endif
    }
}
