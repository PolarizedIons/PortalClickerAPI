using PortalClickerApi.Database.Models;

namespace PortalClickerApi.Models.Responses
{
    public class LeaderboardResponse
    {
        private readonly ClickerPlayer _player;

        public LeaderboardResponse(ClickerPlayer player)
        {
            _player = player;
        }

        public string UserName => _player.User.UserName;
        public ulong PortalCount => (ulong)_player.PortalCount;
    }
}
