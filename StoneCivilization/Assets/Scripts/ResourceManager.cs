using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ResourceManager : MonoBehaviour
{
    public int woodAmount = 0;
    public int maxWoodAmount = 10;
    public int foodAmount= 0;
    public int maxFoodAmount = 10;
    public int stoneAmount = 0;
    public int maxStoneAmount = 10;
    public int time = 8;
    public int peopleAmount = 1;
    private const float MaxEnergy = 4;
    public float energy = 0;
    public float livingPlaces;
    public float simpleHouseCapacity = 2;
    public float houseCapacity = 4;
    public float goodHouseCapacity = 7;
    public int days = 1;
    public int fishingDayBonus;
    public int goodAxesBonus;

    private bool _isAssignedToForest = false;
    private bool _isAssignedToMountains = false;
    private bool _isAssignedToSea = false;

    [SerializeField] private TextMeshProUGUI _woodAmountLabel;
    [SerializeField] private TextMeshProUGUI _maxWoodAmountLabel;
    [SerializeField] private TextMeshProUGUI _foodAmountLabel;
    [SerializeField] private TextMeshProUGUI _maxFoodAmountLabel;
    [SerializeField] private TextMeshProUGUI _stoneAmountLabel;
    [SerializeField] private TextMeshProUGUI _maxStoneAmountLabel;
    [SerializeField] private TextMeshProUGUI _peopleAmountLabel;
    [SerializeField] private TextMeshProUGUI _timeChangeLabel;
    [SerializeField] private TextMeshProUGUI _dayCountLabel;
    [SerializeField] private TextMeshProUGUI _randomEventDescription;
    [SerializeField] private TextMeshProUGUI _houseDescriptionLabel;
    [SerializeField] private TextMeshProUGUI _houseUpgradeButtonLabel;
    [SerializeField] private TextMeshProUGUI _foodWarehouseUpgradeDescription;
    [SerializeField] private TextMeshProUGUI _woodWarehouseUpgradeDescription;
    [SerializeField] private TextMeshProUGUI _stoneWarehouseUpgradeDescription;
    [SerializeField] private GameObject NPCFeedPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject _FoodWarehouseUpgradePanel;
    [SerializeField] private GameObject _WoodWarehouseUpgradePanel;
    [SerializeField] private GameObject _StoneWarehouseUpgradePanel;
    [SerializeField] private GameObject _simpleHousePanel;
    [SerializeField] private GameObject _randomEventPanel;
    [SerializeField] private GameObject _foodWarehouse;
    [SerializeField] private GameObject _goodFoodWarehouse;
    [SerializeField] private GameObject _woodWarehouse;
    [SerializeField] private GameObject _goodWoodWarehouse;
    [SerializeField] private GameObject _stoneWarehouse;
    [SerializeField] private GameObject _goodStoneWarehouse;
    [SerializeField] private GameObject _simpleHouse;
    [SerializeField] private GameObject _house;
    [SerializeField] private GameObject _goodHouse;
    [SerializeField] TMP_Dropdown dropdownForest;
    [SerializeField] TMP_Dropdown dropdownMountains;
    [SerializeField] TMP_Dropdown dropdownSea;
    [SerializeField] TMP_Dropdown dropdownSeaNPC;
    [SerializeField] TMP_Dropdown dropdownSeaNPCA;
    [SerializeField] TMP_Dropdown dropdownSeaNPCB;
    [SerializeField] TMP_Dropdown dropdownSeaNPCC;
    [SerializeField] TMP_Dropdown dropdownSeaNPCD;
    [SerializeField] TMP_Dropdown dropdownSeaNPCE;
    [SerializeField] TMP_Dropdown dropdownMountainsNPC;
    [SerializeField] TMP_Dropdown dropdownMountainsNPCA;
    [SerializeField] TMP_Dropdown dropdownMountainsNPCB;
    [SerializeField] TMP_Dropdown dropdownMountainsNPCC;
    [SerializeField] TMP_Dropdown dropdownMountainsNPCD;
    [SerializeField] TMP_Dropdown dropdownMountainsNPCE;
    [SerializeField] TMP_Dropdown dropdownForestNPC;
    [SerializeField] TMP_Dropdown dropdownForestNPCA;
    [SerializeField] TMP_Dropdown dropdownForestNPCB;
    [SerializeField] TMP_Dropdown dropdownForestNPCC;
    [SerializeField] TMP_Dropdown dropdownForestNPCD;
    [SerializeField] TMP_Dropdown dropdownForestNPCE;
    [SerializeField] Image energyBar;
    [SerializeField] Image eventImage;
    [SerializeField] GameObject _sea;
    [SerializeField] GameObject _forest;
    [SerializeField] GameObject _mountains;

    public Dictionary<string, bool> _events = new Dictionary<string, bool>();

    public string[] names = { "Tom", "Bob", "Kaseki", "Chrome", "Kohaku", "Suika", "Kinro", "Ginro", "Gen", "Magma", "Nikki", "Ukyo", "Minami", "Yo", "Ryusui", "Francois", "Tsukasa", "Hyoga", "Homura", "Yuzuriha" };
    int indexNames = 0;

    public List<CharacterInfo> NPCs;
    public List<string> NPCNames;
    public List<TMP_Dropdown> Dropdowns;
    public List<TMP_Dropdown> NPCDropdowns;

    public List<GameObject> NPCPrefabs;
    int indexPrefabs = 0;
    public List<GameObject> workingPlacesSea;
    public List<GameObject> workingPlacesMountains;
    public List<GameObject> workingPlacesForest;
    public List<Sprite> eventSprites;


    void Start()
    {
        _woodAmountLabel.text = woodAmount.ToString();
        _maxWoodAmountLabel.text = maxWoodAmount.ToString();
        _foodAmountLabel.text = foodAmount.ToString();
        _maxFoodAmountLabel.text= maxFoodAmount.ToString();
        _stoneAmountLabel.text = stoneAmount.ToString();
        _maxStoneAmountLabel.text = maxStoneAmount.ToString();
        _peopleAmountLabel.text = peopleAmount.ToString();
        _timeChangeLabel.text = time.ToString();
        _dayCountLabel.text = days.ToString();
        energy = MaxEnergy;
        NPCNames.Add("None");
        
        livingPlaces = GameObject.FindGameObjectsWithTag("SimpleHouse").Length * simpleHouseCapacity;

        _events.Add("storm", false);
        _events.Add("fishingDay", false);
        _events.Add("wolves", false);
        _events.Add("goodAxes", false);
        _events.Add("landslide", false);
        _events.Add("beavers", false);
        _events.Add("foodTheft", false);
    }
    void Update()
    {
        CheckWin();

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

           RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
                        
            if (hit.collider == null)
            {
                //Debug.Log("NoCollision");
            }
            else if (hit.collider.gameObject.tag == "NPC")
            {
                NPCFeedPanel.SetActive(true);
                var NPC = hit.collider.gameObject.GetComponent<CharacterInfo>();
                NPC.feedMenuActive = true;
            }
            else if (hit.collider.gameObject.tag == "SimpleFoodWarehouse")
            {
                _FoodWarehouseUpgradePanel.SetActive(true);
            }
            else if (hit.collider.gameObject.tag == "FoodWarehouse")
            {
                _FoodWarehouseUpgradePanel.SetActive(true);
                _foodWarehouseUpgradeDescription.text = "To upgrade you need \n \nWood: 18 \nStone:18 \n \nFuture capacity: 40";
            }
            else if (hit.collider.gameObject.tag == "SimpleWoodWarehouse")
            {
                _WoodWarehouseUpgradePanel.SetActive(true);
            }
            else if (hit.collider.gameObject.tag == "WoodWarehouse")
            {
                _WoodWarehouseUpgradePanel.SetActive(true);
                _woodWarehouseUpgradeDescription.text = "To upgrade you need \n \nWood: 18 \nStone:18 \n \nFuture capacity: 40";
            }
            else if (hit.collider.gameObject.tag == "SimpleStoneWarehouse")
            {
                _StoneWarehouseUpgradePanel.SetActive(true);
            }
            else if (hit.collider.gameObject.tag == "StoneWarehouse")
            {
                _StoneWarehouseUpgradePanel.SetActive(true);
                _stoneWarehouseUpgradeDescription.text = "To upgrade you need \n \nWood: 18 \nStone:18 \n \nFuture capacity: 40";
            }
            else if (hit.collider.gameObject.tag == "SimpleHouse")
            {
                _simpleHousePanel.SetActive(true);
                if (GameObject.FindGameObjectsWithTag("SimpleHouse").Length == 3 || GameObject.FindGameObjectsWithTag("House").Length > 0)
                {
                   _houseDescriptionLabel.text = "Upgrade simple house? \nWood: -19 \nStone:-19 \nCapacity:+2";
                   _houseUpgradeButtonLabel.text = "Upgrade house";
                }
            }
            else if (hit.collider.gameObject.tag == "House")
            {
                if (GameObject.FindGameObjectsWithTag("House").Length == 3 || GameObject.FindGameObjectsWithTag("GoodHouse").Length > 0)
                {
                    _simpleHousePanel.SetActive(true);
                    _houseDescriptionLabel.text = "Upgrade house? \nWood: -38 \nStone:-38 \nCapacity:+3";
                    _houseUpgradeButtonLabel.text = "Upgrade house";
                }
            }

            if (_events["storm"])
            {
                _sea.GetComponent<BoxCollider2D>().enabled = false;
            }
            else _sea.GetComponent<BoxCollider2D>().enabled = true;

            if (_events["wolves"])
            {
                _forest.GetComponent<BoxCollider2D>().enabled = false;
            }
            else _forest.GetComponent<BoxCollider2D>().enabled = true;

            if (_events["landslide"])
            {
                _mountains.GetComponent<BoxCollider2D>().enabled = false;
            }
            else _mountains.GetComponent<BoxCollider2D>().enabled = true;
        }

        foreach (CharacterInfo NPC in NPCs)
       {
           if(NPC.IsAssignedToForest && !NPC.atWorkingPlaceForest)
           {
                if (NPC.Agent.isStopped == true) NPC.Agent.isStopped = false;
                var workingPlace = workingPlacesForest.First(place => place.GetComponent<WorkingPlaceForest>().isFree == true);
                var direction = (workingPlace.transform.position - NPC.gameObject.transform.position).normalized;
                NPC.Animator.SetFloat("X", direction.x);
                NPC.Animator.SetFloat("Y", direction.y);

                NPC.Agent.SetDestination(workingPlace.transform.position);
           }

            if (NPC.IsAssignedToSea && !NPC.atWorkingPlaceSea)
            {
                if (NPC.Agent.isStopped == true) NPC.Agent.isStopped = false;
                var workingPlace = workingPlacesSea.First(place => place.GetComponent<WorkingPlaceSea>().isFree == true);
                var direction = (workingPlace.transform.position - NPC.gameObject.transform.position).normalized;
                NPC.Animator.SetFloat("X", direction.x);
                NPC.Animator.SetFloat("Y", direction.y);

                NPC.Agent.SetDestination(workingPlace.transform.position);

            }

            if (NPC.IsAssignedToMountains && !NPC.atWorkingPlaceMountains)
            {
                if (NPC.Agent.isStopped == true) NPC.Agent.isStopped = false;
                var workingPlace = workingPlacesMountains.First(place => place.GetComponent<WorkingPlaceMountains>().isFree == true);
                var direction = (workingPlace.transform.position - NPC.gameObject.transform.position).normalized;
                NPC.Animator.SetFloat("X", direction.x);
                NPC.Animator.SetFloat("Y", direction.y);

                NPC.Agent.SetDestination(workingPlace.transform.position);
            }

            if (!NPC.IsAssignedToForest && !NPC.IsAssignedToMountains && !NPC.IsAssignedToSea && NPC.gameObject.transform.position != NPC.initialPosition)
            {
                var direction = (NPC.initialPosition - NPC.gameObject.transform.position).normalized;
                NPC.Animator.SetFloat("X", direction.x);
                NPC.Animator.SetFloat("Y", direction.y);

                if (NPC.Agent.isStopped == true) NPC.Agent.isStopped = false;
                NPC.Agent.SetDestination(NPC.initialPosition);
            }
        }
    }

    public void Feed()
    {
        if (foodAmount > 0)
        {
            energy++;
            foodAmount--;
            energyBar.fillAmount = energy / MaxEnergy;
            _foodAmountLabel.text = foodAmount.ToString();
        }
        else
        {
            // add UI message 
        }
    }
    public void FeedNPC()
    {
        if (foodAmount > 0)
        {
            foreach (CharacterInfo NPC in NPCs)
            {
                if (NPC.feedMenuActive)
                {
                    NPC.energy++;
                    foodAmount--;
                    NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
                    _foodAmountLabel.text = foodAmount.ToString();
                }
            }
        }
        else
        {
            // add UI message 
        }
    }
    public void NPCExit()
    {
        NPCFeedPanel.SetActive(false);
        foreach (CharacterInfo NPC in NPCs)
        {
            if (NPC.feedMenuActive)
            {
                NPC.feedMenuActive = false;
            }
        }
    }

    public void UpgradeFoodWarehouse()
    {
        if (GameObject.FindGameObjectsWithTag("FoodWarehouse").Length < 1)
        {
            if (woodAmount >= 8 && stoneAmount >= 8 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 8;
                stoneAmount -= 8;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();
                maxFoodAmount = 20;
                Destroy(GameObject.FindGameObjectsWithTag("SimpleFoodWarehouse")[0]);
                Instantiate(_foodWarehouse);
                _maxFoodAmountLabel.text = maxFoodAmount.ToString();
                _FoodWarehouseUpgradePanel.SetActive(false);
                ChangePhase();
            }
            else if (woodAmount <= 8 || stoneAmount <= 8 || energy == 0)
            {
                Debug.Log("Not enough resourses or energy for upgrade.");
            }
        }
        else if (GameObject.FindGameObjectsWithTag("FoodWarehouse").Length == 1)
        {
            if (woodAmount >= 18 && stoneAmount >= 18 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 18;
                stoneAmount -= 18;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();
                maxFoodAmount = 40;
                Destroy(GameObject.FindGameObjectsWithTag("FoodWarehouse")[0]);
                Instantiate(_goodFoodWarehouse);
                _maxFoodAmountLabel.text = maxFoodAmount.ToString();

                _FoodWarehouseUpgradePanel.SetActive(false);
                ChangePhase();
            }
            else if (woodAmount <= 18 || stoneAmount <= 18 || energy == 0)
            {
                Debug.Log("Not enough resourses or energy for upgrade.");
            }
        }
    }
    public void UpgradeWoodWarehouse()
    {
        if (GameObject.FindGameObjectsWithTag("WoodWarehouse").Length < 1)
        {
            if (woodAmount >= 8 && stoneAmount >= 8 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 8;
                stoneAmount -= 8;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                maxWoodAmount = 20;
                _maxWoodAmountLabel.text = maxWoodAmount.ToString();
                Destroy(GameObject.FindGameObjectsWithTag("SimpleWoodWarehouse")[0]);
                Instantiate(_woodWarehouse);

                _WoodWarehouseUpgradePanel.SetActive(false);
                ChangePhase();
            }
            else if (woodAmount <= 8 || stoneAmount <= 8 || energy == 0)
            {
                Debug.Log("Not enough resourses or energy for upgrade.");
            }
        }
        else if (GameObject.FindGameObjectsWithTag("WoodWarehouse").Length == 1)
        {
            if (woodAmount >= 18 && stoneAmount >= 18 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 18;
                stoneAmount -= 18;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                maxWoodAmount = 40;
                Destroy(GameObject.FindGameObjectsWithTag("WoodWarehouse")[0]);
                Instantiate(_goodWoodWarehouse);
                _maxWoodAmountLabel.text = maxWoodAmount.ToString();

                _WoodWarehouseUpgradePanel.SetActive(false);
                ChangePhase();
            }
            else if (woodAmount <= 18 || stoneAmount <= 18 || energy == 0)
            {
                Debug.Log("Not enough resourses or energy for upgrade.");
            }
        }
    }
    public void UpgradeStoneWarehouse()
    {
        if (GameObject.FindGameObjectsWithTag("StoneWarehouse").Length < 1)
        {
            if (woodAmount >= 8 && stoneAmount >= 8 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 8;
                stoneAmount -= 8;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                maxStoneAmount = 20;
                _maxStoneAmountLabel.text = maxStoneAmount.ToString();
                Destroy(GameObject.FindGameObjectsWithTag("SimpleStoneWarehouse")[0]);
                Instantiate(_stoneWarehouse);

                _StoneWarehouseUpgradePanel.SetActive(false);
                ChangePhase();
            }
            else if (woodAmount <= 8 || stoneAmount <= 8 || energy == 0)
            {
                Debug.Log("Not enough resourses or energy for upgrade.");
            }
        }
        else if (GameObject.FindGameObjectsWithTag("StoneWarehouse").Length == 1)
        {
            if (woodAmount >= 18 && stoneAmount >= 18)
            {
                AssignAllToBuilding();

                woodAmount -= 18;
                stoneAmount -= 18;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                maxStoneAmount = 40;
                Destroy(GameObject.FindGameObjectsWithTag("StoneWarehouse")[0]);
                Instantiate(_goodStoneWarehouse);
                _maxStoneAmountLabel.text = maxStoneAmount.ToString();

                _StoneWarehouseUpgradePanel.SetActive(false);
                ChangePhase();
            }
            else if (woodAmount <= 18 || stoneAmount <= 18 || energy == 0)
            {
                Debug.Log("Not enough resourses or energy for upgrade.");
            }
        }
    }
    public void AddorUpgradeHouse()
    {
        if (GameObject.FindGameObjectsWithTag("House").Length == 3 || (GameObject.FindGameObjectsWithTag("GoodHouse").Length > 0 && GameObject.FindGameObjectsWithTag("GoodHouse").Length < 3))
        {
            if (woodAmount >= 38 && stoneAmount >= 38 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 38;
                stoneAmount -= 38;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                var newHouse = Instantiate(_goodHouse);
                var oldHouse = GameObject.FindGameObjectsWithTag("House")[0];
                newHouse.transform.position = oldHouse.transform.position;
                Destroy(oldHouse);
                livingPlaces = (GameObject.FindGameObjectsWithTag("House").Length - 1) * houseCapacity + GameObject.FindGameObjectsWithTag("GoodHouse").Length *goodHouseCapacity;
                _simpleHousePanel.SetActive(false);
                ChangePhase();
            }
            else
            {
                Debug.Log("Not enough resourses or energy to upgrade house.");
            }
        }
        if (GameObject.FindGameObjectsWithTag("SimpleHouse").Length == 3 || (GameObject.FindGameObjectsWithTag("House").Length > 0 && GameObject.FindGameObjectsWithTag("House").Length < 3 && GameObject.FindGameObjectsWithTag("GoodHouse").Length < 1))
        {
            if (woodAmount >= 19 && stoneAmount >= 19 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 19;
                stoneAmount -= 19;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                var newHouse = Instantiate(_house);
                var oldHouse = GameObject.FindGameObjectsWithTag("SimpleHouse")[0];
                newHouse.transform.position = oldHouse.transform.position;
                Destroy(oldHouse);
                livingPlaces = (GameObject.FindGameObjectsWithTag("SimpleHouse").Length -1) * simpleHouseCapacity + GameObject.FindGameObjectsWithTag("House").Length * houseCapacity;
                _simpleHousePanel.SetActive(false);
                ChangePhase();
            }
            else
            {
                Debug.Log("Not enough resourses or energy to upgrade house.");
            }
        }

        if (GameObject.FindGameObjectsWithTag("SimpleHouse").Length > 0 && GameObject.FindGameObjectsWithTag("SimpleHouse").Length < 3 && GameObject.FindGameObjectsWithTag("House").Length < 1)
        {
            if (woodAmount >= 9 && stoneAmount >= 9 && energy > 0)
            {
                AssignAllToBuilding();

                woodAmount -= 9;
                stoneAmount -= 9;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                var newHouse = Instantiate(_simpleHouse);
                if (GameObject.FindGameObjectsWithTag("SimpleHouse").Length == 3) newHouse.transform.position = new Vector3(_simpleHouse.transform.position.x + 5, _simpleHouse.transform.position.y, _simpleHouse.transform.position.z);
                livingPlaces = GameObject.FindGameObjectsWithTag("SimpleHouse").Length * simpleHouseCapacity;
                _simpleHousePanel.SetActive(false);
                ChangePhase();
            }
             else
            {
                Debug.Log("Not enough resourses or energy to add house.");
            }
        }
    }

    public void HandleInputDataForest(int value)
    {
        if (value == 1 && energy > 0)
        {
            _isAssignedToForest = true;
            _isAssignedToMountains = false;
            _isAssignedToSea = false;
             dropdownMountains.value = 0;
             dropdownSea.value = 0;            
        }
        else if (energy == 0)
        {
            dropdownForest.value = 0;
        }
    }
    public void HandleInputDataMountains(int value)
    {
        if (value == 1 && energy > 0)
        {
            _isAssignedToMountains = true;
            _isAssignedToForest = false;
            _isAssignedToSea = false;
           dropdownForest.value = 0;
           dropdownSea.value = 0;
        }
        else if (energy == 0)
        {
            dropdownMountains.value = 0;
        }
    }
    public void HandleInputDataSea(int value)
    {
        if (value == 1 && energy > 0)
        {
            _isAssignedToSea = true;
            _isAssignedToMountains = false;
            _isAssignedToForest = false;
            dropdownForest.value = 0;
            dropdownMountains.value = 0;
        }
        else if (energy == 0)
        {
            dropdownSea.value = 0;
        }
    }
    public void HandleInputDataSeaNPC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = true;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPC.value = 0;
            }

            foreach(TMP_Dropdown dropdown in NPCDropdowns)
            {
                if(value == dropdown.value && dropdown != dropdownSeaNPC)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for(int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
                && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
                {
                    NPCs[i-1].IsAssignedToSea = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownSeaNPC.value && i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
            && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
            {
                NPCs[i - 1].IsAssignedToSea = false;
            }
        }
    }
    public void HandleInputDataSeaNPCA(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = true;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPCA.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownSeaNPCA)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownSeaNPC.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
                && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToSea = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownSeaNPC.value && i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
            && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
            {
                NPCs[i - 1].IsAssignedToSea = false;
            }
        }
    }
    public void HandleInputDataSeaNPCB(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = true;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPCB.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownSeaNPCB)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownSeaNPCA.value && i != dropdownSeaNPC.value && i != dropdownSeaNPCC.value
                && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToSea = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownSeaNPC.value && i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
            && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
            {
                NPCs[i - 1].IsAssignedToSea = false;
            }
        }
    }
    public void HandleInputDataSeaNPCC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = true;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPCC.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownSeaNPCC)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPC.value
                && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToSea = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownSeaNPC.value && i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
            && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
            {
                NPCs[i - 1].IsAssignedToSea = false;
            }
        }
    }
    public void HandleInputDataSeaNPCD(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = true;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPCD.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownSeaNPCD)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
                && i != dropdownSeaNPC.value && i != dropdownSeaNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToSea = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownSeaNPC.value && i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
            && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
            {
                NPCs[i - 1].IsAssignedToSea = false;
            }
        }
    }
    public void HandleInputDataSeaNPCE(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = true;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPCE.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownSeaNPCE)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
                && i != dropdownSeaNPCD.value && i != dropdownSeaNPC.value)
                {
                    NPCs[i - 1].IsAssignedToSea = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownSeaNPC.value && i != dropdownSeaNPCA.value && i != dropdownSeaNPCB.value && i != dropdownSeaNPCC.value
            && i != dropdownSeaNPCD.value && i != dropdownSeaNPCE.value)
            {
                NPCs[i - 1].IsAssignedToSea = false;
            }
        }
    }
    public void HandleInputDataMountainsNPC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToMountains = true;
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPC.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownMountainsNPC)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
                && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToMountains = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
            && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
            {
                NPCs[i - 1].IsAssignedToMountains = false;
            }
        }
    }
    public void HandleInputDataMountainsNPCA (int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = true;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPCA.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownMountainsNPCA)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
                && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToMountains = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
            && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
            {
                NPCs[i - 1].IsAssignedToMountains = false;
            }
        }
    }
    public void HandleInputDataMountainsNPCB(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = true;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPCB.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownMountainsNPCB)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCC.value
                && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToMountains = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
            && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
            {
                NPCs[i - 1].IsAssignedToMountains = false;
            }
        }
    }
    public void HandleInputDataMountainsNPCC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = true;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPCC.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownMountainsNPCC)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCA.value
                && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToMountains = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
            && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
            {
                NPCs[i - 1].IsAssignedToMountains = false;
            }
        }
    }
    public void HandleInputDataMountainsNPCD(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = true;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPCD.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownMountainsNPCD)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
                && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToMountains = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
            && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
            {
                NPCs[i - 1].IsAssignedToMountains = false;
            }
        }
    }
    public void HandleInputDataMountainsNPCE(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = true;
                NPC.IsAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPCE.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownMountainsNPCE)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
                && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCA.value)
                {
                    NPCs[i - 1].IsAssignedToMountains = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownMountainsNPC.value && i != dropdownMountainsNPCA.value && i != dropdownMountainsNPCB.value && i != dropdownMountainsNPCC.value
            && i != dropdownMountainsNPCD.value && i != dropdownMountainsNPCE.value)
            {
                NPCs[i - 1].IsAssignedToMountains = false;
            }
        }
    }
    public void HandleInputDataForestNPC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToForest = true;
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPC.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownForestNPC)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
                && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToForest = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
            && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
            {
                NPCs[i - 1].IsAssignedToForest = false;
            }
        }
    }
    public void HandleInputDataForestNPCA(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = true;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPCA.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownForestNPCA)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownForestNPC.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
                && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToForest = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
            && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
            {
                NPCs[i - 1].IsAssignedToForest = false;
            }
        }
    }
    public void HandleInputDataForestNPCB(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = true;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPCB.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownForestNPCB)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCC.value
                && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToForest = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
            && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
            {
                NPCs[i - 1].IsAssignedToForest = false;
            }
        }
    }
    public void HandleInputDataForestNPCC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = true;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPCC.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownForestNPCC)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownForestNPC.value && i != dropdownForestNPCB.value && i != dropdownForestNPCA.value
                && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToForest = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
            && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
            {
                NPCs[i - 1].IsAssignedToForest = false;
            }
        }
    }
    public void HandleInputDataForestNPCD(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = true;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPCD.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownForestNPCD)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownForestNPC.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
                && i != dropdownForestNPCA.value && i != dropdownForestNPCE.value)
                {
                    NPCs[i - 1].IsAssignedToForest = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
            && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
            {
                NPCs[i - 1].IsAssignedToForest = false;
            }
        }
    }
    public void HandleInputDataForestNPCE(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0)
            {
                NPC.IsAssignedToSea = false;
                NPC.IsAssignedToMountains = false;
                NPC.IsAssignedToForest = true;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPCE.value = 0;
            }

            foreach (TMP_Dropdown dropdown in NPCDropdowns)
            {
                if (value == dropdown.value && dropdown != dropdownForestNPCE)
                {
                    dropdown.value = 0;
                }
            }
        }
        else if (value == 0)
        {
            for (int i = 1; i <= NPCs.Count; i++)
            {
                if (i != dropdownForestNPC.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
                && i != dropdownForestNPCD.value && i != dropdownForestNPCA.value)
                {
                    NPCs[i - 1].IsAssignedToForest = false;
                }
            }
        }
        for (int i = 1; i <= NPCs.Count; i++)
        {
            if (i != dropdownForestNPC.value && i != dropdownForestNPCA.value && i != dropdownForestNPCB.value && i != dropdownForestNPCC.value
            && i != dropdownForestNPCD.value && i != dropdownForestNPCE.value)
            {
                NPCs[i - 1].IsAssignedToForest = false;
            }
        }
    }

    public void ChangePhase()
    {
        if (_isAssignedToForest)
        {
                woodAmount += UnityEngine.Random.Range(1, 3) + goodAxesBonus;
                if (woodAmount > maxWoodAmount) woodAmount = maxWoodAmount;

                _woodAmountLabel.text = woodAmount.ToString();
                _isAssignedToForest = false;
                energy--;
                energyBar.fillAmount = energy / MaxEnergy;
        }
        else if (_isAssignedToMountains)
        {
                stoneAmount += UnityEngine.Random.Range(1, 3);
                if (stoneAmount > maxStoneAmount) stoneAmount = maxStoneAmount;

                _stoneAmountLabel.text = stoneAmount.ToString();
                dropdownMountains.value = 0;
                _isAssignedToMountains = false;
                energy--;
                energyBar.fillAmount = energy / MaxEnergy;
        }
        else if (_isAssignedToSea )
        {
                foodAmount += UnityEngine.Random.Range(1, 3) + fishingDayBonus;
                if (foodAmount > maxFoodAmount) foodAmount = maxFoodAmount;

                _foodAmountLabel.text = foodAmount.ToString();
                dropdownSea.value = 0;
                _isAssignedToSea = false;
                energy--;
                energyBar.fillAmount = energy / MaxEnergy;
        }

        foreach (CharacterInfo NPC in NPCs)
        {
            if (NPC.IsAssignedToSea)
            {
                foodAmount += UnityEngine.Random.Range(1, 3) + NPC.foodBonus + fishingDayBonus;
                if (foodAmount > maxFoodAmount) foodAmount = maxFoodAmount;

                _foodAmountLabel.text = foodAmount.ToString();
                NPC.IsAssignedToSea = false;
                NPC.energy--;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
            }

            if (NPC.IsAssignedToMountains)
            {
                stoneAmount += UnityEngine.Random.Range(1, 3) + NPC.stoneBonus;
                if (stoneAmount > maxStoneAmount) stoneAmount = maxStoneAmount;

                _stoneAmountLabel.text = stoneAmount.ToString();
                dropdownMountainsNPC.value = 0;
                NPC.IsAssignedToMountains = false;
                NPC.energy--;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
            }

            if (NPC.IsAssignedToForest )
            {
                woodAmount += UnityEngine.Random.Range(1, 3) + NPC.woodBonus + goodAxesBonus;
                if (woodAmount > maxWoodAmount) woodAmount = maxWoodAmount;

                _woodAmountLabel.text = woodAmount.ToString();
                NPC.IsAssignedToForest = false;
                NPC.energy--;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
            }
        }

        if (time < 24)
        {
            time += 4;
            _timeChangeLabel.text = time.ToString();
        }
        else if (time == 24)
        {
            time = 8;
            _timeChangeLabel.text = time.ToString();

            days++;
            _dayCountLabel.text = days.ToString();

            if (energy < MaxEnergy)
            {
                energy += 1;
                energyBar.fillAmount = energy / MaxEnergy;
            }

            if (livingPlaces > NPCs.Count + 1)
            {
                AddNPC();
            }
            else Debug.Log("Not enough living places to add NPC");


            RandomEvent();
            DropdownsUpdate();

            if (_events["beavers"])
            {
                woodAmount -= 5;
                if (woodAmount < 0) woodAmount = 0;

                _woodAmountLabel.text = woodAmount.ToString();
            }

            if (_events["foodTheft"])
            {
                foodAmount -= 5;
                if (foodAmount < 0) foodAmount = 0;

                _foodAmountLabel.text = foodAmount.ToString();
            }

            if (_events["fishingDay"])
            {
                fishingDayBonus = 1;
            }
            else fishingDayBonus = 0;

            if (_events["goodAxes"])
            {
                goodAxesBonus = 1;
            }
            else goodAxesBonus = 0;

        }

        foreach (TMP_Dropdown dropdown in Dropdowns)
        {
            dropdown.value = 0;
        }
    }
    public void RandomEvent()
    {
        foreach (var key in _events.Keys.ToList())
        {
            _events[key] = false;
        }

        System.Random rand = new System.Random();
        var randomEvent = _events.ElementAt(rand.Next(0, _events.Count)).Key;
        _events[randomEvent] = true;
        _randomEventPanel.SetActive(true);

        switch (randomEvent)
        {
            case "storm":
                _randomEventDescription.text = "The sea's raging with a storm today.No chance to catch fish...";
                eventImage.sprite = eventSprites[0];
                break;
            case "fishingDay":
                _randomEventDescription.text = "It's a splendid day for fishing. The catch will surely be bountiful!";
                eventImage.sprite = eventSprites[1];
                break;
            case "wolves":
                _randomEventDescription.text = "All night the wolves howled in the forest. We better not to go there.";
                eventImage.sprite = eventSprites[2];
                break;
            case "goodAxes":
                _randomEventDescription.text = "Axes are sharpened, ready for forest logging to be more productive!";
                eventImage.sprite = eventSprites[3];
                break;
            case "landslide":
                _randomEventDescription.text = "A landslide in the mountains. We better not to go there.";
                eventImage.sprite = eventSprites[4];
                break;
            case "beavers":
                _randomEventDescription.text = "Cursed beavers gnawed at our tree!";
                eventImage.sprite = eventSprites[5];
                break;
            case "foodTheft":
                _randomEventDescription.text = "Someone ate some of our supplies last night. Nobody can be trusted...";
                eventImage.sprite = eventSprites[6];
                break;
        }
    }

    public void AddNPC()
    {
        var npc = Instantiate(NPCPrefabs[indexPrefabs]);
        indexPrefabs++;
        var position = npc.transform.position;
        var npcInfo = npc.GetComponent<CharacterInfo>();
        var animator = npc.GetComponent<Animator>();
        var rb = npc.GetComponent<Rigidbody2D>();
        npcInfo.Init(names, indexNames, position, animator, rb);
        indexNames++;
        NPCs.Add(npcInfo);
        NPCNames.Add(npcInfo.characterName);
        peopleAmount++;
        _peopleAmountLabel.text = peopleAmount.ToString();
    }

    public void DropdownsUpdate()
    {
        foreach (TMP_Dropdown dropdown in NPCDropdowns)
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(NPCNames);
        }

    }

    public void AssignAllToBuilding()
    {
        foreach (CharacterInfo NPC in NPCs)
        {
            NPC.IsAssignedToForest = false;
            NPC.IsAssignedToMountains = false;
            NPC.IsAssignedToSea = false;

            if (NPC.energy > 0)
            {
                NPC.energy -= 1;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
            }
        }

        foreach (TMP_Dropdown dropdown in Dropdowns)
        {
            dropdown.value = 0;
        }

        _isAssignedToForest = false;
        _isAssignedToMountains = false;
        _isAssignedToSea = false;

        if (energy > 0)
        {
            energy -= 1;
            energyBar.fillAmount = energy / MaxEnergy;
        }
    }

    public void CheckWin()
    {
        if (NPCs.Count == 20)
        {
            winPanel.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }
}