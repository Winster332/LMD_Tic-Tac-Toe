using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMD_Tic_tac_toe.GUI
{
	public class ManagerHistory
	{
		#region variables
		private List<History> history;
		#endregion
		#region ManagerHistory
		public ManagerHistory()
		{
			history = new List<History>();
		}
		#endregion
		#region add
		public void add(int[] table, int index, List<float> x)
		{
			History h = new History();
			h.index = index;
			h.table = table;
			h.x = x;

			this.history.Add(h);
		}
		#endregion
		#region getHistory
		public List<History> getHistory()
		{
			return history;
		}
		#endregion
		#region Clear
		public void Clear()
		{
			history.Clear();
		}
		#endregion
	}

	public class History
	{
		#region variables
		public int[] table;
		public int index;
		public List<float> x;
		#endregion
		#region History
		public History()
		{
			x = new List<float>();
		}
		#endregion
	}
}
