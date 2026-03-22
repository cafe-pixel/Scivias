using System;

using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(SC_SensorSystem))]
    public class SensorSystemEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            // SC_SensorSystem sensor = (SC_SensorSystem) target;
            // Handles.color = Color.red;
            // Handles.DrawWireArc (sensor.transform.position, Vector3.up, Vector3.forward, 360, sensor.SensorAngle);
            // Vector3 viewAngleA = sensor.DirFromAngle (-sensor.VisionDistance / 2);
            // Vector3 viewAngleB = sensor.DirFromAngle (sensor.VisionDistance / 2);
            //
            // Handles.DrawLine (sensor.transform.position, sensor.transform.position + viewAngleA * sensor.SensorAngle);
            // Handles.DrawLine (sensor.transform.position, sensor.transform.position + viewAngleB * sensor.SensorAngle);
            
            // SC_SensorSystem sensor = (SC_SensorSystem) target;
            // Handles.color = Color.red;
            // Handles.DrawWireArc (sensor.transform.position, Vector3.up, Vector3.forward, sensor.SensorAngle, sensor.VisionDistance);
            // Vector3 viewAngleA = sensor.DirFromAngle (-sensor.SensorAngle / 2);
            // Vector3 viewAngleB = sensor.DirFromAngle (sensor.SensorAngle / 2);
            //
            // Handles.DrawLine (sensor.transform.position, sensor.transform.position + viewAngleA * sensor.SensorAngle);
            // Handles.DrawLine (sensor.transform.position, sensor.transform.position + viewAngleB * sensor.SensorAngle);
        }
    }

    
}
