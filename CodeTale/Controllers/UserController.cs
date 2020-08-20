using CodeTale.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CodeTale.Controllers
{
    public class UserController : Controller
    {
        
            [HttpGet]
            public ActionResult Registration()
            {
                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Registration([Bind(Exclude = "IsEmailVerified,ActivationCode")]User user)
            {
                bool Status = false;
                string message = "";
                //

                //Model Validation

                if (ModelState.IsValid)
                {
                    //Email Exists

                    #region //Email is already exists
                    var IsExist = IsEmailExist(user.EmailID);
                    if (IsExist)
                    {
                        ModelState.AddModelError("Email Exist", "Email Already Exists");
                        return View(user);
                    }
                    #endregion

                    #region Generate Activation Code
                    user.ActivationCode = Guid.NewGuid();
                    #endregion

                    #region Password hashing
                    user.Password = Crypto.Hash(user.Password);
                    user.ConfirmPassword = Crypto.Hash(user.ConfirmPassword);
                    #endregion
                    user.IsEmailVerified = false;

                    #region Save Data to Database

                    using (CodetaleDb dc = new CodetaleDb())
                    {
                        dc.Users.Add(user);
                        dc.SaveChanges();

                        //Send Email to User
                        SendVerificationLinkEmail(user.EmailID, user.ActivationCode.ToString());
                        message = "Registration successfully done. Account activation link" +
                                  "has been sent to your email id:" + user.EmailID;
                        Status = true;
                    }
                    #endregion

                }
                else
                {
                    message = "Invalid Request";
                }

                ViewBag.Message = message;
                ViewBag.Status = Status;
                return View(user);
            }

            [HttpGet]
            public ActionResult VerifyAccount(string id)
            {
                bool Status = false;
                using (CodetaleDb dc = new CodetaleDb())
                {
                    dc.Configuration.ValidateOnSaveEnabled = false; //Avoid confirm password does not match issue
                                                                    //on save changes
                    var v = dc.Users.Where(a => a.ActivationCode == new Guid(id)).FirstOrDefault();

                    if (v != null)
                    {
                        v.IsEmailVerified = true;
                        dc.SaveChanges();
                        Status = true;
                    }
                    else
                    {
                        ViewBag.Message = "Invalid Request";
                    }
                }
                ViewBag.Status = Status;
                return View();
            }

            [HttpGet]
            public ActionResult Login()
            {
                return View();
            }

            //Post Login Action
            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Login(UserLogin login, string ReturnUrl = "")
            {
                string message = "";

                using (CodetaleDb dc = new CodetaleDb())
                {
                var v = dc.Users.Where(a => a.EmailID == login.EmailID).FirstOrDefault();
                if (v != null)
                    {
                        if (string.Compare(Crypto.Hash(login.Password), v.Password) == 0)
                        {
                            int timeout = login.RememberMe ? 15 : 5;//15 minutes or 5 minutes 
                            var ticket = new FormsAuthenticationTicket(login.EmailID, login.RememberMe, timeout);
                            string encrypted = FormsAuthentication.Encrypt(ticket);
                            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                            cookie.Expires = DateTime.Now.AddMinutes(timeout);
                            cookie.HttpOnly = true;
                            Response.Cookies.Add(cookie);

                            if (Url.IsLocalUrl(ReturnUrl))
                            {
                                return Redirect(ReturnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }

                        }
                        else
                        {
                            message = "Invalid Credentials";
                        }
                    }
                    else
                    {
                        message = "Invalid Credentials";
                    }
                }

                ViewBag.Message = message;
                return View();
            }

            [Authorize]
            [HttpPost]
            public ActionResult Logout()
            {
                FormsAuthentication.SignOut();
                return RedirectToAction("Login", "User");
            }

            [NonAction]
            public bool IsEmailExist(string emailID)
            {
                using (CodetaleDb dc = new CodetaleDb())
                {
                    var v = dc.Users.Where(a => a.EmailID == emailID).FirstOrDefault();
                    return v != null;

                }
            }

            [NonAction]
            public void SendVerificationLinkEmail(string emailID, string activationCode)
            {
                var verifyUrl = "/User/VerifyAccount/" + activationCode;
                var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

                var fromEmail = new MailAddress("bravesdevs1@outlook.com", "CodeTale Corporation");

                var toEmail = new MailAddress(emailID);

                var fromEmailpassword = "Devang8962";

                string subject = "Account Created Successfully";

                string body = "<br/><br/>Successfully created. Please click the link below to verify your Account" +
                    " <br/> <br/><a href='" + link + "'>" + link + "</a> ";

                var smtp = new SmtpClient
                {
                    Host = "smtp-mail.outlook.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail.Address, fromEmailpassword)
                };


                using (var message = new MailMessage(fromEmail, toEmail)
                {
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                }) smtp.Send(message);

            }
        }
    }