using UnityEngine;

namespace Commander.Examples
{
    public class HealthManager : MonoBehaviour
    {
        private int _currentHealth = 100;

        private void HealthChange(int health)
        {
            _currentHealth += health;
            Debug.Log("Player health is: " + _currentHealth);
        }

        #region Runtime Commands

        [Command("give_health", "Gives x amount of health to player")]
        public void AddHealthCommand(int healthToGive)
        {
            // Flip value if user inputted negative value
            if (healthToGive < 0)
            {
                healthToGive = -healthToGive;
            }
            HealthChange(healthToGive);
        }

        [Command("take_health", "Takes x amount of health from the player")]
        public void RemoveHealthCommand(int healthToTake)
        {
            // Flip value if user inputted negative value
            if (healthToTake < 0)
            {
                healthToTake = -healthToTake;
            }
            HealthChange(-healthToTake);
        }

        #endregion
    }
}