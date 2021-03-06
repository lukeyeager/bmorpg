﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMORPG.NetworkPackets
{
    /// <summary>
    /// Player sends this to the Server
    /// </summary>
    [Serializable]
    class PlayerMovePacket : NetworkPacket
    {
        [NonSerialized]
        public const String Identifier = "PLAYER_MOVE";

        public enum MoveType
        {
            None,
            Item,
            Equipment,
            Ability
        };
		
        public int moveID;
        public MoveType moveType = MoveType.None;

        public PlayerMovePacket()
        {
            PacketType = Identifier;
        }

    }
}
