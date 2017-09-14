using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace KABBOUCHI
{
	public static class Konsole
	{
		public static bool developmentBuildOnly = false;
		public static string commandPrefix = "/";
		public static Dictionary<string, KommandInfo> kommandes = new Dictionary<string, KommandInfo>();

		public static void RegisterAll()
		{
			if (developmentBuildOnly && Debug.isDebugBuild) return;

			var scripts = UnityEngine.Object.FindObjectsOfType<MonoBehaviour>();

			foreach (var mono in scripts)
			{
				Type monoType = mono.GetType();

				var methods = monoType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

				for (int i = 0; i < methods.Length; i++)
				{
					Kommand attribute = Attribute.GetCustomAttribute(methods[i], typeof(Kommand)) as Kommand;

					if (attribute != null)
					{
						kommandes[attribute.command] = new KommandInfo()
						{
							classInstance = mono,
							method = methods[i],
							kommand = attribute
						};

					}
				}
			}

		}

		public static void Register(MonoBehaviour script)
		{
			if (developmentBuildOnly && Debug.isDebugBuild) return;

			Type monoType = script.GetType();

			var methods = monoType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

			for (int i = 0; i < methods.Length; i++)
			{
				Kommand attribute = Attribute.GetCustomAttribute(methods[i], typeof(Kommand)) as Kommand;

				if (attribute != null)
				{
					kommandes[attribute.command] = new KommandInfo()
					{
						classInstance = script,
						method = methods[i],
						kommand = attribute
					};

				}
			}

		}

		public static void Excute(string fullCommand)
		{
			if (developmentBuildOnly && Debug.isDebugBuild) return;

			if (fullCommand.StartsWith(commandPrefix, StringComparison.Ordinal) == false)
			{
				throw new KonsoleInvalidkommandException();
			}

			var c = fullCommand.Split(null, 2);

			var command = c[0].TrimStart(commandPrefix.ToCharArray());
			var args = c.Length > 1 ? c[1] : null;
			string[] inputs = null;

			if (args != null)
			{
				inputs = args.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);
			}


			if (kommandes.ContainsKey(command))
			{
				var kInfo = kommandes[command];

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
					throw new KonsoleInvalidkommandException(kInfo.kommand.error);
				}
			}


		}

	}
}