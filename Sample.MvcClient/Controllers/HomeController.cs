using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.MvcClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace Sample.MvcClient.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public IActionResult LogOut()
        {
            return SignOut("Cookies","oidc");
        }

        /// <summary>
        /// 模拟请求api
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> CallApi()
        {
            // 获取访问令牌
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            // 创建HTTP客户端
            var client = new HttpClient();
            // 设置授权请求头
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            // 请求API
            var content = await client.GetStringAsync("http://localhost:5000/IdentityServer");
            // 转换api返回结果
            ViewBag.Josn = JArray.Parse(content).ToString();
            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
