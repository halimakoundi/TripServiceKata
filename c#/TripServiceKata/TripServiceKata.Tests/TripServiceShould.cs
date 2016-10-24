using System.Collections.Generic;
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
        private TripDAO _tripDao;
        private User.User _loggedInUser;
        private User.User _friendedUser;

        public TripServiceShould()
        {
            _loggedInUser = new User.User();
            _friendedUser = new User.User();
        }


        [SetUp]
        public void SetUp()
        {
            _userSession = Substitute.For<UserSessionTest>();
            _tripDao = Substitute.For<TripDAO>();
            _tripService = new TripService(_userSession, _tripDao);

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
            var notFriendedUser = new User.User();

            var trips = _tripService.GetTripsByUser(notFriendedUser);

            Assert.That(trips, Is.Empty);
        }

        [Test]
        public void return_trips_of_a_friend_user()
        {
            var trip = new Trip.Trip();
            _friendedUser.AddTrip(trip);
            _friendedUser.AddFriend(_loggedInUser);
            _userSession.IsUserLoggedIn(_loggedInUser).Returns<bool>(x => true);
            _userSession.GetLoggedUser().Returns(x => _loggedInUser);
            var expectedTrips = new List<Trip.Trip> {trip};
            _tripDao.FindTripsByUser(_friendedUser).Returns(x => expectedTrips);

            var trips = _tripService.GetTripsByUser(_friendedUser);

            Assert.That(trips, Is.EqualTo(expectedTrips));
        }
    }

    public class UserSessionTest : UserSession
    {
        public UserSessionTest() { }

    }
}
