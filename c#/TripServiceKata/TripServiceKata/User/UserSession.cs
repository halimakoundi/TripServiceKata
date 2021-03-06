﻿using TripServiceKata.Exception;

namespace TripServiceKata.User
{
    public class UserSession
    {
        private static readonly UserSession userSession = new UserSession();

        protected UserSession() { }

        public static UserSession GetInstance()
        {
            return userSession;
        }

        public virtual bool IsUserLoggedIn(User user)
        {
            throw new DependendClassCallDuringUnitTestException(
                "UserSession.IsUserLoggedIn() should not be called in an unit test");
        }

        public virtual User GetLoggedUser()
        {
            throw new DependendClassCallDuringUnitTestException(
                "UserSession.GetLoggedUser() should not be called in an unit test");
        }
    }
}
