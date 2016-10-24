using System.Collections.Generic;
using NUnit.Framework;

namespace TripServiceKata.Tests
{
    [TestFixture]
    public class UserShould
    {
        [Test]
        public void return_no_trips_for_user_that_did_not_register_any_trip()
        {
            var user = new User.User();

            var trips = user.Trips();

            Assert.That(trips, Is.Empty);
        }

        [Test]
        public void return_trips_for_user()
        {
            var user = new User.User();
            var trip = new Trip.Trip();
            user.AddTrip(trip);

            var trips = user.Trips();
            
            Assert.That(trips, Is.EqualTo(new List<Trip.Trip> {trip}));
        }
    }
}