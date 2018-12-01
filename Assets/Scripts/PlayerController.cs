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

    private Rigidbody2D body;
    private AudioSource audio;
    private SpriteRenderer sprite;

    public AudioClip jump_sfx;
    public AudioClip shoot_sfx;
    public AudioClip sword_sfx;
    public AudioClip hurt_sfx;

    public GameObject bulletPrefab;

    const float MAX_SPEED = 15;

    enum FacingDirection
    {
        LEFT = -1,
        RIGHT = 1
    };
    FacingDirection facing;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Move player horizontally on A/D, arrow keys, joysticks       
        // Only update the direction the player is facing upon input 
        float horizontal = Input.GetAxis("Horizontal");
        if (horizontal < 0)
        {
            facing = FacingDirection.LEFT;
        }
        else if (horizontal > 0)
        {
            facing = FacingDirection.RIGHT;
        }
        body.velocity += new Vector2(horizontal, 0);

        // Jump on W or space (if player still knows how)
        if (true /*GameManager.checkPlayerCanJump()*/)
        {
            // TODO: Check if player is grounded
            bool trying_to_jump = (Input.GetAxis("Vertical") > 0) || Input.GetButtonDown("Jump");
            if (trying_to_jump && body.velocity.y == 0)
            {
                body.AddForce(-Physics.gravity * body.mass * 32);
                audio.PlayOneShot(jump_sfx);
            }
        }

        // TODO: Punch/sword
        if (Input.GetButtonDown("Hit"))
        {
            audio.PlayOneShot(sword_sfx);
        }

        // Shoot on mouse button (if player still knows how)
        if (true /*GameManager.checkPlayerCanShoot()*/)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                audio.PlayOneShot(shoot_sfx);

                GameObject bullet = GameObject.Instantiate<GameObject>(bulletPrefab);
                bullet.transform.position = gameObject.transform.position;
                var bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.Initialize((float) facing);
            }
        }

        // Clamp player speed
        float modified_max_speed = GameManager.movement_speed * MAX_SPEED;
        body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -MAX_SPEED, MAX_SPEED), body.velocity.y);
    }

    IEnumerator flash()
    {
        sprite.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(0.05f);
        sprite.color = new Color32(0, 0, 0, 255);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            StartCoroutine(flash());
            audio.PlayOneShot(hurt_sfx);
            GameManager.health -= 1;
        }
    }

}
