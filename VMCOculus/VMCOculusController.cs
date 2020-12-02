using System;
using System.Threading;
using UnityEngine;
using uOSC;

namespace VMCOculus
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class VMCOculusController : MonoBehaviour
    {
        public static VMCOculusController Instance { get; private set; }

        private System.Threading.Thread thread;

        //InputDevice _device;
        OVRPose pose;
        uOscClient client;
        public const string DeviceSerial = "BeatSaber Virtual HMD";

        // These methods are automatically called by Unity, you should remove any you aren't using.
        #region Monobehaviour Messages
        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            if (Instance != null) {
                Plugin.Log?.Warn($"Instance of {GetType().Name} already exists, destroying.");
                GameObject.DestroyImmediate(this);
                return;
            }
            GameObject.DontDestroyOnLoad(this); // Don't destroy this object on scene changes
            Instance = this;


            //this._device = InputDevices.GetDeviceAtXRNode(XRNode.TrackingReference);
            this.client = new uOscClient();
            this.thread = new System.Threading.Thread(new ThreadStart(() =>
            {
                while (true) {
                    try {
                        pose = OVRPlugin.GetNodePose(OVRPlugin.Node.Head, OVRPlugin.Step.Render).ToOVRPose();

                        client.Enqueue("/VMC/Ext/Hmd/Pos", DeviceSerial,
                        pose.position.x, pose.position.y, pose.position.z,
                        pose.orientation.x, pose.orientation.y, pose.orientation.z, pose.orientation.w);
                    }
                    catch (Exception e) {
                        Plugin.Log.Error(e);
                    }
                    System.Threading.Thread.Sleep(10);
                }
            }));
            this.thread.Start();
            Plugin.Log?.Debug($"{name}: Awake()");
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
            if (Instance == this)
                Instance = null; // This MonoBehaviour is being destroyed, so set the static instance property to null.

            this.thread.Abort();
            this.client.Dispose();
        }
        #endregion
    }
}
