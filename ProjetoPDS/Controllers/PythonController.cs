using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Text.Unicode;
namespace ProjetoPDS.Controllers
{
    public class PythonController : Controller
    {
        public async Task<string> CallPythonBlur(string pathToFile, string nomeFotoFicheiro, int leftValue, int topValue, int rightValue, int bottomValue)
        {

            using var client = new HttpClient();
            var url = "http://localhost:8000/BlurUsers/";

            var json = JsonConvert.SerializeObject(new { path = pathToFile, fileName = nomeFotoFicheiro,
                                                         left = leftValue, top = topValue, right = rightValue, bottom = bottomValue});
            var content = new StringContent(json, System.Text.Encoding.UTF8,"application/json");
            var response = await client.PostAsync(url, content);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
