﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bulatov
{


    public interface Engine
    {
        void InitEngine();

        void UpdateEngine(Rigidbody rb, Drone_Inputs input);
    }
}