﻿// Itinero - Routing for .NET
// Copyright (C) 2015 Abelshausen Ben
// 
// This file is part of Itinero.
// 
// Itinero is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// Itinero is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Itinero. If not, see <http://www.gnu.org/licenses/>.

using System;

namespace Itinero.Graphs
{
    /// <summary>
    /// Contains extension methods for the graph.
    /// </summary>
    public static class GraphExtensions
    {
        /// <summary>
        /// Gets the vertex on this edge that is not the given vertex.
        /// </summary>
        /// <returns></returns>
        public static uint GetOther(this Edge edge, uint vertex)
        {
            if (edge.From == vertex)
            {
                return edge.To;
            }
            else if (edge.To == vertex)
            {
                return edge.From;
            }
            throw new ArgumentOutOfRangeException(string.Format("Vertex {0} is not part of edge {1}.",
                vertex, edge.Id));
        }
    }
}