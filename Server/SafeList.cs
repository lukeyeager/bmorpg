﻿/*  This file is part of BMORPG.

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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BMORPG_Server
{
    /// <summary>
    /// Thread-safe List
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SafeList<T>
    {
        private List<T> list;
        private object myLock;// = new Object(); instantiate in the constructor so each list has its own lock.
        private T nullObject;

        /// <summary>
        /// Instatiates the SafeList.
        /// </summary>
        /// <param name="nullObj">Use null for this parameter.</param>
        public SafeList(T nullObj)
        {
            list = new List<T>();
            myLock = new Object();
            nullObject = nullObj;
        }

        /// <summary>
        /// This method adds an object to the list
        /// </summary>
        /// <param name="obj">The object to be added to the list.</param>
        public void Push(T obj)
        {
            lock (myLock)
            {
                list.Add(obj);
            }
        }

        /// <summary>
        /// Stores the first object in the list to obj
        /// </summary>
        /// <returns>False if the list is empty</returns>
        public T Pop()
        {
            lock (myLock)
            {
                if (list.Count > 0)
                {
                    T obj = list[0];
                    list.RemoveAt(0);
                    return obj;
                }
                else
                {
                    return nullObject;
                }
            }
        }

        /// <summary>
        /// Gets the number of elements actually contained in the List<T>.
        /// </summary>
        public Int32 Count
        {
            get
            {
                return list.Count;
            }
        }

    }
}
