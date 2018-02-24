using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokemonType {
    Water,
    Fire,
    Grass,
}

[CreateAssetMenu(fileName = "Species", menuName = "Pokemon/Species")]
public class PokemonSpecies : EntityBase {
    public string speciesName;
    public PokemonType type;
}
