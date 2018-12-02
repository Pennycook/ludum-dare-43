/**
 * BSD 3-Clause License
 *
 * Copyright(c) 2018, John Pennycook
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 *
 * * Neither the name of the copyright holder nor the names of its
 *   contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE
 * FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // Singleton instance
    public static GameManager instance = null;

    // Magic constants
    const int KILL_SCORE = 1;
    const int DISSATISFACTION_PER_SECOND = 1;
    public const int MAX_SATISFACTION = 30;

    // Game state
    public static GameObject player;
    public static int numEnemies = 0;

    // Inputs to win/lose conditions
    public static int satisfaction;
    public static int score;
    public static int health;

    // Sacrificial variables
    public static bool can_jump = true;
    public static bool have_gun = true;
    public static bool have_sword = true;
    public static bool have_shield = true;
    public static float movement_speed = 1.0f;
    public static int max_health = 16;

    // UI 
    public CanvasGroup menu;
    bool menu_open = false;

    // Prefabs
    public GameObject enemyPrefab;

    void Awake()
    {
        // Ensure only one GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        // Initialize game state
        player = GameObject.FindGameObjectWithTag("Player");
        satisfaction = MAX_SATISFACTION;
        score = 0;
        health = max_health;

        // Ensure that the menu is hidden
        menu.alpha = 0.0f;
        menu.interactable = false;
        menu_open = false;

        // Monitor dissatisfaction
        StartCoroutine(growDissatisfied());

        // Spawn enemies
        StartCoroutine(spawnEnemies());
    }

    public static void ShowMenu()
    {
        instance.menu.alpha = 1.0f;
        instance.menu.interactable = true;
        instance.menu_open = true;

        Time.timeScale = 0.0f;
    }

    public static void HideMenu()
    {
        instance.menu.alpha = 0.0f;
        instance.menu.interactable = false;
        instance.menu_open = false;

        health = max_health;
        satisfaction = MAX_SATISFACTION;

        Time.timeScale = 1.0f;
    }

    public static bool Paused()
    {
        return instance.menu_open;
    }

    public void Update()
    {
        if (satisfaction == 0 || health == 0)
        {
            ShowMenu();
        }
    }

    // Deity's satisfaction decreases over time
    IEnumerator growDissatisfied()
    {
        while (true)
        {
            if (satisfaction > 0)
            {
                satisfaction -= DISSATISFACTION_PER_SECOND;
            }
            yield return new WaitForSeconds(1.0f);
        }
    }

    // Enemies "randomly" appear
    // Rate really increases with dissatisfaction
    IEnumerator spawnEnemies()
    {
        int MAX_ENEMIES = 20;
        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (numEnemies < MAX_ENEMIES)
            {
                float r = Random.value * MAX_SATISFACTION;
                if (r > satisfaction)
                {
                    GameObject enemy = GameObject.Instantiate<GameObject>(enemyPrefab, this.transform);
                    numEnemies += 1;

                    // TODO: Enemies should really spawn from somewhere that makes sense on the level
                    float x = Random.value * 18;
                    float y = -12;
                    enemy.transform.position = new Vector2(x, y);
                }
            }
        }
    }

}