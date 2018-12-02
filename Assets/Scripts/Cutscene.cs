/*
 * BSD 3-Clause License
 *
 * Copyright(c) 2018, John Pennycook
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 * 
 * * Redistributions of source code must retain the above copyright notice, this
 * list of conditions and the following disclaimer.
 *
 * * Redistributions in binary form must reproduce the above copyright notice,
 *   this list of conditions and the following disclaimer in the documentation
 *   and/or other materials provided with the distribution.
 *
 ** Neither the name of the copyright holder nor the names of its
 * contributors may be used to endorse or promote products derived from
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
 * OR TORT(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE
 * OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Cutscene : MonoBehaviour
{
    public Image image;
    public Text description;

    public AudioClip demon;
    public AudioClip blood;

    private DialogueManager dialogueManager;
    private AudioSource audio;

    void Awake()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
        audio = GetComponent<AudioSource>();
    }

    void Start()
    {
        StartCoroutine(PlayCutscene());
    }

    IEnumerator PlayCutscene()
    {
        Dialogue dialogue = new Dialogue();
        dialogue.color = new Color32(255, 255, 255, 255);
        dialogue.name = "Kurt Fasthold";
        dialogue.sentences = new string[] {
                "The name's Kurt Fasthold.",
            };
        yield return dialogueManager.OpenDialogue(dialogue);
        description.text = "[ I hope you're reading this in a gruff voice. ]";
        dialogue.sentences = new string[] {
                "I used to be like common people.",
                "I used to do whatever common people do.",
                "I wanted to make a difference in this world.",
            };
        yield return dialogueManager.OpenDialogue(dialogue);
        image.color = new Color32(75, 0, 130, 255);
        description.color = new Color32(255, 255, 255, 255);
        description.text = "[ A wild demon appears! It's got tentacles, horns <i>and</i> wings. ]";
        dialogue.sentences = new string[] {
                "So I sold my soul to a demon in exchange for superpowers.",
                "That's right -- Kurt Fasthold is THE SACRIFICER."
            };
        audio.PlayOneShot(demon);
        yield return dialogueManager.OpenDialogue(dialogue);
        image.color = new Color32(139, 0, 0, 255);
        description.color = new Color32(255, 255, 255, 255);
        description.text = "[ Blood!  I hope this is not Chris' blood... ]";
        dialogue.sentences = new string[] {
                "As long as I keep the demon 'sacrified', I can keep my powers.",
            };
        yield return dialogueManager.OpenDialogue(dialogue);
        dialogue.sentences = new string[] {
            "...sacrifices must be made."
        };
        audio.PlayOneShot(blood);
        yield return dialogueManager.OpenDialogue(dialogue);
        SceneManager.LoadScene("Level");
    }
}
