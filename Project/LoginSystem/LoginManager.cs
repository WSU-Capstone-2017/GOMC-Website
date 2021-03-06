﻿using Project.Data;
using Project.Models.LoginSystem;
using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;

namespace Project.LoginSystem
{
    public class LoginManager
    {
	    private static readonly byte[] salt =
		    Encoding.UTF8.GetBytes(
			    "IvgpV69JXsiEb3VdhXuijykfjvWWutgsthAiQs1bdfXf0kKRgdkBGC2MSdJ9Sp92YeWehTXF9tzCywbmJSdW2hTmoClpejFV");

	    private static readonly Func<ProjectDbContext> defaultDbGetter = () => new ProjectDbContext();
	    public Func<ProjectDbContext> DbGetter { get; }

	    public LoginManager(Func<ProjectDbContext> dbGetter = null)
	    {
		    DbGetter = dbGetter ?? defaultDbGetter;
	    }

		public bool LoginIsValid(string email, string password)
        {
            if (IsValidEmail(email) == false) 
            {
                return false;
            }
            if (password.Length <= 7)
            {
                return false;
            }
            using (var db = DbGetter())
            {
                var b = db.Database.SqlQuery<LoginModel>($"select * from Logins where email = '{email}'").ToArray();
                if (b.Length == 0)
                {
                    return false;
                }
                if (b.Length == 1)
                {
                    return b[0].PasswordHash == GetHash(password);                                
                }             
            }
            return false;           
        }

	    public static ValidateSessionResultType ValidateSession(Guid session, Func<ProjectDbContext> dbGetter = null)
	    {
		    using(var db = (dbGetter ?? defaultDbGetter)())
		    {
			    var b = db.Database.SqlQuery<LoginSessions>($"select * from LoginSessions where Session = '{session}'").FirstOrDefault();

			    if(b == null)
			    {
				    return ValidateSessionResultType.SessionInvalid;
			    }

				if(b.Expiration < DateTime.Now)
				{
					return ValidateSessionResultType.SessionExpired;
				}

				return ValidateSessionResultType.SessionValid;
		    }
		}

	    public static int? LoginIdFromSession(Guid session, Func<ProjectDbContext> dbGetter = null)
	    {
		    using(var db = (dbGetter ?? defaultDbGetter)())
		    {
			    var sqlParameter = new SqlParameter("@SessionInput", session);

			    var l = db.Database.SqlQuery<LoginSessions>("dbo.GetLoginIdFromSession @SessionInput", sqlParameter)
				    .SingleOrDefault();

			    if(l == null)
			    {
				    return null;
			    }

			    if(l.Expiration < DateTime.Now)
			    {
				    return null;
			    }

			    return l.LoginId;
		    }
	    }
		public GetLoginIdResult GetLoginId(string email, string password)                
        {
            if (IsValidEmail(email) == false)
            {
                return new GetLoginIdResult(LoginResultType.InvalidEmail);
            }
            if (password.Length <= 7)
            {
                return new GetLoginIdResult(LoginResultType.InvalidPassword);
            }
            using (var db = DbGetter())
            {
                var b = db.Database.SqlQuery<LoginModel>($"select * from Logins where email = '{email}'").ToArray();
                if (b.Length == 0)
                {
                    return new GetLoginIdResult(LoginResultType.InvalidEmail);
                }
                if (b.Length == 1)
                {
                    if (b[0].PasswordHash == GetHash(password))
                    {
                        return new GetLoginIdResult(LoginResultType.Success, b[0].Id);
                    }
                    else
                    {
                        return new GetLoginIdResult(LoginResultType.InvalidPassword, b[0].Id);
                    }
                }
            }
            return null;
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var adrs = new MailAddress(email);
                return adrs.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static string GetHash(string value)                                //Function for getting password to hash value
        {
            var stringbuilder = new StringBuilder();                    //For building a string

            using (var hash = SHA256.Create())                                     //var confonts to w.e hash is and this is where we start creating using sha256 algorithm
            {
                var hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(value).Concat(salt).ToArray());   //Password is in string, gets computed to hash value and byte value

                foreach (var i in hashBytes)                                        //Used for changing it back to string value
                {
                    stringbuilder.Append(i.ToString("x2"));                      //String builder for making the string into one whole string,append is adding var i to the string which will have hex value and 2 bytes each
                }

                return stringbuilder.ToString();                                  //Returning the string created

            }
        }
    }

	public class GetLoginIdResult
	{
		public LoginResultType ResultType { get; set; }
		public int? LoginId { get; set; }
		public GetLoginIdResult(LoginResultType type, int? loginId = null)
		{
			ResultType = type;
			LoginId = loginId;
		}
	}
	public class LoginResult
	{
		public LoginResultType ResultType { get; set; }
		public Guid? Session { get; set; }
		public LoginResult(LoginResultType type, Guid? session = null)
		{
			ResultType = type;
			Session = session;
		}
	}
	public enum ValidateSessionResultType
	{
		SessionValid,
		SessionExpired,
		SessionInvalid
	}

	public enum LoginResultType
	{
		Success,
		InvalidEmail,
		InvalidPassword,
        NeedCaptcha,
        InvalidCaptcha   
	}
}