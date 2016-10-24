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
        private readonly User.User _loggedInUser = new User.User();
        private readonly User.User _friendedUser = new User.User();

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
            Assert.Throws<UserNotLoggedInException>(
                () =>
                {
                    _tripService.GetTripsBy(new User.User());
                });
        }

        [Test]
        public void return_no_trip_when_user_has_no_friends()
        {
            _userSession.IsUserLoggedIn(_loggedInUser).Returns<bool>(x => true);
            _userSession.GetLoggedUser().Returns(x => _loggedInUser);
            var notFriendedUser = new User.User();

            var trips = _tripService.GetTripsBy(notFriendedUser);

            Assert.That(trips, Is.Empty);
        }

        [Test]
        public void return_trips_of_a_friend_user()
        {
            var expectedTrips = GivenUserHasFriendsWithTrips();

            var trips = _tripService.GetTripsBy(_friendedUser);

            Assert.That(trips, Is.EqualTo(expectedTrips));
        }

        private List<Trip.Trip> GivenUserHasFriendsWithTrips()
        {
            var toParis = new Trip.Trip();
            _friendedUser.AddTrip(toParis);
            _friendedUser.AddFriend(_loggedInUser);
            _userSession.IsUserLoggedIn(_loggedInUser).Returns<bool>(x => true);
            _userSession.GetLoggedUser().Returns(x => _loggedInUser);
            var expectedTrips = new List<Trip.Trip> {toParis};
            _tripDao.FindTripsBy(_friendedUser).Returns(x => expectedTrips);
            return expectedTrips;
        }
    }

    public class UserSessionTest : UserSession
    {
        public UserSessionTest() { }
    }
}
