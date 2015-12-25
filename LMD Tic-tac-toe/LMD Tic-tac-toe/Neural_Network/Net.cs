using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMD_Tic_tac_toe.Neural_Network
{
	public class Net
	{
		#region variables
		public List<Neuron> neurons;
		#endregion
		#region Net
		public Net()
		{
			neurons = new List<Neuron>();
		}
		#endregion
		#region Recognize
		public int Recognize(List<float> x, System.Windows.Forms.ListBox listBox)
		{
			listBox.Items.Add("Распознование:");
			int index = 0;
			float max_power = neurons[0].w[0];

			for (int i = 0; i < neurons.Count; i++)
			{
				for (int j = 0; j < neurons[i].w.Count; j++)
					neurons[i].power += neurons[i].w[j] * x[j];

		//		listBox.Items.Add("index - [" + i + "] power - [" + neurons[i].power + "] position - [" + neurons[i].index + "]");
				if (neurons[i].power > max_power)
				{
					max_power = neurons[i].power;
					index = i;
				}
			}


			listBox.Items.Add("Максимальный нейрон - index[" + index + "] power[" + max_power + "] position[" + neurons[index].index + "]");

			for (int i = 0; i < neurons.Count; i++)
				neurons[i].power = 0;

			return neurons[index].index;
		}
		#endregion
		#region AddNeuron
		public void AddNeuron(int index, List<float> w, System.Windows.Forms.ListBox listBox)
		{
			listBox.Items.Add("Добавлен новый нейрон [" + index + "]");
			Neuron n = new Neuron();
			n.index = index;
			n.w = w;
			neurons.Add(n);
		}
		#endregion
		#region Correct
		public void Correct(List<float> x, int index, float speed, int delta, System.Windows.Forms.ListBox listBox)
		{
			listBox.Items.Add("Корректировка весов нейрона [" + neurons[index].index + "]");
			listBox.Items.Add("Delta [" + delta + "] speed [" + speed + "]");
			float p = 0;
			for (int i = 0; i < neurons[index].w.Count; i++)
				neurons[index].w[i] = neurons[index].w[i] + speed * delta * x[i];
			for (int i = 0; i < neurons[index].w.Count; i++)
				p += neurons[index].w[i] * x[i];

			listBox.Items.Add("Результат: index [" + index + "] position [" + neurons[index].index + "] power [" + p + "]");
		}
		#endregion
	}
}
