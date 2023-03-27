using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScrapApi.Data;

namespace StockScrapApi.Helpers
{
    public class GetFirebaseData
    {
        private readonly ApplicationDbContext _context;
        private readonly HttpClient _client;

        public GetFirebaseData(ApplicationDbContext context, HttpClient client)
        {
            _context = context;
            _client = client;
        }
        public async Task<bool> FetchData()
        {
            JObject personsRaw = JObject.Parse(await new HttpClient().GetStringAsync("https://data-archive-27724-default-rtdb.asia-southeast1.firebasedatabase.app/persons.json"));
            JObject companyRaw = JObject.Parse(await new HttpClient().GetStringAsync("https://data-archive-27724-default-rtdb.asia-southeast1.firebasedatabase.app/companies.json"));

            List<Person> persons = new List<Person>();
            List<Company> companies = new List<Company>();

            foreach (var ind in personsRaw)
            {
                Person person = JsonConvert.DeserializeObject<Person>(ind.Value.ToString());
                persons.Add(person);

                var response = await _client.GetAsync(person.imageUrl);
            }


            foreach (var ind in companyRaw)
            {
                Company company = JsonConvert.DeserializeObject<Company>(ind.Value.ToString());
                companies.Add(company);

                var response = await _client.GetAsync(company.logo);

            }

            var results = persons.Join(companies, a => a.CompanyId, b => b.companyId,
                (a, b) => new NewPerson
                {
                    Name = a.Name,
                    Designation = a.Designation,
                    Phone = a.phone,
                    CompanyCode = b.tradingCode,
                    imageUrl = a.imageUrl
                }).ToList();

            Console.Write(JsonConvert.SerializeObject(results[1]));

            return true;
        }

        public class Person
        {
            public string PersonId { get; set; }
            public string Designation { get; set; }
            public string Name { get; set; }
            public string Bio { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string imageUrl { get; set; }
            public string CompanyId { get; set; }
        }

        public class Company
        {
            public string companyId { get; set; }
            public string companyName { get; set; }
            public string scripCode { get; set; }
            public string logo { get; set; }
            public string siteLink { get; set; }
            public DateTime? time { get; set; }
            public string tradingCode { get; set; }
        }

        public class NewPerson
        {
            public string Designation { get; set; }
            public string Name { get; set; }
            public string Bio { get; set; }
            public string Phone { get; set; }
            public string email { get; set; }
            public string imageUrl { get; set; }
            public string CompanyCode { get; set; }
        }
    }
}