﻿using System;

namespace KABBOUCHI
{
    [AttributeUsage(AttributeTargets.Method)]
    public class Konsole : Attribute
    {

        public string command;
        public string description;
        public string error = "Command is not valid";

        public Konsole(string command, string description = null, string error = null)
        {
            this.command = command;
            if (description != null)
            {
                this.description = description;
            }

            if (error != null)
            {
                this.error = error;
            }

        }
    }

}