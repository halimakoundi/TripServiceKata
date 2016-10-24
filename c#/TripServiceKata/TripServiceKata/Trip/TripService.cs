using System.Collections.Generic;
using TripServiceKata.Exception;
using TripServiceKata.User;

namespace TripServiceKata.Trip
{
    public class TripService
    {
        private readonly UserSession _userSession;
        private readonly TripDAO _tripDAO;

        public TripService(UserSession userSession, TripDAO tripDao)
        {
            _userSession = userSession;
            _tripDAO = tripDao;
        }

        public List<Trip> GetTripsByUser(User.User user)
        {
            var loggedUser = _userSession.GetLoggedUser();

            SkipIfUserNotLoggedIn(loggedUser);

            return TripsFor(user, loggedUser);
        }

        private List<Trip> TripsFor(User.User user, User.User loggedUser)
        {
            var tripList = new List<Trip>();
            if (user.IsFriendWith(loggedUser))
            {
                tripList = _tripDAO.FindTripsByUser(user);
            }
            return tripList;
        }

        private static void SkipIfUserNotLoggedIn(User.User loggedUser)
        {
            if (loggedUser == null)
            {
                throw new UserNotLoggedInException();
            }
        }
    }
}
