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
            User.User loggedUser = _userSession.GetLoggedUser();

            SkipIfUserNotLoggedIn(loggedUser);

            var tripList = TripsFor(user, loggedUser, _tripDAO);
            return tripList;
        }

        private List<Trip> TripsFor(User.User user, User.User loggedUser, TripDAO tripDao)
        {
            List<Trip> tripList = new List<Trip>();
            bool isFriend = false;
            foreach (User.User friend in user.GetFriends())
            {
                if (friend.Equals(loggedUser))
                {
                    isFriend = true;
                    break;
                }
            }
            if (isFriend)
            {
                tripList = tripDao.FindTripsByUser(user);
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
