using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AI;

public class CharacterInfo : MonoBehaviour
{
    [SerializeField] public string characterName;
    [SerializeField] public int woodBonus;
    [SerializeField] public int stoneBonus;
    [SerializeField] public int foodBonus;
    [SerializeField] public float energy = 0;
    [SerializeField] public float MaxEnergy = 4;
    [SerializeField] public float speed;
    [SerializeField] public Vector3 initialPosition;
    [SerializeField] private TextMeshProUGUI _nameLabel;
    [SerializeField] public Image energyBar;
    public Animator Animator { get; set; }
    public Rigidbody2D Rigidbody { get; set; }
    public NavMeshAgent Agent { get; set; }

    public bool IsAssignedToForest {get; set; }
    public bool IsAssignedToMountains { get; set; }
    public bool IsAssignedToSea { get; set; }
    public bool feedMenuActive = false;
    public bool atWorkingPlaceSea = false;
    public bool atWorkingPlaceForest = false;
    public bool atWorkingPlaceMountains = false;

    public void Init(string[] names, int index, Vector3 position, Animator animator, Rigidbody2D rigidbody2D)
    {
        characterName = names[index];
        initialPosition = position;
        Animator = animator;
        Rigidbody = rigidbody2D;
        Rigidbody.gravityScale = 0;
        woodBonus = Random.Range(0, 2);
        stoneBonus = Random.Range(0, 2);
        foodBonus = Random.Range(0, 2);
        MaxEnergy = 4;
        speed = 4;
        energy = MaxEnergy;
        _nameLabel.text = characterName;
    }

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        Agent.updateRotation = false;
        Agent.updateUpAxis = false;
    }
}
