﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TheLizzards.Common.DataParts.Entites;

namespace TheLizzards.Mvc.DataParts.ApplicationServices
{
	public sealed class EmailModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			try
			{
				var emailValue
					= bindingContext
						.ValueProvider
						.GetValue(bindingContext.ModelName)
						.FirstValue;

				var email = new Email(emailValue);

				bindingContext.Result = ModelBindingResult.Success(email);
			}
			catch (ArgumentException)
			{
				bindingContext.Result = ModelBindingResult.Failed();
			}
			return TaskCache.CompletedTask;
		}
	}
}