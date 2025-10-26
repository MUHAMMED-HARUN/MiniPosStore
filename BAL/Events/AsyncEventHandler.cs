using System;
using System.Threading.Tasks;

namespace BAL.Events
{
	public delegate Task AsyncEventHandler<in TEventArgs>(object sender, TEventArgs e) where TEventArgs : EventArgs;
}


