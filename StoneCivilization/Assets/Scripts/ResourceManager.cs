using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
    [SerializeField] private GameObject NPCFeedPanel;
    [SerializeField] private GameObject _simpleFoodWarehouseUpgradePanel;
    [SerializeField] private GameObject _simpleHousePanel;
    [SerializeField] private GameObject _randomEventPanel;
    [SerializeField] private GameObject _NPC;
    [SerializeField] private GameObject _foodWarehouse;
    [SerializeField] private GameObject _simpleHouse;
    [SerializeField] TMP_Dropdown dropdownForest;
    [SerializeField] TMP_Dropdown dropdownMountains;
    [SerializeField] TMP_Dropdown dropdownSea;
    [SerializeField] TMP_Dropdown dropdownSeaNPC;
    [SerializeField] TMP_Dropdown dropdownMountainsNPC;
    [SerializeField] TMP_Dropdown dropdownForestNPC;
    [SerializeField] Image energyBar;
    [SerializeField] GameObject _sea;
    [SerializeField] GameObject _forest;
    [SerializeField] GameObject _mountains;

    public Dictionary<string, bool> _events = new Dictionary<string, bool>();

    public string[] names = { "Tom", "Bob", "Kaseki", "Chrome", "Kohaku", "Suika", "Kinro", "Ginro", "Gen", "Magma", "Nikki", "Ukyo", "Minami", "Yo", "Ryusui", "Francois", "Tsukasa", "Hyoga", "Homura", "Yuzuriha" };
    int index = 0;

    public List<CharacterInfo> NPCs;
    public List<string> NPCNames;
    

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

    public void Feed()
    {
        if(foodAmount > 0)
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
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

           RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

                        
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
                _simpleFoodWarehouseUpgradePanel.SetActive(true);
            }
            else if (hit.collider.gameObject.tag == "SimpleHouse")
            {
                _simpleHousePanel.SetActive(true);
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
           if(NPC.isAssignedToForest)
           {
              NPC.gameObject.transform.position = Vector3.MoveTowards(NPC.gameObject.transform.position, new Vector3(_forest.transform.position.x+5, _forest.transform.position.y, _forest.transform.position.z), NPC.speed * Time.deltaTime);
            }

            if (NPC.isAssignedToSea)
            {
                NPC.gameObject.transform.position = Vector3.MoveTowards(NPC.gameObject.transform.position, new Vector3(_sea.transform.position.x -8.5f, _sea.transform.position.y, _sea.transform.position.z), NPC.speed * Time.deltaTime);
            }

            if (NPC.isAssignedToMountains)
            {
                NPC.gameObject.transform.position = Vector3.MoveTowards(NPC.gameObject.transform.position, new Vector3(_mountains.transform.position.x, _mountains.transform.position.y + 3.5f, _mountains.transform.position.z), NPC.speed * Time.deltaTime);
            }
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

    public void UpgradeSimpleFoodWarehouse()
    {
        if (woodAmount >= 6 && stoneAmount >= 6)
        {
            woodAmount -= 6;
            stoneAmount -= 6;
            _woodAmountLabel.text = woodAmount.ToString();
            _stoneAmountLabel.text = stoneAmount.ToString();

            maxFoodAmount = 20;
            Destroy(GameObject.FindGameObjectsWithTag("SimpleFoodWarehouse")[0]);
            Instantiate(_foodWarehouse);
            _maxFoodAmountLabel.text = maxFoodAmount.ToString();
            _simpleFoodWarehouseUpgradePanel.SetActive(false);
        }
    }

    public void AddSimpleHouse()
    {
        if (woodAmount >= 9 && stoneAmount >= 9)
        {
            if (GameObject.FindGameObjectsWithTag("SimpleHouse").Length < 3)
            {
                woodAmount -= 9;
                stoneAmount -= 9;
                _woodAmountLabel.text = woodAmount.ToString();
                _stoneAmountLabel.text = stoneAmount.ToString();

                var newHouse = Instantiate(_simpleHouse);
                if (GameObject.FindGameObjectsWithTag("SimpleHouse").Length == 3) newHouse.transform.position = new Vector3(_simpleHouse.transform.position.x + 3, _simpleHouse.transform.position.y, _simpleHouse.transform.position.z);
                livingPlaces = GameObject.FindGameObjectsWithTag("SimpleHouse").Length * simpleHouseCapacity;
                _simpleHousePanel.SetActive(false);
            }
            else
            {
                Debug.Log("Not enough space for new houses, upgrade existing ones.");
            }
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
                NPC.isAssignedToSea = true;
                NPC.isAssignedToMountains = false;
                NPC.isAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownSeaNPC.value = 0;
            }

            if (value == dropdownMountainsNPC.value || value  == dropdownForestNPC.value)
            {
                dropdownMountainsNPC.value = 0;
                dropdownForestNPC.value = 0;
            }
        }
    }

    public void HandleInputDataMountainsNPC(int value)
    {
        if (value > 0)
        {
            var NPC = NPCs[value - 1];
            if (NPC.energy > 0 )
            {
                NPC.isAssignedToMountains = true;
                NPC.isAssignedToSea = false;
                NPC.isAssignedToForest = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownMountainsNPC.value = 0;
            }

            if(value == dropdownSeaNPC.value || value == dropdownForestNPC.value)
            {
                dropdownSeaNPC.value = 0;
                dropdownForestNPC.value = 0;
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
                NPC.isAssignedToForest = true;
                NPC.isAssignedToSea = false;
                NPC.isAssignedToMountains = false;
            }
            else if (NPC.energy == 0)
            {
                dropdownForestNPC.value = 0;
            }

            if (value == dropdownSeaNPC.value || value == dropdownMountainsNPC.value)
            {
                dropdownSeaNPC.value = 0;
                dropdownMountainsNPC.value = 0;
            }
        }
    }

    public void ChangePhase()
    {
        if (_isAssignedToForest && energy > 0)
        {
            if (woodAmount < maxWoodAmount)
            {
                woodAmount += UnityEngine.Random.Range(1, 3) + goodAxesBonus;
                if (woodAmount > maxWoodAmount) woodAmount = maxWoodAmount;

                _woodAmountLabel.text = woodAmount.ToString();
                dropdownForest.value = 0;
                _isAssignedToForest = false;
                energy--;
                energyBar.fillAmount = energy / MaxEnergy;
            }
            else
            {
                dropdownForest.value = 0;
                _isAssignedToForest = false;
            }
        }
        else if (_isAssignedToMountains && energy > 0)
        {
            if (stoneAmount < maxStoneAmount)
            {
                stoneAmount += UnityEngine.Random.Range(1, 3);
                if (stoneAmount > maxStoneAmount) stoneAmount = maxStoneAmount;

                _stoneAmountLabel.text = stoneAmount.ToString();
                dropdownMountains.value = 0;
                _isAssignedToMountains = false;
                energy--;
                energyBar.fillAmount = energy / MaxEnergy;
            }
            else
            {
                dropdownMountains.value = 0;
                _isAssignedToMountains = false;
            }
        }
        else if (_isAssignedToSea && energy > 0)
        {
            if (foodAmount < maxFoodAmount)
            {
                foodAmount += UnityEngine.Random.Range(1, 3) + fishingDayBonus;
                if (foodAmount > maxFoodAmount) foodAmount = maxFoodAmount;

                _foodAmountLabel.text = foodAmount.ToString();
                dropdownSea.value = 0;
                _isAssignedToSea = false;
                energy--;
                energyBar.fillAmount = energy / MaxEnergy;
            }
            else
            {
                dropdownSea.value = 0;
                _isAssignedToSea = false;
            }
        }

        foreach (CharacterInfo NPC in NPCs)
        {
            if (NPC.isAssignedToSea && NPC.energy > 0 && foodAmount < maxFoodAmount)
            {
                foodAmount += UnityEngine.Random.Range(1, 3) + NPC.foodBonus + fishingDayBonus;
                if (foodAmount > maxFoodAmount) foodAmount = maxFoodAmount;

                _foodAmountLabel.text = foodAmount.ToString();
                dropdownSeaNPC.value = 0;
                NPC.isAssignedToSea = false;
                NPC.energy--;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
                Debug.Log(NPC.energy);
            }
            else if (NPC.isAssignedToSea && (NPC.energy == 0 || foodAmount >= maxFoodAmount))
            {
                dropdownSeaNPC.value = 0;
                NPC.isAssignedToSea = false;
            }

            if (NPC.isAssignedToMountains && NPC.energy > 0 && stoneAmount < maxStoneAmount)
            {
                stoneAmount += UnityEngine.Random.Range(1, 3) + NPC.stoneBonus;
                if (stoneAmount > maxStoneAmount) stoneAmount = maxStoneAmount;

                _stoneAmountLabel.text = stoneAmount.ToString();
                dropdownMountainsNPC.value = 0;
                NPC.isAssignedToMountains = false;
                NPC.energy--;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
            }
            else if (NPC.isAssignedToMountains && (NPC.energy == 0 || stoneAmount >= maxStoneAmount))
            {
                dropdownMountainsNPC.value = 0;
                NPC.isAssignedToMountains = false;
            }

            if (NPC.isAssignedToForest && NPC.energy > 0 && woodAmount < maxWoodAmount)
            {
                woodAmount += UnityEngine.Random.Range(1, 3) + NPC.woodBonus + goodAxesBonus;
                if (woodAmount > maxWoodAmount) woodAmount = maxWoodAmount;

                _woodAmountLabel.text = woodAmount.ToString();
                dropdownForestNPC.value = 0;
                NPC.isAssignedToForest = false;
                NPC.energy--;
                NPC.energyBar.fillAmount = NPC.energy / NPC.MaxEnergy;
            }
            else if (NPC.isAssignedToForest && (NPC.energy == 0 || woodAmount >= maxWoodAmount))
            {
                dropdownForestNPC.value = 0;
                NPC.isAssignedToForest = false;
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

            energy += 1;
            energyBar.fillAmount = energy / MaxEnergy;

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
                break;
            case "fishingDay":
                _randomEventDescription.text = "It's a splendid day for fishing. The catch will surely be bountiful!";
                break;
            case "wolves":
                _randomEventDescription.text = "All night the wolves howled in the forest. We better not to go there.";
                break;
            case "goodAxes":
                _randomEventDescription.text = "Axes are sharpened, ready for forest logging to be more productive!";
                break;
            case "landslide":
                _randomEventDescription.text = "A landslide in the mountains. We better not to go there.";
                break;
            case "beavers":
                _randomEventDescription.text = "Cursed beavers gnawed at our tree!";
                break;
            case "foodTheft":
                _randomEventDescription.text = "Someone ate some of our supplies last night. Nobody can be trusted...";
                break;
        }
    }

    public void AddNPC()
    {
        var npc = Instantiate(_NPC);
        var npcInfo = npc.GetComponent<CharacterInfo>();
        npcInfo.Init(names, index);
        npc.transform.position = UnityEngine.Random.insideUnitCircle * 12;
        index++;
        NPCs.Add(npcInfo);
        NPCNames.Add(npcInfo.characterName);
        peopleAmount++;
        _peopleAmountLabel.text = peopleAmount.ToString();
    }

    public void DropdownsUpdate()
    {
        dropdownSeaNPC.ClearOptions();
        dropdownSeaNPC.AddOptions(NPCNames);
        dropdownMountainsNPC.ClearOptions();
        dropdownMountainsNPC.AddOptions(NPCNames);
        dropdownForestNPC.ClearOptions();
        dropdownForestNPC.AddOptions(NPCNames);
    }
}
