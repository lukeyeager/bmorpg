using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMORPG.NetworkPackets
{
    [Serializable]
    class PlayerMovePacket : NetworkPacket
    {
        [NonSerialized]
        public const String Identifier = "PLAYER_MOVE";

        // TODO:
        // Add more information needed for player X to make a move

        String moveName = "moveName";
        int moveID = 0; // check the server for this moveID, calculate the new game state, then update each client


        public PlayerMovePacket()
        {
            PacketType = Identifier;
        }

    }
}
