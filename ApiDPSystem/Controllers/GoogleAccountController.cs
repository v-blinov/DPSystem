using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ApiDPSystem.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class GoogleAccountController : Controller
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        private readonly string _accountUrlEndpoint;
        private readonly string _redirectUrlEndpoint;
        private readonly string _revokeUrlEndpoint;
        private readonly string _tokenUrlEndpoint;

        public GoogleAccountController(IConfiguration configuration)
        {
            _clientId = configuration.GetValue<string>("Authentication:Google:ClientId");
            _clientSecret = configuration.GetValue<string>("Authentication:Google:ClientSecret");

            _accountUrlEndpoint = configuration.GetValue<string>("OAuth:AccountUrlEndpoint");
            _tokenUrlEndpoint = configuration.GetValue<string>("OAuth:TokenUrlEndpoint");
            _revokeUrlEndpoint = configuration.GetValue<string>("OAuth:RevokeUrlEndpoint");
            _redirectUrlEndpoint = configuration.GetValue<string>("OAuth:RedirectUrlEndpoint");
        }


        [HttpGet]
        public IActionResult OAuthGoogleLoginGetUrl()
        {
            try
            {
                string url = $"{_accountUrlEndpoint}?" +
                             $"client_id={_clientId}&" +
                             $"redirect_uri={_redirectUrlEndpoint}&" +
                              "response_type=code&" +
                              "access_type=offline&" +
                              "prompt=consent&" +
                              "scope=openid%20profile%20email";

                return Ok(url);
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OAuthGetAccessToken(string code)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, string>()
                    {
                        { "code", code },
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "redirect_uri", HttpUtility.UrlDecode(_redirectUrlEndpoint) },
                        { "access_type", "offline" },
                        { "prompt", "consent" },
                        { "grant_type", "authorization_code" }
                    };

                    var formContent = new FormUrlEncodedContent(data);
                    var apiResponse = await client.PostAsync(_tokenUrlEndpoint, formContent);
                    var result = await apiResponse.Content.ReadAsStringAsync();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OAuthRefreshTokenAsync(string refreshToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, string>()
                    {
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "refresh_token", refreshToken },
                        { "grant_type", "refresh_token" },
                    };

                    var formContent = new FormUrlEncodedContent(data);
                    var apiResponse = await client.PostAsync(_tokenUrlEndpoint, formContent);
                    var result = await apiResponse.Content.ReadAsStringAsync();

                    return Ok(result);
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet]
        public async Task<IActionResult> OAuthRevokeTokenAsync(string accessToken)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var data = new Dictionary<string, string>()
                    {
                        { "client_id", _clientId },
                        { "client_secret", _clientSecret },
                        { "token", accessToken }
                    };

                    var formContent = new FormUrlEncodedContent(data);
                    await client.PostAsync(_revokeUrlEndpoint, formContent);

                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #region AdditionalFunctionality
        //[HttpGet]
        //public async Task<IActionResult> GoogleResponse()
        //{
        //    try
        //    {
        //        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        //        if (info == null)
        //            return RedirectToAction("Login");

        //        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        //        string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
        //        if (result.Succeeded)
        //            return Ok(userInfo);
        //        else
        //        {
        //            User user = new User
        //            {
        //                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
        //                UserName = info.Principal.FindFirst(ClaimTypes.Email).Value,
        //                FirstName = "TEST",
        //                LastName = "test"
        //                //LastName = info.Principal.FindFirstValue(ClaimTypes.Name).Split(" ")[1] 
        //            };

        //            IdentityResult identResult = await _userManager.CreateAsync(user);
        //            if (identResult.Succeeded)
        //            {
        //                identResult = await _userManager.AddLoginAsync(user, info);
        //                if (identResult.Succeeded)
        //                {
        //                    await _signInManager.SignInAsync(user, false);
        //                    return Ok(userInfo.ToList());
        //                }
        //            }
        //            return StatusCode(StatusCodes.Status403Forbidden);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error("", ex);
        //        return StatusCode(StatusCodes.Status500InternalServerError);
        //    }
        //}
        #endregion
    }
}
