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

public class PlayerController : MonoBehaviour
{

    // Inputs to win/lose conditions
    int satisfaction;
    int score;
    int health;

    // Sacrificial variables
    bool can_jump = true;
    bool can_dodge = true;
    bool have_gun = true;
    bool have_sword = true;
    float movement_speed = 1.0f;
    int max_health = 128;

    // Handles to 
    Rigidbody2D body;

    void Start()
    {
        satisfaction = 100;
        score = 0;
        health = max_health;

        body = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Move player horizontally on A/D, arrow keys, joysticks
        // TODO: Clamp velocity :)
        float horizontal = Input.GetAxis("Horizontal");
        body.velocity += new Vector2(horizontal, 0);

        // Jump on W or space (if player still knows how)
        if (can_jump)
        {
            // TODO: Check if player is grounded
            bool trying_to_jump = (Input.GetAxis("Vertical") > 0) || Input.GetButtonDown("Jump");
            if (trying_to_jump && body.velocity.y == 0)
            {
                body.AddForce(-Physics.gravity * body.mass * 32);
            }
        }

        // TODO: Shoot
    }

}
