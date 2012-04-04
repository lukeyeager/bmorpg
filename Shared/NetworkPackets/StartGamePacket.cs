/*  This file is part of BMORPG.

    BMORPG is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    BMORPG is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with BMORPG.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.IO;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace BMORPG.NetworkPackets
{
    [Serializable]
    class StartGamePacket : NetworkPacket
    {
        public struct MiniEquipment
        {
            public enum EquipType
            {
                Armor,
                Weapon
            }
            public String name;
            public bool equipped;
            public EquipType type;
        }
        [NonSerialized]
        public const String Identifier = "START_GAME";

        public String opponentUsername = "";
        public bool myTurn = false;

        public List<String> items;
        public List<MiniEquipment> equipments;
        public List<String> abilities;

        public StartGamePacket()
        {
            PacketType = Identifier;
        }
    }
}
