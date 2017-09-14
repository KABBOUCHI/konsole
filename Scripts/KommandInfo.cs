using System;
using UnityEngine;
using System.Reflection;

namespace KABBOUCHI
{
	public class KommandInfo
	{
		public MonoBehaviour classInstance;
		public MethodInfo method;
		public Kommand kommand;
	}
}