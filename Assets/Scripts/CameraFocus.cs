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

public class CameraFocus : MonoBehaviour
{

    public GameObject focus;
    public SuperTiled2Unity.SuperMap map;

    private Camera camera;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    /**
     * Ensure that the camera stays focused on the specified GameObject.
     */
    void Update()
    {
        // Initial focus point is the GameObject itself
        Vector3 focus_point = new Vector3(focus.transform.position.x, focus.transform.position.y, transform.position.z);

        // Compute the screen bounds of the map
        // Assumes that the orthographic camera is set up correctly...
        float tiles_x = (camera.pixelWidth / map.m_TileWidth);
        float xmin = tiles_x / 2;
        float xmax = map.m_Width - tiles_x / 2;

        float tiles_y = (camera.pixelHeight / map.m_TileHeight);
        float ymin = -(map.m_Height - tiles_y / 2);
        float ymax = -tiles_y / 2;

        // Clamp the camera focus position inside the map
        focus_point.x = Mathf.Clamp(focus_point.x, xmin, xmax);
        focus_point.y = Mathf.Clamp(focus_point.y, ymin, ymax);
    
        // Point the camera at the focus point
        transform.position = focus_point;
    }

}
