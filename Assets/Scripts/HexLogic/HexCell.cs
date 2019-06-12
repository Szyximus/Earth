
   //Copyright (c) 2019, Szymon Jakóbczyk


using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.HexLogic;
using System.Threading;

//represents a single hexagonal field on a planet
public class HexCell : MonoBehaviour
{
    //public GameObject ObjectInCell;
    List<HexCell> Neighbors;
    List<HexCell> Children;
    HexCell Parent;
    Planet Planet;
    public EHexState State { get; set; }
    GameObject City;
    float ExpansionTimer;
    float UpgradeTimer;
    int CityLevel;



    #region IPointerClickHandler implementation
    private void OnMouseExit()
    {
        Planet.Selection.transform.position = new Vector3(0,-100,0);

    }

    private void OnMouseOver()
    {
        if (State != EHexState.Ocean && State != EHexState.Sea)
        {
            Planet.Selection.transform.position = this.transform.position;
            Quaternion rotation = Quaternion.LookRotation(this.transform.position - new Vector3(0, 0, 0), Vector3.up *Time.time) * Quaternion.Euler(90,0,0);
            Planet.Selection.transform.rotation = rotation;
            
        }
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("IPointerClickHandler");
        if (State != EHexState.Ocean && State != EHexState.Sea)
        {
            FoundCity();
        }
    }
    #endregion

    private void Awake()
    {
        Planet = gameObject.GetComponentInParent<Planet>();

        Neighbors = Physics.OverlapSphere(transform.position, 0.03f /*Radius*/)
            .Except(new[] { GetComponent<Collider>() })
            .Where(o => o.tag == "HexCell")
            .Select(c => c.gameObject.GetComponent<HexCell>())
            .ToList();
        City = null;
        Children = null;
        Parent = null;

        ExpansionTimer = Planet.ExpansionTimer;
        UpgradeTimer = Planet.UpgradeTimer;
        CityLevel = 0;

    }

    void Update()
    {
        Planet.Selection.transform.rotation = Planet.Selection.transform.rotation * Quaternion.Euler(0, 0.01f * Time.time, 0);
        if (City != null && Parent == null)
        {
            ExpansionTimer = ExpansionTimer - Time.deltaTime;
            if (ExpansionTimer < 0)
            {
                ExpandCity();
            }
        }
        if (City != null && CityLevel != 3)
        {
            switch (State)
            {
                case EHexState.Flatland:
                    UpgradeTimer = UpgradeTimer - Time.deltaTime;
                    break;
                case EHexState.Rainforest:
                    UpgradeTimer = UpgradeTimer - Time.deltaTime * 0.5f;
                    break;
                case EHexState.Tundra:
                    UpgradeTimer = UpgradeTimer - Time.deltaTime * 0.2f;
                    break;
                case EHexState.Mountains:
                    UpgradeTimer = UpgradeTimer - Time.deltaTime * 0.25f;
                    break;
                case EHexState.Desert:
                    UpgradeTimer = UpgradeTimer - Time.deltaTime * 0.1f;
                    break;
                case EHexState.Ice:
                    UpgradeTimer = UpgradeTimer - Time.deltaTime * 0.05f;
                    break;
            }
            if (UpgradeTimer < 0)
            {
                UpgradeCity();
            }
        }
    }

        public void RandomAreaSet(EHexState biome, float chanceForNeighbor, float falloff)
    {
        if (State != biome)
        {
            State = biome;
            UpdateMaterial();


            foreach (HexCell hex in Neighbors)
            {
                if (hex.State != biome)
                {
                    float random = Random.Range(0f, 1f);
                    if (random <= chanceForNeighbor)
                    {
                        hex.RandomAreaSet(biome, chanceForNeighbor * falloff, falloff);
                    }
                }

            }
        }
    }

    public void RandomAreaSetOnLand(EHexState biome, float chanceForNeighbor, float falloff)
    {
        if (State != EHexState.Ocean && State != EHexState.Sea && State != biome)
        {
            State = biome;
            UpdateMaterial();

            foreach (HexCell hex in Neighbors)
            {
                if (hex.State != biome && hex.State != EHexState.Ocean && hex.State != EHexState.Sea)
                {
                    float random = Random.Range(0, 1f);
                    if (random <= chanceForNeighbor)
                    {
                        hex.RandomAreaSetOnLand(biome, chanceForNeighbor * falloff, falloff);
                    }
                }

            }
        }
    }

    public void RandomAreaSetOnLandNotIce(EHexState biome, float chanceForNeighbor, float falloff)
    {
        bool WasIce = false;
        if (State != EHexState.Ocean && State != EHexState.Sea && State != biome)
        {
            
            if (State == EHexState.Ice)
            {
                WasIce = true;     
            }
            State = biome;
            UpdateMaterial();

            if (WasIce){
                gameObject.GetComponent<MeshRenderer>().material = Planet.IceMaterial;

            }


            foreach (HexCell hex in Neighbors)
            {
                if (hex.State != biome && hex.State != EHexState.Ocean && hex.State != EHexState.Sea)
                {
                    float random = Random.Range(0, 1f);
                    if (random <= chanceForNeighbor)
                    {
                        hex.RandomAreaSetOnLandNotIce(biome, chanceForNeighbor * falloff, falloff);
                    }
                }

            }
        }
    }

    public void GenerateSea()
    {
        if (State != EHexState.Ocean && State != EHexState.Sea)
        {
            foreach (HexCell hex in Neighbors)
            {
                if (hex.State == EHexState.Ocean)
                {
                    hex.State = EHexState.Sea;
                    hex.UpdateMaterial();
                }

            }
        }

    }

    void UpdateMaterial()
    {
        switch (State)
        {
            case EHexState.Ocean:
                gameObject.GetComponent<MeshRenderer>().material = Planet.OceanMaterial;
                break;
            case EHexState.Sea:
                gameObject.GetComponent<MeshRenderer>().material = Planet.SeaMaterial;
                break;
            case EHexState.Ice:
                gameObject.GetComponent<MeshRenderer>().material = Planet.IceMaterial;
                break;
            case EHexState.Desert:
                gameObject.GetComponent<MeshRenderer>().material = Planet.DesertMaterial;
                break;
            case EHexState.Flatland:
                gameObject.GetComponent<MeshRenderer>().material = Planet.FlatlandMaterial;
                break;
            case EHexState.Tundra:
                gameObject.GetComponent<MeshRenderer>().material = Planet.TundraMaterial;
                break;
            case EHexState.Rainforest:
                gameObject.GetComponent<MeshRenderer>().material = Planet.RainforestMaterial;
                break;
            case EHexState.Mountains:
                gameObject.GetComponent<MeshRenderer>().material = Planet.MountainsMaterial;
                break;
            default:
                gameObject.GetComponent<MeshRenderer>().material = Planet.OceanMaterial;
                break;
        }

    }

    public void Reset()
    {
        State = EHexState.Ocean;
        UpdateMaterial();
        RemoveTowns();
    }

    public void RemoveTowns()
    {
        var children = new List<GameObject>();
        foreach (Transform child in transform) children.Add(child.gameObject);
        children.ForEach(child => Destroy(child));
        City = null;
        Planet.Cities.Remove(this);
        CityLevel = 0;
        Parent = null;
        Children = null;
        UpgradeTimer = Planet.UpgradeTimer;
        ExpansionTimer = Planet.ExpansionTimer;

    }

    private void FoundCity()
    {
        
        if (City == null && State != EHexState.Ocean && State != EHexState.Sea) {

            GameObject GO = Instantiate(Planet.Huts);

            GO.transform.parent = this.transform;
            GO.transform.position = this.transform.position*0.9f;
            GO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * 0.004f;
            Quaternion rotation = Quaternion.LookRotation(this.transform.position, new Vector3(Random.Range(-1,1), Random.Range(-1, 1), Random.Range(-1, 1)));
            GO.transform.rotation = rotation;
            City = GO;
            StartCoroutine(GrowCity());
            Planet.Cities.Add(this);


            GameObject Effect = Instantiate(Planet.SpawnEffect);

            Effect.transform.parent = this.transform;
            Effect.transform.position = this.transform.position;
            Effect.transform.rotation = rotation;

            Destroy(Effect, 1f);

            ExpansionTimer = Planet.ExpansionTimer;
            Parent = null;
            Children = new List<HexCell>();
            CityLevel = 1;
            UpgradeTimer = Planet.UpgradeTimer;

        }
    }


    private void FoundChildCity(HexCell parent)
    {

        if (City == null && State != EHexState.Ocean && State != EHexState.Sea)
        {

            GameObject GO = Instantiate(Planet.Huts);

            GO.transform.parent = this.transform;
            GO.transform.position = this.transform.position * 0.9f;
            GO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * 0.004f;
            Quaternion rotation = Quaternion.LookRotation(this.transform.position, new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
            GO.transform.rotation = rotation;
            City = GO;
            StartCoroutine(GrowCity());
            Planet.Cities.Add(this);


            GameObject Effect = Instantiate(Planet.SpawnEffect);

            Effect.transform.parent = this.transform;
            Effect.transform.position = this.transform.position;
            Effect.transform.rotation = rotation;

            Destroy(Effect, 1f);

            ExpansionTimer = -1;
            UpgradeTimer = Planet.UpgradeTimer;

            Parent = parent;
            CityLevel = 1;

        }
    }

    private void UpgradeCity()
    {
        if (CityLevel == 2)
        {
            Destroy(City);
            GameObject GO = Instantiate(Planet.Blocks);

            GO.transform.parent = this.transform;
            GO.transform.position = this.transform.position * 0.9f;
            GO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * 0.004f;
            Quaternion rotation = Quaternion.LookRotation(this.transform.position, new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
            GO.transform.rotation = rotation;
            City = GO;
            StartCoroutine(GrowCity());

            GameObject Effect = Instantiate(Planet.SpawnEffect);

            Effect.transform.parent = this.transform;
            Effect.transform.position = this.transform.position;
            Effect.transform.rotation = rotation;


            Destroy(Effect, 1f);
            CityLevel = 3;

            UpgradeTimer = Planet.UpgradeTimer;
        }
        else if (CityLevel == 1)
        {
            Destroy(City);
            GameObject GO = Instantiate(Planet.Houses);

            GO.transform.parent = this.transform;
            GO.transform.position = this.transform.position * 0.9f;
            GO.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f) * 0.004f;
            Quaternion rotation = Quaternion.LookRotation(this.transform.position, new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), Random.Range(-1, 1)));
            GO.transform.rotation = rotation;
            City = GO;
            StartCoroutine(GrowCity());

            GameObject Effect = Instantiate(Planet.SpawnEffect);

            Effect.transform.parent = this.transform;
            Effect.transform.position = this.transform.position;
            Effect.transform.rotation = rotation;

            Destroy(Effect, 1f);
            CityLevel = 2;

            UpgradeTimer = Planet.UpgradeTimer;
        }
    }

    System.Collections.IEnumerator GrowCity() {
        float startTime = Time.time;

       // Vector3 start_pos = City.transform.position; //Starting position.

        while (Time.time - startTime < 0.5) //the movement takes exactly 1 s. regardless of framerate
        {

            City.transform.position += this.transform.position * Time.deltaTime * 0.21f;
            City.transform.localScale += new Vector3(1.25f, 1.25f, 1.25f) * Time.deltaTime *0.004f;
            yield return null;
        }
        City.transform.position = this.transform.position;
        City.transform.localScale = new Vector3(1, 1, 1) * 0.004f;
        Planet.Selection.transform.position = new Vector3(0, -100, 0);
    }


    public void ExpandCity()
    {
        List<HexCell> FreeNeighbors = new List<HexCell>();
        foreach (HexCell Cell in Neighbors)
        {
            if (Cell.City == null) FreeNeighbors.Add(Cell);
        }

        if (Children != null)
        {
            foreach (HexCell City in Children)
            {
                foreach (HexCell FreeLand in City.Neighbors.Where(obj => obj.City == null).ToList())
                {
                    FreeNeighbors.Add(FreeLand);
                }
            }
        }
        if (FreeNeighbors.Count() != 0) {

            var PlaceToBuild = FreeNeighbors[Random.Range(0, FreeNeighbors.Count())];
            var Lottery = Random.Range(0f, 1f);

            switch (PlaceToBuild.State)
            {
                case EHexState.Flatland:
                    if (Lottery >= 0.1f)
                    {
                        PlaceToBuild.FoundChildCity(this);
                        Children.Add(PlaceToBuild);
                    }
                    break;
                case EHexState.Rainforest:
                    if (Lottery >= 0.5f)
                    {
                        PlaceToBuild.FoundChildCity(this);
                        Children.Add(PlaceToBuild);
                    }
                    break;
                case EHexState.Tundra:
                    if (Lottery >= 0.8f)
                    {
                        PlaceToBuild.FoundChildCity(this);
                        Children.Add(PlaceToBuild);
                    }
                    break;
                case EHexState.Mountains:
                    if (Lottery >= 0.75f)
                    {
                        PlaceToBuild.FoundChildCity(this);
                        Children.Add(PlaceToBuild);
                    }
                    break;
                case EHexState.Desert:
                    if (Lottery >= 0.9f)
                    {
                        PlaceToBuild.FoundChildCity(this);
                        Children.Add(PlaceToBuild);
                    }
                    break;
                case EHexState.Ice:
                    if (Lottery >= 0.95f)
                    {
                        PlaceToBuild.FoundChildCity(this);
                        Children.Add(PlaceToBuild);
                    }
                    break;
            }
            ExpansionTimer = Planet.ExpansionTimer;
        }

    }
}
