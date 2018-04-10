//-----------------------------------------------------------------------
// <copyright file="ARCoreBackgroundRenderer.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore
{
    using System.Collections;
    using System.Collections.Generic;
    using GoogleARCoreInternal;
    using UnityEngine;
    using UnityEngine.XR;

    //// TODO (mtsmall): Consider if this component is the best way to expose background rendering and discuss approach
    //// with Unity.

    /// <summary>
    /// Renders the device's camera as a background to the attached Unity camera component.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class ARCoreBackgroundRenderer : MonoBehaviour
    {
        /// <summary>
        /// A material used to render the AR background image.
        /// </summary>
        [Tooltip("A material used to render the AR background image.")]
        public Material BackgroundMaterial;

        private Camera m_Camera;

        private ARBackgroundRenderer m_BackgroundRenderer;

        private void OnEnable()
        {
            if (Application.isEditor)
            {
                enabled = false;
                return;
            }

            if (BackgroundMaterial == null)
            {
                Debug.LogError("ArCameraBackground:: No material assigned.");
                return;
            }

            m_Camera = GetComponent<Camera>();
        }

        private void OnDisable()
        {
            if (m_BackgroundRenderer != null)
            {
                m_BackgroundRenderer.camera = null;
                m_BackgroundRenderer = null;
            }
        }

        private void Update()
        {
            if (BackgroundMaterial == null)
            {
                // A background rending material has not been assigned.
                return;
            }
            else if (Frame.CameraImage.Texture == null)
            {
                // TODO (mtsmall): Consider rendering a default background in this case.
                // No texture is available.
                return;
            }

            const string mainTexVar = "_MainTex";
            const string topLeftRightVar = "_UvTopLeftRight";
            const string bottomLeftRightVar = "_UvBottomLeftRight";

            BackgroundMaterial.SetTexture(mainTexVar, Frame.CameraImage.Texture);

            ApiDisplayUvCoords uvQuad = Frame.CameraImage.DisplayUvCoords;

            BackgroundMaterial.SetVector(topLeftRightVar,
                new Vector4(uvQuad.TopLeft.x, uvQuad.TopLeft.y, uvQuad.TopRight.x, uvQuad.TopRight.y));
            BackgroundMaterial.SetVector(bottomLeftRightVar,
                new Vector4(uvQuad.BottomLeft.x, uvQuad.BottomLeft.y, uvQuad.BottomRight.x, uvQuad.BottomRight.y));

            m_Camera.projectionMatrix = Frame.CameraImage.GetCameraProjectionMatrix(m_Camera.nearClipPlane, m_Camera.farClipPlane);

            if (m_BackgroundRenderer == null)
            {
                m_BackgroundRenderer = new ARBackgroundRenderer();
                m_BackgroundRenderer.backgroundMaterial = BackgroundMaterial;
                m_BackgroundRenderer.camera = m_Camera;
                m_BackgroundRenderer.mode = ARRenderMode.MaterialAsBackground;
            }
        }
    }
}