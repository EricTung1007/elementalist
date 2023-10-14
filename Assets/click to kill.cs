using UnityEngine;

public class ClickToDamage : MonoBehaviour
{
    public health enemyHealth; // Reference to the enemy's health script

    void Update()
    {
        // Check for a mouse click
        if (Input.GetMouseButtonDown(0)) // 0 corresponds to the left mouse button
        {
            // Apply 1 point of damage to the enemy's health
            enemyHealth.Damage(1);
        }
    }
}
