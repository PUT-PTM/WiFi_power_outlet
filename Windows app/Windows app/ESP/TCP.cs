using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace ESP
{
	public class TCP
	{
		public TCP()
		{
			Tcp = new TcpClient();
			StreamData = Stream.Null;
			Ascii = new ASCIIEncoding();
			Log = new List<string>();

		}

		public TcpClient Tcp { get; set; }
		public Stream StreamData { get; set; }
		public ASCIIEncoding Ascii { get; set; }
		public List<string> Log { get; set; }

		

		public string CreateLog()
		{
			//foreach (var e in Log)
			//{

			//}

			return Log.Last();
		}

		public void Dispose()
		{
			Tcp = null;
			StreamData = null;
			Ascii = null;
			Log = null;
		}
	}
}