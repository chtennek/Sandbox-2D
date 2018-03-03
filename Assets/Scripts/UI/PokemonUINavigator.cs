using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokemonUINavigator : UINavigator {
    public Text textHP;
    public Text textATK;

    private PokemonBehaviour m_selectedPokemon;
    public PokemonBehaviour SelectedPokemon {
        get { return m_selectedPokemon; }
        set {
            m_selectedPokemon = value;
            UpdateStatsMenu();
        }
    }

    protected override void Update() {
        if (input.GetButtonDown(submitButton) && Selected != null)
        {
            PokemonBehaviour pokemon = Selected.GetComponent<PokemonBehaviour>();
            if (pokemon != null)
                SelectedPokemon = pokemon;
        }

        base.Update();
    }

    private void UpdateStatsMenu() {
        Pokemon pokemon = SelectedPokemon.pokemon;
        if (textHP != null)
            textHP.text = pokemon.currentHP + "/" + pokemon.maxHP;
        if (textATK != null)
            textATK.text = pokemon.ATK.ToString();
    }
}
