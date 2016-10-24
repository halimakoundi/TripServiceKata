using NUnit.Framework;
using TripServiceKata.Exception;
using TripServiceKata.Trip;

namespace TripServiceKata.Tests
{
    [TestFixture]
    public class TripServiceShould
    {

        [Test]
        public void throw_an_error_when_user_not_logged_in()
        {
            var user = new User.User();
            var tripService = new TripService();


            Assert.Throws<DependendClassCallDuringUnitTestException>(
                () =>
                {
                    var trips = tripService.GetTripsByUser(user);
                });
        }
    }
}
