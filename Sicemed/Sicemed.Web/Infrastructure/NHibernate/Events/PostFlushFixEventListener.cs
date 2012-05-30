using System;
using NHibernate;
using NHibernate.Event;
using NHibernate.Event.Default;
using log4net;

namespace Sicemed.Web.Infrastructure.NHibernate.Events
{
	/// <summary>
	/// https://groups.google.com/forum/?hl=en&fromgroups#!topic/nhusers/CohlBNhxp8w
	/// 
	/// Workaround for this issue https://groups.google.com/forum/?hl=es&fromgroups#!topic/nhusers/WHU1J3D0V44
	/// </summary>
	public class PostFlushFixEventListener : DefaultFlushEventListener
	{

		public override void OnFlush(FlushEvent @event)
		{
			try
			{
				base.OnFlush(@event);
			}
			catch (Exception ex)
			{
				//swallow is a bug in NH
			}
		}

	}
}