using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PokemonActorSprite {
    Down,
    Left,
    Up
}

public class PokemonActorBehaviour : MonoBehaviour {
    public Actor actor;
    public Dialogue dg;

    private SpriteRenderer rend;

    private void Awake() {
        rend = GetComponent<SpriteRenderer>();
    }

    public void InteractFrom(Vector2 position) {
        Face(position - (Vector2)transform.position);
    }

    public void Face(Vector2 direction) {
        rend.sprite = actor.sprites[0];
    }
}
