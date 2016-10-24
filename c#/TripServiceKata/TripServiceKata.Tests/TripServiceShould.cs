using NSubstitute;
using NUnit.Framework;
using TripServiceKata.Exception;
using TripServiceKata.Trip;
using TripServiceKata.User;

namespace TripServiceKata.Tests
{
    [TestFixture]
    public class TripServiceShould
    {
        private UserSessionTest _userSession;


        [SetUp]
        public void SetUp()
        {
            _userSession = Substitute.For<UserSessionTest>();
        }

        [Test]
        public void throw_an_error_when_user_not_logged_in()
        {
            var user = new User.User();
            var tripService = new TripService(_userSession);

            Assert.Throws<DependendClassCallDuringUnitTestException>(
                () =>
                {
                    tripService.GetTripsByUser(user);
                });
        }


    }

    public class UserSessionTest : UserSession
    {
        private static UserSession _instance;

        public UserSessionTest() { }

    }
}
