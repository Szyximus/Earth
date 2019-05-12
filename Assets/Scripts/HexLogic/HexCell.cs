
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
    Planet Planet;
    public EHexState State { get; set; }
    GameObject City;
    float ExpansionTimer;

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

        ExpansionTimer = Planet.ExpansionTimer;

    }

    void Update()
    {
        Planet.Selection.transform.rotation = Planet.Selection.transform.rotation * Quaternion.Euler(0, 0.01f * Time.time, 0);
        if (City != null) {
            ExpansionTimer = ExpansionTimer - Time.deltaTime;
            if (ExpansionTimer < 0)
            {
                ExpandCity();
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

    }

    private void FoundCity()
    {
        
        if (City == null && State != EHexState.Ocean && State != EHexState.Sea) {

            GameObject GO = Instantiate(Planet.City);

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
        var FreeNeighbors = Neighbors.Where(obj => obj.City == null).ToArray();
        if (FreeNeighbors.Count() != 0) {

            var PlaceToBuild = FreeNeighbors[Random.Range(0, FreeNeighbors.Count())];
            var Lottery = Random.Range(0f, 1f);

            switch (PlaceToBuild.State)
            {
                case EHexState.Flatland:
                    if (Lottery >= 0.1f) PlaceToBuild.FoundCity();
                    break;
                case EHexState.Rainforest:
                    if (Lottery >= 0.5f) PlaceToBuild.FoundCity();
                    break;
                case EHexState.Tundra:
                    if (Lottery >= 0.8f) PlaceToBuild.FoundCity();
                    break;
                case EHexState.Mountains:
                    if (Lottery >= 0.15f) PlaceToBuild.FoundCity();
                    break;
                case EHexState.Desert:
                    if (Lottery >= 0.9f) PlaceToBuild.FoundCity();
                    break;
                case EHexState.Ice:
                    if (Lottery >= 0.95f) PlaceToBuild.FoundCity();
                    break;
            }
            ExpansionTimer = Planet.ExpansionTimer;
        }

    }

    /*
    public void AssignObject(GameObject objectInCell)
    {
        this.ObjectInCell = objectInCell;
    }

    public void ClearObject()
    {
        ObjectInCell = null;
    }

    public bool IsEmpty()
    {
        if (ObjectInCell == null) return true;
        return false;
    }

    public bool IsDiscoveredBy(Player player)
    {
        if (discoveredBy.Contains(player))
        {
            return true;
        }
        return false;
    }

    public void Discover(Ownable owned)
    {
        visibleByList.Add(owned);
        discoveredBy.Add(owned.GetOwner());

        State = EHexState.Visible;
        if (!IsEmpty()) ObjectInCell.SetActive(true);
        gameObject.GetComponentInChildren<MeshRenderer>().material = VisibleMaterial;

    }

    public void Hide(Ownable owned)
    {
        visibleByList.Remove(owned);
        if (visibleByList.Count == 0)
        {
            State = EHexState.Hidden;
            gameObject.GetComponentInChildren<MeshRenderer>().material = HiddenMaterial;

            if (!IsEmpty())
            {
                if(ObjectInCell.tag != "Star" && ObjectInCell.tag != "Planet")
                    ObjectInCell.SetActive(false);
                if(ObjectInCell.tag == "Star")
                    gameObject.GetComponentInChildren<MeshRenderer>().material = VisibleMaterial;
            }
        }
    }

    public void Hide()
    {
        visibleByList.Clear();
        State = EHexState.Hidden;
        gameObject.GetComponentInChildren<MeshRenderer>().material = HiddenMaterial;

        if (!IsEmpty())
        {
            if (ObjectInCell.tag != "Star" && ObjectInCell.tag != "Planet")
                ObjectInCell.SetActive(false);
            if (ObjectInCell.tag == "Star")
                gameObject.GetComponentInChildren<MeshRenderer>().material = VisibleMaterial;
        }
    }

    public void UnDiscover()
    {
        visibleByList.Clear();
        State = EHexState.Undiscovered;

        if (!IsEmpty() && ObjectInCell.tag != "Star")
        {
            ObjectInCell.SetActive(false);
        }

        gameObject.GetComponentInChildren<MeshRenderer>().material = UndiscoveredMaterial;
        if (!IsEmpty() && ObjectInCell.tag == "Star")
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = VisibleMaterial;
        }
    } */
}
