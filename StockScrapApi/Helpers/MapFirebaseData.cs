using AutoMapper;
using Microsoft.EntityFrameworkCore;
using StockScrapApi.Data;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace StockScrapApi.Helpers
{
    public class MapFirebaseData : IMapFirebaseData
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<MapFirebaseData> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public MapFirebaseData(ApplicationDbContext context, IMapper mapper, ILogger<MapFirebaseData> logger, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task MoveData()
        {
            var data = await _context.personsFirebase.Join(_context.companiesFirebase, a => a.CompanyId, b => b.companyID,
                (a, b) => new PersonRawDTO
                {
                    Bio = a.Bio,
                    Designation = a.Designation,
                    tradingCode = b.tradingCode,
                    Name = a.Name,
                    Email = a.email,
                    Phone = a.phone,
                    ImageUrl = a.imageUrl,
                    FirebaseCompId = b.companyID
                }).ToListAsync();

            var persons = new List<Person>();

            foreach(var ind in data)
            {
                var CompId = await _context.companies.Where(a=> a.CompanyCode == ind.tradingCode).Select(b=> b.Id).FirstOrDefaultAsync();
      
                if(CompId != Guid.Empty)
                {
                    var person = _mapper.Map<Person>(ind);
                    person.CompanyId = CompId;

                    //persons.Add(person);
                    _context.Add(person);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "failed to Move Persons Data for {0}.", person.Email);
                    }
                }
            }

        }

        public async Task GetProfilePictures()
        {
            var persons = _context.persons.ToList();
            int count = 1;

            foreach(var person in persons) {
                var check = _context.profilePictures.Where(a => a.PersonId == person.Id).Any();
                if(!check)
                {
                    var client = new HttpClient();
                    //var guid = Guid.NewGuid().ToString();

                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Persons", String.Format("{0}.jpg", person.Id));

                    try
                    {
                        var imageBytes = await client.GetByteArrayAsync(person.ImageUrl);
                        await File.WriteAllBytesAsync(path, imageBytes);
                        var profPic = new ProfilePicture();

                        profPic.PersonId = person.Id;
                        //profPic.ImagePath = string.Format("{0}.jpg", person.Id);
                        profPic.ImagePath = path;

                        _context.Add(profPic);
                        await _context.SaveChangesAsync();
                        count++;
                        _logger.LogInformation("Fetched Image {0} of {1}", count, persons.Count);
                    }
                    catch (Exception ex)
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        _logger.LogError(ex, "An error occured writing image.");
                    }
                }
            }
        }

        public async Task GetCompanyLogo()
        {
            int count = 1;
            var Data = await _context.companies.Join(_context.companiesFirebase, a => a.CompanyCode, b => b.tradingCode,
                (a, b) => new CompanyRawDto
                {
                    CompanyId = a.Id,
                    ImageUrl = b.logo
                }).ToListAsync();

            foreach(var company in Data)
            {
                if(!_context.companyLogos.Where(a=> a.CompanyId == company.CompanyId).Any())
                {
                    var client = new HttpClient();
                    //var guid = Guid.NewGuid().ToString();

                    var path = Path.Combine(_webHostEnvironment.WebRootPath, "Images", "Companies", "logo", String.Format("{0}.jpg", company.CompanyId));

                    try
                    {
                        var imageBytes = await client.GetByteArrayAsync(company.ImageUrl);
                        await File.WriteAllBytesAsync(path, imageBytes);
                        var companyLogo = new CompanyLogo();

                        companyLogo.CompanyId = company.CompanyId;
                        //companyLogo.LogoPath = string.Format("{0}.jpg", company.CompanyId);
                        companyLogo.LogoPath = path;

                        _context.Add(companyLogo);
                        await _context.SaveChangesAsync();
                        count++;
                        _logger.LogInformation("Fetched Image {0} of {1}", count, Data.Count);
                    }
                    catch (Exception ex)
                    {
                        if (File.Exists(path))
                        {
                            File.Delete(path);
                        }
                        _logger.LogError(ex, "An error occured writing image.");
                    }
                }


            }
        }
    }
}
