using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private Weapon weaponHolder; // Weapon object assigned in the Inspector
    private Weapon weapon; // Weapon to be picked up by the player
    private BoxCollider2D boxCollider; // BoxCollider2D component

    // Called when the script instance is being loaded
    void Awake()
    {
        // Initialize weapon with weaponHolder
        weapon = weaponHolder;

        // Get the BoxCollider2D component attached to the GameObject
        boxCollider = GetComponent<BoxCollider2D>();

        // Ensure the BoxCollider2D is set as a trigger
        if (boxCollider != null)
        {
            boxCollider.isTrigger = true;
        }
    }

    // Called before the first frame update
    void Start()
    {
        // If weapon is not null, disable all visual components initially
        if (weapon != null)
        {
            TurnVisual(false);
        }
    }

    // Called when another collider enters the trigger collider attached to the GameObject
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding is the player by checking the tag
        if (other.gameObject.CompareTag("Player"))
        {
            // Set the parent of the weapon to be the player
            weapon.transform.parent = other.transform;

            // Enable the weapon's visuals
            TurnVisual(true, weapon);

            // Optionally, you could disable the pickup object to prevent further interaction
            gameObject.SetActive(false); // Disable the WeaponPickup object after pickup
        }
    }

    // Method to enable or disable all visual components of the weapon
    void TurnVisual(bool on)
    {
        // Use the overloaded method to enable/disable visuals on the main weapon
        TurnVisual(on, weapon);
    }

    // Overloaded method to enable weapon visuals with a specified weapon
    void TurnVisual(bool on, Weapon weapon)
    {
        if (weapon != null)
        {
            // Enable or disable the Animator component
            Animator animator = weapon.GetComponent<Animator>();
            if (animator != null)
            {
                animator.enabled = on;
            }

            // Enable or disable the SpriteRenderer component
            SpriteRenderer spriteRenderer = weapon.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = on;
            }
        }
    }
}
