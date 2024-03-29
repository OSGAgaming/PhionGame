﻿using System;
using System.Collections.Generic;

namespace QueefCord.Core.Resources
{
	public static class Assets<T>
	{
		private static T[] assets = new T[32];
		private static IDictionary<string, int> identifierToSlot = new Dictionary<string, int>();
		private static int nextFreeSlot;

		public static int Count { get; private set; }

		public static Handle<T> Register(string identifier, T asset)
		{
			if (identifierToSlot.ContainsKey(identifier))
				throw new Exception("Cannot register multiple assets with the same name: " + identifier);

			int slot = nextFreeSlot++;

			if (slot >= assets.Length)
				Array.Resize(ref assets, assets.Length * 2);

			assets[slot] = asset;
			identifierToSlot[identifier] = slot;

			return new Handle<T>(slot);
		}

		public static Handle<T> Get(string identifier)
		{
			if (!identifierToSlot.ContainsKey(identifier))
				throw new Exception("Asset does not exist: " + identifier);

			return new Handle<T>(identifierToSlot[identifier]);
		}

		internal static T Get(int slot) => assets[slot];

		internal static bool Has(string identifier) => identifierToSlot.ContainsKey(identifier);
	}
}
