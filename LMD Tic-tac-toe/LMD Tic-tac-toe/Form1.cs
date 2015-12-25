using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;

namespace LMD_Tic_tac_toe
{
	public partial class Form1 : Form
	{
		#region variables
		public enum STATE_PROGRAM { auto = 0, game = 1, handle = 2 };
		public STATE_PROGRAM state;
		public Neural_Network.Net net;
		public GUI.LMD_Information info;
		public GUI.LMD_Game game;
		public GUI.LMD_TopBoard topBoard;
		public GUI.ManagerHistory mHistory;
		public GUI.AutoRemember autoRem;
		private int count_step = 0;
		#endregion
		#region Form1
		public Form1()
		{
			InitializeComponent();

			this.BackColor = Color.FromArgb(0, 140, 220);
		}
		#endregion
		#region Form_Load
		private void Form1_Load(object sender, EventArgs e)
		{
			#region Initialize
			net = new Neural_Network.Net();
			topBoard = new GUI.LMD_TopBoard(this, false);
			info = new GUI.LMD_Information(this);
			game = new GUI.LMD_Game(this);
			mHistory = new GUI.ManagerHistory();
			autoRem = new GUI.AutoRemember(game, info, net, mHistory, this);
			#endregion
			#region changedState
			topBoard.changedState += (state) =>
			{
				this.state = state;

				switch (state)
				{
					case STATE_PROGRAM.auto:
						info.WriteLine("Включен режим автоматического обучения");
						autoRem.Start();
						count_step = 0;
						break;
					case STATE_PROGRAM.game:
						info.WriteLine("Включен режим игрового обучения");
						autoRem.Stop();
						count_step = 0;
						this.Text = "LMD Tic-Tac-Toe";
						break;
					case STATE_PROGRAM.handle:
						info.WriteLine("Включен режим ручного обучения");
						autoRem.Stop();
						count_step = 0;
						this.Text = "LMD Tic-Tac-Toe";
						break;
				}
				game.enable = true;
			};
			#endregion
			#region Clear
			topBoard.butClick += () =>
				{
					info.WriteLine("Поле было очищено");
					game.Clear();
					info.listBox.Items.Clear();
					info.listBox.Items.Add("------------------------------------------------ Tic-Tac-Toe -----------------------------------------------------------------");
					count_step = 0;
					game.enable = true;
				};
			#endregion
			#region Remember
			info.remember += (int index) =>
				{
					net.AddNeuron(index, game.getVectorX(), info.listBox);
					
					step_x(game.getVectorX(), index);
				};
			#endregion
			#region Save
			info.save += () =>
				{
					Save();
				};
			#endregion
			#region Load
			info.load += () =>
				{
					LoadNS();
				};
			#endregion
			#region Correct
			info.correct += (float speed, int delta, int index) =>
				{
					net.Correct(game.getVectorX(), index, speed, delta, info.listBox);
				};
			#endregion
			#region step
			game.step += (List<float> x, int index, String str) =>
				{
					count_step++;

					#region Handle
					if (state == STATE_PROGRAM.handle)
					{
						info.WriteLine("Ход x: " + index);

						if (net.neurons.Count <= 0)
						{
							info.WriteLine("Заблокировано");
							MessageBox.Show("Нужно сеть обучить!");
						}
						else
						{
							if (str == "x")
							{
								int o_index = net.Recognize(game.getVectorX(), info.listBox);
								Boolean b_rect = game.CheckRect(o_index);

								if (b_rect)
								{
									info.WriteLine("Ход о: " + o_index);
									game.setO(o_index);
								}
								else if (!b_rect)
								{
									info.WriteLine("Жизнь нейронную сеть к такому не готовила");
									MessageBox.Show("Сеть не знает что делать!");
								}
							}
						}
					}
					#endregion
					#region Game
					if (state == STATE_PROGRAM.game)
					{
						if (game.CheckWinX())
						{
							info.WriteLine("Крестики победили!");
							mHistory.add(game.getTableConvertHistory(), index, game.getVectorX());

							List<GUI.History> history = mHistory.getHistory();
							int num = 0;

							foreach (GUI.History h in history)
							{
								num++;
								int[] table = h.table;
								info.WriteLine(num + " - index[" + h.index + "]");
								info.WriteLine(table[0] + "|" + table[1] + "|" + table[2]);
								info.WriteLine(table[3] + "|" + table[4] + "|" + table[5]);
								info.WriteLine(table[6] + "|" + table[7] + "|" + table[8]);
							}

							int _index = history[history.Count - 1].index;
							List<float> _x = history[history.Count - 2].x;

							info.WriteLine("Требуемый нейрон[" + _index + "]");

							net.AddNeuron(_index, _x, info.listBox);

							mHistory.Clear();
							game.enable = false;
						}
						else if (game.CheckWinO())
						{
							info.WriteLine("Нолики победили!");
							mHistory.add(game.getTableConvertHistory(), index, game.getVectorX());
							autoRem.win_o();
							mHistory.Clear();
							game.enable = false;
						}
						else if (game.CheckNoWin())
						{
							info.WriteLine("Ничья!");
							mHistory.Clear();
							game.enable = false;
						}
						else
						{
							if (count_step <= 8)
							{
								if (str == "x")
								{
									step_x(game.getVectorX(), index);
									mHistory.add(game.getTableConvertHistory(), index, game.getVectorX());
								}
								else if (str == "o")
								{
								}
							}
							else
							{
								count_step = 0;
							}
						}
					}
					#endregion
				};
			#endregion

			info.WriteLine("Включен режим игрового обучения");
			this.state = Form1.STATE_PROGRAM.game;
		}
		#endregion
		#region save
		private void Save()
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Neural_Network.Neuron));
			List<Neural_Network.Neuron> ns = net.neurons;

			if (!Directory.Exists("brains\\"))
				Directory.CreateDirectory("brains\\");

			for (int i = 0; i < ns.Count; i++)
			{
				using (Stream stream = new FileStream(string.Concat(new object[] 
						{
							@"brains\",
							i,
							"_neuron",
							".xml"
						}), FileMode.Create))
				{
					serializer.Serialize(stream, ns[i]);
				}
			}

			info.WriteLine("Сохранение данных из brains\\ закончено.");
			info.WriteLine("Было сохранено: " + ns.Count + " нейронов");
		}
		#endregion
		#region Load
		private void LoadNS()
		{
			List<Neural_Network.Neuron> nns = new List<Neural_Network.Neuron>();
			XmlSerializer serializer = new XmlSerializer(typeof(Neural_Network.Neuron));
			string[] names = Directory.GetFiles(@"brains\");

			for (int i = 0; i < names.Length; i++)
			{
				using (Stream stream = new FileStream(names[i], FileMode.Open))
				{
					Neural_Network.Neuron sns = (Neural_Network.Neuron)serializer.Deserialize(stream);

					nns.Add(sns);
				}
			}

			info.WriteLine("Загрузка данных из brains\\ закончена.");
			info.WriteLine("Было загружено: " + nns.Count + " нейронов");

			net.neurons = nns;
		}
		#endregion
		#region step_x
		public void step_x(List<float> x, int index)
		{
			info.WriteLine("Ход x: " + index);

			if (net.neurons.Count <= 0)
			{
				int i = game.setRandom(GUI.TYPE_STEP.O);
				info.WriteLine("Ход o: " + i);
				info.WriteLine("Нейрон не активен");
			}
			else
			{
				int o_index = net.Recognize(x, info.listBox);
				Boolean b_rect = game.CheckRect(o_index);

				if (b_rect)
				{
					info.WriteLine("Ход о: " + o_index);
					game.setO(o_index);
					info.WriteLine("Нейрон активен");
				}
				else if (!b_rect)
				{
					int i = game.setRandom(GUI.TYPE_STEP.O);
					info.WriteLine("Ход o: " + i);
					info.WriteLine("Нейрон не активен");
				}
			}
		}
		#endregion
	}
}
