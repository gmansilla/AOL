using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class RescueInfo
    {
        string rescueArea;
        public string RescueArea
        { get { return rescueArea; } }

        bool rescued;
        public bool IsRescued
        { get { return rescued; } }

        public RescueInfo(string newRescueArea)
        {
            rescueArea = newRescueArea;
            rescued = false;
        }

        public void Rescue()
        {
            rescued = true;
        }
    }
}
