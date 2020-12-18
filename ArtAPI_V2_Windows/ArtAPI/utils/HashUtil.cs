using System;
using System.Collections;


namespace ArtAPI.utils
{
	public	class	HashUtil {
		public	void	Add(Hashtable table, string key, string val) {
			if (table.ContainsKey(key))	table[key]		= val;
			else						table.Add(key, val);
		}

		public	Object	Get(Hashtable table, string key) {
			if (table.ContainsKey(key))		return	table[key];
			return	null;
		}
	}
}
