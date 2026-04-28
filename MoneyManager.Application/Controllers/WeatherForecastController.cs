using AutoMapper;
using MoneyManager.DTO;
using MoneyManager.Models;
using MoneyManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MoneyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherForecastController : ControllerBase
    {
        private readonly WeatherService _weatherService;
        private readonly IMapper _mapper;

        // This is the constructor Dependency injection
        // Here we add the services which we registered in program.cs to this controller
        public WeatherForecastController(WeatherService weatherService, IMapper mapper)
        {
            _weatherService = weatherService;
            _mapper = mapper;
        }

        // Apply pagination
        // To get 10 records per page
        [HttpGet]
        public async Task<IActionResult> GetAll(int page=1, int pageSize=10)
        {
            //throw new Exception("Testing the exception middleware");
            
            return Ok(await _weatherService.GetAll(page, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> PostWeather(WeatherRequest request)
        {
            // Map the DTO request to the DB entity [Manual method]
            // var weather = new Weather()
            // {
            //     City = request.City,
            //     Temperature = request.Temperature
            // };

            // Map the DTO request to DB entity using Auto mapper
            var weather = _mapper.Map<Weather>(request);

            var result = await _weatherService.Post(weather);

            // This sends:
            // 201 Created
            // Location header
            // Proper REST response
            return CreatedAtAction(nameof(GetById), new{id = result.Id}, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _weatherService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeather(int id, WeatherRequest request)
        {
            // Map the DTO request to the DB entity
            // This is manual object mapping.
            var weather = new Weather()
            {
                City = request.City,
                Temperature = request.Temperature
            };
            var result = await _weatherService.Update(id, weather);
            if(!result) return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id)
        {
            var result = await _weatherService.Delete(id);
            if(!result) return NotFound();

            return NoContent();
        }
    }
}
