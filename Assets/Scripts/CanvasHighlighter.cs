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
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CanvasHighlighter : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    Image image;
    AudioSource audio;
    CanvasGroup group;

    public enum Sacrifice
    {
        JUMP,
        SHOOT,
        SWORD,
        SHIELD,
        SPEED,
        HEALTH
    };
    public Sacrifice sacrifice;

    private bool disabled = false;

    void Start()
    {
        image = GetComponent<Image>();
        audio = GetComponentInParent<AudioSource>();
        group = GetComponent<CanvasGroup>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!group.interactable || disabled) return;

        audio.Play();

        // Update the variable corresponding to this selection.
        bool disable = true;
        switch (sacrifice)
        {
            case Sacrifice.JUMP:
                GameManager.can_jump = false;
                break;

            case Sacrifice.SHOOT:
                GameManager.have_gun = false;
                break;

            case Sacrifice.SWORD:
                GameManager.have_sword = false;
                break;

            case Sacrifice.SHIELD:
                GameManager.have_shield = false;
                break;

            case Sacrifice.SPEED:
                GameManager.movement_speed *= 0.5f;
                if (GameManager.movement_speed <= 0.1f)
                {
                    GameManager.movement_speed = 0;
                }
                else
                {
                    disable = false;
                }
                break;

            case Sacrifice.HEALTH:
                GameManager.max_health /= 2;
                if (GameManager.max_health == 0)
                {
                    GameManager.max_health = 1;
                }
                else
                {
                    disable = false;
                }
                break;
        }

        if (disable)
        {
            group.interactable = false;
            image.color = new Color32(128, 128, 128, 255);
            group.alpha = 0.5f;
            disabled = true;
        }
        GameManager.HideMenu();
    }   

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!group.interactable || disabled) return;
        image.color = new Color32(255, 255, 0, 255);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!group.interactable || disabled) return;
        image.color = new Color32(255, 255, 255, 255);
    }
}