using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipCharacterGlow : MonoBehaviour
{

    [SerializeField] Transform character;
    [SerializeField] SpriteRenderer glowSprite;

    void Update()
    {
        if(character.transform.localScale.x < 0)
        {
            glowSprite.flipX = true;
        }
        else
        {
            glowSprite.flipX = false;
        }
    }
}
