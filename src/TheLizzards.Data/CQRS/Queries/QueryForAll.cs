﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TheLizzards.Data.CQRS.DataAccess;
using TheLizzards.Data.DDD;

namespace TheLizzards.Data.CQRS.Queries
{
	public abstract class QueryForAll<TPayload> : IAsyncQuery<IEnumerable<TPayload>>
		where TPayload : IAggregateRoot
	{
		private readonly IDataContext dataContext;
		private readonly ILoggerFactory loggerFactory;
		private readonly DatabaseParts parts;

		protected QueryForAll(IDataContext dataContext, ILoggerFactory loggerFactory, DatabaseParts parts)
		{
			this.dataContext = dataContext;
			this.loggerFactory = loggerFactory;
			this.parts = parts;
		}

		public Task<IEnumerable<TPayload>> Execute()
			=> new QueryForAllBuilder<TPayload>()
				.WithDataContext(this.dataContext)
				.WithLogger(this.loggerFactory)
				.WithDatabaseParts(this.parts)
				.Execute();
	}
}