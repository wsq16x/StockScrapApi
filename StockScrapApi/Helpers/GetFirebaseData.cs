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

        public async Task FetchData()
        {
            if (!_context.personsFirebase.Any())
            {
                List<PersonFirebase> persons = new List<PersonFirebase>();

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

            if (!_context.companiesFirebase.Any())
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

            //var client = new HttpClient();
            //var guid = Guid.NewGuid().ToString();

            //var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Companies", "logo", string.Format("{0}.jpg", guid));

            //var compId = _context.companies.Where(a => a.CompanyCode == company.tradingCode).Select(b => b.Id).FirstOrDefault();

            //var companyLogo = new CompanyLogo
            //{
            //    CompanyId = compId,
            //    LogoPath = path
            //};



            //var results = persons.Join(companies, a => a.CompanyId, b => b.companyId,
            //    (a, b) => new PersonRawDTO
            //    {
            //        Name = a.Name,
            //        Designation = a.Designation,
            //        Phone = a.phone,
            //        Email = a.email,
            //        Bio = a.Bio,
            //        ImageUrl = a.imageUrl,
            //        CompanyCode = b.tradingCode
            //    }).ToList();

            //foreach(var result in results)
            //{
            //    var compId = _context.companies.Where(a=> a.CompanyCode == result.CompanyCode).Select(b => b.Id).FirstOrDefault();
            //    var person = _mapper.Map<Person>(result);
            //    person.CompanyId = compId;

            //    var client = new HttpClient();
            //    var guid = Guid.NewGuid().ToString();

            //    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Persons", String.Format("{0}.jpg", guid));

            //    try
            //    {
            //        var imageBytes = await client.GetByteArrayAsync(result.ImageUrl);
            //        await File.WriteAllBytesAsync(path, imageBytes);
            //        person.ImagePath = path;

            //        _context.Add(person);
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogInformation(ex, "An error occured writing image.");
            //    }

            //}


            await _context.SaveChangesAsync();
        }
    }
}