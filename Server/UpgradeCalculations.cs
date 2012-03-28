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

namespace BMORPG_Server
{
    /// <summary>
    /// Static class used to perform calculations related to a Player leveling up
    /// </summary>
    static class UpgradeCalculations
    {
        /// <summary>
        /// Calculates the experience necessary for a player to reach the next level
        /// </summary>
        /// <returns>XP</returns>
        public static int ExpForNextLevel(int level)
        {
            int sum = 0;

            // Exp = Sum from i=1 to level of i^2
            for (int i = 1; i <= level; i++)
                sum += i * i;

            return sum * 10;
        }

        /// <summary>
        /// Calculates the experience rewarded for a win
        /// </summary>
        /// <param name="theirLevel">Opponent's level</param>
        /// <returns>XP</returns>
        public static int ExpForWin(int theirLevel)
        {
            return theirLevel * 10;
        }

        /// <summary>
        /// Calculates the experience rewarded for a loss
        /// </summary>
        /// <param name="theirLevel">Opponent's level</param>
        /// <returns>XP</returns>
        public static int ExpForLoss(int theirLevel)
        {
            return theirLevel * 4;
        }

        /// <summary>
        /// Calculates the money rewarded for a win
        /// </summary>
        /// <param name="theirLevel">Opponent's level</param>
        /// <returns>Money</returns>
        public static int MoneyForWin(int theirLevel)
        {
            return theirLevel * 10;
        }

        /// <summary>
        /// Calculates the money rewarded for a loss
        /// </summary>
        /// <param name="theirLevel">Opponent's level</param>
        /// <returns>Money</returns>
        public static int MoneyForLoss(int theirLevel)
        {
            return 0;
        }
    }
}
