﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

namespace SimpleSharing
{
    public class LocalPlayerManager : MonoBehaviour
    {
#if WINDOWS_UWP
        GestureRecognizer gr;

        void Start()
        {
            // Send pose data less frequently than Update frequency.
            // Sending data too frequently will saturate the socket and force a disconnect.
            InvokeRepeating("SendTransform", 0, 0.1f);

            gr = new GestureRecognizer();
            gr.SetRecognizableGestures(GestureSettings.Tap);

            gr.Tapped += Tapped;

            gr.StartCapturingGestures();
        }

        private void Tapped(TappedEventArgs e)
        {
            if (SharingManager.Instance != null)
            {
                Vector3 position = Camera.main.transform.position;
                Vector3 direction = Camera.main.transform.forward;
                RaycastHit hitInfo;

                if (Physics.Raycast(position, direction, out hitInfo))
                {
                    SharingManager.Instance.SendAirTap(position, direction, hitInfo.point);
                }
            }
        }

        private void OnDestroy()
        {
            CancelInvoke("SendTransform");
            gr.StopCapturingGestures();
        }

        private void SendTransform()
        {
            if (SharingManager.Instance != null)
            {
                SharingManager.Instance.SendPose();
            }
        }
#endif
    }
}
