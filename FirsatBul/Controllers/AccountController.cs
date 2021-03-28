using FirsatBul.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FirsatBul.Entities;

namespace FirsatBul.Controllers
{
    public class AccountController : Controller
    {


        private UserManager<AppUser> userManager;
        private SignInManager<AppUser> signInManager;

        public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr)
        {
            userManager = userMgr;
            signInManager = signinMgr;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            Login login = new Login();
            login.ReturnUrl = returnUrl;
            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (ModelState.IsValid)
            {


                var logInResponse = await ExternalApiRequestForSignIn(login);
                if ((int)logInResponse.StatusCode == 200)// servisten olumlu dönmüştür bizde yoksa oluşturup içeri alcaz varsa direk alcaz.
                {
                    string normalizedEmail = "";
                    string normalizedUserName = "";
                    if (!login.Email.Contains("@"))
                    {
                        normalizedEmail = login.Email + "@firstabul.com";
                        normalizedUserName = login.Email;
                    }
                    else
                    {
                        normalizedEmail = login.Email;
                        normalizedUserName = login.Email.Split("@")[0];
                    }
                    AppUser user = new AppUser
                    {
                        Email = normalizedEmail,
                        UserName = normalizedUserName
                    };
                    AppUser appUser = await userManager.FindByEmailAsync(normalizedEmail);
                    if (appUser != null)
                    {
                        UserLoginInfo info = new UserLoginInfo("hh", "hh", "ll");
                        string[] userInfo = { login.Email, login.Email };
                        TempData["UserEmail"] = normalizedEmail;
                        TempData["UserName"] = normalizedUserName;
                        var identResult = await userManager.AddLoginAsync(user, info);

                        await signInManager.SignInAsync(user, false);
                        return Redirect(login.ReturnUrl ?? "/");//View(userInfo);


                    }
                    else
                    {
                        UserLoginInfo info = new UserLoginInfo("hh", "hh", "ll");
                        string[] userInfo = { login.Email, login.Email };
                        IdentityResult identResult = await userManager.CreateAsync(user);
                        if (identResult.Succeeded)
                        {
                            identResult = await userManager.AddLoginAsync(user, info);
                            TempData["UserEmail"] = normalizedEmail;
                            TempData["UserName"] = normalizedUserName;
                            await signInManager.SignInAsync(user, false);
                            return View(userInfo);

                        }
                        return AccessDenied();
                    }


                }
                else
                {
                    ModelState.AddModelError(nameof(login.Email), "Login Failed: Invalid Email or password");
                }


                return View(login);
            }
            return View();
        }
            public async Task<IActionResult> Logout()
            {
                await signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }

            public IActionResult AccessDenied()
        {
            return View();
        }

            [AllowAnonymous]
            public IActionResult GoogleLogin()
            {
                string redirectUrl = Url.Action("GoogleResponse", "Account");
                var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
                return new ChallengeResult("Google", properties);
            }

            [AllowAnonymous]
            public async Task<IActionResult> GoogleResponse()
            {
                ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                    return RedirectToAction(nameof(Login));

                var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
                string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
                if (result.Succeeded)
            {
                TempData["LogInUser"] = userInfo;
                return RedirectToAction("Index", "Home"); /*View(userInfo);*/

            }
            else
                {
                    AppUser user = new AppUser
                    {
                        Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                        UserName = info.Principal.FindFirst(ClaimTypes.Surname).Value
                    };


                    IdentityResult identResult = await userManager.CreateAsync(user);
                    if (identResult.Succeeded)
                    {
                        identResult = await userManager.AddLoginAsync(user, info);
                        if (identResult.Succeeded)
                        {
                            await signInManager.SignInAsync(user, false);
                            return View(userInfo);
                        }
                    }
                    return AccessDenied();
                }
            }


            public async Task<IRestResponse> ExternalApiRequestForSignIn(Login login)
            {

                var data = new JObject();
                data["email"] = login.Email;
                data["password"] = login.Password;
                string json = JsonConvert.SerializeObject(data);

                var client = new RestClient("https://service.firsatbull.com.tr/api/Auth/login");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json-patch+json");
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                return response;
            }
        }
}
