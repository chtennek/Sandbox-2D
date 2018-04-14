using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IMoveable {}

public class PokemonBehaviour : MonoBehaviour, IMoveable {
    public Pokemon pokemon;

    private Text nameDisplay;

    private void Awake() {
        nameDisplay = GetComponentInChildren<Text>();
    }

    private void OnValidate() {
        nameDisplay = GetComponentInChildren<Text>();
        if (nameDisplay != null)
            nameDisplay.text = pokemon.Name;
    }
}

[System.Serializable]
public class Pokemon {
    public PokemonSpecies species;
    public string nickname = "";
    public int level = 5;
    public int maxHP = 20;
    public int currentHP = 20;

    public string Name { get { return (species == null) ? "" : species.speciesName; }}

    [Space]

    public int ATK = 10;
    public int DEF = 10;
    public int spATK = 10;
    public int spDEF = 10;
}