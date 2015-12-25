using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMD_Tic_tac_toe.GUI
{
	public class AutoRemember
	{
		#region variables
		private ManagerHistory mHistory;
		private Form1 form;
		private int count_step = 0;
		private System.Windows.Forms.Timer timer;
		private LMD_Game game;
		private LMD_Information info;
		private Neural_Network.Net net;
		#endregion
		#region AutoManager
		public AutoRemember(LMD_Game game, LMD_Information info, Neural_Network.Net net, ManagerHistory mHistory, Form1 form)
		{
			this.game = game;
			this.info = info;
			this.net = net;
			this.mHistory = mHistory;
			this.form = form;

			timer = new System.Windows.Forms.Timer();
			timer.Interval = 10;
			timer.Tick += timer_Tick;
		}
		#endregion
		#region timer_udate
		private void timer_Tick(object sender, EventArgs e)
		{
			form.Text = "Tic-Tac-Toe Round[" + game.count_round + "] X[" + game.count_win_x + "] O[" + game.count_win_o + "] N[" + net.neurons .Count+ "]";
			count_step++;

			if (count_step >= 9)
			{
				count_step = 0;
				game.Clear();
				game.count_round++;
			}
			else
			{
				int index = 0;

				if (game.present_step == TYPE_STEP.X)
				{
					index = game.setRandom(TYPE_STEP.X);
				}
				else if (game.present_step == TYPE_STEP.O)
				{
					index = net.Recognize(game.getVectorX(), info.listBox);

					if (game.CheckRect(index))
					{
						game.setO(index);
						info.WriteLine("Нейрон активирован");
					}
					else
					{
						index = game.setRandom(TYPE_STEP.O);
						info.WriteLine("Нейрон активирован");
					}
				}

				mHistory.add(game.getTableConvertHistory(), index, game.getVectorX());

				if (game.CheckWinX())
				{
					count_step = 0;
					info.WriteLine("Победил X!");
					win_x();

					game.count_win_x++;
					game.Clear();
					mHistory.Clear();
					game.present_step = TYPE_STEP.X;
				}
				else if (game.CheckWinO())
				{
					count_step = 0;
					info.WriteLine("Победил O!");
					win_o();

					game.count_win_o++;
					game.Clear();
					mHistory.Clear();
					game.present_step = TYPE_STEP.X;
				}
				else if (game.CheckNoWin())
				{
					count_step = 0;
					info.WriteLine("Ничья!");
					win_x();
					game.Clear();
					game.count_no_win++;
					mHistory.Clear();
					game.present_step = TYPE_STEP.X;
				}

				if (info.listBox.Items.Count >= 5000)
					info.listBox.Items.Clear();
			}
		}
		#endregion
		#region win_x
		public void win_x()
		{
			List<GUI.History> history = mHistory.getHistory();
			int num = 0;
			int d = 1;
			foreach (History h in history)
			{
				num++;
				int[] table = h.table;
				info.WriteLine(num + " - index[" + h.index+ "]");
				info.WriteLine(table[0] + "|" + table[1] + "|" + table[2]);
				info.WriteLine(table[3] + "|" + table[4] + "|" + table[5]);
				info.WriteLine(table[6] + "|" + table[7] + "|" + table[8]);
			}

			int index = history[history.Count - 1].index;
			List<float> x = history[history.Count - 2].x;

			info.WriteLine("Требуемый нейрон[" + index + "]");

			net.AddNeuron(index, x, info.listBox);
		}
		#endregion
		#region win_o
		public void win_o()
		{
		}
		#endregion
		#region Start
		public void Start()
		{
			timer.Start();

			game.Clear();

			if (net.neurons.Count <= 0)
				net.AddNeuron(5, game.getVectorX(), info.listBox);
		}
		#endregion
		#region Stop
		public void Stop()
		{
			game.Clear();
			timer.Stop();
		}
		#endregion
	}
}
