using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace LMD_Tic_tac_toe.GUI
{
	public class LMD_Information : Panel
	{
		#region variables
		public delegate void Remember(int index);
		public delegate void Correct(float speed, int delta, int index);
		public delegate void Save();
		public delegate void LoadNS();

		public event Save save;
		public event LoadNS load;
		public event Correct correct;
		public event Remember remember;

		private RichTextBox[] rtb;
		private Button[] but;
		public ListBox listBox;
		private Control parent;
		private LMD_TopBoard topBoard;
		#endregion
		#region LMD_Information
		public LMD_Information(Control parent)
		{
			this.parent = parent;
			this.BackColor = Color.FromArgb(100, 100, 100);
			this.topBoard = ((Form1)parent).topBoard;
			this.Size = new Size(parent.Width / 2 - 2, parent.Height - topBoard.Height-2);
			this.Location = new Point(parent.Width / 2+1, topBoard.Bottom);
			parent.Controls.Add(this);

			listBox = new ListBox();
			listBox.BackColor = Color.FromArgb(120, 120, 120);
			listBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			listBox.Size = new Size(this.Width - 20, this.Height - 100);
			listBox.ForeColor = Color.FromArgb(192, 192, 192);
			listBox.Location = new Point(10, 4);
			this.Controls.Add(listBox);

			listBox.Items.Add("------------------------------------------------ Tic-Tac-Toe -----------------------------------------------------------------");

			rtb = new RichTextBox[4];
			for (int i = 0; i < rtb.Length; i++)
			{
				rtb[i] = new RichTextBox();
				rtb[i].BorderStyle = System.Windows.Forms.BorderStyle.None;
				rtb[i].BackColor = Color.FromArgb(140, 140, 140);
				rtb[i].Size = new Size(30, 15);
				rtb[i].Location = new Point(100, listBox.Bottom);
				rtb[i].ForeColor = Color.FromArgb(70, 70, 70);
				this.Controls.Add(rtb[i]);
			}
			rtb[0].Width = 40;
			rtb[0].Location = new Point(65, listBox.Bottom + 21);
			rtb[1].Location = new Point(80, listBox.Bottom + 60);
			rtb[2].Location = new Point(170, listBox.Bottom + 60);
			rtb[3].Location = new Point(262, listBox.Bottom + 60);

			but = new Button[4];
			for (int i = 0; i < but.Length; i++)
			{
				but[i] = new Button();
				but[i].FlatAppearance.BorderSize = 0;
				but[i].FlatStyle = FlatStyle.Flat;
				but[i].Size = new Size(80, 100 / 4 - 2);
				but[i].BackColor = Color.FromArgb(50, 50, 50);
				but[i].ForeColor = Color.FromArgb(150, 150, 150);
				but[i].Font = new Font("Arial", 9);

				but[i].Click += (o, e) =>
				{
					try
					{
						Button b = (Button)o;

						switch (b.Text)
						{
							case "Обучить": if (remember != null) remember(int.Parse(rtb[0].Text)); break;
							case "Сохранить": if (save != null) save(); break;
							case "Загрузить": if (load != null) load(); break;
							case "Correct": if (correct != null) correct(float.Parse(rtb[1].Text), int.Parse(rtb[2].Text), int.Parse(rtb[3].Text)); break;
						}
					}
					catch { MessageBox.Show("Не корректная запись в поле!"); }
				};

				this.Controls.Add(but[i]);
			}
			but[0].Location = new Point(115, listBox.Bottom + 16);
			but[1].Location = new Point(200, listBox.Bottom + 16);
			but[2].Location = new Point(280, listBox.Bottom + 16);
			but[3].Location = new Point(300, listBox.Bottom + 56);
			but[3].Width = 60;
			but[3].Text = "Correct";
			but[0].Text = "Обучить";
			but[1].Text = "Сохранить";
			but[2].Text = "Загрузить";

			Label[] lab = new Label[4];
			for (int i = 0; i < lab.Length; i++)
			{
				lab[i] = new Label();
				lab[i].ForeColor = Color.FromArgb(12, 192, 192);
				lab[i].Font = new Font("Arial", 10f);
				this.Controls.Add(lab[i]);
			}
			lab[0].Location = new Point(7, listBox.Bottom + 20);
			lab[0].Text = "Индекс: ";

			lab[1].Location = new Point(7, listBox.Bottom + 60);
			lab[1].Text = "Скорость: ";

			lab[2].Width = 60;
			lab[2].Location = new Point(115, listBox.Bottom + 60);
			lab[2].Text = "Дельта: ";

			lab[3].Location = new Point(207, listBox.Bottom + 60);
			lab[3].Text = "Индекс: ";

			this.Paint += (o, e) =>
				{
					Graphics g = e.Graphics;
					g.Clear(this.BackColor);
					Pen pen = new Pen(Color.FromArgb(30, 120, 30), 3);
					
					g.DrawRectangle(pen, 5, listBox.Bottom + 9, this.Width - 10, 35);
					g.DrawRectangle(pen, 5, listBox.Bottom + 50, this.Width - 10, 35);
				};
		}
		#endregion
		#region WriteLine
		public void WriteLine(String text)
		{
			listBox.Items.Add(text);
		}
		#endregion
	}
}
