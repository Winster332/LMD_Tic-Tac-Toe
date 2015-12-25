using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LMD_Tic_tac_toe.Neural_Network
{
	public class Neuron
	{
		public List<float> w;
		public float power;
		public int index;

		public Neuron()
		{
			w = new List<float>();
		}
	}
}
