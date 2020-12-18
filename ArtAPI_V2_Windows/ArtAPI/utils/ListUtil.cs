using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.ListView;

namespace ArtAPI.utils
{
	public	class	ListUtil
	{
		public	void	SetValue(ListView lv, string key, int col, string value) {
			foreach(ListViewItem item in lv.Items) {
				if (item.SubItems[0].Text == key) {
					try {
						item.SubItems[col].Text	= value;
					} catch(Exception e) {}
					return;
				}
			} 
		}

		public	void	SetValueCrack(ListView lv, string key, string value) {
			foreach(ListViewItem item in lv.Items) {
				if (item.SubItems[1].Text == key) {
					item.SubItems[2].Text	= value;
					return;
				}
			}

		}

		public	String	GetValueCrack(ListView lv, string key) {
			foreach(ListViewItem item in lv.Items) {
				if (item.SubItems[1].Text == key) {
					return	item.SubItems[2].Text;
				}
			}
			return	"";
		}
		
		public	String	GetValue(ListView list, ListViewItem item, string title) {
			int		index	= -1;
			foreach(ColumnHeader column in list.Columns) {
				if (title.CompareTo(column.Text) == 0) {
					index = column.Index;
					break;
				}
			}
			if (index > -1) {
				return	item.SubItems[index].Text;
			}
			return	"";
		}

/*
		ListViewItem item1 = new ListViewItem();
		item1.SubItems.Add(level.ToString());
		item1.SubItems.Add(tag);
		item1.SubItems.Add(time);
		item1.SubItems.Add(msg);

		lv_log_list.Items.Insert(0, item1);
*/
		public	void	AddList(ListView list, string first, params object[] args) {
			ListViewItem item = new ListViewItem(first);
			foreach(var value in args) {
				item.SubItems.Add(value.ToString());
			}
			list.Items.Add(item);
		}
	}
}
