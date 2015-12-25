using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace LMD_Tic_tac_toe.GUI
{
	public class LMD_TopBoard : Panel
	{
		#region variables
		public Form1.STATE_PROGRAM type_game
		{
			get
			{
				return _type_game;
			}
			set
			{
				_type_game = value;
				Color color = Color.FromArgb(70, 70, 70);
				switch (_type_game)
				{
					case Form1.STATE_PROGRAM.auto:
						but[3].BackColor = color;
						but[4].BackColor = this.BackColor;
						but[5].BackColor = this.BackColor;
						break;
					case Form1.STATE_PROGRAM.game:
						but[4].BackColor = color;
						but[3].BackColor = this.BackColor;
						but[5].BackColor = this.BackColor;
						break;
					case Form1.STATE_PROGRAM.handle:
						but[5].BackColor = color;
						but[4].BackColor = this.BackColor;
						but[3].BackColor = this.BackColor;
						break;
				}

				if (changedState != null)
					changedState(_type_game);
			}
		}
		public delegate void ButtonClick();
		public delegate void ChangedSTATE(Form1.STATE_PROGRAM state);
		public delegate void ButtonClickMenu();

		public event ChangedSTATE changedState;
		public event ButtonClick butClick;
		public event ButtonClickMenu butClickMenu;

		public Point pos_title_no_free;
		public Button[] but;
		public Button[] but_mode;
		public Button but_account;
		public Color color_title;
		public Color color_move;
		private Form1.STATE_PROGRAM _type_game;
		private Form form;
		#endregion
		#region LMD_TopBoard
		public LMD_TopBoard(Form form, Boolean free)
		{
			#region this
			this.pos_title_no_free = new Point(35, 5);
			this.form = form;
			this.color_title = Color.FromArgb(192, 192, 192);
			this.color_move = Color.FromArgb(100, 100, 100);
			form.Controls.Add(this);
			this.BackColor = Color.FromArgb(50, 50, 50);
			base.Location = new Point(1, 1);
			base.Size = new Size(form.Width - 2, 25);

			base.Paint += delegate(object o, PaintEventArgs e)
			{
				if (free)
					e.Graphics.DrawString(form.Text, new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(this.color_title), pos_title_no_free);
				else if (!free)
					e.Graphics.DrawString(form.Text, new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(this.color_title), 5, 5);

				e.Graphics.DrawString("move", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(this.color_move), new PointF((float)(this.Width / 2 - 30), 5f));
			};

			form.TextChanged += (o, e) =>
				{
					this.Invalidate();
				};
			#endregion
			#region mouse
			bool b_m_Click = false;
			Point p_m_Down = default(Point);
			base.MouseDown += delegate(object o, MouseEventArgs e)
			{
				b_m_Click = true;
				p_m_Down = e.Location;
			};
			base.MouseMove += delegate(object o, MouseEventArgs e)
			{
				if (b_m_Click)
				{
					form.Location = new Point(Cursor.Position.X - p_m_Down.X, Cursor.Position.Y - p_m_Down.Y);
				}
			};
			base.MouseUp += delegate(object o, MouseEventArgs e)
			{
				b_m_Click = false;
			};
			#endregion
			#region no free
			if (!free)
			{
				this.but = new Button[6];
				int num = base.Width - 40;
				for (int i = 0; i < this.but.Length; i++)
				{
					this.but[i] = new Button();
					base.Controls.Add(this.but[i]);
					this.but[i].Size = new Size(40, base.Height);
					this.but[i].FlatAppearance.BorderSize = 0;
					this.but[i].FlatStyle = FlatStyle.Flat;
					this.but[i].Location = new Point(num, 0);
					if (i > 0)	this.but[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(70, 70, 70);
					this.but[i].Click += delegate(object param0, EventArgs param1)
					{
						for (int j = 0; j < this.but.Length; j++)
						{
							if (this.but[j].ContainsFocus)
							{
								switch (j)
								{
									case 0:
										form.Close();
										break;
									case 1:
										{
											if (butClick != null)
												butClick();
											break;
										}
									case 2:
										form.WindowState = FormWindowState.Minimized;
										break;

									case 3:
										type_game = Form1.STATE_PROGRAM.auto;
										break;
									case 4:
										type_game = Form1.STATE_PROGRAM.game;
										break;
									case 5:
										type_game = Form1.STATE_PROGRAM.handle;
										break;
								}

							}
						}
					};
					if (i == 2)
						num -= 3;
					if (i > 2)
					{
						but[i].FlatAppearance.MouseDownBackColor = Color.FromArgb(80, 80, 80);
					}
					num -= this.but[i].Width;
				}
				this.but[0].BackColor = Color.FromArgb(150, 150, 50, 50);
				this.but[0].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("X", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb((int)this.but[0].BackColor.R, 192, 192, 192)), new PointF(13f, 5f));
				};
				this.but[1].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("C", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 5.5f));
				};
				this.but[2].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("_", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 3.5f));
				};
				this.but[3].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("A", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 3.5f));
				};
				this.but[4].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("G", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 3.5f));
				};
				this.but[5].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("H", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 3.5f));
				};

			}
			#endregion
			#region free
			else if (free)
			{
				this.but = new Button[3];
				int num = base.Width - 40;
				for (int i = 0; i < this.but.Length; i++)
				{
					this.but[i] = new Button();
					base.Controls.Add(this.but[i]);
					this.but[i].Size = new Size(40, base.Height);
					this.but[i].FlatAppearance.BorderSize = 0;
					this.but[i].FlatStyle = FlatStyle.Flat;
					this.but[i].Location = new Point(num, 0);
					this.but[i].Click += delegate(object param0, EventArgs param1)
					{
						for (int j = 0; j < this.but.Length; j++)
						{
							if (this.but[j].ContainsFocus)
							{
								switch (j)
								{
									case 0:
										form.Close();
										break;
									case 1:
										{

											try
											{
												if (butClick != null)
													butClick();

												System.Diagnostics.Process.Start("READ_TO_END.docx");
											}
											catch
											{
												MessageBox.Show(@"Не удалось открыть файл документации.\nВозможные причины:\n
												Не не установелн Microsoft office\nФайл был утерян", "Ошибка открытия документации!");
											};

											break;
										}
									case 2:
										form.WindowState = FormWindowState.Minimized;
										break;
								}

							}
						}
					};
					num -= this.but[i].Width + 1;
				}
				this.but[0].BackColor = Color.FromArgb(150, 150, 50, 50);
				this.but[0].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("X", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb((int)this.but[0].BackColor.R, 192, 192, 192)), new PointF(13f, 5f));
				};
				this.but[1].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("?", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 5.5f));
				};
				this.but[2].Paint += delegate(object o, PaintEventArgs e)
				{
					e.Graphics.DrawString("_", new Font("Arial", 10f, FontStyle.Bold), new SolidBrush(Color.FromArgb(192, 192, 192)), new PointF(14f, 3.5f));
				};

				but_account = new Button();
				but_account.Text = "A";
				but_account.FlatAppearance.BorderSize = 0;
				but_account.FlatStyle = FlatStyle.Flat;
				but_account.BackColor = Color.FromArgb(192, 192, 192);
				but_account.ForeColor = this.BackColor;
				but_account.Size = new Size(25, this.Height - 6);
				but_account.Location = new Point(5, 3);
				but_account.Enabled = false;
				but_account.Click += (o, e) =>
				{
					//	BlankAccount ba = new BlankAccount();
					//	ba.Show();
				};
				this.Controls.Add(but_account);
			}
			#endregion

			this.type_game = Form1.STATE_PROGRAM.game;
		}
		#endregion
	}
}
