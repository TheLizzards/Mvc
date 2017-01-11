﻿using System.Net;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using TheLizzards.Localisation.Entities;

namespace TheLizzards.Localisation.Services
{
	public sealed class GoogleGeocodingService : IGeocodingService
	{
		private const string serviceUrl = "http://maps.google.com/maps/api/geocode/xml?address={address}&sensor=false";

		public GoogleGeocodingService()
		{
		}

		public async Task<LocationPoint> GeocodeAsync(Address address)
		{
			var encodedAddress = GetEncodedAddress(address);
			var formattedQueryUrl =
				serviceUrl
				.Replace("{address}", encodedAddress);

			var queryResults = await RunQuery(formattedQueryUrl);

			return queryResults.Location;
		}

		private async Task<GoogleGeocodingResults> RunQuery(string formattedQueryUrl)
		{
			using (var client = new HttpClient())
			{
				using (var result = await client.GetAsync(formattedQueryUrl))
				{

					var resultContent = await result.Content.ReadAsStringAsync();

					return new GoogleGeocodingResults(resultContent);
				}
			}
		}


		private string GetEncodedAddress(Address address)
			=> WebUtility.HtmlEncode(address.ToString());
	}
}
