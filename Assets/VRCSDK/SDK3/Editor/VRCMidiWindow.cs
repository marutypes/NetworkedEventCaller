﻿using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.UIElements;
using UnityEngine.Experimental.UIElements;
using UnityEngine.Experimental.UIElements.StyleEnums;
using VRC.SDKBase.Midi;

namespace VRC.SDK3.Midi
{
    public class VRCMidiWindow : EditorWindow
    {
        private VisualElement _rootView;
        private TextField _deviceNameField;
        
        public const string DEVICE_NAME_STRING = "VRC.SDK3.Midi.Device";

        [MenuItem("VRChat SDK/Utilities/Midi")]
        private static void ShowWindow()
        {
            VRCMidiWindow foo = CreateInstance(typeof(VRCMidiWindow)) as VRCMidiWindow;
            // ReSharper disable once PossibleNullReferenceException
            foo.titleContent = new GUIContent("Midi");
            foo.maxSize = new Vector2(256, 80);
            foo.ShowUtility();
        }

        private void OnEnable()
        {
            _rootView = this.GetRootVisualContainer();
            _rootView.Add(new Label("Midi Settings")
            {
                style = {
                    fontSize = 18,
                    marginTop = 10,
                    marginBottom = 10,
                }
            });

            // Container for Device Name label and field
            VisualContainer deviceNameContainer = new VisualContainer() { style = {flexDirection = FlexDirection.Row} };
            _rootView.Add(deviceNameContainer);
            
            // Label for Field
            deviceNameContainer.Add(new Label("Device Name"));
            
            // Input Name Field
            _deviceNameField = new TextField()
            {
                isDelayed = true,
                value = EditorPrefs.GetString(DEVICE_NAME_STRING),
                style = { flexGrow = 1 },
            };
            _deviceNameField.OnValueChanged(evt => EditorPrefs.SetString(DEVICE_NAME_STRING, evt.newValue));
            //deviceNameContainer.Add(_deviceNameField);

            // Get available device names
            VRCPortMidiInput midi = new VRCPortMidiInput();
            var deviceNames = midi.GetDeviceNames().ToList();
            
            // Add blank device name to use if specified device not found
            deviceNames.Insert(0, "");
            string currentDeviceValue = EditorPrefs.GetString(DEVICE_NAME_STRING);
            string defaultValue = deviceNames.Contains(currentDeviceValue) ? currentDeviceValue : "";
            
            // Create and add device popup
            var deviceNamePopupField = new PopupField<string>(deviceNames, defaultValue)
            {
                style = {flexGrow = 1},
                name = "midiDevicePopUp",
            };
            deviceNamePopupField.OnValueChanged(evt => EditorPrefs.SetString(DEVICE_NAME_STRING, evt.newValue));
            deviceNameContainer.Add(deviceNamePopupField);
        }
    }
}