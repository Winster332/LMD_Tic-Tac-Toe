using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LMD_Tic_tac_toe.GUI
{
	public enum TYPE_STEP { X = 0, O = 1, NONE = 2 };

	public class LMD_Game : Panel
	{
		#region variables
		public delegate void Step(List<float> x, int index, String symb);
		public event Step step;
		public TYPE_STEP present_step;
		public int count_win_x = 0;
		public int count_win_o = 0;
		public int count_no_win = 0;
		public int count_round = 0;
		public Boolean enable = true;
		private Button[] but;
		private Control parent;
		private LMD_TopBoard topBoard;
		#endregion
		#region LMD_Game
		public LMD_Game(Control parent)
		{
			this.parent = parent;
			this.BackColor = Color.FromArgb(100, 100, 100);
			this.topBoard = ((Form1)parent).topBoard;
			this.Size = new Size(parent.Width / 2 - 2, parent.Height - topBoard.Height - 2);
			this.Location = new Point(1, topBoard.Bottom);
			this.Paint += (o, e) =>
				{
					Graphics g = e.Graphics;
					g.Clear(this.BackColor);
					Pen pen = new Pen(Color.FromArgb(192, 192, 192), 10);

					g.DrawLine(pen, 0, this.Height / 3, this.Width, this.Height / 3);
					g.DrawLine(pen, 0, this.Height / 3 + this.Height / 3, this.Width, this.Height / 3 + this.Height / 3);

					g.DrawLine(pen, this.Width / 3, 0, this.Width / 3, this.Height);
					g.DrawLine(pen, this.Width / 3 + this.Width / 3, 0, this.Width / 3 + this.Width / 3, this.Height);
				};
			parent.Controls.Add(this);

			but = new Button[9];
			for (int i = 0; i < but.Length; i++)
			{
				but[i] = new Button();
				but[i].FlatAppearance.BorderSize = 0;
				but[i].FlatStyle = FlatStyle.Flat;
			//	but[i].BackColor = Color.FromArgb(192, 192, 192);
				but[i].Font = new System.Drawing.Font("Arial", 40f, FontStyle.Bold);
				but[i].ForeColor = Color.FromArgb(220, 120, 50);
				but[i].Location = new Point(0, 0);
				but[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(130, 130, 130);
				but[i].Size = new Size(this.Width / 3-2, this.Height / 3-2);
				but[i].Click += (o, e) =>
					{
						if (enable)
						{
							List<float> x = getVectorX();

							if (step != null)
								for (int j = 0; j < but.Length; j++)
									if (but[j].ContainsFocus)
									{
										if (CheckRect(j + 1))
										{
											but[j].Text = "x";
											step(x, j + 1, "x");
										}
									}
						}
					};
				
				this.Controls.Add(but[i]);
			}
			but[1].Location = new Point(but[0].Right + 4, 0);
			but[2].Location = new Point(but[1].Right + 4, 0);
			but[3].Location = new Point(0, but[0].Bottom + 4);
			but[4].Location = new Point(but[3].Right + 4, but[0].Bottom + 4);
			but[5].Location = new Point(but[4].Right + 4, but[0].Bottom + 4);
			but[6].Location = new Point(0, but[5].Bottom + 4);
			but[7].Location = new Point(but[6].Right + 4, but[5].Bottom + 4);
			but[8].Location = new Point(but[7].Right + 4, but[5].Bottom + 4);
		}
		#endregion
		#region setO
		public void setO(int index)
		{
			but[index - 1].Text = "o";

			present_step = TYPE_STEP.X;

			if (step != null)
				step(getVectorX(), index, "o");
		}
		#endregion
		#region setX
		public void setX(int index)
		{
			but[index - 1].Text = "x";

			present_step = TYPE_STEP.O;

			if (step != null)
				step(getVectorX(), index, "x");
		}
		#endregion
		#region getVectorX
		public List<float> getVectorX()
		{
			List<float> x = new List<float>();

			for (int i1 = 0; i1 < but.Length; i1++)
				if (but[i1].Text == "o")
					x.Add(-0.1f);
				else if (but[i1].Text == "x")
					x.Add(0.9f);
				else x.Add(-0.5f);

			return x;
		}
		#endregion
		#region getTableConvertHistory
		public int[] getTableConvertHistory()
		{
			int[] n = new int[9];

			for (int i = 0; i < n.Length; i++)
				if (but[i].Text == "x")
					n[i] = 1;
				else if (but[i].Text == "o")
					n[i] = 2;
				else n[i] = 0;

			return n;
		}
		#endregion
		#region setRandom
		public int setRandom(TYPE_STEP type)
		{
			Random random = new Random();
			int index = 0;
			Boolean rect = false;

			do
			{
				index = random.Next(1, 10);
				rect = CheckRect(index);
			} while (!rect);

			if (type == TYPE_STEP.X)
				setX(index);
			else if (type == TYPE_STEP.O)
				setO(index);

			return index;
		}
		#endregion
		#region CheckRect
		public Boolean CheckRect(int index)
		{
			Boolean result = false;

			if (getCore(index) == "x")
				result = false;
			else if (getCore(index) == "o")
				result = false;
			else result = true;

			return result;
		}
		#endregion
		#region getCore
		public String getCore(int index)
		{
			return but[index - 1].Text;
		}
		#endregion
		#region CheckWinX
		public Boolean CheckWinX()
		{
			Boolean result = false;
			Color color = Color.FromArgb(150, 150, 150);

			if (getCore(1) == "x" && getCore(2) == "x" && getCore(3) == "x")
			{
				result = true;

				but[0].BackColor = color;
				but[1].BackColor = color;
				but[2].BackColor = color;
			}
			if (getCore(4) == "x" && getCore(5) == "x" && getCore(6) == "x")
			{
				result = true;

				but[3].BackColor = color;
				but[4].BackColor = color;
				but[5].BackColor = color;
			}
			if (getCore(7) == "x" && getCore(8) == "x" && getCore(9) == "x")
			{
				result = true;

				but[6].BackColor = color;
				but[7].BackColor = color;
				but[8].BackColor = color;
			}
			if (getCore(1) == "x" && getCore(5) == "x" && getCore(9) == "x")
			{
				result = true;

				but[0].BackColor = color;
				but[4].BackColor = color;
				but[8].BackColor = color;
			}
			if (getCore(3) == "x" && getCore(5) == "x" && getCore(7) == "x")
			{
				result = true;

				but[2].BackColor = color;
				but[4].BackColor = color;
				but[6].BackColor = color;
			}
			if (getCore(1) == "x" && getCore(4) == "x" && getCore(7) == "x")
			{
				result = true;

				but[0].BackColor = color;
				but[3].BackColor = color;
				but[6].BackColor = color;
			}
			if (getCore(2) == "x" && getCore(5) == "x" && getCore(8) == "x")
			{
				result = true;

				but[1].BackColor = color;
				but[4].BackColor = color;
				but[7].BackColor = color;
			}
			if (getCore(3) == "x" && getCore(6) == "x" && getCore(9) == "x")
			{
				result = true;

				but[2].BackColor = color;
				but[5].BackColor = color;
				but[8].BackColor = color;
			}

			return result;
		}
		#endregion
		#region CheckWinO
		public Boolean CheckWinO()
		{
			Boolean result = false;
			Color color = Color.FromArgb(150, 150, 150);

			if (getCore(1) == "o" && getCore(2) == "o" && getCore(3) == "o")
			{
				result = true;

				but[0].BackColor = color;
				but[1].BackColor = color;
				but[2].BackColor = color;
			}
			if (getCore(4) == "o" && getCore(5) == "o" && getCore(6) == "o")
			{
				result = true;

				but[3].BackColor = color;
				but[4].BackColor = color;
				but[5].BackColor = color;
			}
			if (getCore(7) == "o" && getCore(8) == "o" && getCore(9) == "o")
			{
				result = true;

				but[6].BackColor = color;
				but[7].BackColor = color;
				but[8].BackColor = color;
			}
			if (getCore(1) == "o" && getCore(5) == "o" && getCore(9) == "o")
			{
				result = true;

				but[0].BackColor = color;
				but[4].BackColor = color;
				but[8].BackColor = color;
			}
			if (getCore(3) == "o" && getCore(5) == "o" && getCore(7) == "o")
			{
				result = true;

				but[2].BackColor = color;
				but[4].BackColor = color;
				but[6].BackColor = color;
			}
			if (getCore(1) == "o" && getCore(4) == "o" && getCore(7) == "o")
			{
				result = true;

				but[0].BackColor = color;
				but[3].BackColor = color;
				but[6].BackColor = color;
			}
			if (getCore(2) == "o" && getCore(5) == "o" && getCore(8) == "o")
			{
				result = true;

				but[1].BackColor = color;
				but[4].BackColor = color;
				but[7].BackColor = color;
			}
			if (getCore(3) == "o" && getCore(6) == "o" && getCore(9) == "o")
			{
				result = true;

				but[2].BackColor = color;
				but[5].BackColor = color;
				but[8].BackColor = color;
			}

			return result;
		}
		#endregion
		#region CheckNoWin
		public Boolean CheckNoWin()
		{
			Boolean result = false;
			int n = 0;

			for (int i = 0; i < but.Length; i++)
				if (but[i].Text == "")
					n++;

			if (n >= 9)
				result = true;

			return result;
		}
		#endregion
		#region Clear
		public void Clear()
		{
			for (int i = 0; i < but.Length; i++)
			{
				but[i].Text = "";
				but[i].BackColor = this.BackColor;
			}
		}
		#endregion
	}
}
