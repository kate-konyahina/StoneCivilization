using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] public int woodBonus;
    [SerializeField] public int stoneBonus;
    [SerializeField] public int foodBonus;
    [SerializeField] public float energy = 0;
    [SerializeField] public float MaxEnergy = 4;
    [SerializeField] public float speed;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] public Image energyBar;

    public bool isAssignedToForest = false;
    public bool isAssignedToMountains = false;
    public bool isAssignedToSea = false;
    public bool feedMenuActive = false;

    public CharacterInfo( string characterName, int woodBonus, int stoneBonus, int foodBonus)
    {
        this.characterName = characterName;
        this.woodBonus = woodBonus;
        this.stoneBonus = stoneBonus;
        this.foodBonus = foodBonus;
    }

    public void Init(string[] names, int index)
    {
        characterName = names[index];
        woodBonus = Random.Range(0, 1);
        stoneBonus = Random.Range(0, 1);
        foodBonus = Random.Range(0, 1);
        MaxEnergy = 4;
        speed = 2;
        energy = MaxEnergy;
        _nameLabel.text = characterName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
