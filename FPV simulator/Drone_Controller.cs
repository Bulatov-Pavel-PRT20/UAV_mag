using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Bulatov
{
    [RequireComponent(typeof(Drone_Inputs))]

    public class Drone_Controller : Base_Rigidbody
    {
        #region Variables
        [Header("Control Properties")]
        [SerializeField] private float minMaxPitch = 30f;
        [SerializeField] private float minMaxRoll = 30f;
        [SerializeField] private float yawPower = 4f;
        [SerializeField] private float lerpSpeed = 2f;

        private Drone_Inputs input;
        private List<Engine> engines = new List<Engine>();

        private float finalPitch;
        private float finalRoll;
        private float yaw;
        private float finalYaw;
        #endregion

        #region Main Methods

        // Start is called before the first frame update
        void Start()
        {
            input = GetComponent<Drone_Inputs>();
            engines = GetComponentsInChildren<Engine>().ToList<Engine>();
        }
        #endregion

        #region Custom Methods
        protected override void HandlePhysics()
        {
            HandleEngines();
            HandleControls();
        }
        protected virtual void HandleEngines()
        {
            //rb.AddForce(Vector3.up * (rb.mass * Physics.gravity.magnitude));
            foreach(Engine engine in engines)
            {
                engine.UpdateEngine(rb, input);
            }
        }

        protected virtual void HandleControls()
        {
            float pitch = input.Cyclic.y * minMaxPitch;
            float roll = -input.Cyclic.x * minMaxRoll;
            yaw += input.Pedals * yawPower;

            finalPitch = Mathf.Lerp(finalPitch, pitch, Time.deltaTime * lerpSpeed);
            finalRoll = Mathf.Lerp(finalRoll, roll, Time.deltaTime * lerpSpeed);
            finalYaw = Mathf.Lerp(finalYaw, yaw, Time.deltaTime * lerpSpeed);

            Quaternion rot = Quaternion.Euler(finalPitch, finalYaw, finalRoll);
            rb.MoveRotation(rot);
        }
        #endregion
    }
}
