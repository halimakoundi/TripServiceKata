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
        private TripService _tripService;


        [SetUp]
        public void SetUp()
        {
            _userSession = Substitute.For<UserSessionTest>();
            _tripService = new TripService(_userSession);

        }

        [Test]
        public void throw_an_error_when_user_not_logged_in()
        {
            var user = new User.User();

            Assert.Throws<UserNotLoggedInException>(
                () =>
                {
                    _tripService.GetTripsByUser(user);
                });
        }

        [Test]
        public void return_no_trip_when_user_has_no_friends()
        {
            var user = new User.User();
            _userSession.IsUserLoggedIn(user).Returns<bool>(x => true);
            _userSession.GetLoggedUser().Returns(x => user);

            var trips = _tripService.GetTripsByUser(user);

            Assert.That(trips, Is.Empty);
        }


        [Test]
        public void throw_an_exception_when_accessing_friend_trip()
        {
            var user = new User.User();
            var friendUserWithTrips = new User.User();
            friendUserWithTrips.AddTrip(new Trip.Trip());
            friendUserWithTrips.AddFriend(user);
            _userSession.IsUserLoggedIn(user).Returns<bool>(x => true);
            _userSession.GetLoggedUser().Returns(x => user);

            Assert.Throws<DependendClassCallDuringUnitTestException>(
                () =>
                {
                    _tripService.GetTripsByUser(friendUserWithTrips);
                });
        }
    }

    public class UserSessionTest : UserSession
    {
        public UserSessionTest() { }

    }
}
