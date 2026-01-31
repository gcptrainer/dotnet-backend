using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using dotnetapp.Models;
using dotnetapp.Services;
using Microsoft.EntityFrameworkCore;
using dotnetapp.Data;
using System.Globalization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;


namespace dotnetapp.Controllers
{
    

    [ApiController]
    [Route("/api/cakes")]
    public class CakeController: ControllerBase
    {
        private readonly  CakeService _cakeService;
        private readonly IConfiguration _configuration;
        public CakeController(CakeService service,
        IConfiguration configuration){
            _cakeService=service;
            _configuration = configuration;
        }

         [HttpGet]
        public async Task<ActionResult<IEnumerable<Cake>>> GetAllCakes(){
            var list=await _cakeService.GetAllCakes();
            return Ok(list);

        } 


        [HttpGet("{cakeId}")]
        public async Task<ActionResult<Cake>> GetCakeById(int cakeId){
           var ele=await _cakeService.GetCakeById(cakeId);
            if(ele is null) return NotFound();
            return Ok(ele);
        } 

        [HttpPost]
        public async Task<ActionResult> AddCake([FromForm] CakeDTO request){
            // var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            // var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CakeImage.FileName)}";
            // if (!Directory.Exists(uploadsFolder))
            //     Directory.CreateDirectory(uploadsFolder);

            // var filePath = Path.Combine(uploadsFolder, fileName);

            // using (var stream = new FileStream(filePath, FileMode.Create))
            // {
            //     await request.CakeImage.CopyToAsync(stream); 
            // }
            // var cake = new Cake {
            //     Name = request.Name,
            //     Price = request.Price,
            //     Category = request.Category,
            //     Quantity = request.Quantity,
            //     CakeImage = filePath
            // };

            var blobServiceClient = new BlobServiceClient(_configuration["AzureStorage:ConnectionString"]);
            var containerClient = blobServiceClient.GetBlobContainerClient(
                _configuration["AzureStorage:ContainerName"]);

            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.CakeImage.FileName)}";
            var blobClient = containerClient.GetBlobClient(fileName);

            using var stream = request.CakeImage.OpenReadStream();
            await blobClient.UploadAsync(stream, overwrite: true);

            var filePath = blobClient.Uri.ToString();

             var cake = new Cake {
                Name = request.Name,
                Price = request.Price,
                Category = request.Category,
                Quantity = request.Quantity,
                CakeImage = filePath
            };

            try
            {
                bool ans = await _cakeService.AddCake(cake);
                if(ans) return Ok("Cake added successfully");
                else return  StatusCode(500,"Failed to add cake.");
                
            }
            catch(Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpPut("{cakeId}")]
        public async Task<ActionResult> UpdateCake(int cakeId,[FromBody] Cake cake){
            try
            {
                if(cakeId<0) return BadRequest();
                var stat= await _cakeService.UpdateCake(cakeId,cake);
                if(!stat) return NotFound("Cannot find any cake");
                if (stat) return Ok("Cake Updated Successfully");
                else return StatusCode(500,"Failed to update cake");
            }
            catch(Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }

        [HttpDelete("{cakeId}")]
        public async Task<ActionResult> DeleteCake(int cakeId){
            try
            {
                if(cakeId<0) return BadRequest();
                var stat= await _cakeService.DeleteCake(cakeId);
                if(stat) return   Ok("Cake deleted successfully");
                else return NotFound("Cannot find any cake");
            }
            catch(Exception e)
            {
                return StatusCode(500,e.Message);
            }
        }
    }
}
