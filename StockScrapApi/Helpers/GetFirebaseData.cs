using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
            JObject personsRaw = JObject.Parse(await new HttpClient().GetStringAsync("https://data-archive-27724-default-rtdb.asia-southeast1.firebasedatabase.app/persons.json"));
            JObject companyRaw = JObject.Parse(await new HttpClient().GetStringAsync("https://data-archive-27724-default-rtdb.asia-southeast1.firebasedatabase.app/companies.json"));

            List<PersonRaw> persons = new List<PersonRaw>();
            List<CompanyRaw> companies = new List<CompanyRaw>();

            foreach (var ind in personsRaw)
            {
                PersonRaw person = JsonConvert.DeserializeObject<PersonRaw>(ind.Value.ToString());
                persons.Add(person);

            }


            foreach (var ind in companyRaw)
            {
                CompanyRaw company = JsonConvert.DeserializeObject<CompanyRaw>(ind.Value.ToString());
                companies.Add(company);

                var client = new HttpClient();
                var guid = Guid.NewGuid().ToString();

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Companies", "logo", string.Format("{0}.jpg", guid));

                

                var compId = _context.companies.Where(a => a.CompanyCode == company.tradingCode).Select(b => b.Id).FirstOrDefault();

                var companyLogo = new CompanyLogo
                {
                    CompanyId = compId,
                    LogoPath = path
                };

                var check = _context.companyLogos.Where(a => a.CompanyId == compId).Any();

                if (!check)
                {

                    try
                    {
                        var imageBytes = await client.GetByteArrayAsync(company.logo);
                        await File.WriteAllBytesAsync(path, imageBytes);
                        _context.Add(companyLogo);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex, "An error occured writing image.");
                    }

                }

                client.Dispose();

            }

            var results = persons.Join(companies, a => a.CompanyId, b => b.companyId,
                (a, b) => new PersonRawDTO
                {
                    Name = a.Name,
                    Designation = a.Designation,
                    Phone = a.phone,
                    Email = a.email,
                    Bio = a.Bio,
                    ImageUrl = a.imageUrl,
                    CompanyCode = b.tradingCode
                }).ToList();

            Console.Write(JsonConvert.SerializeObject(results[1]));

            foreach(var result in results)
            {
                var compId = _context.companies.Where(a=> a.CompanyCode == result.CompanyCode).Select(b => b.Id).FirstOrDefault();
                var person = _mapper.Map<Person>(result);
                person.CompanyId = compId;

                var client = new HttpClient();
                var guid = Guid.NewGuid().ToString();

                var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Persons", String.Format("{0}.jpg", guid));


                try
                {
                    var imageBytes = await client.GetByteArrayAsync(result.ImageUrl);
                    await File.WriteAllBytesAsync(path, imageBytes);
                    person.ImagePath = path;

                    _context.Add(person);
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "An error occured writing image.");
                }


            }

            _context.SaveChanges();

        }

        public class PersonRaw
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

        public class CompanyRaw
        {
            public string companyId { get; set; }
            public string companyName { get; set; }
            public string scripCode { get; set; }
            public string logo { get; set; }
            public string siteLink { get; set; }
            public DateTime? time { get; set; }
            public string tradingCode { get; set; }
        }
    }
}