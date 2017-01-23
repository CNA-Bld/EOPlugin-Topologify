using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Topologify
{
	public class Record
	{
		public List<int> DerivedCompleted { get; set; } = new List<int>();
		public List<int> MarkedCompleted { get; set; } = new List<int>();
	}
}
