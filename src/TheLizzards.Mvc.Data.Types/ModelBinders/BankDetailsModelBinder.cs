﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TheLizzards.Data.Types.Entites;

namespace TheLizzards.Mvc.Data.Types.ApplicationServices
{
	public sealed class BankDetailsModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			var key = $"{bindingContext.ModelName}.AccountNumber";
			var details = GetBankDetails(bindingContext, key);

			bindingContext.Result = ModelBindingResult.Success(details.Item2);

			return TaskCache.CompletedTask;
		}

		private static Tuple<CultureInfo, BankDetails, string> GetBankDetails(
			ModelBindingContext bindingContext
			, string key)
		{
			var accountNumberField = bindingContext
				.ValueProvider
				.GetValue(key);
			var fieldValue = accountNumberField.FirstValue;

			return Tuple.Create(
				accountNumberField.Culture
				, new BankDetails(accountNumberField.FirstValue)
				, accountNumberField.FirstValue);
		}
	}
}