using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI staminaBar;
    [SerializeField] private TextMeshProUGUI healthBar;
    [SerializeField] private TextMeshProUGUI dashBar;
    [SerializeField] private TextMeshProUGUI rollBar;

    private float staminaUsage = 0.3f; // players base stamina usage per action
    private float speed = 2.5f; // players speed
    private GameObject Player;
    private Rigidbody2D rb;
    private float stamina = 20; // players current stamina
    private float health = 20; // players current health
    private float staminaMax = 20; // players max stamina
    private float healthMax = 20; // players max health
    private float staminaRegen = 0.01f; // multiple these by current each second (they are percents)
    private float healthRegen = 0.0f; // multiple these by current each second (they are percents)
    private float dash = 0f; // how many seconds left untill ready
    private float roll = 0f; // how many seconds left untill ready
    private float dashMax = 10f; // how long it takes in seconds for a dash to recharge
    private float rollMax = 20f; // how long it takes in seconds for a roll to recharge

    // bools
    private bool isMoving = false;
    private bool isRunning = false;
    private bool RunningCoroutine = false; // is the running coroutine running
    private bool alive = true;
    private void Start()
    {
        Player = this.gameObject;
        rb = Player.GetComponent<Rigidbody2D>();
        StartCoroutine(RegenStats());
        StartCoroutine(RegenStamina()); // stamin is separate due to its need to stop for one second before resuming
        // get needed objects for player and start regen coroutines
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && isMoving)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
        // check if trying to run
        SetInfoBars();
        // display the current stats values to text element
    }
    private void FixedUpdate()
    {
        float DirX = Input.GetAxis("Horizontal");
        float DirY = Input.GetAxis("Vertical");
        // WASD or arrow keys input

        if (DirX != 0 || DirY != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        // check if move keys are pressed

        if (DirX != 0 && DirY != 0)
        {
            DirX *= 0.8f;
            DirY *= 0.8f;
            // if going diagnaly slow the player down
        }

        rb.velocity = new Vector2(DirX * speed, DirY * speed);
        // move Player in proper direction if walking or running

        if (isRunning && !RunningCoroutine)
        {
            StartCoroutine(Running());
            // if not running and conditions met -> run
        }
    }

    void SetInfoBars()
    {
        staminaBar.text = "Stamina: " + Mathf.Round(stamina * 100) / 100;
        healthBar.text = "Health: " + Mathf.Round(health * 100) / 100;
        // display stamina and heath to a text element
        if(dash + dashMax == dashMax)
        {
            dashBar.text = "Dash: Ready";
        }
        else
        {
            dashBar.text = "Dash: " + dash;
        }
        // display time left until dash ready or if it is ready
        if (roll + rollMax == rollMax)
        {
            rollBar.text = "Roll: Ready";
        }
        else
        {
            rollBar.text = "roll: " + roll;
        }
        // display time left until dash ready or if it is ready
    }

    IEnumerator RegenStats()
    {
        while (alive)
        {
            health = ChangeStats(health, healthRegen, healthMax);
            // try to change stats every tick (1/10 second for now)
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    IEnumerator RegenStamina()
    {
        float temp = stamina;
        // ^ for checking if they just stopped running
        while (alive)
        {
            yield return new WaitForSeconds(0.1f);
            if(stamina < temp)
            {
                yield return new WaitForSeconds(1f);
                // if there is less stamina than last tick -> wait to regen stamina
            }
            else
            {
                stamina = ChangeStats(stamina, staminaRegen, staminaMax);
                // if stamina is >= than increase
            }
            temp = stamina;
            // set temp to stamina again
        }
        yield return null;
    }

    float ChangeStats(float current, float percent, float max)
    {
        // params: current amount of that stat, Percent that is increased each time, max number for that stat

        if(current + (max * percent) < max)
        {
            return current + (max *= percent);
            // up the current by the percent if it would not make it greater than its max
        }
        else
        {
            return max;
            // if it would make it greater than max, set it to max to fill that stat
        }
    }
    IEnumerator Running()
    {
        RunningCoroutine = true;
        // coroutine is active
        while(alive && Input.GetKey(KeyCode.LeftShift) && stamina >= staminaUsage && isMoving)
        {
            speed = 5;
            stamina -= staminaUsage;
            yield return new WaitForSeconds(0.1f);
            // take away stamin for running;
        }
        RunningCoroutine = false;
        speed = 2.5f;
        // coroutine has ended
    }
}
