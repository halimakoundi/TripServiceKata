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

            Assert.Throws<UserNotLoggedInException>(
                () =>
                {
                    tripService.GetTripsByUser(user);
                });
        }

        [Test]
        public void return_no_trip_when_user_has_no_friends()
        {
            var user = new User.User();
            _userSession.IsUserLoggedIn(user).Returns<bool>(x => true);
            _userSession.GetLoggedUser().Returns<User.User>(x => user);
            var tripService = new TripService(_userSession);

            var trips = tripService.GetTripsByUser(user);

            Assert.That(trips, Is.Empty);
        }
    }

    public class UserSessionTest : UserSession
    {
        public UserSessionTest() { }

    }
}
