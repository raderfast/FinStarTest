using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Models;
using Newtonsoft.Json;
using RestSharp;
namespace FinStarClient.Pages
{
    public partial class FetchData
    {
        private List<ValueSet> ValueSets = new List<ValueSet>();

        private int _page = 1;

        private int _itemsOnPage = 2;

        private int _pageCount = 1;

        protected override async Task OnInitializedAsync()
        {
            try
            {


                var result = await Http.GetAsync(@"https://localhost:7136/TestService");

                if (result.IsSuccessStatusCode)
                {
                    ValueSets = JsonConvert.DeserializeObject<List<ValueSet>>(await result.Content.ReadAsStringAsync()) ?? new List<ValueSet>();
                }

                _pageCount = Convert.ToInt32(Math.Ceiling((double)ValueSets.Count / _itemsOnPage));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
