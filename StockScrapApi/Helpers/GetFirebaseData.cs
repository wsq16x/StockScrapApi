using AutoMapper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScrapApi.Data;
using StockScrapApi.Models;

namespace StockScrapApi.Helpers
{
    public class GetFirebaseData : IGetFirebaseData
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<GetFirebaseData> _logger;

        public GetFirebaseData(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper, ILogger<GetFirebaseData> logger)
        {
            _context = context;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public async Task<bool> FetchData()
        {
            if (!_context.personsFirebase.Any())
            {
                List<PersonFirebase> persons = new List<PersonFirebase>();

                try
                {
                    JObject personsRaw = JObject.Parse(await new HttpClient().GetStringAsync("https://data-archive-27724-default-rtdb.asia-southeast1.firebasedatabase.app/persons.json"));

                    foreach (var ind in personsRaw)
                    {
                        PersonFirebase person = JsonConvert.DeserializeObject<PersonFirebase>(ind.Value.ToString());
                        person.email = person.email == "Email Not Found" ? null : person.email;
                        person.email = person.email == "Phone Not Found" ? null : person.phone;

                        if (person.CompanyId == null)
                        {
                            _logger.LogInformation("Found null value for Person {0}", person.PersonId);
                        }
                        persons.Add(person);
                    }

                    await _context.AddRangeAsync(persons);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Failed to fetch person data from firebase!");
                    return false;
                }


            }

            if (!_context.companiesFirebase.Any())
            {
                try
                {
                    JObject companyRaw = JObject.Parse(await new HttpClient().GetStringAsync("https://data-archive-27724-default-rtdb.asia-southeast1.firebasedatabase.app/companies.json"));

                    List<CompanyFirebase> companies = new List<CompanyFirebase>();
                    foreach (var ind in companyRaw)
                    {
                        CompanyFirebase company = JsonConvert.DeserializeObject<CompanyFirebase>(ind.Value.ToString());

                        if (company.companyID == null)
                        {
                            _logger.LogInformation("Found null value for company {0}", company.tradingCode);
                        }
                        else
                        {
                            companies.Add(company);
                        }
                    }
                    await _context.AddRangeAsync(companies);
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex, "Failed to fetch Company data from firebase!");
                    return false;
                }

            }
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex, "Failed to write firebase Data.");
                return false;
            }
        }
    }
}