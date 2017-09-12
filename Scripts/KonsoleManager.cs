using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KABBOUCHI
{
    public class KonsoleManager : MonoBehaviour
    {
        public string commandPrefix = "/";
        public Dictionary<string, KonsoleInfo> konsoles;

        static KonsoleManager instance;
        public static KonsoleManager Instance
        {
            get
            {
                if (instance == null) return FindObjectOfType<KonsoleManager>();
                return instance;

            }
        }

        void Awake()
        {
            var scripts = FindObjectsOfType<MonoBehaviour>();

            konsoles = new Dictionary<string, KonsoleInfo>();

            foreach (var mono in scripts)
            {
                Type monoType = mono.GetType();

                var methods = monoType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                for (int i = 0; i < methods.Length; i++)
                {
                    Konsole attribute = Attribute.GetCustomAttribute(methods[i], typeof(Konsole)) as Konsole;

                    if (attribute != null)
                    {
                        konsoles[attribute.command] = new KonsoleInfo()
                        {
                            classInstance = mono,
                            method = methods[i],
                            konsole = attribute
                        };

                    }
                }
            }

        }

        public void Excute(string fullCommand)
        {
            if (fullCommand.StartsWith(commandPrefix, StringComparison.Ordinal) == false)
            {
                throw new KonsoleInvalidCommandException();
            }

            var c = fullCommand.Split(null, 2);

            var command = c[0].TrimStart(commandPrefix.ToCharArray());
            var args = c.Length > 1 ? c[1] : null;
            string[] inputs = null;

            if (args != null)
            {
                inputs = args.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
            }


            if (konsoles.ContainsKey(command))
            {
                var kInfo = konsoles[command];

                try
                {

                    var objs = new List<object>();

                    var parameters = kInfo.method.GetParameters();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var p = parameters[i];
                        var input = inputs[i];
                        var convertedInput = Convert.ChangeType(input, p.ParameterType);
                        objs.Add(convertedInput);
                    }

                    kInfo.method.Invoke(kInfo.classInstance, objs.Count > 0 ? objs.ToArray() : null);
                }
                catch (Exception)
                {
                    throw new KonsoleInvalidCommandException(kInfo.konsole.error);
                }
            }


        }

    }
}