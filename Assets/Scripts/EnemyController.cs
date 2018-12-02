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

public class EnemyController : MonoBehaviour
{

    private Rigidbody2D body;
    private SpriteRenderer sprite;
    private AudioSource audio;
    private int health;

    const float MAX_SPEED = 10;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        audio = GetComponent<AudioSource>();
        health = 3;
    }

    void Update()
    {
        // Don't do anything once dead
        if (health <= 0) return;

        // Simple placeholder AI
        // Move towards the player at some fixed pace
        const int PACE = 10;
        Vector2 direction = GameManager.player.transform.position - gameObject.transform.position;
        direction.Normalize();
        direction.y = 0;
        body.velocity += direction * PACE * Time.deltaTime;
        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -MAX_SPEED, MAX_SPEED), body.velocity.y);

        // TODO: Fight back!
    }

    IEnumerator flash()
    {
        sprite.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.1f);
        sprite.color = new Color32(255, 255, 255, 255);
    }

    public void hit()
    {
        if (health > 0)
        {
            audio.PlayOneShot(audio.clip);
            StartCoroutine(flash());
            health -= 1;
            if (health == 0)
            {
                body.freezeRotation = false;
                body.AddForceAtPosition(new Vector2(5, 5), new Vector2(1, 1));
                gameObject.layer = 9; // Limited physics for dead enemies
                GameManager.score += 1;
                GameManager.satisfaction += 1;
                GameManager.numEnemies -= 1;
                Destroy(gameObject, 5);
            }
        }
    }

}