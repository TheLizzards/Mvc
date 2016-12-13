﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TheLizzards.Data.CQRS.Contracts;

namespace TheLizzards.Data.CQRS.Entities
{
	public sealed class CommandBus : ICommandBus
	{
		private readonly IEnumerable<ICommandHandler> commandHandlers;

		private bool disposedValue = false;

		public CommandBus(IEnumerable<ICommandHandler> commandHandlers)
		{
			this.commandHandlers = commandHandlers;
		}

		public Task Execute(ICommand command)
			=> Task.Run(() 
				=> GetCommandsHandlers(command)
					.ToList()
					.ForEach(async handler => await handler.Execute(command)));

		public async Task<IEnumerable<ValidationResult>> Validate(ICommand command)
		{
			var results = new List<ValidationResult>(10);
			
			foreach(var handler in GetCommandsHandlers(command))
			{
				results.AddRange(await handler.Validate(command));
			}
			
			return results;
		}

		public void Dispose() => Dispose(true);

		private IEnumerable<ICommandHandler> GetCommandsHandlers(ICommand command)
			=> this.commandHandlers.Where(x => x.CanHandle(command));

		private void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					foreach (var handler in commandHandlers)
					{
						handler.Dispose();
					}
				}

				disposedValue = true;
			}
		}
	}
}
