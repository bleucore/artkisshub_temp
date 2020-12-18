
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArtAPI.utils;

namespace ArtAPI.network.payload
{
	public	class	PayloadBase
	{
		protected	Hashtable	tuples	= new Hashtable();
		protected	HashUtil	util	= new HashUtil();

		public	void	SetValue(Control ctrl, string ctrl_name, string value) {
			foreach (Control c in ctrl.Controls) {
				if (c.GetType().ToString() == "System.Windows.Forms.GroupBox" ||
					c.GetType().ToString() == "System.Windows.Forms.Panel") {
					SetValue(c, ctrl_name, value);
				} else {
					//Console.WriteLine("control Name : {0}, Value : {1}", c.Name, c.Text.ToString());
					if (ctrl_name == c.Name) {
						Console.WriteLine("Name : {0}, Value : {1}", c.Name, c.Text.ToString());
						if (c.GetType().ToString() == "System.Windows.Forms.CheckBox") {
							((CheckBox)c).Checked	= StaticUtil.ToBoolean(value);
						} else {
							c.Text	= value;
							//util.Add(tuples, key.Value, c.Text.ToString());
						}
					}
				}
			}
		}

		public	void	SetValueAll(Control ctrl, Dictionary<string, string> keys)
		{
			foreach (Control c in ctrl.Controls) {
				if (c.GetType().ToString() == "System.Windows.Forms.GroupBox" ||
					c.GetType().ToString() == "System.Windows.Forms.Panel") {
					SetValueAll(c, keys);
				} else {
					//Console.WriteLine("control Name : {0}, Value : {1}", c.Name, c.Text.ToString());
					foreach (var key in keys) {
						if (key.Key == c.Name) {
							Console.WriteLine("Name : {0}, Value : {1}", c.Name, c.Text.ToString());
							if (c.GetType().ToString() == "System.Windows.Forms.CheckBox") {
								((CheckBox)c).Checked	= StaticUtil.ToBoolean(util.Get(tuples, key.Value).ToString());
							} else {
								c.Text	= util.Get(tuples, key.Value).ToString(); 
								//util.Add(tuples, key.Value, c.Text.ToString());
							}
						}
					}
				}
			}
		}

		public	void	GetValue(Control ctrl, Dictionary<string, string> keys)
		{
			foreach (Control c in ctrl.Controls) {
				if (c.GetType().ToString() == "System.Windows.Forms.GroupBox" ||
					c.GetType().ToString() == "System.Windows.Forms.Panel") {
					GetValue(c, keys);
				} else {
					//Console.WriteLine("control Name : {0}, Value : {1}", c.Name, c.Text.ToString());
					foreach (var key in keys) {
						if (key.Key == c.Name) {
							if (c.GetType().ToString() == "System.Windows.Forms.CheckBox") {
								Console.WriteLine("Name : {0}, Value : {1}", c.Name, ((CheckBox)c).Checked);
								util.Add(tuples, key.Value, ((CheckBox)c).Checked.ToString());
							} else {
								Console.WriteLine("Name : {0}, Value : {1}", c.Name, c.Text.ToString());
								util.Add(tuples, key.Value, c.Text.ToString());
							}
						}
					}
				}
			}
		}

		public	void	Test(Control ctrl) {
			foreach (Control c in ctrl.Controls) {
				if (c.GetType().ToString() == "System.Windows.Forms.GroupBox")
					Console.WriteLine("Control Name : {0}, type : {1}", c.Name, c.GetType().ToString());
			}

			foreach (DictionaryEntry tuple in tuples) {
				Console.WriteLine("Tuple -> Name : {0}, Value : {1}", tuple.Key, tuple.Value);
			}
		}
	}
}
